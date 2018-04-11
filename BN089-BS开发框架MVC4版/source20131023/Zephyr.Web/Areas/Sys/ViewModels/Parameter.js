/**
* 模块名：System
* 程序名: parameter.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var viewModel = function() {
    var self = this;
    this.grid = {
        size: { w: 4, h: 40 },
        url: '/api/sys/parameter',
        queryParams: ko.observable(),
        pagination: true,
        loadFilter: function (d) {
            d.rows = utils.copyProperty(d.rows, 'ParamCode', '_id');
            return d;
        }
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = this.gridEdit.begin;
    this.grid.onClickRow = this.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (editors, row) {
        if (!row.IsUserEditable) {
            var readonly = com.readOnlyHandler('input');
            readonly(editors.ParamCode.target, true);
            readonly(editors.ParamValue.target, true);
            readonly(editors.Description.target, true);
        }
    };
    
    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        self.gridEdit.addnew({IsUserEditable:true});
    };
    this.deleteClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row.IsUserEditable) return com.message('warning', '此参数不能被删除！');
        self.gridEdit.deleterow();
    }
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', '请先选择一个参数！');
        self.gridEdit.begin()
    };
    this.grid.onDblClickRow = this.editClick;
    this.saveClick = function () {
        var post = {};
        post.list = self.gridEdit.getChanges(['_id','ParamCode', 'ParamValue', 'IsUserEditable', 'Description']);
        if (self.gridEdit.isChangedAndValid()) {
            com.ajax({
                url: '/api/sys/parameter/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    self.gridEdit.accept();
                }
            });
        }
    };
};
 