/**
* 模块名：mms viewModel
* 程序名: Permission.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
 
function viewModel() {
    var self = this;
    this.grid = {
        size: { w: 4, h: 40 },
        url: '/api/sys/permission',
        idField: '_id',
        queryParams: ko.observable(),
        treeField: 'PermissionName',
        loadFilter: function (d) {
            d = utils.copyProperty(d.rows || d, ["PermissionCode"], ["_id"], false);
            return utils.toTreeData(d, '_id', 'ParentCode', "children");
        } 
    };
    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        if (self.grid.onClickRow()) {
            var row = { _id: utils.uuid(), PermissionCode: '', PermissionName: '' };
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
            var treeData = JSON.parse(JSON.stringify(self.grid.treegrid('getData')).replace(/_id/g, "id").replace(/PermissionName/g, "text"));
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
        }
    };
   
    this.grid.OnBeforeDestroyEditor = function (editors, row) {
        row.ParentName = editors['ParentCode'].target.combotree('getText');
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
        post.list = new com.editTreeGridViewModel(self.grid).getChanges(['_id', 'PermissionCode', 'PermissionName', 'ParentCode']);
        if (self.grid.onClickRow() && post.list._changed) {
            com.ajax({
                url: '/api/sys/permission/edit',
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
 