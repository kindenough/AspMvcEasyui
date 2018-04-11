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
	/// the abstract pdf object class
	/// </summary>
	public abstract class PdfObject
	{
		internal PdfObject()
		{
			
		}
		internal PdfObject(Byte[] buffer,int id)
		{
			this.buffer=buffer;
			this.id=id;
		}
		
		#region properties
		internal PdfDocument PdfDocument;
		/// <summary>
		/// 
		/// </summary>
		protected int id;
		internal int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id=value;
			}
		}
		private Byte[] buffer;
		/// <summary>
		/// 
		/// </summary>
		protected string type;
		internal string Type
		{
			get
			{
				return type;
			}
		}
		internal string HeadR
		{
			get
			{
				return this.id.ToString()+" 0 R ";
			}
		}
		internal string HeadObj
		{
			get
			{
				return this.id.ToString()+" 0 obj\n";
			}
		}
		
		#endregion
		internal virtual int StreamWrite(System.IO.Stream stream){return 0;}
		internal PdfObject Clone()
		{
			return this.MemberwiseClone() as PdfObject;
		}
	}
}
