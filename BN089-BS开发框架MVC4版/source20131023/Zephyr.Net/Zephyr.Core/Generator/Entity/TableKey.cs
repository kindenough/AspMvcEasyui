using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Zephyr.Data;

namespace Zephyr.Core.Generator
{
    public class TableKey
    {
        public string table_qualifier { get; set; }
        public string table_owner { get; set; }
        public string table_name { get; set; }
        public string column_name { get; set; }
        public int key_seq { get; set; }
        public string pk_name { get; set; }
    }
}
