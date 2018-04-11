using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zephyr.Utils.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtConnectorRule : EscherRecord
	{
		public MsofbtConnectorRule(EscherRecord record) : base(record) { }

		public MsofbtConnectorRule()
		{
			this.Type = EscherRecordType.MsofbtConnectorRule;
		}

	}
}
