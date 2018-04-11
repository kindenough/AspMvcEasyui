using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace Zephyr.Utils
{
    public partial class ZConvert
    {
        public static T ToEnum<T>(object obj, T defaultEnum)
        {
            string str = To<string>(obj);

            if (Enum.IsDefined(typeof(T),str))
                return (T)Enum.Parse(typeof(T),str);

            int num;
            if (int.TryParse(str, out num))
            {
                if (Enum.IsDefined(typeof(T), num))
                    return (T)Enum.ToObject(typeof(T), num);
            }

            return defaultEnum;
        }
    }
}
