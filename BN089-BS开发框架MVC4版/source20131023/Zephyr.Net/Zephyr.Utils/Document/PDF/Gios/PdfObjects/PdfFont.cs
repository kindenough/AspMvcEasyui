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
using System.Text;

namespace Zephyr.Utils.Gios.Pdf
{
	internal class PdfFont : PdfObject
	{
		private string name,typename;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		internal PdfFont(int id,string name,string typename)
		{
			this.name=name;
			this.typename=typename;
			this.id=id;
		}
		internal PdfFont(string name,string typename)
		{
			this.name=name;
			this.typename=typename;
		}
		internal static string FontToPdfType(System.Drawing.Font f)
		{
			string name="";
			switch (f.Name)
			{
				case "Times New Roman":
					if (f.Bold) name="Times-Bold"; else name="Times-Roman";
					break;
				case "Courier New":
					if (f.Bold) name="Courier-Bold"; else name="Courier";
					break;
				default:
					if (f.Bold) name="Helvetica-Bold"; else name="Helvetica";
					break;
			}
			return name;
		}
		
		internal override int StreamWrite(System.IO.Stream stream)
		{
			string text="";
			text+="/Type /Font\n";
			text+="/Subtype /Type1\n/";
			text+="Name /"+name+"\n";
			text+="/BaseFont /"+typename+"\n";
			text+="/Encoding /WinAnsiEncoding\n";
			
			string s="";
			s+=this.HeadObj;
			s+="<<\n";
			s+="/Type /Font\n/";
			s+="Subtype /Type1\n/";
			s+="Name /"+name+"\n";
			s+="/BaseFont /"+typename+"\n";
			s+="/Encoding /WinAnsiEncoding\n";
			s+=">>\n";
			s+="endobj\n";
			Byte[] b=ASCIIEncoding.ASCII.GetBytes(s);
			stream.Write(b,0,b.Length);
			return b.Length;
		}

	}
	
}
