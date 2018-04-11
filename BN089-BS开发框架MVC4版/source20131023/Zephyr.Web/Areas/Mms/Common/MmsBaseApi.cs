using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Web.Areas.Mms.Common;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MmsBaseApi<TMasterModel,TMasterService,TDetailModel, TDetailService> : ApiController
        where TMasterModel : ModelBase, new()
        where TDetailModel : ModelBase, new()
        where TMasterService : ServiceBase<TMasterModel>, new()
        where TDetailService : ServiceBase<TDetailModel>, new()
    {
        #region 属性
        private TMasterService _masterService;
        private TDetailService _detailService;
        public TMasterService masterService
        { 
            get 
            {
                if (_masterService == null)
                    _masterService = new TMasterService();
                return _masterService;
            } 
        }
        public TDetailService detailService
        {
            get
            {
                if (_detailService == null)
                    _detailService = new TDetailService();
                return _detailService;
            }
        }
        public string projectCode { get { return MmsHelper.GetCurrentProject(); } }
        public string userName { get { return MmsHelper.GetUserName(); } }
        #endregion

        #region 自动完成
        // GET api/mms/send/getbillno
        public virtual List<dynamic> GetBillNo(string q)
        {
            var pQuery = ParamQuery.Instance().Select("top 10 BillNo").AndWhere("BillNo", q, Cp.StartWith);
            return masterService.GetDynamicList(pQuery);
        }
        #endregion

        #region 采播
        // 取得新的主表Bill GET api/mms/send/getnewbillno
        public virtual string GetNewBillNo()
        {
            return masterService.GetNewKey("BillNo", "dateplus");
        }

        // 取得新的明细表RowId GET api/mms/send/getnewrowid
        public virtual string GetNewRowId(int id,string BillNo)
        {
            return detailService.GetNewKey("RowId", "maxplus", id, ParamQuery.Instance().AndWhere("BillNo", BillNo));
        }
        #endregion

        #region 查询
        // 查询主表数据列表 GET api/mms/send 
        public virtual dynamic Get(RequestWrapper query)
        {
            if (!query.IsLoadedSettings)
                query.LoadSettingXmlString(@"
<settings defaultOrderBy='BillNo'>
    <select>*</select>
    <from>{0}</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>",typeof(TMasterModel).Name);
            var pQuery = query.ToParamQuery();
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }

        // 取得编辑页面中的主表数据及上一页下一页主键 GET api/mms/send/geteditmaster 
        public virtual dynamic GetEditMaster(string id)
        {
            return new
            {
                form = masterService.GetModel(ParamQuery.Instance().AndWhere("BillNo", id)),
                scrollKeys = masterService.ScrollKeys("BillNo", id, ParamQuery.Instance().AndWhere("ProjectCode", projectCode))
            };
        }

        // 查询明细表 GET api/mms/send/getdetail
        public virtual dynamic GetDetail(string id)
        {
            var query = RequestWrapper
                .InstanceFromRequest()
                .SetRequestData("BillNo", id)
                .LoadSettingXmlString(@"
<settings defaultOrderBy='MaterialCode'>
    <select>
        A.*, B.MaterialName,B.Model,B.Material
    </select>
    <from>
        {0} A
        left join mms_material B on B.MaterialCode = A.MaterialCode
    </from>
    <where>
        <field name='BillNo' cp='equal'></field>
    </where>
</settings>",typeof(TDetailModel).Name);

            var pQuery = query.ToParamQuery();
            var result = masterService.GetDynamicListWithPaging(pQuery);
            return result;
        }
        #endregion

        #region 删除
        // 删除 DELETE api/mms/send
        public virtual void Delete(string id)
        {
            var result = masterService.Delete(ParamDelete.Instance().AndWhere("BillNo", id));
            MmsHelper.ThrowHttpExceptionWhen(result <= 0, "单据删除失败[BillNo={0}]，请重试或联系管理员！", id);
        }
        #endregion

        #region 审核
        // 审核 DELETE api/mms/send/audit
        [HttpPost]
        public virtual void Audit(string id, JObject data)
        {
            var status = data["status"].ToString();
            var comment = data["comment"].ToString();
            var result = new MmsService().Audit(typeof(TMasterModel).Name, id, status, comment);
            MmsHelper.ThrowHttpExceptionWhen(result < 0, "单据审核失败[BillNo={0}]，请重试或联系管理员！", id);
        }
        #endregion

        #region 保存
        // 保存 POST api/mms/send
        [HttpPost]
        public virtual void Edit(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where><field name='BillNo' cp='equal'></field></where>
</settings>",typeof(TMasterModel).Name);

            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>{0}</table>
    <where>
        <field name='BillNo' cp='equal'></field>
        <field name='RowId'  cp='equal'></field>
    </where>
</settings>",typeof(TDetailModel).Name);
             
            var result = masterService.Edit(formWrapper, listWrapper, data);
        }
        #endregion
    }
}