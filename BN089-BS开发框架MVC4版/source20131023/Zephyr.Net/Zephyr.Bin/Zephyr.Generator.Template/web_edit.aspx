<%@ Page Title="" Language="C#" MasterPageFile="~/common/master/Common.Master" AutoEventWireup="true" Inherits="Zephyr.Core.PageBase" %>

<asp:Content ID="h" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="c" ContentPlaceHolderID="content" runat="server">
    <div class="z-toolbar">
        <a id="a_add" href="#" plain="true" class="easyui-linkbutton" icon="icon-add" title="添加">添加</a>
        <a id="a_edit" href="#" plain="true" class="easyui-linkbutton" icon="icon-edit" title="编辑">编辑</a>
        <a id="a_del" href="#" plain="true" class="easyui-linkbutton" icon="icon-cross" title="删除">删除</a>
        <a id="a_save" href="#" plain="true" class="easyui-linkbutton" icon="icon-save" title="保存">保存</a>
        <a id="a_cancel" href="#" plain="true" class="easyui-linkbutton" icon="icon-cancel" title="取消">取消</a>
        <a id="a_export" href="#" class="easyui-splitbutton" data-options="menu:'#dropdown',iconCls:'icon-download'">导出</a>  
    </div>
      
    <div id="dropdown" style="width:100px; display:none;">  
        <div id="a_xls"  data-options="iconCls:'icon-ext-xls'">    Excel2003   </div>  
        <div id="a_xlsx" data-options="iconCls:'icon-page_excel'"> Excel2007   </div>  
        <div id="a_doc"  data-options="iconCls:'icon-ext-doc'">    Word2003    </div>  
    </div> 

    <div id="master" class="container_12">
        <div class="grid_1 lbl">{0:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{0:ColumnName}" name="{0:ColumnName}" class="{0:Class}" {0:DataCp}/></div>
        <div class="grid_1 lbl">{1:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{1:ColumnName}" name="{1:ColumnName}" class="{1:Class}" {1:DataCp}/></div>
        <div class="grid_1 lbl">{2:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{2:ColumnName}" name="{2:ColumnName}" class="{2:Class}" {2:DataCp}/></div>
        <div class="grid_1 lbl">{3:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{3:ColumnName}" name="{3:ColumnName}" class="{3:Class}" {3:DataCp}/></div>
        

        <div class="clear"></div>

        <div class="grid_1 lbl">{4:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{4:ColumnName}" name="{4:ColumnName}" class="{4:Class}" {4:DataCp}/></div>
        <div class="grid_1 lbl">{5:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{5:ColumnName}" name="{5:ColumnName}" class="{5:Class}" {5:DataCp}/></div>
        <div class="grid_1 lbl">{6:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{6:ColumnName}" name="{6:ColumnName}" class="{6:Class}" {6:DataCp}/></div>
        <div class="grid_1 lbl">{7:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{7:ColumnName}" name="{7:ColumnName}" class="{7:Class}" {7:DataCp}/></div>

        <div class="clear"></div>
        
        <div class="grid_1 lbl">{8:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{8:ColumnName}" name="{8:ColumnName}" class="{8:Class}" {8:DataCp}/></div>
        <div class="grid_1 lbl">{9:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{9:ColumnName}" name="{9:ColumnName}" class="{9:Class}" {9:DataCp}/></div>
        <div class="grid_1 lbl">{10:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{10:ColumnName}" name="{10:ColumnName}" class="{10:Class}" {10:DataCp}/></div>
        <div class="grid_1 lbl">{11:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{11:ColumnName}" name="{11:ColumnName}" class="{11:Class}" {11:DataCp}/></div>

        <div class="clear"></div>

        <div class="grid_1 lbl">{12:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{12:ColumnName}" name="{12:ColumnName}" class="{12:Class}" {12:DataCp}/></div>
        <div class="grid_1 lbl">{13:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{13:ColumnName}" name="{13:ColumnName}" class="{13:Class}" {13:DataCp}/></div>
        <div class="grid_1 lbl">{14:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{14:ColumnName}" name="{14:ColumnName}" class="{14:Class}" {14:DataCp}/></div>
        <div class="grid_1 lbl">{15:ColumnName}</div>
        <div class="grid_3 val"><input type="text" id="{15:ColumnName}" name="{15:ColumnName}" class="{15:Class}" {15:DataCp}/></div>
 
        <div class="clear"></div>
    </div>
    <table id="list"></table>  
    <script type="text/javascript" src="{FileName}.aspx.js"></script>
</asp:Content>
