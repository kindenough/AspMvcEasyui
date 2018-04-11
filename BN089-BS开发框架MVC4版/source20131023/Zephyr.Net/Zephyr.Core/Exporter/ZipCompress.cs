/*************************************************************************
 * 文件名称 ：ZipCompress.cs                          
 * 描述说明 ：压缩为ZIP
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System.IO;
using Zephyr.Utils.Ionic.Zip;

namespace Zephyr.Core
{
    public class ZipCompress: ICompress
    {
        public string Suffix(string orgSuffix)
        {
            return "zip";
        }

        public Stream Compress(Stream fileStream,string fullName)
        {
            using (var zip = new ZipFile())
            {
                zip.AddEntry(fullName, fileStream);
                Stream zipStream = new MemoryStream();
                zip.Save(zipStream);
                return zipStream;
            }
        }
    }
}
