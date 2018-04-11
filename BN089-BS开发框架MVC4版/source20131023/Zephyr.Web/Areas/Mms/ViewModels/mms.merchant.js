/**
* 模块名：mms viewModel
* 程序名: merchant.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var viewModel = function () {
    var self = this;
    this.grid = {
        size: { w: 189, h: 40 },
        url: "/api/mms/merchant",
        queryParams: ko.observable(),
        pagination: true
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onClickRow = self.gridEdit.begin;
    this.grid.OnAfterCreateEditor = function (edt) {
        com.readOnlyHandler('input')(edt["MerchantsCode"].target, true);
    };
    this.tree = {
        method:'GET',
        url: '/api/mms/merchant/GetTypes',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['MerchantsTypeCode as id', 'MerchantsTypeName as text']);
            return [{id:'',text:'所有类别',children:filter}];
        },
        onSelect: function (node) {
            self.CodeType(node.id);
        }
    };

    this.CodeType = ko.observable();
    this.CodeType.subscribe(function (value) {
        self.grid.queryParams({ MerchantsTypeCode: value });
    });

    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        if (!self.CodeType()) return com.message('warning', '请先在左边选择要添加的类别！');
        com.ajax({
            type: 'GET',
            url: '/api/mms/merchant/getnewcode/' + self.CodeType(),
            success: function (d) {
                var row = { MerchantsTypeCode: self.CodeType(), MerchantsCode: d };
                self.gridEdit.addnew(row);
            }
        });
    };
    this.editClick = function () {
        var row = self.grid.treegrid('getSelected');
        self.gridEdit.begin(row);
    };
    this.deleteClick = self.gridEdit.deleterow;
    this.saveClick = function(){
        self.gridEdit.ended();
        var post = { list: self.gridEdit.getChanges() };
        if (self.gridEdit.isChangedAndValid) {
            com.ajax({
                url: '/api/mms/merchant/edit',
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
            title: "&nbsp;客商类别",
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
                    url: "/api/mms/merchant/gettypes",
                    queryParams: ko.observable()
                };
                this.gridEdit = new com.editGridViewModel(this.grid);
                this.grid.OnAfterCreateEditor = function (editors,row) {
                    if (!row._isnew) com.readOnlyHandler('input')(editors["MerchantsTypeCode"].target, true);
                };
                this.grid.onClickRow = that.gridEdit.begin;
                //this.grid.onDblClickRow = that.gridEdit.begin;
                this.grid.toolbar = [
                    { text: '新增', iconCls: 'icon-add1', handler: function () { that.gridEdit.addnew(); } }, '-',
                    { text: '删除', iconCls: 'icon-cross', handler: that.gridEdit.deleterow }
                ];
                this.confirmClick = function () {
                    that.gridEdit.ended();
                    if (that.gridEdit.isChangedAndValid()) {
                        var list = that.gridEdit.getChanges(['MerchantsTypeCode', 'MerchantsTypeName', 'MerchantsProperty', 'Remark']);
                        com.ajax({
                            url: '/api/mms/merchant/edittype/',
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