/**
* 模块名：mms viewModel
* 程序名: Detail.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

(function ($, com) {
    com.psi = com.psi||{};

    //计算总金额 用法：
    //this.grid.OnAfterCreateEditor = com.psi.calcTotalMoney(self, "Num", "UnitPrice", "Money", "TotalMoney");
    com.psi.bindCalcTotalMoney = function (self, fieldNum, fieldUnitPrice, fieldRowTotal, fieldAllTotal) {
        return function (editors) {
            var RowTotal = editors[fieldRowTotal].target;   //Money
            var Num = editors[fieldNum].target;             //Num
            var UnitPrice = editors[fieldUnitPrice].target; //UnitPrice

            com.readOnlyHandler('input')(editors[fieldRowTotal].target, true);
            var calc = function () {
                var rowTotalMoney = Num.numberbox('getValue') * UnitPrice.numberbox('getValue');
                RowTotal.numberbox('setValue', rowTotalMoney);
                var allMoney = rowTotalMoney - Number(editors[fieldRowTotal].oldHtml.replace(',', '') * 100) / 100;
                $.each(self.grid.datagrid('getData').rows, function () {
                    var addMoney = (Number(this[fieldRowTotal] * 100) / 100) || 0;
                    allMoney += addMoney
                });
                self.form.TotalMoney(allMoney);
            };
            Num.blur(calc);
            UnitPrice.blur(calc);
        };
    };

    //选择货物 用法 
    // this.grid.toolbar.push(com.psi.selectGoodsButton);
    com.psi.selectGoodsButton = function (self, apiUrl) {
        var handler = function () {
            var isExist = {}, existData = self.grid.datagrid('getData').rows;
            for (var j in existData)
                isExist[existData[j].GoodNo] = true;
            com.dialog({
                title: "选择材料",
                width: 600,
                height: 450,
                html: _selectGoodsHtml,
                viewModel: function (w) {
                    var that = this;
                    this.form = {
                        Id: ko.observable(),
                        Name: ko.observable()
                    };
                    this.grid = {
                        height: 345,
                        width: 585,
                        url: "/api/psi/goods",
                        queryParams: ko.observable(),
                        pagination: true,
                        singleSelect: false,
                        rownumbers: false
                    };
                    this.searchClick = function () {
                        that.grid.queryParams(ko.toJS(that.form));
                    };
                    this.clearClick = function () {
                        $.each(this.form, function () { this(''); });
                        this.searchClick();
                    };
                    this.cancelClick = function () {
                        w.dialog('close');
                    };
                    this.confirmClick = function () {
                        var arr = [],rows = that.grid.datagrid('getSelections') || [];
                        for (var i in rows)
                            if (!isExist[rows[i].Id]) arr.push(rows[i]);

                        if (arr.length == 0)
                            return that.cancelClick();

                        com.ajax({
                            type: 'GET',
                            url: self.urls.getrowid + arr.length,
                            complete: that.cancelClick,
                            showLoading: false,
                            success: function (d) {
                                var ids = d.split(',');
                                for (var i in arr)
                                    self.gridEdit.addnew($.extend({ BillNo: self.scrollKeys.current(), GoodNo: arr[i].Id, RowId: ids[i] }, self.defaultRow, arr[i]));
                            }
                        });
                    };
                }
            });
        };
        return { text: '添加材料', iconCls: 'icon-search', handler: handler };
    };

    var _selectGoodsHtml = function () { /*
        <style type="text/css">
            .z-toolbar{ border-width:0; margin:0;}
            .lbl { text-align:right; line-height:25px;}
            .datagrid-wrap{ border-width:0 !important;}
        </style>

        <div class="z-toolbar" style="border-bottom-width:1px;">
            <div class="container_16">
                <div class="grid_2 lbl" id="valueTitle">编码</div>
                <div class="grid_4 val"><input type="text" data-bind="value:form.Id"  class="z-txt" style="width:100%"  /></div>
                <div class="grid_2 lbl" id="textTitle">名称</div>
                <div class="grid_4 val"><input type="text" data-bind="value:form.Name" class="z-txt" style="width:100%"/></div>
                <div class="grid_4 val" style="margin-top:0px;">
                    <a id="btnSearch" href="#" plain="true" class="easyui-linkbutton" icon="icon-search" title="查询" data-bind="click:searchClick">查询</a>
                    <a id="btnClear" href="#" plain="true" class="easyui-linkbutton" icon="icon-clear" title="清空" data-bind="click:clearClick">清空</a>
                </div>
            </div>
        </div>

        <table id="list" data-bind="datagrid:grid">
            <thead>  
            <tr>  
                <th field="ck" checkbox="true"></th>
                <th field="Id"                  sortable="true" align="left"    width="70"  editor="text" >货物编号   </th>  
                <th field="Name"                sortable="true" align="left"    width="150" editor="text" >货物名称     </th> 
                <th field="Catagory"            sortable="true" align="left"    width="100"  editor="text" >类别   </th>  
                <th field="Brand"               sortable="true" align="left"    width="100"  editor="text">品牌   </th>  
                <th field="Model"               sortable="true" align="left"    width="100"  editor="text" >型号规格   </th>  
            </tr>                            
            </thead>      
        </table> 
                 
        <div class="z-toolbar" style="text-align: center;padding:4px 0;border-top-width:1px; ">
            <button id="btnConfirm" class="sexybutton" data-bind="click:confirmClick"><span><span><span class="ok">确定</span></span></span></button> &nbsp; 
            <button id="btnCancel" class="sexybutton"  data-bind="click:cancelClick"><span><span><span class="cancel">取消</span></span></span></button>
        </div>
    */};
})(jQuery, com);