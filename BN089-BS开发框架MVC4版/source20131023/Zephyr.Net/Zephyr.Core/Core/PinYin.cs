/*************************************************************************
 * 文件名称 ：PinYin.cs                          
 * 描述说明 ：支持中文拼音首字母查询 for sql server only
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System.Collections.Generic;
using System.Configuration;

namespace Zephyr.Core
{
    //for sql server only
    public class PinYin
    {
        private static Dictionary<string, bool> IsValid = new Dictionary<string, bool>();

        public static void GenaratePinYinFunc()
        {
            try
            {
                var settins = ConfigurationManager.ConnectionStrings;
                for (var i = 0; i < settins.Count; i++)
                   if (settins[i].ProviderName=="SqlServer") IsSupportPinYin(settins[i].Name);
            }
            catch
            {
                //go on
            }

        }

        public static bool IsSupportPinYin(string module)
        {
            if (!IsValid.ContainsKey(module))
            {
                using (var db = Db.Context(module))
                {
                    //去数据库判断是否存在函数fun_getPY
                    var list = db.Sql(GetJudgeSQL()).QueryMany<dynamic>();
                    if (list.Count == 0) //创建函数fun_getPY
                        db.Sql(GetFnPinYinSQL()).Execute();

                    //再判断是否创建成功fun_getPY
                    list = db.Sql(GetJudgeSQL()).QueryMany<dynamic>();

                    //保存结果
                    IsValid[module] = list.Count > 0;
                }
            }
            return IsValid[module];
        }

        private static string GetJudgeSQL()
        {
            const string sql = @"select name from dbo.sysobjects where id = object_id(N'[dbo].[fun_getPY]') and xtype in (N'FN', N'IF', N'TF')";
            return sql;
        }

        private static string GetFnPinYinSQL()
        {
            var sql = @"
--set ANSI_NULLS ON
--set QUOTED_IDENTIFIER ON
 
-- =============================================
-- Description:	提供中文首字母
-- Demo: select * from 表 where fun_getPY(字段) like N'%zgr%'
-- =============================================
CREATE FUNCTION [dbo].[fun_getPY]
(
	@str NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
AS
BEGIN
	DECLARE @word NCHAR(1),@PY NVARCHAR(4000)
	SET @PY=''
	WHILE len(@str)>0
	BEGIN
		SET @word=left(@str,1)
		SET @PY=@PY+(CASE WHEN unicode(@word) BETWEEN 19968 AND 19968+20901
		THEN (SELECT TOP 1 PY FROM (
		SELECT 'A' AS PY,N'驁' AS word
		UNION ALL SELECT 'B',N'簿'
		UNION ALL SELECT 'C',N'錯'
		UNION ALL SELECT 'D',N'鵽'
		UNION ALL SELECT 'E',N'樲'
		UNION ALL SELECT 'F',N'鰒'
		UNION ALL SELECT 'G',N'腂'
		UNION ALL SELECT 'H',N'夻'
		UNION ALL SELECT 'J',N'攈'
		UNION ALL SELECT 'K',N'穒'
		UNION ALL SELECT 'L',N'鱳'
		UNION ALL SELECT 'M',N'旀'
		UNION ALL SELECT 'N',N'桛'
		UNION ALL SELECT 'O',N'漚'
		UNION ALL SELECT 'P',N'曝'
		UNION ALL SELECT 'Q',N'囕'
		UNION ALL SELECT 'R',N'鶸'
		UNION ALL SELECT 'S',N'蜶'
		UNION ALL SELECT 'T',N'籜'
		UNION ALL SELECT 'W',N'鶩'
		UNION ALL SELECT 'X',N'鑂'
		UNION ALL SELECT 'Y',N'韻'
		UNION ALL SELECT 'Z',N'咗'
		) T 
		WHERE word>=@word COLLATE Chinese_PRC_CS_AS_KS_WS 
		ORDER BY PY ASC) ELSE @word END)
		SET @str=right(@str,len(@str)-1)
	END
	RETURN @PY
END";
            return sql;
        }
    }
}
