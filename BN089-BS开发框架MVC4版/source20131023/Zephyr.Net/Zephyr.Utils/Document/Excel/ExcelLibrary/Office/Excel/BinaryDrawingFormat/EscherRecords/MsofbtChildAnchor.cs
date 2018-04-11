using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Zephyr.Utils.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtChildAnchor : EscherRecord
	{
		public MsofbtChildAnchor(EscherRecord record) : base(record) { }

		public MsofbtChildAnchor()
		{
			this.Type = EscherRecordType.MsofbtChildAnchor;
		}

	}
}
