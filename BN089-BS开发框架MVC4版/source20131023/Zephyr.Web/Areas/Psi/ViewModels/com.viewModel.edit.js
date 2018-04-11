/**
* 模块名：psi viewModel
* 程序名: com.editPageViewModel.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

com.viewModel = com.viewModel || {};
com.viewModel.edit = function (data) {
    var self = this;

    this.dataSource = data.dataSource;                      //下拉框的数据源
    this.urls = data.urls;
    this.resx = data.resx;
    this.scrollKeys = ko.mapping.fromJS(data.scrollKeys);   //数据滚动按钮（上一条下一条）
    this.form = ko.mapping.fromJS(data.form||data.defaultForm);               //表单数据
    this.setting = data.setting;
    this.defaultRow = data.defaultRow;
 
    this.grid = {                                           //表格设置
        size: { w: 6, h: 177 },
        pagination: false,
        remoteSort: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };
    this.gridEdit = new com.editGridViewModel(self.grid);
    this.grid.onDblClickRow = self.gridEdit.begin;
    this.grid.onClickRow = self.gridEdit.ended;
    this.grid.toolbar = [
    {
        text: '删除材料',
        iconCls: 'icon-remove',
        handler: self.gridEdit.deleterow
    }];

    this.rejectClick = function () {
        ko.mapping.fromJS(data.form, {}, self.form);
        self.gridEdit.reject();
        com.message('success', self.resx.rejected);
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
            url: self.urls.getmaster + id,
            success: function (d) {
                ko.mapping.fromJS(d, {}, self);
                ko.mapping.fromJS(d, {}, data);
            }
        });
        self.grid.url(self.urls.getdetail + id);
        self.grid.datagrid('loaded');
    };
    this.saveClick = function () {
        var post = {};
        post.form = com.formChanges(self.form, data.form, self.setting.postFormKeys);
        post.list = self.gridEdit.getChanges(self.setting.postListFields);
        if ((self.gridEdit.ended() && com.formValidate()) && (post.form._changed || post.list._changed)) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', self.resx.editSuccess);
                    ko.mapping.fromJS(post.form, {}, data.form); //更新旧值
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.auditClick = function () {
        com.auditDialog(function (d) {
            com.ajax({
                type: 'POST',
                url: self.urls.audit + self.scrollKeys.current(),
                data: JSON.stringify(d),
                success: function () {
                    com.message('success', self.resx.auditSuccess);
                }
            });
        });
    };
    this.printClick = function () {
        com.openTab('打印报表', '/report?p1=0002&p2=2012-1-1', 'icon-printer_color');
    };
};
