/*************************************************************************
 * 文件名称 ：APP.cs                          
 * 描述说明 ：框架配置类
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Configuration;
using Zephyr.Data;

namespace Zephyr.Core
{
    public class APP
    {
        //消息字段
        public static string MSG_NOTAUTH_CODE = "NotAuth";
        public static string MSG_NOTAUTH_TEXT = "未登陆";
        public static string MSG_SAVE_SUCCESS = "保存成功！";
        public static string MSG_DELETE_SUCCESS = "删除成功！";
        public static string MSG_UPDATE_SUCCESS = "更新成功！";
        public static string MSG_INSERT_SUCCESS = "新增成功！";
        public static string MSG_FILE_NOT_EXIST = "请求的文件不存在！";
        public static string MSG_MISS_MODULE_ATTR = "业务类{0}，缺少特性ModuleAttribute";

        //配置自动更新的字段
        public static string FIELD_UPDATE_PERSON = "UpdatePerson";
        public static string FIELD_UPDATE_DATE = "UpdateDate";
        public static string FIELD_CREATE_PERSON = "CreatePerson";
        public static string FIELD_CREATE_DATE = "CreateDate";

        //BLL的一些设置
        public static string SERVICE_BLL_NAMESPACE = "Zephyr.Models"; //指定bll层的命名空间
        public static string SERVICE_BLL_FILE_NAME = "Zephyr.Web"; //指定bll层所在的dll名称，如Zephyr.BLL.dll 则值为Zephyr.BLL
        public static Type SERVICE_BLL_GET_TYPE(string model) { return Type.GetType(model + "Service"); }

        //编辑
        public static string EDIT_TYPE = "_edit_type";       //编辑时用于识别编辑类型的临时字段
        public static string EDIT_EXCEPT_START_WITH = "_";   //编辑时忽略字段的开头标识

        //下载
        public static string DOWNLOAD_ROOT_PATH = "/files/"; //通过download service url方式下载的根目录

        //数据库的一些设置
        public static string DB_DEFAULT_CONN_NAME = ConfigurationManager.ConnectionStrings[1].Name;
       
        //事件支持
        //public static Action<AjaxAction> OnAjaxRequst = null;
        public static Action<CommandEventArgs> OnDbExecuting = null;

        //框架初始化函数
        public static void Init() 
        {
            LogHelper.Init();
            PinYin.GenaratePinYinFunc();
        }
    }
}
