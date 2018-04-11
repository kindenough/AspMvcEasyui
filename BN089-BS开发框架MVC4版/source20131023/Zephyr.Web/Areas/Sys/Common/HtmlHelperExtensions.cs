using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zephyr.Models;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RenderToolbar(this HtmlHelper htmlHelper)
        {
            var buttons = new sys_menuService().GetCurrentUserMenuButtons();
            var toolbar = new TagBuilder("div");
            toolbar.AddCssClass("z-toolbar");
            var addition = string.Empty;
        
            foreach (var btn in buttons)
            {
                var link = new TagBuilder("a");
                link.MergeAttribute("href", "#");

                if (btn.ButtonCode.Equals("download"))
                {
                    link.MergeAttribute("class", "easyui-splitbutton");
                    link.MergeAttribute("data-options", "menu:'#dropdown',iconCls:'icon-download'");
                    addition += DropDownDiv();
                }
                else
                {
                    link.MergeAttribute("plain", "true");
                    link.MergeAttribute("class", "easyui-linkbutton");
                    link.MergeAttribute("icon", btn.ButtonIcon);
                    link.MergeAttribute("title", btn.ButtonName);
                    link.MergeAttribute("data-bind", "click:" + btn.ButtonCode + "Click");
                }

                link.SetInnerText(btn.ButtonName);
                toolbar.InnerHtml += link.ToString();
            }

            return new MvcHtmlString(toolbar.ToString() + addition);
        }

        public static MvcHtmlString HideColumn(this HtmlHelper htmlHelper,List<sys_roleMenuColumnMap> cols, string field)
        {
            var result = string.Empty;
            var fields = cols.Where(x => x.FieldName == field);
            if (fields.Count() > 0 && fields.First().IsReject == true)
                result = "hidden=true";
            
            return new MvcHtmlString(result);
        }

        private static string DropDownDiv()
        {
            var div = new TagBuilder("div");
            div.MergeAttribute("id", "dropdown");
            div.MergeAttribute("style", "width:100px; display:none;");

            var div1 = new TagBuilder("div");
            div1.MergeAttribute("suffix", "xls");
            div1.MergeAttribute("data-options", "iconCls:'icon-ext-xls'");
            div1.MergeAttribute("data-bind", "click:downloadClick");
            div1.SetInnerText("Excel2003");
            div.InnerHtml += div1.ToString();

            var div2 = new TagBuilder("div");
            div2.MergeAttribute("suffix", "xlsx");
            div2.MergeAttribute("data-options", "iconCls:'icon-page_excel'");
            div2.MergeAttribute("data-bind", "click:downloadClick");
            div2.SetInnerText("Excel2007");
            div.InnerHtml += div2.ToString();

            var div3 = new TagBuilder("div");
            div3.MergeAttribute("suffix", "doc");
            div3.MergeAttribute("data-options", "iconCls:'icon-ext-doc'");
            div3.MergeAttribute("data-bind", "click:downloadClick");
            div3.SetInnerText("Word2003");
            div.InnerHtml += div3.ToString();

            return div.ToString();
        }
    }
}