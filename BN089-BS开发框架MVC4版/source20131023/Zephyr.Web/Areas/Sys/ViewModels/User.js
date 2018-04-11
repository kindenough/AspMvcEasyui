/**
* 模块名：mms viewModel
* 程序名: menu.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var viewModel = function(){
    var self = this;
    this.refreshClick=function(){
        window.location.reload();
    };
    this.addClick=function(){
        self.gridEdit.addnew({});
    };
    this.editClick=function(){
        var row = self.grid.datagrid('getSelected');
        var index = self.grid.datagrid('getRowIndex',row);
        self.gridEdit.begin(index,row);
    };
    this.deleteClick=function(){
        self.gridEdit.deleterow();
    };
    this.grid = {
        size: { w: 189, h: 40 },
        url: "/api/sys/user",
        queryParams: ko.observable(),
        pagination: true,
        customLoad: false
    };
    this.gridEdit = new com.editGridViewModel(this.grid);
    this.grid.onDblClickRow = self.gridEdit.begin;
    this.grid.onClickRow = self.gridEdit.ended;
    this.grid.OnAfterCreateEditor =function(editors,row){
        if (row._isnew == undefined ) 
            com.readOnlyHandler('input')(editors.UserCode.target,true);
    };
    this.tree = {
        method:'GET',
        url:'/api/sys/organize',
        loadFilter:function(d){
            var filter = utils.filterProperties(d,['OrganizeCode as id','OrganizeName as text','ParentCode as pid']);
            return utils.toTreeData(filter,'id','pid','children');
        },
        onSelect:function(node){
            self.grid.queryParams({OrganizeCode:node.id});
        }
    };
    this.saveClick = function () {
        self.gridEdit.ended();
        var post = {};
        post.list = self.gridEdit.getChanges(['UserCode', 'UserName', 'MaterialCode', 'Description', 'IsEnable']);
        if (self.gridEdit.ended() && post.list._changed) {
            com.ajax({
                url: '/api/sys/user/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.passwordClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', '请先选择一个用户！');
        com.message('confirm', '确定要把选中用户的密码重置为<span style="color:red">1234</span>吗？', function (b) {
            if (b) {
                com.ajax({
                    type: 'POST',
                    url: '/api/sys/user/postresetpassword/' + row.UserCode,
                    success: function () {
                        com.message('success', '密码已重置成功！');
                    }
                });
            }
        });
    };
};

var setOrganize = function (row) {
    if (row._isnew) 
        return com.message('warning', '请先保存再设置机构！');
  
    com.dialog({
        title: "设置机构",
        width: 600,
        height: 450,
        html:"#setorganize-template",
        viewModel:function(w){
            var that = this;
            this.UserName = ko.observable(row.UserName);
            this.graph = ko.observable();
            com.ajax({
                type: 'GET',
                url: '/api/sys/user/getorganizewithusercheck/' + row.UserCode,
                success: function (d) {
                    var treeData = utils.toTreeData(d, "OrganizeCode", "ParentCode", "children");
                    that.graph(renderTreeGraph(treeData)[0].outerHTML);
                    w.find(".td-node").each(function () {
                        var checked = $(this).data("node").Checked == "true";
                        $(this).prepend(com.formatCheckbox(checked)).css({ "background-color": "#f6f6ff", "color": !checked ? "" : "#FF0000" });
                    }).click(function () {
                        var $this = $(this), checked = $this.find("img").attr("value") == "true";
                        var img2 = $(com.formatCheckbox(!checked));
                        $this.find("img").attr("src", img2.attr("src")).attr("value", img2.attr("value"));
                        $this.css({ "background-color": "#f6f6ff", "color": checked ? "" : "#FF0000" });
                    });
                }
            });

            this.confirmClick = function () {
                var organizes = [];
                w.find("img[value=true]").each(function () {
                    organizes.push({OrganizeCode:$(this).parent().data("node").OrganizeCode});
                });
                com.ajax({
                    url: '/api/sys/user/edituserorganizes/' + row.UserCode,
                    data: ko.toJSON(organizes),
                    success: function (d) {
                        that.cancelClick();
                        com.message('success', '保存成功！');
                    }
                });
            };
            this.cancelClick = function () {
                w.dialog('close');
            };
        }
    });
};
var setRole = function (row) {
    if (row._isnew)
        return com.message('warning', '请先保存再设置角色！');

    com.dialog({
        title: "设置角色",
        width: 600,
        height: 450,
        html: "#setrole-template",
        viewModel: function (w) {
            var thisRole = this;
            this.UserName = ko.observable(row.UserName);
            com.loadCss('/Content/css/metro/css/modern.css', parent.document);
            com.ajax({
                type: 'GET',
                url: '/api/sys/user/getrolewithusercheck/' + row.UserCode,
                success: function (d) {
                    var ul = w.find(".listview");
                    for (var i in d)
                        ul.append(utils.formatString('<li role="{0}" class="{2}">{1}</li>',d[i].RoleCode,d[i].RoleName,d[i].Checked=='true'?'selected':''));
                    ul.find("li").click(function () {
                        if ($(this).hasClass('selected'))
                            $(this).removeClass('selected');
                        else
                            $(this).addClass('selected');
                    });
                }
            });
            this.confirmClick = function () {
                var roles = [];
                w.find("li.selected").each(function () {
                    roles.push({ RoleCode: $(this).attr('role') });
                });
                com.ajax({
                    url: '/api/sys/user/edituserroles/' + row.UserCode,
                    data: ko.toJSON(roles),
                    success: function (d) {
                        thisRole.cancelClick();
                        com.message('success', '保存成功！');
                    }
                });
            };
            this.cancelClick = function () {
                w.dialog('close');
            };
        }
    });
};

var userSetting = function (row) {
    if (row._isnew)
        return com.message('warning', '请先保存再修改用户设置！');

    com.dialog({
        title: "用户设置",
        width: 600,
        height: 410,
        html: "#manage-template",
        viewModel: function (w) {
            var that = this;
            this.grid = {
                width: 586,
                height: 340,
                pagination: false,
                pageSize: 10,
                url: "/api/sys/user/getsettinglist/" + row.UserCode,
                queryParams: ko.observable()
            };
            this.cancelClick = function () {
                w.dialog('close');
            };
            this.gridEdit = new com.editGridViewModel(this.grid);
            this.grid.onClickRow = that.gridEdit.ended;
            this.grid.onDblClickRow = that.gridEdit.begin;
            this.grid.toolbar = [
                { text: '新增', iconCls: 'icon-add1', handler: function () { that.gridEdit.addnew({ UserCode: row.UserCode }); } }, '-',
                { text: '编辑', iconCls: 'icon-edit', handler: that.gridEdit.begin }, '-',
                { text: '删除', iconCls: 'icon-cross', handler: that.gridEdit.deleterow }
            ];
            this.confirmClick = function () {
                if (!that.gridEdit.isChangedAndValid()) return;
                var list = that.gridEdit.getChanges(['ID', 'UserCode', 'SettingCode', 'SettingName', 'SettingValue', 'Description']);
                com.ajax({
                    url: '/api/sys/user/editusersetting',
                    data: ko.toJSON({ list: list }),
                    success: function (d) {
                        that.cancelClick();
                        com.message('success', '保存成功！');
                    }
                });
            };
        }
    });
};
 
ko.bindingViewModel(new viewModel());