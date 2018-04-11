using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Zephyr.Data;

namespace Zephyr.Core.Generator
{
    //取得表的方法
    public class GenTables
    {
        public static List<Table> GetTables(string dbType,string conString)
        {
            var gen = GetGenProvider(dbType);
            var provider = GetProvider(dbType);

            using (var db = new DbContext().ConnectionString(conString, provider))
            {
                var Tables = db.Sql(gen.SqlGetTableNames()).QueryMany<Table>();

                foreach (var T in Tables)
                {
                    T.TableKeys = db.Sql(gen.SqlGetTableKeys(T.TableName)).QueryMany<TableKey>().Select<TableKey, string>(x => x.column_name).ToList<string>();
                    T.TableSchemas = db.Sql(gen.SqlGetTableSchemas(T.TableName)).QueryMany<TableSchema>();
                }
                return Tables;
            }
        }

        public static ISqlGen GetGenProvider(string type)
        {
            switch (type)
            {
                case "Oracle":
                    return new OracleGen();

                case "SqlServer":
                default:
                    return new SqlServerGen();
            }
        }


        private static IDbProvider GetProvider(string type)
        {
            switch (type)
            {
                case "Oracle":
                    return new OracleProvider();

                case "SqlServer":
                default:
                    return new SqlServerProvider();
            }
        }

    }
}
