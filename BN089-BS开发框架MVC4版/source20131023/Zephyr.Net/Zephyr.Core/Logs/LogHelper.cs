/*************************************************************************
 * 文件名称 ：LogHelper.cs                          
 * 描述说明 ：日志处理
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.IO;
using System.Reflection;
using log4net;

namespace Zephyr.Core
{
    public class LogHelper
    {
        #region 全局设置
        public static void Init()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var xml = assembly.GetManifestResourceStream("Zephyr.Core.Logs.Default.config");
            log4net.Config.XmlConfigurator.Configure(xml);
        }

        public static void Init(string path)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
        }

        public static void Init(Stream xml)
        {
            log4net.Config.XmlConfigurator.Configure(xml);
        }
        #endregion

        public static void Logger(ILog log, string function, ErrorHandle errorHandleType, Action tryHandle, Action<Exception> catchHandle = null, Action finallyHandle = null)
        {
            try
            {
                log.Debug(function);
                tryHandle();
            }
            catch (Exception ex)
            {
                log.Error(function + "失败", ex);

                if (catchHandle != null)
                    catchHandle(ex);
                
                if (errorHandleType == ErrorHandle.Throw) 
                    throw ex;
            }
            finally
            {
                if (finallyHandle != null)
                    finallyHandle();
            }
        }
    }
}
