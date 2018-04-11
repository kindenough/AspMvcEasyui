/**
* 模块名：mms viewModel
* 程序名: menu.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
 
function viewModel() {
    var self = this;
    this.grid = {
        size: { w: 4, h: 40 },
        url: '/api/sys/menu/getall',
        idField: '_id',
        queryParams: ko.observable(),
        treeField: 'MenuName',
        loadFilter: function (d) {
            d = utils.copyProperty(d.rows || d, ["MenuCode", "IconClass"], ["_id", "iconCls"], false);
            return utils.toTreeData(d, '_id', 'ParentCode', "children");
        } 
    };
    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        if (self.grid.onClickRow()) {
            var row = { _id: utils.uuid(),MenuCode:'',MenuName:''};
            self.grid.treegrid('append', { parent: '', data: [row] });
            self.grid.treegrid('select', row._id);
            self.grid.$element().data("datagrid").insertedRows.push(row);
            self.editClick();
        }
    };
    this.editClick = function () {
        var row = self.grid.treegrid('getSelected');
        if (row) {
            //取得父节点数据
            var treeData = JSON.parse(JSON.stringify(self.grid.treegrid('getData')).replace(/_id/g, "id").replace(/MenuName/g, "text"));
            treeData.unshift({ "id": 0, "text": "" });

            //设置上级菜单下拉树
            var gridOpt = $.data(self.grid.$element()[0], "datagrid").options;
            var col = $.grep(gridOpt.columns[0], function (n) { return n.field == 'ParentCode' })[0];
            col.editor = { type: 'combotree', options: { data: treeData } };
            col.editor.options.onBeforeSelect = function (node) {
                var isChild = utils.isInChild(treeData, row._id, node.id);
                com.messageif(isChild, 'warning', '不能将自己或下级设为上级节点');
                return !isChild;
            };
            
            //开始编辑行数据
            self.grid.treegrid('beginEdit', row._id);
            self.edit_id = row._id;
            var eds = self.grid.treegrid('getEditors', row._id);
            var edt = function (field) { return $.grep(eds, function (n) { return n.field == field })[0]; };
            self.afterCreateEditors(edt);
        }
    };
    this.afterCreateEditors = function (editors) {
        var iconInput = editors("IconClass").target;
        var onShowPanel = function () {
            iconInput.lookup('hidePanel');
            com.dialog({
                title: "&nbsp;选择图标",
                iconCls: 'icon-node_tree',
                width: 700,
                height: 500,
                url: "/Content/page/icon.html",
                viewModel: function (w) {
                    w.find('#iconlist').css("padding", "5px");
                    w.find('#iconlist li').attr('style', 'float:left;border:1px solid #fff; line-height:20px; margin-right:4px;width:16px;cursor:pointer')
                     .click(function () {
                         iconInput.lookup('setValue',$(this).find('span').attr('class').split(" ")[1]);
                         w.dialog('close');
                     }).hover(function () {
                         $(this).css({ 'border': '1px solid red' });
                     }, function () {
                         $(this).css({ 'border': '1px solid #fff' });
                     });
                }
            });
        };
        iconInput.lookup({ customShowPanel: true, onShowPanel: onShowPanel, editable: true });
        iconInput.lookup('resize', iconInput.parent().width());
        iconInput.lookup('textbox').unbind();
    };
    this.grid.OnBeforeDestroyEditor = function (editors, row) {
        row.ParentName = editors['ParentCode'].target.combotree('getText');
        row.IconClass = editors["IconClass"].target.lookup('textbox').val();
    };
    this.deleteClick = function () {
        var row = self.grid.treegrid('getSelected');
        if (row) {
            self.grid.$element().treegrid('remove', row._id);
            self.grid.$element().data("datagrid").deletedRows.push(row);
        }
    };
    this.grid.onDblClickRow = self.editClick;
    this.grid.onClickRow = function () {
        var edit_id = self.edit_id;
        if (!!edit_id) {
            if (self.grid.treegrid('validateRow', edit_id)) { //通过验证
                self.grid.treegrid('endEdit', edit_id);
                self.edit_id = undefined;
            }
            else { //未通过验证
                self.grid.treegrid('select', edit_id);
                return false;
            }
        }
        return true;
    };
    this.saveClick = function () {
        self.grid.onClickRow();
        var post = {};
        post.list = new com.editTreeGridViewModel(self.grid).getChanges(['_id', 'MenuName', 'MenuCode', 'ParentCode', 'IconClass', 'URL', 'IsVisible', 'IsEnable', 'MenuSeq']);
        if (self.grid.onClickRow() && post.list._changed) {
            com.ajax({
                url: '/api/sys/menu/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    self.grid.treegrid('acceptChanges');
                    self.grid.queryParams({});
                }
            });
        }

    };
}

var setButton = function (MenuCode) {
    com.dialog({
        title: "设置按钮",
        width: 555,
        height: 400,
        html: "#button-template",
        viewModel: function (w) {
            var self = this;
            com.loadCss('/Content/css/metro/css/modern.css', parent.document);
            this.buttons = ko.observableArray();
            this.refresh = function () {
                com.ajax({
                    url: '/api/sys/menu/getmenubuttons/' + MenuCode,
                    type: 'GET',
                    async: false,
                    success: function (d) {
                        self.buttons(ko.mapping.fromJS(d)());
                    }
                });
            };
            this.refresh();
            this.checkAll = ko.observable(false);
            this.checkAll.subscribe(function (value) {
                $.each(self.buttons(), function () {
                    this.Selected(value ? 1 : 0);
                });
            });
            this.buttonClick = function (row) {
                row.Selected(row.Selected() ? 0 : 1);
            };
            this.confirmClick = function () {
                var data = utils.filterProperties($.grep(self.buttons(), function (row) {
                    return row.Selected() > 0;
                }), ['ButtonCode']);
                com.ajax({
                    url: '/api/sys/menu/editmenubuttons/' + MenuCode,
                    data: ko.toJSON(data),
                    success: function (d) {
                        com.message('success', '保存成功！');
                        self.cancelClick();
                    }
                });
            };
            this.manageClick = function () {
                com.dialog({
                    title: "管理按钮库",
                    width: 600,
                    height: 410,
                    html: "#manage-template",
                    viewModel: function (w_sub) {
                        var that = this;
                        this.grid = {
                            width: 586,
                            height: 340,
                            pagination: false,
                            pageSize: 10,
                            url: "/api/sys/menu/getbuttons",
                            queryParams: ko.observable()
                        };
                        this.cancelClick = function () {
                            w_sub.dialog('close');
                        };
                        this.gridEdit = new com.editGridViewModel(this.grid);
                        this.grid.OnAfterCreateEditor = function (editors, row) {
                            if (!row._isnew) com.readOnlyHandler('input')(editors["ButtonCode"].target, true);
                        };
                        this.grid.onClickRow = that.gridEdit.ended;
                        this.grid.onDblClickRow = that.gridEdit.begin;
                        this.grid.toolbar = [
                            { text: '新增', iconCls: 'icon-add1', handler: function () { that.gridEdit.addnew(); } }, '-',
                            { text: '编辑', iconCls: 'icon-edit', handler: that.gridEdit.begin }, '-',
                            { text: '删除', iconCls: 'icon-cross', handler: that.gridEdit.deleterow }
                        ];
                        this.confirmClick = function () {
                            if (!that.gridEdit.isChangedAndValid()) return;
                            var list = that.gridEdit.getChanges(['ButtonCode', 'ButtonName','ButtonIcon', 'ButtonSeq', 'Description']);
                            com.ajax({
                                url: '/api/sys/menu/editbutton',
                                data: ko.toJSON({ list: list }),
                                success: function (d) {
                                    that.cancelClick();
                                    self.refresh();
                                    com.message('success', '保存成功！');
                                }
                            });
                        };
                    }
                });
                
            };
            this.cancelClick = function () {
                w.dialog('close');
            };
        }
    });
};