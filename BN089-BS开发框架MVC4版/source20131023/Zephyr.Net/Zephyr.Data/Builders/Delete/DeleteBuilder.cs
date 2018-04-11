namespace Zephyr.Data
{
	internal class DeleteBuilder : BaseDeleteBuilder, IDeleteBuilder
	{
		public DeleteBuilder(IDbCommand command, string tableName)
			: base(command, tableName)
		{
		}

		public IDeleteBuilder Where(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(columnName, value, parameterType, size);
			return this;
		}

        //add by liuhuisheng
        public IDeleteBuilder Where(string sql)
        {
            Actions.WhereAction(sql);
            return this;
        }
	}
}
