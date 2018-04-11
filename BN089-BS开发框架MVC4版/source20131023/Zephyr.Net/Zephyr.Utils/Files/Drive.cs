using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Zephyr.Utils
{
    public partial class ZFiles
    {
        #region 获取本地驱动器名列表
        /// <summary>
        /// 获取本地驱动器名列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetLocalDrives()
        {
            return Directory.GetLogicalDrives();
        }
        #endregion
    }
}
