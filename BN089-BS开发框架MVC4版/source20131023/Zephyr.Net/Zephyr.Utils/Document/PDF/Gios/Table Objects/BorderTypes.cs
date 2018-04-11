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

namespace Zephyr.Utils.Gios.Pdf
{
	/// <summary>
	/// describes a kind of border for a PdfTable
	/// </summary>
	public enum BorderType
	{
		/// <summary>
		/// table without borders
		/// </summary>
		None=0,
		/// <summary>
		/// table boreder on the bounds (just the encloding rectangle)
		/// </summary>
Bounds=1,
		/// <summary>
		/// only the row (horizontal) lines
		/// </summary>
Rows=2,
		/// <summary>
		/// table boreder on the bounds with horizontal lines
		/// </summary>
RowsAndBounds=3,
		/// <summary>
		/// only the column (vertical) lines
		/// </summary>
Columns=4,
		/// <summary>
		/// table boreder on the bounds with vertical lines
		/// </summary>
ColumnsAndBounds=5,
		/// <summary>
		/// table completely bordered (rectangle, rows and lines)
		/// </summary>
CompleteGrid=6,
		/// <summary>
		/// table with horizontal and vertical lines without the encloding rectangle
		/// </summary>
		RowsAndColumns=7
	}
}
