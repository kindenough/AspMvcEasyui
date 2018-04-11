using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Zephyr.Data;

namespace Zephyr.Core.Generator
{
    //表结构
    public class Table
    {
        public string TableName { get; set; }

        public List<string> TableKeys { get; set; }

        public List<TableSchema> TableSchemas { get; set; }

        public string Identity
        {
            get{return Util.GetIdentity(TableSchemas);}
        }
    }
}
