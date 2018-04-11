namespace Zephyr.Data.Providers.Common.Builders
{
	internal class DeleteBuilderSqlGenerator
	{
		public string GenerateSql(IDbProvider provider, string parameterPrefix, BuilderData data)
		{
            //mod by liuhuisheng start
            //var whereSql = "";
            var whereSql = data.WhereSql;
            //mod by liuhuisheng end
			foreach (var column in data.Columns)
			{
				if (whereSql.Length > 0)
					whereSql += " and ";

				whereSql += string.Format("{0} = {1}{2}",
												provider.EscapeColumnName(column.ColumnName),
												parameterPrefix,
												column.ParameterName);
			}

			var sql = string.Format("delete from {0} where {1}", data.ObjectName, whereSql);
			return sql;
		}
	}
}
