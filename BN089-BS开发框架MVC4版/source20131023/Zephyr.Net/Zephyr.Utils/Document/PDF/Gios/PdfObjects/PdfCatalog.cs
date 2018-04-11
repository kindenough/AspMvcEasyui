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
	internal class PdfCatalog : PdfObject
	{
		public PdfCatalog(PdfDocument PdfDocument)
		{
			this.PdfDocument=PdfDocument;
			this.id=this.PdfDocument.GetNextId;
		}
		private string FirstKid
		{
			get
			{
				for (int x=1;x<this.PdfDocument._nextid;x++)
				{
					object o=this.PdfDocument.PdfObjects[x.ToString()+" 0 obj\n"];
					if (o!=null)
						if (o.GetType()==typeof(PdfPage)) return((PdfObject)o).HeadR;
				}
				return null;
			}
		}
		internal override int StreamWrite(System.IO.Stream stream)
		{
			string s="";
			s+=this.HeadObj;
			s+="<<\n";
			s+="/Type /Catalog\n";
			s+="/Pages "+this.PdfDocument.PdfRoot.HeadR+"\n";
			s+="/OpenAction["+this.FirstKid+"/Fit]\n";
			s+=">>\n";
			s+="endobj\n";
			Byte[] b=ASCIIEncoding.ASCII.GetBytes(s);
			stream.Write(b,0,b.Length);
			return b.Length;
		}

	}
}
