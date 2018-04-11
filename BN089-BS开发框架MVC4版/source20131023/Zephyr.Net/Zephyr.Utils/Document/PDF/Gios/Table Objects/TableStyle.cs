using System;
using System.Drawing;

namespace Smart.Pdf
{
	public class TableStyle
	{
		internal Color alternateBackgroundColor,backgroundColor,foregroundColor
			,headerForegroundColor,headerBackgroundColor;
		internal bool visibleHeaders;

		internal TableStyle()
		{
			this.backgroundColor=Color.White;
			this.alternateBackgroundColor=Color.White;
			this.headerBackgroundColor=Color.White;
			this.headerForegroundColor=Color.Black;
			this.foregroundColor=Color.Black;
			this.visibleHeaders=true;
		}
		public static TableStyle Professional
		{
			get
			{
				TableStyle ts=new TableStyle();
				ts.headerBackgroundColor=Color.White;
				ts.headerForegroundColor=Color.Black;
				ts.foregroundColor=Color.Black;
				ts.alternateBackgroundColor=Color.White;
				ts.backgroundColor=Color.White;
				return ts;
			}
		}
		public static TableStyle WindowsClassic
		{
			get
			{
				TableStyle ts=new TableStyle();
				ts.headerBackgroundColor=Color.Navy;
				ts.headerForegroundColor=Color.White;
				ts.foregroundColor=Color.Black;
				ts.alternateBackgroundColor=Color.Gainsboro;
				ts.backgroundColor=Color.White;
				return ts;
			}
		}
	}
}
