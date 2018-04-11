/**
* 模块名：mms viewModel
* 程序名: warehouse.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var viewModel = function () {
    var self = this;
    this.grid = {
        size: { w: 189, h: 40 },
        url: "/api/mms/warehouse",
        queryParams: ko.observable(),
        pagination: true
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onClickRow = self.gridEdit.begin;
    this.grid.OnAfterCreateEditor = function (edt) {
        com.readOnlyHandler('input')(edt["WarehouseCode"].target, true);
    };
    this.tree = {
        method:'GET',
        url: '/api/mms/project/getlist',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d, ['id', 'text', 'pid']);
            return utils.toTreeData(filter, 'id', 'pid', 'children');
        },
        onSelect: function (node) {
            self.CodeType(node.id);
        }
    };

    this.CodeType = ko.observable();
    this.CodeType.subscribe(function (value) {
        self.grid.queryParams({ ProjectCode: value });
    });

    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        if (!self.CodeType()) return com.message('warning', '请先在左边选择项目！');
        com.ajax({
            type: 'GET',
            url: '/api/mms/warehouse/getnewcode/' + self.CodeType(),
            success: function (d) {
                var row = { ProjectCode: self.CodeType(), WarehouseCode: d };
                self.gridEdit.addnew(row);
            }
        });
    };
    this.editClick = function () {
        var row = self.grid.treegrid('getSelected');
        self.gridEdit.begin(row);
    };
    this.deleteClick = self.gridEdit.deleterow;
    this.saveClick = function () {
        self.gridEdit.ended();
        var post = { list: self.gridEdit.getChanges(['ProjectCode','WarehouseCode', 'WarehouseName', 'Remark']) };
        if (self.gridEdit.isChangedAndValid) {
            com.ajax({
                url: '/api/mms/warehouse/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    self.gridEdit.accept();
                }
            });
        }
    };
};