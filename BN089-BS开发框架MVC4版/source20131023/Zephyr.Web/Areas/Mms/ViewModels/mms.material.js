/**
* 模块名：mms viewModel
* 程序名: material.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var viewModel = function () {
    var self = this;
    this.grid = {
        size: { w: 239, h: 40 },
        url: "/api/mms/material",
        queryParams: ko.observable(),
        pagination: true
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onClickRow = self.gridEdit.begin;
    this.grid.OnAfterCreateEditor = function (edt) {
        com.readOnlyHandler('input')(edt["MaterialCode"].target, true);
    };
    this.tree = {
        method:'GET',
        url: '/api/mms/material/GetTypes',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['MaterialType as id', 'MaterialTypeName as text','ParentCode as pid']);
            var data = utils.toTreeData(filter, 'id', 'pid', "children");
            return [{ id: '', text: '所有类别', children: data }];
        },
        onSelect: function (node) {
            self.CodeType(node.id);
        }
    };

    this.CodeType = ko.observable();
    this.CodeType.subscribe(function (value) {
        self.grid.queryParams({ MaterialType: value });
    });

    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        if (!self.CodeType()) return com.message('warning', '请先在左边选择要添加的类别！');
        com.ajax({
            type: 'GET',
            url: '/api/mms/material/getnewcode/' + self.CodeType(),
            success: function (d) {
                var row = { MaterialType: self.CodeType(), MaterialCode: d };
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
        var post = {list:self.gridEdit.getChanges()};
        if (self.gridEdit.isChangedAndValid()) {
            com.ajax({
                url: '/api/mms/material/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.typeClick = function () {
        com.dialog({
            title: "&nbsp;材料类别",
            iconCls:'icon-node_tree',
            width: 600,
            height: 410,
            html: "#type-template",
            viewModel: function (w) {
                var that = this;
                this.grid = {
                    width: 586,
                    height: 340,
                    idField: 'MaterialType',
                    treeField: 'MaterialType',
                    url: "/api/mms/material/gettypes",
                    queryParams: ko.observable(),
                    loadFilter: function (d) {
                        return utils.toTreeData(d.rows||d, 'MaterialType', 'ParentCode', "children");
                    },
                };
                this.gridEdit = new com.editTreeGridViewModel(this.grid);
                this.grid.OnAfterCreateEditor = function (editors,row) {
                    if (!row._isnew) com.readOnlyHandler('input')(editors["MaterialType"].target, true);
                };
                this.grid.onClickRow = that.gridEdit.begin;
                this.grid.toolbar = [
                    { text: '新增', iconCls: 'icon-add1', handler: function () { that.gridEdit.addnew(); } }, '-',
                    { text: '删除', iconCls: 'icon-cross', handler: that.gridEdit.deleterow }
                ];
                this.confirmClick = function () {
                    that.gridEdit.ended();
                    if (that.gridEdit.isChangedAndValid()) {
                        var list = that.gridEdit.getChanges(['MaterialType', 'MaterialTypeName', 'ParentCode', 'Type', 'Remark']);
                        com.ajax({
                            url: '/api/mms/material/edittype/',
                            data: ko.toJSON({list:list}),
                            success: function (d) {
                                that.cancelClick();
                                self.tree.$element().tree('reload');
                                com.message('success', '保存成功！');
                            }
                        });
                    }
                };
                this.cancelClick = function () {
                    w.dialog('close');
                };
            }
        });
    };
};