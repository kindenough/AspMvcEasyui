<%@ Page Title="" Language="C#" MasterPageFile="~/common/master/Common.Master" AutoEventWireup="true" Inherits="Zephyr.Core.PageBase" %>

<asp:Content ID="h" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="content" runat="server">
    <div class="z-toolbar">
        <a id="a_add"    href="#" plain="true" class="easyui-linkbutton" icon="icon-add" title="新增">新增</a>
        <a id="a_edit"   href="#" plain="true" class="easyui-linkbutton" icon="icon-edit" title="编辑">编辑</a>
        <a id="a_del"    href="#" plain="true" class="easyui-linkbutton" icon="icon-cross" title="删除">删除</a>
        <a id="a_audit"  href="#" plain="true" class="easyui-linkbutton" icon="icon-folder_key" title="审核">审核</a>
        <a id="a_export" href="#" class="easyui-splitbutton" data-options="menu:'#dropdown',iconCls:'icon-download'">导出</a>
    </div>

    <div id="dropdown" style="width:100px; display:none;">  
        <div id="a_xls"  data-options="iconCls:'icon-ext-xls'">    Excel2003   </div>  
        <div id="a_xlsx" data-options="iconCls:'icon-page_excel'"> Excel2007   </div>  
        <div id="a_doc"  data-options="iconCls:'icon-ext-doc'">    Word2003    </div>  
    </div> 

    <div id="condition" class="container_12" style="position:relative;">
        <div class="grid_1 lbl">{0:ColumnName}</div>
        <div class="grid_2 val"><input type="text" id="{0:ColumnName}" name="{0:ColumnName}" data-cp="like" class="{0:Class}"/></div>
        <div class="grid_1 lbl">{1:ColumnName}</div>
        <div class="grid_2 val"><input type="text" id="{1:ColumnName}" name="{1:ColumnName}" data-cp="like" class="{1:Class}"/></div>
        <div class="grid_1 lbl">{2:ColumnName}</div>
        <div class="grid_2 val"><input type="text" id="{2:ColumnName}" name="{2:ColumnName}" data-cp="like" class="{2:Class}"/></div>
      
        <div class="clear"></div>
       
        <div class="grid_1 lbl">{3:ColumnName}</div>
        <div class="grid_2 val"><input type="text" id="{3:ColumnName}" name="{3:ColumnName}" data-cp="like" class="{3:Class}"/></div>
        <div class="grid_1 lbl">{4:ColumnName}</div>
        <div class="grid_2 val"><input type="text" id="{4:ColumnName}" name="{4:ColumnName}" data-cp="like" class="{4:Class}"/></div>
        <div class="grid_1 lbl">{5:ColumnName}</div>
        <div class="grid_2 val"><input type="text" id="{5:ColumnName}" name="{5:ColumnName}" data-cp="like" class="{5:Class}"/></div>

        <div class="clear"></div>

         <div class="prefix_9" style="position:absolute;top:5px;height:0;">  
            <a id="a_search" href="#" class="buttonHuge button-blue" style="margin:0 15px;">查询</a> 
            <a id="a_reset" href="#" class="buttonHuge button-blue">清空</a>
        </div>
    </div>

    <table id="gridlist"></table> 

    <script type="text/javascript" src="{FileName}.aspx.js"></script>
</asp:Content>
