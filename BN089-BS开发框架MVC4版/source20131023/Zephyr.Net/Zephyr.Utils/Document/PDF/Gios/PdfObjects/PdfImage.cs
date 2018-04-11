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
using System.IO;
using System.Text;

namespace Zephyr.Utils.Gios.Pdf
{
	/// <summary>
	/// a 72dpi jpeg based Image for a PdfPage
	/// </summary>
	public class PdfImage : PdfObject
	{
		internal MemoryStream ImageStream;
		string file;
		internal Bitmap bmp;
		/// <summary>
		/// gets the height of the loaded picture
		/// </summary>
		public int Height
		{
			get
			{
				return bmp.Height;
			}
		}
		/// <summary>
		/// gets the width of the loaded picture
		/// </summary>
		public int Width
		{
			get
			{
				return bmp.Width;
			}
		}
		
		internal PdfImage(int id,string file)
		{
			this.id=id;
			this.file=file;
			ImageStream=new MemoryStream();
			try
			{
				this.bmp=new Bitmap(file);
			}
			catch
			{
				throw new Exception("Error Loading Image File");
			}
		}
		
		internal int StreamGifWrite(System.IO.Stream stream)
		{
			FileStream fs;
			try
			{
				fs = File.OpenRead(this.file);
			}
			catch {throw new Exception("Can't open image file");}
			byte[] data = new byte[fs.Length];
				
			System.Drawing.Image i=Image.FromFile(file);
				
			string text1="";
			text1+=this.HeadObj;
			text1+="<</Type/XObject\n/Subtype/Image\n";
			text1+="/Width "+bmp.Width.ToString()+"\n/Height "+bmp.Height.ToString()+"\n";
			text1+="/BitsPerComponent 1\n";
			text1+="/Name/I"+this.ID+"\n";
			text1+="/ColorSpace/Indexed\n";
			//text1+="/Filter[/LZWDecode]\n";
			text1+="/Length "+data.Length.ToString()+"\n";
				
			text1+=">>\nstream\n";
			string text3="";
			text3+="\nendstream\n";
			text3+="endobj\n";

			Byte[] part1=ASCIIEncoding.ASCII.GetBytes(text1);
			fs.Read (data, 0, data.Length);

			Byte[] part3=ASCIIEncoding.ASCII.GetBytes(text3);
				
			stream.Write(part1,0,part1.Length);
			stream.Write(data,0,data.Length);
			stream.Write(part3,0,part3.Length);
			fs.Close();
			return part1.Length+data.Length+part3.Length;
		}
		internal override int StreamWrite(System.IO.Stream stream)
		{
			if (this.file.ToLower().EndsWith(".gif"))
			{
				return this.StreamGifWrite(stream);
			}
			
			FileStream fs;
			try
			{
				fs = File.OpenRead(this.file);
			}
			catch {throw new Exception("Can't open image file");}
			byte[] data = new byte[fs.Length];
				
			System.Drawing.Image i=Image.FromFile(file);
				
			//i.Save(this.ImageStream,System.Drawing.Imaging.ImageFormat.Jpeg);
			//((data=ImageStream.ToArray();
			string text1="";
			text1+=this.HeadObj;
			//text1+="<< /Length "+ImageStream.Length.ToString()+" /Filter /FlateDecode>>\n";
			text1+="<</Type/XObject\n/Subtype/Image\n";
			text1+="/Width "+bmp.Width.ToString()+"\n/Height "+bmp.Height.ToString()+"\n";
			text1+="/BitsPerComponent 8\n";
			text1+="/Name/I"+this.ID+"\n";
			text1+="/ColorSpace/DeviceRGB\n";
			//text1+="/Filter /FlateDecode\n";
			//text1+="/Resolution ["+bmp.HorizontalResolution.ToString("0.##").Replace(",",".");
			//text1+=" "+bmp.VerticalResolution.ToString("0.##").Replace(",",".")+"]\n";
			text1+="/Filter[/DCTDecode]\n";
			//text1+="/DecodeParms[<<>>]\n";
			text1+="/Length "+data.Length.ToString()+"\n";
				
			text1+=">>\nstream\n";
			string text3="";
			text3+="\nendstream\n";
			text3+="endobj\n";

			Byte[] part1=ASCIIEncoding.ASCII.GetBytes(text1);
			//Byte[] part2=sr.BaseStream.
			fs.Read (data, 0, data.Length);

			//Byte[] part2=this.ImageStream.ToArray();
			Byte[] part3=ASCIIEncoding.ASCII.GetBytes(text3);
				
			stream.Write(part1,0,part1.Length);
			stream.Write(data,0,data.Length);
			stream.Write(part3,0,part3.Length);
			fs.Close();
			return part1.Length+data.Length+part3.Length;
		}

	}
}
