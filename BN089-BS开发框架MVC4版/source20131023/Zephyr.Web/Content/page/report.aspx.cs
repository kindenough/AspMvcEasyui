using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stimulsoft.Report;

namespace Zephyr.Web
{
    public partial class report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StiWebViewer1.Report = GetReport();
        }

        protected void StiWebViewer1_ReportDesign(object sender, EventArgs e)
        {
            StiWebDesigner1.Design(GetReport());
        }

        protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiWebDesigner.StiSaveReportEventArgs e)
        {
            var report = e.Report;
            report.Save(GetReportPath());
        }

        private StiReport GetReport()
        {
            var report = new StiReport();
            report.Load(GetReportPath());

            ChangeConnectString(report);

            report.Compile();
            SetReportParamaters(report);
           
            return report;
        }

        private void SetReportParamaters(StiReport report)
        {
            //report.CompiledReport.DataSources["check"].Parameters["FromDate"].ParameterValue = DateTime.Parse("10/05/1999");
            var dataSource = report.CompiledReport.DataSources;
            foreach (Stimulsoft.Report.Dictionary.StiDataSource ds in dataSource)
            {
                var param = Request.QueryString;
                foreach (string key in param.Keys)
                {
                    if (!ds.Parameters.Contains(key)) continue;
                    var p = ds.Parameters[key];
                    var v = param[key];
                    p.ParameterValue = v;
                }
            }
        }

        private string GetReportPath()
        {
            var path = String.Format("~/Areas/{0}/Reports/{1}.mrt",Request["area"], Request["rpt"]);
            path = Server.MapPath(path);
            if (!System.IO.File.Exists(path))
                path = Server.MapPath("~/Content/page/reports/helloworld.mrt");
            return path;
        }

        private void ChangeConnectString(StiReport report)
        {
            //((Stimulsoft.Report.Dictionary.StiSqlDatabase)(report.Dictionary.Databases["Connection1"])).ConnectionString = ConfigurationManager.ConnectionStrings["Mms"].ConnectionString;
            foreach (Stimulsoft.Report.Dictionary.StiSqlDatabase item in report.Dictionary.Databases)
            {
                var prefix = item.Name.Split('_')[0];
                item.ConnectionString = ConfigurationManager.ConnectionStrings[prefix].ConnectionString;
            }
        }

        private string GetRouteValue(string name)
        {
            var oParam = Page.RouteData.Values[name];
            var value = (oParam == null || oParam.ToString() == string.Empty) ? string.Empty : oParam.ToString();
            return value;
        }
    }
}