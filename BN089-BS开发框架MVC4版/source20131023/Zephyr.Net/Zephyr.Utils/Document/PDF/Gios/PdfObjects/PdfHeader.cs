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
	internal class PdfHeader : PdfObject
	{
		private string subject,title,author,creationdate;
		internal PdfHeader(PdfDocument PdfDocument,string subject,string title,string author)
		{
			this.PdfDocument=PdfDocument;
			this.id=this.PdfDocument.GetNextId;
			this.subject=subject;
			this.title=title;
			this.author=author;
			this.creationdate=DateTime.Today.ToShortDateString();
		}
		internal override int StreamWrite(System.IO.Stream stream)
		{
			string s="";
			s+=this.HeadObj;
			s+="<<\n";
			s+="/Subject ("+subject+")\n/Title ("+title+")\n/Creator (Gios Pdf.NET Library)\n";
			s+="/Producer(Paolo Gios - http://www.paologios.com)\n";
			s+="/Author ("+author+")\n/CreationDate ("+creationdate+")\n";
			s+=">>\n";
			s+="endobj\n";
			Byte[] b=ASCIIEncoding.ASCII.GetBytes(s);
			stream.Write(b,0,b.Length);
			return b.Length;
		}

	}
}