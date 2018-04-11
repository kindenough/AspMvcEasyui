/*************************************************************************
 * 文件名称 ：ModelBase.cs                          
 * 描述说明 ：定义实体基类
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Zephyr.Utils;

namespace Zephyr.Core
{
    public class ModelBase
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, List<string>>> _cachedAtrributes = new ConcurrentDictionary<Type, Dictionary<string, List<string>>>();
 
        public static List<string> GetAttributeFields<TModel, TAttribute>()
        {
            var key = typeof(TAttribute).Name;
            var thisAttributes =_cachedAtrributes.GetOrAdd(typeof(TModel), BuildAtrributeDictionary);
            return thisAttributes.ContainsKey(key) ? thisAttributes[typeof(TAttribute).Name] : new List<string>();
        }
 
        private static Dictionary<string, List<string>> BuildAtrributeDictionary(Type TModel)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var property in TModel.GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
                if (attributes != null)
                    foreach (var key in attributes.Select(attr => attr.GetType().Name))
                    {
                        if (!result.ContainsKey(key))
                            result.Add(key, new List<string>());

                        result[key].Add(property.Name);
                    }
            }

            return result;
        }

        public dynamic Extend(object obj)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
           
            EachHelper.EachObjectProperty(this, (i, name, value) => {
                expando.Add(name, value);
            });

            EachHelper.EachObjectProperty(obj, (i, name, value) => {
                expando[name] = value;
            });

            return expando;
        }
    }
}
