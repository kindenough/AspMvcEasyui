using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Zephyr.Data;

namespace Zephyr.Core.Generator
{
    //接口
    public interface ISqlGen
    {
        string SqlGetTableNames();                          //取得表名
        string SqlGetTableSchemas(string tableName);        //取得表结构
        string SqlGetTableKeys(string tableName);           //sql server 调用sp_pkeys取得主key
    }
}
