/*************************************************************************
 * 文件名称 ：NoneCompress.cs                          
 * 描述说明 ：空压缩接口实现
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System.IO;

namespace Zephyr.Core
{
    public class NoneCompress: ICompress
    {
        public string Suffix(string orgSuffix)
        {
            return orgSuffix;
        }

        public Stream Compress(Stream fileStream,string fullName)
        {
            return fileStream;
        }
    }
}
