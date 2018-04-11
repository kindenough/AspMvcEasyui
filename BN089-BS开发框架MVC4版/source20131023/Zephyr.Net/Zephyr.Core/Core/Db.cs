/*************************************************************************
 * 文件名称 ：Db.cs                          
 * 描述说明 ：定义数据库连接
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Zephyr.Data;

namespace Zephyr.Core
{
    public class Db
    {
        public static IDbContext Context()
        {
            return Context(APP.DB_DEFAULT_CONN_NAME);
        }
      
        public static IDbProvider Provider(string providerName) 
        {
            var providers = new Dictionary<string, IDbProvider>(){
                {"DB2",new DB2Provider()},
                {"MySql",new MySqlProvider()},
                {"Oracle",new OracleProvider()},
                {"PostgreSql",new PostgreSqlProvider()},
                {"SqlAzure",new SqlAzureProvider()},
                {"Sqlite",new SqliteProvider()},
                {"SqlServerCompact",new SqlServerCompactProvider()},
                {"SqlServer",new SqlServerProvider()}
            };
            return providers[providerName] != null ? providers[providerName] : new SqlServerProvider();
        }
        

        public static IDbContext Context(string ConnName)
        {
            var Log = LogManager.GetLogger(ConnName);
            var setting = ConfigurationManager.ConnectionStrings[ConnName];
            var db = new DbContext().ConnectionString(setting.ConnectionString, Provider(setting.ProviderName));
           
            db.OnExecuting(x =>
            {
                if (APP.OnDbExecuting != null) APP.OnDbExecuting(x);
                var sql = x.Command.CommandText;
                for (int i = x.Command.Parameters.Count - 1;i>=0;i--)
                {
                    var item = x.Command.Parameters[i] as IDataParameter;
                    sql = sql.Replace(item.ParameterName, String.Format("'{0}'", item.Value));
                }
 
                Log.Debug(sql);
            });

            db.OnError(e =>
            {
                var ex = e.Exception as SqlException;
                if (ex != null)
                {
                    var error = "";
                    switch (ex.Number)
                    {
                        case -2:
                            error = "超时时间已到。在操作完成之前超时时间已过或服务器未响应";
                            break;
                        case 4060:
                            // Invalid Database
                            error = "数据库不可用,请检查系统设置后重试！";
                            break;
                        case 18456:
                            // Login Failed
                            error = "登陆数据库失败！";
                            break;
                        case 547:
                            // ForeignKey Violation
                            error = "数据已经被引用，更新失败，请先删除引用数据并重试！";
                            break;
                        case 2627:
                            // Unique Index/Constriant Violation
                            error = "主键重复，更新失败！";
                            break;
                        case 2601:
                            // Unique Index/Constriant Violation   
                            break;
                        default:
                            // throw a general DAL Exception   
                            break;
                    }
                    if (!string.IsNullOrEmpty(error))
                        throw new Exception(error);
                }
            });

            return db;
        }
    }
}
