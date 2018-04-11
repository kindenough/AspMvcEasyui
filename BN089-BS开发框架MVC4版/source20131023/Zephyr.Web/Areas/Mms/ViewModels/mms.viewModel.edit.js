/**
* 模块名：mms viewModel
* 程序名: mms.viewModel.edit.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var mms = mms || {};
mms.viewModel = mms.viewModel || {};
 
mms.viewModel.edit = function (data) {
    var self = this;

    this.dataSource = data.dataSource;                          //下拉框的数据源
    this.urls = data.urls;                                      //api服务地址
    this.resx = data.resx;                                      //中文资源
    this.scrollKeys = ko.mapping.fromJS(data.scrollKeys);       //数据滚动按钮（上一条下一条）
    this.form = ko.mapping.fromJS(data.form||data.defaultForm); //表单数据
    this.setting = data.setting;
    this.defaultRow = data.defaultRow;                          //默认grid行的值
    this.defaultForm = data.defaultForm;                        //主表的默认值
    this.readonly = ko.computed(function () {return self.form.ApproveState() == "passed";});

    this.grid = {                                           
        size: { w: 6, h: 177 },
        pagination: false,
        remoteSort: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };
    this.gridEdit = new com.editGridViewModel(self.grid);
    this.grid.onClickRow = function () { //this.grid.onDblClickRow
        if (self.readonly()) return;
        self.gridEdit.begin();
    };
    this.grid.toolbar = "#gridtb";
    this.addRowClick = function () {
        if (self.readonly()) return;
        mms.com.selectMaterial(self, { _xml: 'mms.material_dict' });
    };
    this.removeRowClick = function () {
        if (self.readonly()) return;
        self.gridEdit.deleterow();
        if (self.form.TotalMoney)
            mms.com.calcTotalMoneyWhileRemoveRow(self, "Money", "TotalMoney");
    };
    this.rejectClick = function () {
        if (self.readonly()) return;
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
        if (self.readonly()) return;
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {           //传递到后台的数据
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
            list: self.gridEdit.getChanges(self.setting.postListFields)
        };
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
        self.gridEdit.ended();
        var updateArray = ['ApproveState', 'ApproveRemark'];
        mms.com.auditDialog(this.form, function (d) {
            com.ajax({
                type: 'POST',
                url: self.urls.audit + self.scrollKeys.current(),
                data: JSON.stringify(d),
                success: function () {
                    com.message('success', d.status == "passed" ? self.resx.auditPassed : self.resx.auditReject);
                    if (data.form)
                        for (var i in updateArray) data.form[updateArray[i]] = self.form[updateArray[i]]();
                },
                error: function (e) {
                    if (data.form)
                        for (var i in updateArray) self.form[updateArray[i]](data.form[updateArray[i]]);
                    com.message('error',e.responseText);
                }
            });
        });
    };
    this.approveButton = {
        iconCls: ko.computed(function () { return self.form.ApproveState() == "passed" ? "icon-user-reject" : "icon-user-accept"; }),
        text: ko.computed(function () { return self.form.ApproveState() == "passed" ? "反审" : "审核"; })
    };
    this.printClick = function () {
        com.openTab('打印报表', '/report?area=mms&rpt=' + self.urls.report + '&BillNo=' + self.form.BillNo(), 'icon-printer_color');
    };
};
