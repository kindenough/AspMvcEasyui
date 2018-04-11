using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zephyr.Utils.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtOPT : EscherRecord
	{
		public MsofbtOPT(EscherRecord record) : base(record) { }

		public MsofbtOPT()
		{
			this.Type = EscherRecordType.MsofbtOPT;
		}

	}
}
