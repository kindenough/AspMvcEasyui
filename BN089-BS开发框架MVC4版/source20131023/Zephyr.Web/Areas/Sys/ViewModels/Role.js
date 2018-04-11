/**
* 模块名：mms viewModel
* 程序名: menu.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

function viewModel() {
    var self = this;
    this.gridlist = $('#gridlist');
    this.grid = {
        size: { w: 4, h: 40 },
        url: '/api/sys/role',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            for (var i in d) d[i]._id = d[i].RoleCode;
            return {rows:d,total:d.length};
        }
    };
    this.gridEdit = new com.editGridViewModel(self.grid);
    this.grid.onDblClickRow = self.gridEdit.begin;
    this.grid.onClickRow = self.gridEdit.ended;
    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        self.gridEdit.addnew({});
    };
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        var index = self.grid.datagrid('getRowIndex', row);
        self.gridEdit.begin(index, row);
    };
    
    this.deleteClick = self.gridEdit.deleterow;
    
    this.saveClick = function () {
        var post = {};
        post.list = new com.editGridViewModel(self.gridlist).getChanges(['_id', 'RoleName', 'RoleCode', 'RoleSeq', 'Description']);
        if (self.gridEdit.ended() && post.list._changed) {
            com.ajax({
                url: '/api/sys/role/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    self.grid.queryParams({});
                }
            });
        }

    };
}

var permissionTab = function (row) {
    com.dialog({
        title: "角色授限",
        width: 800,
        height: 600,
        html: "#permission-template",
        viewModel: function (win) {
            var self = this;
            this.role = ko.mapping.fromJS(row);
            this.tab = {
                onSelect: function (title, index) {
                    if (title == '按钮权限') {
                        //取得菜单权限中的选中行，并重新加开到按钮权限列表中
                        var temp = {},data = [],panel = self.grid2.treegrid('getPanel');
                        utils.eachTreeRow(self.grid.treegrid('getData'), function (node) {
                            if (node.checked) {
                                data.push(utils.filterProperties(node, ['children', 'Description'], true));
                                temp[node.MenuCode] = node;
                            }
                        });
                        self.grid2.treegrid('loadData', data);

                       //checkbox点击处理函数
                       var checkHandler = function (obj,value) {
                           if (!obj.length) return;
                           var map = { "0": "/Content/images/checknomark.gif", "1": "/Content/images/checkmark.gif" };
                           obj.attr("src", map[value]).attr("value", value);
                           temp[obj.attr("MenuCode")]["btn_" + obj.attr("ButtonCode")] = parseInt(obj.attr("value"));
                       };

                        //注册checkbox点击事件
                        panel.find("td[field]").unbind("click").click(function () {
                            var img = $(this).find("img"), value = img.attr("value") == "1" ? "0" : "1";
                            checkHandler(img, value);
                        
                            if (img.attr("ButtonCode")== "_checkall") 
                                panel.find("img[MenuCode=" + img.attr("MenuCode") + "]").each(function () {
                                    checkHandler($(this), value);
                                });
                        });

                        //注册全选checkbox的事件
                        panel.find(".datagrid-header .icon-chk_unchecked").unbind("click").click(function () {
                            var chk = $(this),
                                value = chk.hasClass("icon-chk_checked") ? "0" : "1",
                                iconcls = chk.hasClass("icon-chk_checked") ? "icon-chk_unchecked" : "icon-chk_checked";
                            chk.removeClass("icon-chk_unchecked").removeClass("icon-chk_checked").addClass(iconcls);

                            panel.find("img").each(function () {
                                checkHandler($(this), value);
                            });
                        });
                    }
                    else if (title == '字段权限')
                    {
                        var temp = {}, data = [];
                        utils.eachTreeRow(self.grid.treegrid('getData'), function (node) {
                            if (node.checked) {
                                data.push(utils.filterProperties(node, ['children', 'Description'], true));
                                temp[node.MenuCode] = node;
                            }
                        });
                       
                        self.grid4.OnBeforeDestroyEditor(function (editors,row) {
                            temp[row.MenuCode]["AllowColumns"] = editors["AllowColumns"].target.val();
                            temp[row.MenuCode]["RejectColumns"] = editors["RejectColumns"].target.val();
                        });
                        self.grid4.treegrid('loadData', data);
                    }
                }
            };
 
            this.grid = {
                height: 460,
                width: 774,
                url: '/api/sys/menu/getenabled/'+row.RoleCode,
                idField: 'MenuCode',
                queryParams: ko.observable(),
                treeField: 'MenuName',
                singleSelect: false,
                onCheck: function (node) {
                    node.checked = true;
                },
                onUncheck: function (node) {
                    node.checked = false;
                },
                onCheckAll:function(rows){
                    utils.eachTreeRow(rows, function (node) { node.checked = true; });
                },
                onUncheckAll: function (rows) {
                    utils.eachTreeRow(rows, function (node) { node.checked = false; });
                },
                loadFilter: function (d) {
                    var formatterChk = function (ButtonCode) {
                        return function (value, row) {
                            if (value >= 0)
                                return '<img MenuCode="' + row.MenuCode + '" ButtonCode="' + ButtonCode + '" value="' + value + '" src="/Content/images/' + (value ? "checkmark.gif" : "checknomark.gif") + '"/>';
                        };
                    }
                    var cols = [[]];
                    for (var i in d.buttons)
                        cols[0].push({ field: 'btn_'+d.buttons[i].ButtonCode, width: 50, align: 'center', title: utils.formatString('<span class="icon {1}">{0}</span>', d.buttons[i].ButtonName, d.buttons[i].ButtonIcon), formatter: formatterChk(d.buttons[i].ButtonCode) });
                    self.grid2.columns(cols);

                    return utils.toTreeData(d.menus, 'MenuCode', 'ParentCode', "children");
                }
            };

            this.grid2 = {
                height: 460,
                width: 774,
                idField: 'MenuCode',
                treeField: 'MenuName',
                frozenColumns: [[
                    { field: 'MenuName', width: 150, title: '菜单' },
                    {
                        field: 'btn__checkall',
                        width: 50,
                        align: 'center',
                        title: '<span class="icon icon-chk_unchecked">全选</span>',
                        formatter: function (v, r) {
                            for (var i in r) {
                                if (i.indexOf("btn_") > -1 && r[i] > -1) {
                                    return '<img MenuCode="' + r.MenuCode + '" ButtonCode="_checkall" src="/Content/images/' + (v ? "checkmark.gif" : "checknomark.gif") + '"/>';
                                }
                            }
                        }
                    }
                ]],
                columns: ko.observableArray(),
                loadFilter: function (d) {
                    return utils.toTreeData(d, 'MenuCode', 'ParentCode', "children");
                } 
            };

            this.grid3check = function (node,value) {
                node.checked = value;
                var img = self.grid3.treegrid('getPanel').find('img[PermissionCode=' + node.PermissionCode + ']');
                value ? img.show() : img.hide();
                img.val(node.IsDefault);
            };
            this.grid3 = {
                height: 460,
                width: 774,
                url: '/api/sys/permission/GetRolePermission/' + row.RoleCode,
                idField: 'PermissionCode',
                queryParams: ko.observable(),
                treeField: 'PermissionName',
                singleSelect: false,
                columns: [[
                    {field:'chk',checkbox:true},
                    {field:'PermissionName', width:150,title:'授权名称'},
                    {field:'PermissionCode',width:100,title:'授权代码'},
                    {
                        field: 'IsDefault', width: 60, title: '是否默认', align: 'center', formatter: function (v, r) {
                        return '<img value="'+r.IsDefault+'" style="display:'+(r.checked?'':'none')+'" PermissionCode="'+ r.PermissionCode +'" src="/Content/images/' + (v ? "checkmark.gif" : "checknomark.gif") + '"/>';
                    }}
                ]],
                onCheck: function (node) {
                    self.grid3check(node, true);
                },
                onUncheck: function (node) {
                    self.grid3check(node, false);
                },
                onCheckAll: function (rows) {
                    utils.eachTreeRow(rows, function (node) { self.grid3check(node, true); });
                },
                onUncheckAll: function (rows) {
                    utils.eachTreeRow(rows, function (node) { self.grid3check(node, false); });
                },
                onLoadSuccess: function (r, d) {
                    self.grid3.treegrid('getPanel').find("td[field=IsDefault]").unbind('click').click(function (event) {
                        var img = $(this).find("img"),value = img.attr("value")=="1"?"0":"1";
                        var map = { "0": "/Content/images/checknomark.gif", "1": "/Content/images/checkmark.gif" };
                        if (value == "1")
                            self.grid3.treegrid('getPanel').find("img[PermissionCode]").attr("src", map["0"]).val(0);
                        img.attr("src", map[value]).val(value);
                        event.stopPropagation();
                    });
                },
                loadFilter: function (d) {
                    return utils.toTreeData(d, 'PermissionCode', 'ParentCode', "children");
                }
            };

            this.grid4 = {
                height: 460,
                width: 774,
                idField: 'MenuCode',
                treeField: 'MenuName',
                columns: [[
                    { field: 'MenuName', width: 150, title: '菜单' },
                    { field: 'AllowColumns', width: 270, title: '允许', editor: 'text' },
                    { field: 'RejectColumns', width: 300, title: '拒绝', editor: 'text' }
                ]],
                loadFilter: function (d) {
                    return utils.toTreeData(d, 'MenuCode', 'ParentCode', "children");
                }
            };
            this.grid4Edit = new com.editTreeGridViewModel(this.grid4);
            this.grid4.onDblClickRow = this.grid4Edit.begin;
            this.grid4.onClickRow = this.grid4Edit.ended;
            this.grid4.OnBeforeDestroyEditor = ko.observable();
            
            this.confirmClick = function () {
                var post = {menus:[],buttons:[],permissions:[],columns:[]};
                utils.eachTreeRow(self.grid.treegrid('getData'),function(node){
                    if (node.checked) {
                        //1 取得菜单权限数据  
                        post.menus.push({ MenuCode: node.MenuCode });
 
                        //2 取得按钮权限数据
                        for (var btn in node) 
                            if (btn.substr(0, 4) == 'btn_' && node[btn] == '1' && btn != 'btn__checkall')
                                post.buttons.push({ MenuCode: node.MenuCode, ButtonCode: btn.split('_')[1] });

                        //3取得列权限数据
                        if (node.AllowColumns || node.RejectColumns)
                            post.columns.push({ MenuCode: node.MenuCode, AllowColumns: node.AllowColumns, RejectColumns: node.RejectColumns });
                    }
                });

                //4 取得授权代码数据
                var panel3 = self.grid3.treegrid('getPanel');
                utils.eachTreeRow(self.grid3.treegrid('getData'), function (node) {
                    if (node.checked) {
                        var img = panel3.find("img[PermissionCode=" + node.PermissionCode + "][value=1]");
                        post.permissions.push({ PermissionCode: node.PermissionCode, IsDefault: img.length });
                    }
                });
 
                com.ajax({
                    url: '/api/sys/role/editpermission/' + row.RoleCode,
                    data: ko.toJSON(post),
                    success: function (d) {
                        self.cancelClick();
                        com.message('success', '保存成功！');
                    }
                });

            };
            this.cancelClick = function () {
                win.dialog('close');
            };
           
        }
    });
};

var memberDialog = function (row) {
    var users = data.users;
    var organizes = data.organizes;

    com.dialog({
        title: "管理成员",
        width: 500,
        height: 400,
        html: "#members-template",
        viewModel: function (win) {
            var self = this;
            this.members = ko.observableArray([]);
            this.memberText = function (item) {
                return utils.formatString('[{0}] {1} | {2}', item.MemberType == 'user' ? '用户' : '机构', item.MemberName, item.MemberCode);
            };
            com.ajax({
                type: 'GET',
                url: '/api/sys/role/GetRoleMembers/' + row.RoleCode,
                success: function (d) {
                    self.members(d);
                }
            });
            this.RoleName = utils.formatString("{0} ({1})", row.RoleName, row.RoleCode);
            this.Description = row.Description || " ";
            this.addClick = function () {
                com.dialog({
                    title: "选择成员",
                    width: 600,
                    height: 500,
                    html: "#choose-members-template",
                    viewModel: function (w) {
                        var that = this;
                        for (var i in users) users[i].Checked = false;
                        for (var i in organizes) organizes[i].Checked = false;
                        this.users = ko.mapping.fromJS(users);
                        this.organizes = ko.mapping.fromJS(organizes);
                        this.checkAllUser = ko.observable(false);
                        this.checkAllUser.subscribe(function (b) {
                            var list = that.users();
                            for (var i in list)
                                list[i].Checked(b);
                        });
                        this.checkAllOrganize = ko.observable(false);
                        this.checkAllOrganize.subscribe(function (b) {
                            var list = that.organizes();
                            for (var i in list)
                                list[i].Checked(b);
                        });
                        this.confirmClick = function () {
                            var userlist = this.users(),organizelist=this.organizes(),memberMap = {},members = ko.toJS(self.members);
                            for (var j in members)
                                memberMap[members[j].MemberType + '|' + members[j].MemberCode] = true;

                            for (var i in userlist)
                                if (userlist[i].Checked()) {
                                    var item = { MemberName: userlist[i].UserName(), MemberCode: userlist[i].UserCode(), MemberType: 'user' };
                                    if (!memberMap[item.MemberType + '|' +item.MemberCode]) self.members.push(item);
                                }

                            for (var i in organizelist)
                                if (organizelist[i].Checked()) {
                                    var item = { MemberName: organizelist[i].OrganizeName(), MemberCode: organizelist[i].OrganizeCode(), MemberType: 'organize' };
                                    if (!memberMap[item.MemberType + '|' +item.MemberCode]) self.members.push(item);
                                }
                            this.cancelClick();
                        };
                        this.cancelClick = function () { w.dialog('close'); };
                    }
                });
            };

            this.selectValue = ko.observable();
            this.deleteClick = function () {
                if (this.selectValue()) {
                    self.members.remove(this.selectValue());
                }
            };
            this.clearClick = function () {
                self.members([]);
            };
            this.confirmClick = function () {
                com.ajax({
                    url: '/api/sys/role/editrolemembers/' + row.RoleCode,
                    data: ko.toJSON(self.members),
                    success: function (d) {
                        com.message('success', '保存成功！');
                        self.cancelClick();
                    }
                });
            };
            this.cancelClick = function () { win.dialog('close'); };
        }
        });

};