using System;
using System.Collections;
using System.Drawing;

namespace Smart.Pdf
{
	/*
	public class PdfTextBlock : PdfObject
	{
		public PdfTextBlock(double startx,double starty)
		{this.textes=new ArrayList();}
		public ArrayList textes;
		public void Write(Font Font,Color Color, string Text)
		{
			PdfArea pa=new PdfArea();

			if (this.textes.Count==0)
			{
				pa.posx=0;
				pa.posy=0;
			}				
			else
			{
				pa.posx=((PdfText)textes[textes.Count-1]).area.TopRightVertex.X;
				pa.posy=((PdfText)textes[textes.Count-1]).area.TopRightVertex.Y;
			}

			pa.width=Utility.NeededArea(Font,Text).width;
			pa.height=Utility.NeededArea(Font,Text).height;

			this.textes.Add(new PdfText(Font,Color,Text,pa));
		}
		internal string ToLineStream()
		{
			string text="";
			text+="BT\n";
			foreach (PdfText pt in this.textes)
			{
				text+=Utility.FontToFontLine(pt.Font);
				text+=Utility.ColorrgLine(pt.Color);
				text+="1 0 0 1 "+pt.area.posx.ToString("0.##").Replace(",",".");
				text+=" "+(Settings.PH-(pt.Font.Height*0.465)).ToString("0.##").Replace(",",".");
				text+=" Tm ("+Utility.TextEncode(pt.Text)+") Tj\n";
			}
			text+="ET\n";
			return text;
		}
		
		
		internal override int StreamWrite(System.IO.Stream stream)
		{
			int num=this.id;
			string text=this.ToLineStream();
			Byte[] part2;
			if (Settings.FlateCompression) part2=Utility.Deflate(text); else
				part2=System.Text.ASCIIEncoding.ASCII.GetBytes(text);

			string s1="";
			s1+=num.ToString()+" 0 obj\n";
			s1+="<< /Lenght "+part2.Length;
			if (Settings.FlateCompression) s1+=" /Filter /FlateDecode";
			s1+=">>\n";
			s1+="stream\n";

			string s3="\nendstream\n";
			s3+="endobj\n";
				
			Byte[] part1=System.Text.ASCIIEncoding.ASCII.GetBytes(s1);
			Byte[] part3=System.Text.ASCIIEncoding.ASCII.GetBytes(s3);
				
				
			stream.Write(part1,0,part1.Length);
			stream.Write(part2,0,part2.Length);
			stream.Write(part3,0,part3.Length);

			return part1.Length+part2.Length+part3.Length;
		}
	}
	public class PdfText
	{
		internal Font Font;
		internal Color Color;
		internal string Text;
		public PdfArea area;
		internal PdfText(Font Font,Color Color, string Text,PdfArea area)
		{
			this.Color=Color;
			this.Font=Font;
			this.Text=Text;
			this.area=area;
		}
	}
*/
}
