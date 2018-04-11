<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="Zephyr.Web.report" %>
<%@ Register  Namespace="Stimulsoft.Report.Web" TagPrefix="cc1"  Assembly="Stimulsoft.Report.Web, Version=2012.3.1500.0, Culture=neutral, PublicKeyToken=096a9279a87304f1"%>
<%@ Register  Namespace="Stimulsoft.Report.Web" TagPrefix="cc2"  Assembly="Stimulsoft.Report.WebDesign, Version=2012.3.1500.0, Culture=neutral, PublicKeyToken=096a9279a87304f1"%>

<!doctype html>
<html>
    <head runat="server">
        <title></title>
    </head>
    <body style="background-color: #e8e8e8">
        <form id="form1" runat="server">
        <div style="width: 960px;margin: 0 auto;">
            <cc1:StiWebViewer ID="StiWebViewer1" runat="server"  GlobalizationFile="/Content/page/reports/Localization/zh-CHS.xml" ShowDesignButton="True"  onreportdesign="StiWebViewer1_ReportDesign" Theme="Office2010"  BackColor="#e8e8e8"/>
            <cc2:StiWebDesigner ID="StiWebDesigner1" runat="server" LocalizationDirectory="/Content/page/reports/Localization/" Localization="zh-CHS" onsavereport="StiWebDesigner1_SaveReport" />
        </div>
        </form>
    </body>
</html>
