/**
* 模块名：mms viewModel
* 程序名: code.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var viewModel = function(){
    var self = this;
    this.grid = {
        size: { w: 189, h: 40 },
        url: "/api/sys/code",
        queryParams: ko.observable(),
        pagination: true,
        idField: 'Code',
        treeField: 'Code',
        loadFilter: function (data) {
            data.rows = utils.toTreeData(data.rows, 'Code', 'ParentCode', "children");
            return data;
        }
    };
    this.gridEdit = new com.editTreeGridViewModel(this.grid);
    this.grid.onDblClickRow = self.gridEdit.begin;
    this.grid.onClickRow = self.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (edt) {
        com.readOnlyHandler('input')(edt["Code"].target, true);
    };
    this.tree = {
        method:'GET',
        url: '/api/sys/code/getcodetype',
        queryParams: ko.observable(),
        loadFilter:function(d){
            var filter = utils.filterProperties(d.rows||d, ['CodeType as id', 'CodeTypeName as text']);
            return [{id:'',text:'所有类别',children:filter}];
        },
        onSelect: function (node) {
            self.CodeType(node.id);
        }
    };

    this.CodeType = ko.observable();
    this.CodeType.subscribe(function (value) {
        self.grid.queryParams({ CodeType: value });
    });

    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        if (!self.CodeType()) return com.message('warning', '请先在左边选择要添加的类别！');
        com.ajax({
            type: 'GET',
            url: '/api/sys/code/getnewcode',
            success: function (d) {
                var row = {CodeType: self.CodeType(), Code: d };
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
        var post = {};
        post.list = self.gridEdit.getChanges(['CodeType', 'Code', 'Value', 'Text', 'ParentCode', 'Description', 'IsEnable','IsDefault', 'Seq']);
        if (self.gridEdit.isChangedAndValid) {
            com.ajax({
                url: '/api/sys/code/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    //self.grid.queryParams({ CodeType: self.CodeType() });
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.typeClick = function () {
        com.dialog({
            title: "&nbsp;字典类别",
            iconCls:'icon-node_tree',
            width: 600,
            height: 410,
            html: "#type-template",
            viewModel: function (w) {
                var that = this;
                this.grid = {
                    width: 586,
                    height: 340,
                    pagination: true,
                    pageSize:10,
                    url: "/api/sys/code/getcodetype",
                    queryParams: ko.observable()
                };
                this.gridEdit = new com.editGridViewModel(this.grid);
                this.grid.OnAfterCreateEditor = function (editors,row) {
                    if (!row._isnew) com.readOnlyHandler('input')(editors["CodeType"].target, true);
                };
                this.grid.onClickRow = that.gridEdit.ended;
                this.grid.onDblClickRow = that.gridEdit.begin;
                this.grid.toolbar = [
                    { text: '新增', iconCls: 'icon-add1', handler: function () { that.gridEdit.addnew(); } }, '-',
                    { text: '编辑', iconCls: 'icon-edit', handler: that.gridEdit.begin }, '-',
                    { text: '删除', iconCls: 'icon-cross', handler: that.gridEdit.deleterow }
                ];
                this.confirmClick = function () {
                    if (that.gridEdit.isChangedAndValid()) {
                        var list = that.gridEdit.getChanges(['_id','CodeType', 'CodeTypeName', 'Description', 'Seq']);
                        com.ajax({
                            url: '/api/sys/code/editcodetype/',
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