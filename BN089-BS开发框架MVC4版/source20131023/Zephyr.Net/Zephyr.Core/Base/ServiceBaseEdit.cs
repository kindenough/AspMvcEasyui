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
        protected virtual bool OnBeforEdit(EditEventArgs arg)
        {
            return true;
        }

        protected virtual bool OnBeforEditMaster(EditEventArgs arg)
        {
            return true;
        }

        protected virtual void OnAfterEditMaster(EditEventArgs arg)
        {

        }

        protected virtual bool OnBeforEditDetail(EditEventArgs arg)
        {
            return true;
        }

        protected virtual void OnAfterEditDetail(EditEventArgs arg)
        {

        }

        protected virtual void OnAfterEdit(EditEventArgs arg)
        {
            
        }
         
        public int Edit(RequestWrapper formWrapper, RequestWrapper listWrapper, JObject data)
        {
            #region 定义变量
            var types = new Dictionary<string,OptType>{
                {"deleted"  ,OptType.Del},
                {"updated"  ,OptType.Mod},
                {"inserted" ,OptType.Add}
            };

            var handles = new Dictionary<string, Func<RequestWrapper, int>>{
                    {"deleted"  , x => BuilderParse(x.ToParamDelete()).Execute()},
                    {"updated"  , x => BuilderParse(x.ToParamUpdate()).Execute()},
                    {"inserted" , x => BuilderParse(x.ToParamInsert()).Execute()}
            };
            #endregion

            var rowsAffected = 0;

            Logger("编辑记录", () =>
            {
                db.UseTransaction(true); //开启事务
                var editArgs = new EditEventArgs() { db = db, form = data["form"],list = data["list"]};

                if (data["form"] != null && formWrapper != null)
                {
                    editArgs.wrapper = formWrapper;
                    editArgs.formOld = this.GetDynamic(formWrapper.SetRequestData(editArgs.form).ToParamQuery())??new JObject();
                    var rtnBefore = this.OnBeforEdit(editArgs);
                    if (!rtnBefore) return;

                    var pUpdate = formWrapper.SetRequestData(editArgs.form).ToParamUpdate();
                    if (pUpdate.GetData().Columns.Count > 0)//更新主表
                    {
                        rtnBefore = this.OnBeforEditMaster(editArgs);
                        if (rtnBefore)
                        {
                            pUpdate = formWrapper.SetRequestData(editArgs.form).ToParamUpdate(); //在before事件中更改了form中的值，刷新更新时的值
                            rowsAffected = BuilderParse(pUpdate).Execute();
                            if (rowsAffected == 0)
                                rowsAffected = BuilderParse(formWrapper.ToParamInsert()).Execute();

                            editArgs.executeValue = rowsAffected;
                            this.OnAfterEditMaster(editArgs);
                        }
                    }

                    if (editArgs.formOld != null && editArgs.formOld.GetType()!= typeof(JObject))
                        EachHelper.EachObjectProperty(editArgs.formOld as object, (i, name, value) => {
                            if (editArgs.form[name] == null) editArgs.form[name] = JToken.FromObject(value??string.Empty);
                        });
                }

                if (data["list"] != null && listWrapper != null)
                {
                    editArgs.wrapper = listWrapper;

                    foreach (JProperty item in data["list"].Children())
                    {
                        if (!handles.ContainsKey(item.Name))
                            continue;

                        foreach (var row in item.Value.Children())
                        {
                            editArgs.row = row;
                            editArgs.type = types[item.Name];
                            editArgs.rowOld = this.GetDynamic(listWrapper.SetRequestData(editArgs.row).ToParamQuery())?? new JObject();

                            var rtnBefore = this.OnBeforEditDetail(editArgs);
                            if (!rtnBefore) continue;
                            editArgs.executeValue = handles[item.Name](listWrapper.SetRequestData(row));
                            rowsAffected += editArgs.executeValue;

                            this.OnAfterEditDetail(editArgs);
                        }
                    }
                }

                editArgs.executeValue = rowsAffected;
                editArgs.wrapper = formWrapper;

                this.OnAfterEdit(editArgs);

                if (rowsAffected > 0)
                {
                    db.Commit();
                    Msg.Set(MsgType.Success, APP.MSG_SAVE_SUCCESS);
                }
            }, e => db.Rollback());

            return rowsAffected;
        }
    }
}
