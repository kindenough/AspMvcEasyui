/*************************************************************************
 * 文件名称 ：RequestWrapperMethods.cs                          
 * 描述说明 ：请求包装 方法
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Zephyr.Core
{
    public partial class RequestWrapper
    {
        public IEnumerable<string> Except(List<string> keys = null)
        {
             var result = request.AllKeys
                        .Except(ignores)
                        .Except(keys??new List<string>())
                        .Where(x => !x.StartsWith(ignoreStartWith));

            return result;
        }

        public RequestWrapper LoadSettingXml(string url)
        {
            settingXml = XElement.Load(HttpContext.Current.Server.MapPath(url)).ToString();
            return this;
        }

        public RequestWrapper LoadSettingXmlString(string xml,params object[] param)
        {
            settingXml = string.Format(xml,param);
            _alias = null;
            _fields = null;
            _variable = null;
            return this;
        }

        public RequestWrapper SetRequestData(NameValueCollection values)
        {
            this.request = values;
            return this;
        }

        public RequestWrapper SetRequestData(JToken values)
        {
            if (values != null)
            {
                foreach (JProperty item in values.Children())
                    if (item != null) this[item.Name] = item.Value.ToString();
            }
            return this;
        }

        public RequestWrapper SetRequestData(string name,string value)
        {
            this[name] = value;
            return this;
        }

        public dynamic ToDynamic() {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            foreach (string key in this.Except())
                expando.Add(key, this[key]);

            return expando;
        }
 
        public string GetXmlNodeValue(string name) {
            var settings = XElement.Parse(settingXml);
            return getXmlElementValue(settings, name);
        }
 
        #region 私有方法
        private string getXmlElementAttr(XElement element, string attri,string defaultStr="")
        {
            return element.Attribute(attri) == null ? defaultStr : element.Attribute(attri).Value;
        }

        private string getXmlElementValue(XElement element, string name) { 
            return element.Element(name) == null ? string.Empty : element.Element(name).Value;        
        }

        private NameValueCollection _alias;
        private NameValueCollection _fields;
        private NameValueCollection _variable;
        public string getAliasName(string field)
        {
            if (_alias == null)
                initFieldAliasVariable("alias");

            return _alias[field] ?? field;
        }
        public string getFieldName(string alias,bool withTable=false)
        {
            if (string.IsNullOrEmpty(alias)) 
                return string.Empty;

            if (_fields == null)
                initFieldAliasVariable("field");

            var prefix = string.Empty;
            if (alias.IndexOf(".") >= 0)
            {
                var arr = alias.Split('.');
                if (withTable) prefix = arr[0] + ".";
                alias = arr[1];
            }
            return prefix + (_fields[alias] ?? alias);
        }

        public string getVariableName(string field)
        {
            if (_variable == null)
                initFieldAliasVariable("variable");

            return _variable[field]?? field;
        }
        private void initFieldAliasVariable(string initType)
        {
            //if (string.IsNullOrEmpty(this.settingXml))
                //throw new Exception("使用此方法，配置xml不能为空！");
 
            switch (initType){
                case "alias":
                case "field":
                    _alias = new NameValueCollection();
                    _fields = new NameValueCollection();
                    if (string.IsNullOrEmpty(this.settingXml)) return;
                    var select = this.GetXmlNodeValue("select") ?? string.Empty;    //处理类型 projectName as text;   text = 'xxx'作用条件时的情况
                    select.Replace("\r", "").Replace("\n", "").Split(',').ToList().Where(x => x.ToLower().IndexOf(" as ") >= 0).ToList().ForEach(x =>
                    {
                        var array = Regex.Replace(x, @"\s+", " ").Trim().Split(' ');
                        string field = array[0], alias = array[2];
                        _alias.Add(field, alias);
                        _fields.Add(alias, field);
                    });
                    break;
                case "variable":
                    _variable = new NameValueCollection();
                    if (string.IsNullOrEmpty(this.settingXml)) return;
                    var wheres = XElement.Parse(settingXml).Element("where");
                    if (wheres != null)
                    {
                        foreach (var item in wheres.Elements("field"))
                        {
                            var name = getXmlElementAttr(item, "name");
                            var variable = getXmlElementAttr(item, "variable", name);
                            if (name!= variable) _variable[name]=variable;
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}