using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Zephyr.Data;

namespace Zephyr.WorkFlow
{
//    第一部分使用一组服务实现配置jbpm上下文，这些配置的可选项在以后描述特定服务实现的章节中做了描述。
//    第二部分是所有配置资源的引用映射，如果你想要定制某些配置文件，这些资源引用可以被修改。
//    第三部分是在jbpm中使用的一些别名配置，这些配置选项在包含特定主题的章节中做了描述。
//    缺省配置的一组服务被定位于一个简单的web应用环境和最小的依赖，持久化服务将获得一个连接，所有其他服务将会使用这个相同的连接来完成它们的服务，因此，工作流的所有操作都被集中到一个连接的一个事务中，不再需要事务管理器。

    //设置bpm的一些基础配置 如数据库.....
    public class BpmConfiguration
    {
        private string _xml;

        public NameValueCollection AppConfig = new NameValueCollection();

        public static BpmConfiguration CreateInstance()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var xml = assembly.GetManifestResourceStream("Zephyr.WorkFlow.Engine.BpmConfiguration.xml");
            return new BpmConfiguration(xml);
        }

        public BpmConfiguration(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                _xml = streamReader.ReadToEnd();

                var configuration = XElement.Parse(_xml);
                var connectionStrings = configuration.Element("connectionStrings").Element("add");
                AppConfig["providerName"] = connectionStrings.Attribute("providerName").Value;
                AppConfig["connectionString"] = connectionStrings.Attribute("connectionString").Value;
            }
        }
        
        public BpmConfiguration(string path)
            : this(new FileStream(path, FileMode.Open) as Stream)
        {

        }

        

        public BpmContext CreateBpmContext() 
        {
            return new BpmContext(this);
        }

        public IDbContext CreateDbContext() 
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

            var connectionString = AppConfig["connectionString"];
            var providerName = AppConfig["providerName"];
            var provider = providers[providerName] != null ? providers[providerName] : new SqlServerProvider();
            var dbcontext = new DbContext().ConnectionString(connectionString, provider);
            return dbcontext;
        }

        
    }
}
