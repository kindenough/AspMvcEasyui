namespace Zephyr.Data
{
	public interface IDeleteBuilder : IExecute
	{
		BuilderData Data { get; }
		IDeleteBuilder Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        //add by liuhuisheng
        IDeleteBuilder Where(string sql);
    }
}