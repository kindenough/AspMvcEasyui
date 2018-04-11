using System;
using System.Collections;
using System.Drawing;

namespace Zephyr.Utils.Gios.Pdf
{
	/// <summary>
	/// Describes a Flow Text Area for writing text without use Pdf positioning.
	/// </summary>
	public class PdfFlowTextArea : PdfObject
	{
		private double posX,posY;
		private double endX,endY;
		private double startX,startY;
		private double interline;
		internal ArrayList Fonts;
		Font ActualFont;Color ActualColor;
		internal string Stream;
		/// <summary>
		/// gets the Actual X position of the writer.
		/// </summary>
		public double PosX
		{
			get
			{
				return this.posX;
			}
		}
		/// <summary>
		/// gets the Actual Y position of the writer.
		/// </summary>
		public double PosY
		{
			get
			{
				return this.posY;
			}
		}
		/// <summary>
		/// Creates a new Flow Text Area.
		/// </summary>
		/// <param name="TextArea">the PdfArea which will contains the Text.</param>
		/// <param name="Font">the starting Font of the writings.</param>
		/// <param name="Color">the starting Color of the writings.</param>
		public PdfFlowTextArea(PdfArea TextArea,Font Font,Color Color)
		{
			this.Stream="";
			this.posX=TextArea.PosX;
			this.posY=TextArea.PosY;
			this.startX=TextArea.PosX;
			this.startY=TextArea.PosY;
			this.endX=TextArea.BottomRightCornerX;
			this.endY=TextArea.BottomRightCornerY;
			this.interline=Font.Size*1.6;
			this.Fonts=new ArrayList();
			this.SetColor(Color);
			this.SetFont(Font);
		}
		/// <summary>
		/// Creates a new Flow Text Area.
		/// </summary>
		/// <param name="TextArea">the PdfArea which will contains the Text.</param>
		/// <param name="Font">the starting Font of the writings.</param>
		/// <param name="Color">the starting Color of the writings.</param>
		/// <param name="StartX">the starting X position of the writing cursor.</param>
		/// <param name="StartY">the starting Y position of the writing cursor.</param>
		public PdfFlowTextArea(PdfArea TextArea,Font Font,Color Color,double StartX,double StartY)
		{
			this.Stream="";
			this.posX=TextArea.PosX;
			this.posY=TextArea.PosY;
			this.startX=StartX;
			this.startY=StartY;
			this.endX=TextArea.BottomRightCornerX;
			this.endY=TextArea.BottomRightCornerY;
			if (this.startX<this.posX||this.startX>this.endX) throw new Exception("Starting Point cannot be outside the FlowTextArea.");
			if (this.startY<this.posY||this.startY>this.endY) throw new Exception("Starting Point cannot be outside the FlowTextArea.");
			this.Fonts=new ArrayList();
			this.SetColor(Color);
			this.SetFont(Font);
			this.interline=Font.Size*1.6;
		}
		/// <summary>
		/// Creates a new Flow Text Area.
		/// </summary>
		/// <param name="TextArea">the PdfArea which will contains the Text.</param>
		/// <param name="Font">the starting Font of the writings.</param>
		/// <param name="Color">the starting Color of the writings.</param>
		/// <param name="StartX">the starting X position of the writing cursor.</param>
		/// <param name="StartY">the starting Y position of the writing cursor.</param>
		/// <param name="Interline">the Interline Size.</param>
		public PdfFlowTextArea(PdfArea TextArea,Font Font,Color Color,double StartX,double StartY,double Interline)
		{
			this.Stream="";
			this.posX=TextArea.PosX;
			this.posY=TextArea.PosY;
			this.startX=StartX;
			this.startY=StartY;
			this.endX=TextArea.BottomRightCornerX;
			this.endY=TextArea.BottomRightCornerY;
			if (this.startX<this.posX||this.startX>this.endX) throw new Exception("Starting Point cannot be outside the FlowTextArea.");
			if (this.startY<this.posY||this.startY>this.endY) throw new Exception("Starting Point cannot be outside the FlowTextArea.");
			this.Fonts=new ArrayList();
			this.SetColor(Color);
			this.SetFont(Font);
			this.interline=Interline;
		}
		/// <summary>
		/// Creates a new Flow Text Area.
		/// </summary>
		/// <param name="TextArea">the PdfArea which will contains the Text.</param>
		/// <param name="Font">the starting Font of the writings.</param>
		/// <param name="Color">the starting Color of the writings.</param>
		/// <param name="Interline">the Interline Size.</param>
		public PdfFlowTextArea(PdfArea TextArea,Font Font,Color Color,double Interline)
		{
			this.Stream="";
			this.posX=TextArea.PosX;
			this.posY=TextArea.PosY;
			this.startX=TextArea.PosX;
			this.startY=TextArea.PosY;
			this.endX=TextArea.BottomRightCornerX;
			this.endY=TextArea.BottomRightCornerY;
			this.Fonts=new ArrayList();
			this.SetColor(Color);
			this.SetFont(Font);
			this.interline=Interline;
		}
		
		/// <summary>
		/// Writes a string to the FlowTextArea width the current font style, color and size.
		/// </summary>
		/// <param name="text">The string to be written.</param>
		/// <returns>The Substring rejected by the method (text that doesn't fit inside the area.</returns>
		public string Write(string text)
		{
			if (text==null) throw new Exception("Text string cannot be null.");
			return this.Write(text,ActualFont,ActualColor);
		}
		/// <summary>
		/// Writes a string to the FlowTextArea width the current font style, color and size and
		/// applies a carriage return "\n"
		/// </summary>
		/// <param name="text">The line to be written.</param>
		/// <returns>The Substring rejected by the method (text that doesn't fit inside the area.</returns>
		public string WriteLine(string text)
		{
			if (text==null) throw new Exception("Text string cannot be null.");
			return this.Write(text+"\n",ActualFont,ActualColor);
		}
		/// <summary>
		/// Sets the foreground color for the following writings.
		/// </summary>
		/// <param name="Color"></param>
		public void SetColor(Color Color)
		{
			this.Stream+=Utility.ColorrgLine(Color);
			this.ActualColor=Color;
		}
		/// <summary>
		/// Sets the font style for the following writings.
		/// </summary>
		/// <param name="Font"></param>
		public void SetFont(Font Font)
		{
			if (this.ActualFont==null) this.ActualFont=Font;
			Font f=new Font(Font.Name,ActualFont.Size,Font.Style);
			this.ActualFont=Font;
			this.Fonts.Add(Font);
			this.Stream+=Utility.FontToFontLine(Font);
		}
		private string Write(string text,Font Font,Color Color)
		{
			char[] textchars=text.ToCharArray();

			ArrayList words=new ArrayList();
			string aWord="";
			for (int index=0;index<textchars.Length;index++)
			{
				char c=textchars[index];
				switch (c)
				{
					case ' ':
					{
						if (aWord!="") words.Add(aWord); 
						words.Add(" "); aWord=""; break;
					}
					case '\n': words.Add(aWord); words.Add("\n"); aWord=""; break;
					default: aWord+=c; break;
				}
			}
			if (aWord!="") words.Add(aWord);
			words.Add("");
			double startLine=this.posX;
			string oldLine="",newLine="";
			double WordWidth=0,newLineWidth=0,oldLineWidth=0;
			for (int wordIndex=0;wordIndex<words.Count;wordIndex++)
			{
				string word=words[wordIndex] as string;
				
				WordWidth=Utility.Measure(Font,word);
				if (Font.Name!="Courier New") WordWidth=WordWidth*1.008;
				oldLineWidth=newLineWidth;
				newLineWidth+=WordWidth;
				oldLine=newLine;
				newLine+=word;
				
				if (word=="\n")
				{
					this.Stream+=LineToStream(Color,Font,oldLine,startLine,this.posY);
					newLineWidth=0;
					startLine=this.startX;
					newLine="";
					posX=this.startX;
					this.posY+=this.interline;
				}
				else
				if (startLine+newLineWidth>this.endX)
				{
					this.Stream+=LineToStream(Color,Font,oldLine,startLine,this.posY);
					this.posY+=this.interline;
					newLineWidth=WordWidth;
					startLine=this.startX;
					if (word!=" ") newLine=word; else newLine="";
					posX=this.startX;
				}
				
				if (wordIndex==words.Count-1)
				{
					this.Stream+=LineToStream(Color,Font,newLine,startLine,this.posY);
					this.posX=startLine+newLineWidth;
				}
				
				if (this.posY+this.interline>this.endY)
				{
					string s="";
					for (int wordIndex2=wordIndex+1;wordIndex2<words.Count;wordIndex2++)
					{
						s+=words[wordIndex2];
					}
					return newLine+s;
				}
				
			}
		return "";
				
		}
		internal override int StreamWrite(System.IO.Stream stream)
		{
			Byte[] part2;
				
			if (PdfDocument.FlateCompression) part2=Utility.Deflate("BT\n"+this.Stream+"ET\n"); else
				part2=System.Text.ASCIIEncoding.ASCII.GetBytes("BT\n"+this.Stream+"ET\n");

			string s1="";
			s1+=this.id.ToString()+" 0 obj\n";
			s1+="<< /Lenght "+part2.Length;
			if (PdfDocument.FlateCompression) s1+=" /Filter /FlateDecode";
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
		private string LineToStream(Color Color,Font Font,string Line,double PosX,double PosY)
		{
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			
			sb.Append("1 0 0 1 ");
			sb.Append(PosX.ToString("0.##").Replace(",","."));
			sb.Append(" ");
			sb.Append((this.PdfDocument.PH-PosY-(Font.Height*0.525)).ToString("0.##").Replace(",","."));
			sb.Append(" Tm (");
			sb.Append(Utility.TextEncode(Utility.TextEncode(Line)));
			sb.Append(") Tj\n");
			return sb.ToString();
		}
	}
}
