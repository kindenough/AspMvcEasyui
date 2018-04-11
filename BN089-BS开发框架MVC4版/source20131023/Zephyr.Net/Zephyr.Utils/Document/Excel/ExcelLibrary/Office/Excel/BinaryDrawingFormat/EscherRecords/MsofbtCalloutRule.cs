using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zephyr.Utils.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtCalloutRule : EscherRecord
	{
		public MsofbtCalloutRule(EscherRecord record) : base(record) { }

		public MsofbtCalloutRule()
		{
			this.Type = EscherRecordType.MsofbtCalloutRule;
		}

	}
}
