/*************************************************************************
 * 文件名称 ：Attribute.cs                          
 * 描述说明 ：定义属性
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Concurrent;

namespace Zephyr.Core
{
    #region AttributeHelper
    public class AttributeHelper
    {
        private static readonly ConcurrentDictionary<Type, string> _cachedModule = new ConcurrentDictionary<Type, string>();

        public static string GetModuleAttribute(Type type)
        {
            var description = _cachedModule.GetOrAdd(type, _GetModuleAttribute);
            return description;
        }

        private static string _GetModuleAttribute(Type type)
        {
            var attrs = type.GetCustomAttributes(typeof(ModuleAttribute), false);
            if (attrs.Length == 0)
                return APP.DB_DEFAULT_CONN_NAME;

            var module = (ModuleAttribute)attrs[0];
            return module.ModuleName;
        }
    }
    #endregion

    #region DisplayFormatAttribute
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DisplayFormatAttribute : Attribute
    {
        public bool ApplyFormatInEditMode { get; set; }
        public bool ConvertEmptyStringToNull { get; set; }
        public string DataFormatString { get; set; }
        public string NullDisplayText { get; set; }
        public DisplayFormatAttribute() { }
        public DisplayFormatAttribute(string formartString)
        {
            DataFormatString = formartString;
        }
    }
    #endregion

    #region PrimaryKeyAttribute
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PrimaryKeyAttribute : Attribute
    {
        public bool IsPrimaryKey { get; set; }
        public PrimaryKeyAttribute()
        {
            IsPrimaryKey = true;
        }
    }
    #endregion

    #region IdentityAttribute
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IdentityAttribute : Attribute
    {
        public bool IsIdentity { get; set; }
        public IdentityAttribute()
        {
            IsIdentity = true;
        }
    }
    #endregion

    #region Module 业务模块
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleAttribute : Attribute
    {
        public string ModuleName { get; set; }
        public ModuleAttribute(string name)
        {
            ModuleName = name;
        }
    }
    #endregion
}
