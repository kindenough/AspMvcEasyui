using System;
using System.Data;

namespace Zephyr.Data
{
	public class ConnectionEventArgs : EventArgs
	{
		public IDbConnection Connection { get; private set; }

		public ConnectionEventArgs(System.Data.IDbConnection connection)
		{
			Connection = connection;
		}
	}
}
