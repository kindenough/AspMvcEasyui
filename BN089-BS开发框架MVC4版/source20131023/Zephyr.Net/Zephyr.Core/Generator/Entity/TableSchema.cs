using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Zephyr.Data;

namespace Zephyr.Core.Generator
{
    // 表架构
    public class TableSchema
    {
        //列名
        public string ColumnName { get; set; }

        // sql数据类型
        public string SqlTypeName { get; set; }

        // 类型转换,从数据库转换为C#类型
        public string TypeName
        {
            get { return Util.GetTypeName(SqlTypeName, IsNullable); }
        }

        // 该字段最大长度
        public short MaxLength { get; set; }

        // 摘要
        public string Description { get; set; }

        // 是否可为null
        public bool IsNullable { get; set; }

        // 是否为自动增量
        public bool IsIdentity { get; set; }

        // 是否为主键
        public bool IsPrimaryKey { get; set; }
    }
}
