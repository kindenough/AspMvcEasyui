/*************************************************************************
 * 文件名称 ：ServiceBaseEdit.cs                          
 * 描述说明 ：定义数据服务基类中的编辑处理
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Zephyr.Utils;

namespace Zephyr.Core
{
    public partial class ServiceBase<T> where T : ModelBase, new()
    {
        protected virtual bool OnBeforeEditPage(EditPageEventArgs arg)
        {
            return true;
        }

        protected virtual bool OnBeforeEditPageForm(EditPageEventArgs arg)
        {
            return true;
        }

        protected virtual void OnAfterEditPageForm(EditPageEventArgs arg)
        {

        }

        protected virtual bool OnBeforeEditPageRow(EditPageEventArgs arg)
        {
            return true;
        }

        protected virtual void OnAfterEditPageRow(EditPageEventArgs arg)
        {

        }

        protected virtual void OnAfterEditPage(EditPageEventArgs arg)
        {

        }
 
        public int EditPage(JObject data,RequestWrapper formWrapper, List<RequestWrapper> tabsWrapper)
        {
            const string DATA_TYPE_DELETED = "deleted";
            const string DATA_TYPE_UPDATED = "updated";
            const string DATA_TYPE_INSERTED = "inserted";

            const string DATA_TYPE_FORM = "form";
            const string DATA_TYPE_TABS = "tabs";
            const string DATA_TYPE_ARRAY = "Array";

            Dictionary<string, OptType> actionTypes = new Dictionary<string, OptType>{
                {DATA_TYPE_DELETED  ,OptType.Del},
                {DATA_TYPE_UPDATED  ,OptType.Mod},
                {DATA_TYPE_INSERTED ,OptType.Add}};

            var handles = new Dictionary<string, Func<RequestWrapper, int>>{
                {DATA_TYPE_DELETED  , x => BuilderParse(x.ToParamDelete()).Execute()},
                {DATA_TYPE_UPDATED  , x => BuilderParse(x.ToParamUpdate()).Execute()},
                {DATA_TYPE_INSERTED , x => BuilderParse(x.ToParamInsert()).Execute()}};

            var rowsAffected = 0;

            Logger("编辑记录", () =>
            {
                var formData = data[DATA_TYPE_FORM];
                var tabsData = data[DATA_TYPE_TABS];
                var editArgs = new EditPageEventArgs() { db = db, data=data, formWrapper=formWrapper, tabsWrapper=tabsWrapper};

                //开启事务
                db.UseTransaction(true);

                //更新全部数据之前事件
                var rtnBefore = this.OnBeforeEditPage(editArgs);
                if (!rtnBefore) return;

                //更新主表
                if (formData != null && formWrapper != null)
                {
                    formWrapper.SetRequestData(formData);
                    var changedFieldCount = formWrapper.ToParamUpdate().GetData().Columns.Count;

                    //如果有字段被修改则更新
                    if (changedFieldCount > 0)
                    {
                        //到数据库中取得旧值
                        var formOld = BuilderParse(formWrapper.ToParamQuery()).QuerySingleDynamic();
                        var strAction = (formOld == null) ? DATA_TYPE_INSERTED : DATA_TYPE_UPDATED;

                        //事件参数
                        editArgs.dataNew = formData;
                        editArgs.dataOld = formOld??new JObject();
                        editArgs.dataAction = actionTypes[strAction];
                        editArgs.dataWrapper = formWrapper;

                        //主表编辑前事件
                        rtnBefore = this.OnBeforeEditPageForm(editArgs);
                        if (!rtnBefore) return;

                        //主表数据处理
                        formWrapper.SetRequestData(editArgs.dataNew);
                        editArgs.executeValue = handles[strAction](formWrapper);
                        rowsAffected += editArgs.executeValue;

                        //把未修改的数据更新到form新值上,避免在后面的事件中使用formNew值时为null出现错误
                        if (editArgs.dataAction == OptType.Mod)
                        {
                            EachHelper.EachObjectProperty(editArgs.dataOld as object, (i, name, value) =>
                            {
                                if (editArgs.dataNew[name] == null)
                                    editArgs.dataNew[name] = JToken.FromObject(value ?? string.Empty);
                            });
                        }

                        //主表编辑结束事件
                        this.OnAfterEditPageForm(editArgs);
                    }
                }

                //更新tabs
                if (tabsData != null && tabsWrapper != null)
                {
                    var current = 0;
                    var wrapperCount = tabsWrapper.Count;

                    foreach (JToken tab in tabsData.Children())
                    {
                        //如果Tab数据长度大于wrapper长度，则结束
                        if (current > wrapperCount - 1)
                            break;

                        //如果数据为null或未配置相应的wrapper则跳过处理
                        var wrapper = tabsWrapper[current++];
                        if (tab == null || wrapper == null)
                            continue;

                        //判断是form类型还是grid类型
                        bool IsGrid = (tab[DATA_TYPE_DELETED] != null && tab[DATA_TYPE_DELETED].Type.ToString() == DATA_TYPE_ARRAY)
                            || (tab[DATA_TYPE_UPDATED] != null && tab[DATA_TYPE_UPDATED].Type.ToString() == DATA_TYPE_ARRAY)
                            || (tab[DATA_TYPE_INSERTED] != null && tab[DATA_TYPE_INSERTED].Type.ToString() == DATA_TYPE_ARRAY);

                        //事件参数
                        editArgs.dataWrapper = wrapper;
                        editArgs.tabIndex = current;
                        editArgs.tabType = IsGrid ? TabType.Grid : TabType.Form;
                        editArgs.tabData = tab;

                        if (IsGrid)
                        {
                            foreach (JProperty item in tab.Children())
                            {
                                //只处理deleted updated inserted三个节点
                                if (!handles.ContainsKey(item.Name))
                                    continue;

                                //循环每一行数据
                                foreach (var row in item.Value.Children())
                                {
                                    //到数据库中取得旧值
                                    wrapper.SetRequestData(row);
                                    var rowOld = BuilderParse(wrapper.ToParamQuery()).QuerySingleDynamic();

                                    //事件参数
                                    editArgs.dataNew = row;
                                    editArgs.dataOld = rowOld ?? new JObject();
                                    editArgs.dataAction = actionTypes[item.Name];

                                    //行编辑前事件
                                    rtnBefore = this.OnBeforeEditPageRow(editArgs);
                                    if (!rtnBefore) return;

                                    //数据处理
                                    wrapper.SetRequestData(editArgs.dataNew);
                                    editArgs.executeValue = handles[item.Name](wrapper);
                                    rowsAffected += editArgs.executeValue;

                                    //行编辑后事件
                                    this.OnAfterEditPageRow(editArgs);
                                }
                            }
                        }
                        else
                        {
                            wrapper.SetRequestData(tab);
                            var changedFieldCount = wrapper.ToParamUpdate().GetData().Columns.Count;

                            //如果有字段被修改则更新
                            if (changedFieldCount > 0)//更新主表
                            {
                                //到数据库中取得旧值
                                var formOld = BuilderParse(wrapper.ToParamQuery()).QuerySingleDynamic();
                                var strAction = (formOld == null) ? DATA_TYPE_INSERTED : DATA_TYPE_UPDATED;

                                //事件参数
                                editArgs.dataNew = tab;
                                editArgs.dataOld = formOld ?? new JObject();
                                editArgs.dataAction = actionTypes[strAction];
                                editArgs.dataWrapper = wrapper;

                                //Form编辑前事件
                                rtnBefore = this.OnBeforeEditPageRow(editArgs);
                                if (!rtnBefore) return;

                                //Form数据处理
                                wrapper.SetRequestData(editArgs.dataNew);
                                editArgs.executeValue = handles[strAction](wrapper);
                                rowsAffected += editArgs.executeValue;

                                //把未修改的数据更新到form新值上
                                if (editArgs.dataAction == OptType.Mod)
                                {
                                    EachHelper.EachObjectProperty(editArgs.dataOld as object, (i, name, value) =>
                                    {
                                        if (editArgs.dataNew[name] == null)
                                            editArgs.dataNew[name] = JToken.FromObject(value ?? string.Empty);
                                    });
                                }

                                //Form编辑结束事件
                                this.OnAfterEditPageRow(editArgs);
                            }
                        }
                    }

                    //更新全部数据之后事件
                    editArgs.executeValue = rowsAffected;
                    this.OnAfterEditPage(editArgs);

                    //提交事务
                    if (rowsAffected > 0)
                    {
                        db.Commit();
                        Msg.Set(MsgType.Success, APP.MSG_SAVE_SUCCESS);
                    }
                }
            }, e => db.Rollback());

            return rowsAffected;
        }
    }
}
