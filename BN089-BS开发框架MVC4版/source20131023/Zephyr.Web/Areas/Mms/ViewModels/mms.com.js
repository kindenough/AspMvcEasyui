/// <reference path="common.js" />

/**
* 模块名：共通脚本
* 程序名: 材料模块通用方法mms.com.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var mms = mms || {};
mms.com = {};

mms.com.getCurrentProject = function () {
    return window!=parent?parent.$.cookie('CurrentProject'):"";
};

mms.com.formatSrcBillType = function (value) {
    var dict = {
        'receive': '收料单',
        'send': '发料单',
        'adjust':'库存调整',
        'direct':'直入直出单',
        'rentin': '租赁单',
        'return': '退货单',
        'refund': '退库单',
        'transfer': '调拨单',
        'lossReport': '报损单'
    };
    return dict[value]||value;
};

//计算总金额 用法：
//this.grid.OnAfterCreateEditor = com.psi.calcTotalMoney(self, "Num", "UnitPrice", "Money", "TotalMoney");
mms.com.bindCalcTotalMoney = function (self, fieldNum, fieldUnitPrice, fieldRowTotal, fieldAllTotal) {
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
            self.form[fieldAllTotal](allMoney);
        };
        Num.blur(calc);
        UnitPrice.blur(calc);
    };
};

mms.com.calcTotalMoneyWhileRemoveRow = function (self, fieldRowTotal, fieldAllTotal) {
    var allMoney = 0, fieldRowTotal = fieldRowTotal || "Money", fieldAllTotal = fieldAllTotal || "TotalMoney";
    $.each(self.grid.datagrid('getData').rows, function () {
        var addMoney = (Number(this[fieldRowTotal] * 100) / 100) || 0;
        allMoney += addMoney
    });
    self.form[fieldAllTotal](allMoney);
};

mms.com.auditDialog = function () {
    var query = parent.$;
    var winAudit = query('#w_audit_div'), args = arguments;
    if (winAudit.length == 0) {
        var html = utils.functionComment(function () {/*
            <div id="w_audit_wrapper">
                <div id="w_audit_div" class="easyui-dialog"  title="审核" data-options="modal:true,closed:true,iconCls:'icon-user-accept'" style="width:400px;height:210px;" buttons="#w_audit_div_button"> 
                    <div class="container_16" style="width:90%;margin:5%;">  
                        <div class="grid_3 lbl" style="font-weight: bold;color:#7e7789">审核状态</div>  
                        <div class="grid_13 val">
                            通过审核<input type="radio" name="AuditStatus" value="passed" data-bind="checked:form.status,disable:disabled" style="margin-right:10px;" /> 
                            取消审核<input type="radio" name="AuditStatus" value="reject" data-bind="checked:form.status,disable:disabled" />
                        </div>
                        <div class="grid_3 lbl" style="margin-top:5px;font-weight: bold;color:#7e7789" style="font-weight: bold;">审核意见</div>  
                        <div class="grid_13 val"><textarea style="width:272px;height:60px;" class="z-text" data-bind="value:form.comment" ></textarea></div>
                        <div class="clear"></div>
                    </div> 
                </div> 
                <div id="w_audit_div_button" class="audit_button">  
                    <a href="javascript:void(0)" data-bind="click:confirmClick" class="easyui-linkbutton" iconCls="icon-ok" >确定</a>  
                    <a href="javascript:void(0)" data-bind="click:cancelClick" class="easyui-linkbutton" iconCls="icon-cancel" >取消</a>  
                </div> 
            </div>
            */});
        var wrapper = query(html).appendTo("body");
        wrapper.find(".easyui-linkbutton").linkbutton();
        winAudit = wrapper.find(".easyui-dialog").dialog();
    }

    var viewModel = function () {
        var self = this;
        this.disabled = ko.observable(true);
        this.form = {
            status: args[0].ApproveState()=="passed"?"reject":"passed",
            comment: args[0].ApproveRemark()
        };
        this.confirmClick = function () {
            winAudit.dialog('close');
            if (typeof args[1] === 'function') {
                args[0].ApproveState(this.form.status);
                args[0].ApproveRemark(this.form.comment);
                args[1].call(this, ko.toJS(self.form));
            }
        };
        this.cancelClick = function () {
            winAudit.dialog('close');
        };
    }

    var node = winAudit.parent()[0];
    winAudit.dialog('open');
    ko.cleanNode(node);
    ko.applyBindings(new viewModel(), node);
};
 
//弹出选择材料窗口
mms.com.selectMaterial = function (vm, param) {
    var grid = vm.grid;
    var addnew = vm.gridEdit.addnew;
    var defaultRow = vm.defaultRow;
    var url = vm.urls.getrowid;
    defaultRow.BillNo = vm.scrollKeys.current();

    //var isExist = {}, existData = grid.datagrid('getData').rows;
    //for (var j in existData)
    //    isExist[existData[j].MaterialCode] = true;
    var orgRows = grid.datagrid('getData').rows;
    var comapreArray = param._xml == "mms.material_batches" ? ['MaterialCode', 'SrcBillType', 'SrcBillNo'] : ['MaterialCode'];
    var fnEqual = function (row1, row2) {
        for (var key in comapreArray) 
            if (row1[comapreArray[key]] != row2[comapreArray[key]])
                return false;
        return true;
    }
    var fnExist = function (row) {
        for (var i in orgRows)
            if (fnEqual(orgRows[i], row))
                return true;
        return false;
    };

    var target = parent.$('#selectMaterial').length ? parent.$('#selectMaterial') : parent.$('<div id="selectMaterial"></div>').appendTo('body');
    utils.clearIframe(target);

    var opt = { title: '选择在库材料', width: 800, height: 550, modal: true, collapsible: false, minimizable: false, maximizable: true, closable: true };
    opt.content = "<iframe id='frm_win_material' src='/mms/home/lookupmaterial' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>";  //frameborder="0" for ie7
    opt.paramater = param;      //可传参数
    opt.onSelect = function (data) {                //可接收选择数据
        var total = data.total;
        var rows = data.rows;
        com.ajax({
            type: 'GET',
            url: url + total,
            data:{BillNo:vm.form.BillNo},
            success: function (d) {
                var ids = d.split(',');
                for (var i in rows) {
                    if (!fnExist(rows[i])) 
                        addnew($.extend({ RowId: ids[i] }, defaultRow, rows[i]));
                }
            }
        });
    };
    target.window(opt);
};
