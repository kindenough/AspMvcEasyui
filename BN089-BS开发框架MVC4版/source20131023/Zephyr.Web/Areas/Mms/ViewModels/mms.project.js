/**
* 模块名：mms viewModel
* 程序名: project.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

function viewModel(data) {
    var self = this;
    //this.dataSource = data.dataSource; //下拉框的数据源
    //this.scrollKeys = ko.mapping.fromJS(data.scrollKeys);//数据滚动按钮（上一条下一条）
    this.form = ko.mapping.fromJS(data.form); //表单数据

    this.tree = {
        method: 'GET',
        url: '/api/mms/project/getlist',
        loadFilter: function (d) {
            var filter = utils.filterProperties(d, ['id', 'text', 'pid']);
            return utils.toTreeData(filter, 'id', 'pid', 'children');
        },
        onSelect: function (node) {
            self.grid.url("/api/mms/project/getsub/" + node.id);

            com.ajax({
                type: 'GET',
                url: "/api/mms/project/GetProjectInfo/" + node.id,
                success: function (d) {
                    ko.mapping.fromJS(d.form, {}, self.form);
                    ko.mapping.fromJS(d.form, {}, data.form);
                }
            });
        },
        onLoadSuccess: function () {
            var node = $(this).tree('find', self.form.ProjectCode());
            $(this).tree('select', node.target);
        }
    };

    this.grid = {   //表格设置
        method: 'GET',
        url: ko.observable('/api/mms/project/getsub/' + self.form.ProjectCode()),
        idField: 'BuildPartCode',
        treeField: 'BuildPartName',
        parentField:'ParentCode'
    };
   
    this.gridEdit = new com.editTreeGridViewModel(this.grid);
    this.grid.onDblClickRow = self.gridEdit.begin;
    this.grid.onClickRow = self.gridEdit.ended;
    this.grid.toolbar = [{
        text: '新增部位',
        iconCls: 'icon-add',
        handler: function () {
            var node = self.grid.treegrid('getSelected');
            var parent = node == null ? null : node.BuildPartCode;

            com.ajax({
                type: 'GET',
                url: '/api/mms/project/GetNewBuildPart',
                data: { projectCode: self.form.ProjectCode(), parentCode: parent },
                success: function (row) {
                      self.gridEdit.addnew(row);
                }
            });
        }
    }, '-', {
        text: '删除部位',
        iconCls: 'icon-remove',
        handler: self.gridEdit.deleterow
    }];

    this.rejectClick = function () {
        ko.mapping.fromJS(data.form,{}, self.form);
        self.gridEdit.reject();
        com.message('success', '已撤消修改！');
    };
    this.firstClick = function () {
        self.scrollTo(self.scrollKeys.first());
    };
    this.previousClick = function () {
        self.scrollTo(self.scrollKeys.previous());
    };
    this.nextClick = function () {
        self.scrollTo(self.scrollKeys.next());
    };
    this.lastClick = function () {
        self.scrollTo(self.scrollKeys.last());
    };
    this.scrollTo = function (id) {
        if (id == self.scrollKeys.current()) return;
        com.setLocationHashId(id);
        com.ajax({
            type: 'GET',
            url: '/api/mms/send/geteditmaster/' + id,
            success: function (d) {
                ko.mapping.fromJS(d, {}, self);
                ko.mapping.fromJS(d, {}, data);
            }
        });
        self.grid.url("/api/mms/send/getdetail/" + id);
        self.grid.datagrid('loaded');
    };
    this.saveClick = function (a, b, c, d) {
        var post = {};
        post.form = com.formChanges(self.form, data.form, ["ProjectCode"]);
        post.list = self.gridEdit.getChanges(['ProjectCode', 'BuildPartCode', 'BuildPartName', 'ParentCode', 'PartAttr', 'NodeControl', 'ActualBeginTime', 'ActualEndTime', 'ImagePart', 'Remark']);
        if ((self.gridEdit.ended() && com.formValidate()) && (post.form._changed || post.list._changed)) {
            com.ajax({
                url: '/api/mms/project/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    ko.mapping.fromJS(post.form, {}, data.form); //更新旧值
                    self.gridEdit.accept();
                    self.tree.$element().tree('reload');//保存完刷新项目树
                }
            });
        }
    };
    this.auditClick = function () {
        com.auditDialog(function (d) {
            com.ajax({
                type: 'POST',
                url: '/api/mms/send/audit/' + self.scrollKeys.current(),
                data: JSON.stringify(d),
                success: function () {
                    com.message('success', '审核成功！');
                }
            });
        });
    };
    this.printClick = function () {
        com.openTab('打印报表', '/report?p1=0002&p2=2012-1-1', 'icon-printer_color');
    };
    this.refreshClick = function () {
        window.location.reload();
    };
    //新增按钮
    this.addClick = function () {
        com.ajax({
            type: 'GET',
            url: '/api/mms/project/getnewproject',
            success: function (d) {
                ko.mapping.fromJS(d, {}, self.form);
                ko.mapping.fromJS(d, {}, data.form);
                self.grid.url("/api/mms/project/getsub/" + d.ProjectCode);
            }
        });
    };
};
