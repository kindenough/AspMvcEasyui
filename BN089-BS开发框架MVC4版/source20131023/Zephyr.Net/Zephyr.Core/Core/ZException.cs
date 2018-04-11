/*************************************************************************
 * 文件名称 ：ZException.cs                          
 * 描述说明 ：框架错误定义
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;

namespace Zephyr.Core
{
	public class ZException : Exception
	{
		public ZException(string message): base(message){}
        public ZException(string message, Exception innerException) : base(message, innerException) { }
	}
}
