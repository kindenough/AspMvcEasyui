using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Zephyr.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            var EasyuiVersion = "1.3.2";
            ResetIgnorePatterns(bundles.IgnoreList);
         
            //脚本
            bundles.Add(new ScriptBundle("~/Content/js/library").Include(
                "~/Content/js/core/json2.min.js",
                "~/Content/js/core/utils.js",
                "~/Content/js/core/common.js",
                "~/Content/js/core/knockout-2.2.1.js",
                "~/Content/js/core/knockout.mapping-2.4.1.js",
                "~/Content/js/core/knockout.bindings.js",
                "~/Content/js/jquery-plugin/showloading/jquery.showLoading.min.js",
                "~/Content/js/core/jquery.easyui.fix.js"));

            bundles.Add(new ScriptBundle("~/Content/js/index").Include(
                "~/Content/js/jquery/jquery-{version}.min.js",
                "~/Content/js/core/json2.min.js",
                "~/Content/js/jquery-extend/jquery.cookie.js",
                "~/Content/js/core/utils.js",
                "~/Content/js/core/common.js",
                "~/Content/js/core/jquery.easyui.fix.js",
                "~/Content/js/jquery-plugin/jnotify/jquery.jnotify.js",
                "~/Content/js/jquery-plugin/showloading/jquery.showLoading.min.js",
                "~/Content/js/jquery-plugin/ztree/jquery.ztree.all-3.2.min.js",
                "~/Content/js/viewModel/index.js"));
        }

        public static void ResetIgnorePatterns(IgnoreList ignoreList)
        {
            ignoreList.Clear();
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

    }
}