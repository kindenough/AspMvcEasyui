//============================================================================
//Gios Pdf.NET - A library for exporting Pdf Documents in C#
//Copyright (C) 2005  Paolo Gios - www.paologios.com
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================
using System;
using System.Drawing;
using System.Collections;

namespace Zephyr.Utils.Gios.Pdf
{
	/// <summary>
	/// a Column of a PdfTable
	/// </summary>
	public class PdfColumn : PdfCellRange
	{
		internal int index;
		internal double Width;
		private double compansatedWidth=-1;

		internal double CompensatedWidth
		{
			get
			{
				if (this.compansatedWidth==-1)
				{
					double sum=0;
					foreach (PdfColumn pc in this.owner.Columns)
					{
						sum+=pc.Width;
					}
					this.compansatedWidth=(this.owner.TableArea.width/sum)*this.Width;
				}
				return this.compansatedWidth;
			}
			set
			{
				this.compansatedWidth=value;
			}
		}
		internal PdfColumn(PdfTable owner,int index)
		{
			this.owner=owner;
			this.index=index;
			this.startColumn=index;
			this.endColumn=index;
			this.startRow=0;
			this.endRow=this.owner.rows-1;
		}
		/// <summary>
		/// sets the Relative Width of the Column. For example: if the relative widths of a 3 columns
		/// table are 10,10,30; the columns will respectivelly sized as 20%,20%,60% of the table size.
		/// </summary>
		/// <param name="RelativeWidth"></param>
		public void SetWidth(int RelativeWidth)
		{
			if (RelativeWidth<=0) throw new Exception("RelativeWidth must be grater than zero.");
			this.Width=RelativeWidth;
			if (this.owner.header!=null) this.owner.header.Columns[this.index].SetWidth(RelativeWidth);
		}
		
	}
}
