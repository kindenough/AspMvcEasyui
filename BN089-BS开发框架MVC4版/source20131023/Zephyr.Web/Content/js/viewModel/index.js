var wrapper = {};

//设置
wrapper.settings = {
    homeTabTitle: '我的桌面',
    homeTabUrl: '/mms',
    maxTabCount: 10
};

//初始化
wrapper.init = function () {
    com.ajax({ type: 'GET', url: 'api/sys/menu', success: wrapper.initMenu });
    
    $('.loginOut').click(wrapper.logout);
    $('.changepwd').click(wrapper.changePassword);
    $('.myconfig').click(wrapper.mysettings);
    $('.swich_project').click(wrapper.changeProject).html("当前项目：" + com.cookie('CurrentProjectName'));
    $('#notity').jnotifyInizialize({ oneAtTime: true, appendType: 'append' }).css({ 'position': 'absolute', '*top': '2px', 'left': '50%', 'margin': '20px 0px 0px -120px', '*margin': '0px 0px 0px -120px', 'width': '240px', 'z-index': '9999' });
    $('#closeMenu').menu({ onClick: wrapper.rightMenuClick });

    $('#tabs').tabs({
        tools: [{ iconCls: 'icon-arrow_refresh', handler: wrapper.tabRefresh },
                { iconCls: 'icon-screen_full', handler: wrapper.setFullScreen },
                { iconCls: 'panel-tool-close', handler: wrapper.tabClose }],
        onContextMenu: wrapper.tabContextMenu,
        onClose: wrapper.setLocationHash,
        onSelect: wrapper.setLocationHash
    });
};

wrapper.initLocationHash = function (data) {
    var subUrl = location.hash.replace('#!', '');
    $.each(data, function () {
        var s = this.URL.replace('.aspx', '');
        if (this.URL && this.URL != '#' && (subUrl == s || subUrl.indexOf(s + "/") > -1))
            wrapper.addTab(this.MenuName, subUrl, this.IconClass);
    });
};

//事件
wrapper.tabContextMenu = function (e, title) {
    $('#closeMenu').menu('show', {left: e.pageX,top: e.pageY});
    $('#tabs').tabs('select', title);
    e.preventDefault();
};

wrapper.changePassword = function () {
    com.dialog({
        title: "&nbsp;修改密码",
        iconCls: 'icon-key',
        width: 320,
        height: 204,
        html: "#password-template",
        viewModel: function (w) {
            w.find("[name=UserCode]").val("lhs");
            w.find("#pwd_confirm").click(function () { w.dialog('close'); });
            w.find("#pwd_close").click(function () { w.dialog('close'); });
        }
    });
};

wrapper.mysettings = function () {
    wrapper.addTab("个人设置", "/sys/config", "icon icon-wrench_orange");
};

wrapper.changeProject = function () {
    var self = this;
    $("#w").data("lookup", { lookupType: 'project', valueTitle: '项目编码', textTitle: '项目名称' }).window({
        title: '&nbsp;切换项目'
        , width: 600
        , height: 420
        , iconCls: 'icon-flag_france'
        , modal: true
        , collapsible: false
        , minimizable: false
        , maximizable: true
        , closable: true
        , content: "<iframe id='frm_win_project' src='/plugins/lookup?r=" + Math.random() + "' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>" //frameborder="0" for ie7
        , onClose: function () {
            var rtnValue = $(this).data("returnValue");
            if (rtnValue) {
                $(self).find(".l-btn-text").html("当前项目：" + rtnValue.text);
                com.cookie('CurrentProject', rtnValue.value);
            }
        }
    });
};

wrapper.logout = function () {
    $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {
        if (r) location.href = '/Login/Logout';
    });
};

wrapper.setFullScreen = function () {
    var that = $(this);
    if (that.find('.icon-screen_full').length) {
        that.find('.icon-screen_full').removeClass('icon-screen_full').addClass('icon-screen_actual');
        $('[region=north],[region=west]').panel('close')
        var panels = $('body').data().layout.panels;
        panels.north.length = 0;
        panels.west.length = 0;
        if (panels.expandWest) {
            panels.expandWest.length = 0;
            $(panels.expandWest[0]).panel('close');
        }
        $('body').layout('resize');
    } else if ($(this).find('.icon-screen_actual').length) {
        that.find('.icon-screen_actual').removeClass('icon-screen_actual').addClass('icon-screen_full');
        $('[region=north],[region=west]').panel('open');
        var panels = $('body').data().layout.panels;
        panels.north.length = 1;
        panels.west.length = 1;
        if ($(panels.west[0]).panel('options').collapsed) {
            panels.expandWest.length = 1;
            $(panels.expandWest[0]).panel('open');
        }
        $('body').layout('resize');
    }
};

wrapper.rightMenuClick = function (item) {
    var $tab = $('#tabs');
    var currentTab = $tab.tabs('getSelected');
    var titles = wrapper.getTabTitles($tab);

    switch (item.id) {
        case "refresh":
            var src = $(currentTab.panel('options').content).attr('src');
            $tab.tabs('update', { tab: currentTab, options: { content: wrapper.createFrame(src) } });
            break;
        case "close":
            var currtab_title = currentTab.panel('options').title;
            $tab.tabs('close', currtab_title);
            break;
        case "closeall":
            $.each(titles, function () {
                if (this != wrapper.settings.homeTabTitle)
                    $tab.tabs('close', this);
            });
            break;
        case "closeother":
            var currtab_title = currentTab.panel('options').title;
            $.each(titles, function () {
                if (this != currtab_title && this != wrapper.settings.homeTabTitle)
                    $tab.tabs('close', this);
            });
            break;
        case "closeright":
            var tabIndex = $tab.tabs('getTabIndex', currentTab);
            if (tabIndex == titles.length - 1) {
                alert('亲，后边没有啦 ^@^!!');
                return false;
            }
            $.each(titles, function (i) {
                if (i > tabIndex && this != wrapper.settings.homeTabTitle)
                    $tab.tabs('close', this);
            });

            break;
        case "closeleft":
            var tabIndex = $tab.tabs('getTabIndex', currentTab);
            if (tabIndex == 1) {
                alert('亲，前边那个上头有人，咱惹不起哦。 ^@^!!');
                return false;
            }
            $.each(titles, function (i) {
                if (i < tabIndex && this != wrapper.settings.homeTabTitle)
                    $tab.tabs('close', this);
            });
            break;
        case "exit":
            $('#closeMenu').menu('hide');
            break;
    }

};

//方法
wrapper.initSettings = function (settings) {
    wrapper.settings = $.extend(wrapper.settings, settings);
};

wrapper.initMenu = function (d) {
    if (!d || !d.length) {
        $.messager.alert("系统提示", "<font color=red><b>您没有任何权限！请联系管理员。</b></font>", "warning", function () { location.href = '/login'; });
        return;
    }

    $('body').data('menulist', d);
    var visibleMenu = $.grep(d, function (row) {return row.IsVisible;});
    var menus = utils.toTreeData(visibleMenu, 'MenuCode', 'ParentCode', 'children');

    switch (wrapper.settings.navigation) {
        case "tree":
            wrapper.menuTree(menus);
            break;
        case "menubutton":
            wrapper.menuButton(menus);
            break;
        case "accordion":
            wrapper.menuAccordion(menus);
            break;
        default:
            wrapper.menuAccordion(menus);
            break;

    }
    wrapper.initLocationHash(d);
    $("#home").html(wrapper.createFrame(wrapper.settings.homeTabUrl));
};
 
wrapper.tabRefresh = function (url) {
    var $tab = $("#tabs");
    var currentTab = $tab.tabs('getSelected');
    var src = $(currentTab.panel('options').content).attr('src');
    if (typeof src === 'string') src = url;
    $tab.tabs('update', { tab: currentTab, options: { content: wrapper.createFrame(src) } })
};

wrapper.tabClose = function () {
    if (confirm('确认要关闭所有窗口吗？')) 
        wrapper.rightMenuClick({ id: 'closeall' });
};

wrapper.setLocationHash = function(){
    try {
        var $obj = $('#tabs');
        var src = '', tabs = $obj.data().tabs.tabs;
        var tab = $obj.tabs('getSelected');

        var fnSrc = function (tab) {
            var iframe = tab.find('iframe');
            return iframe.length ? iframe[0].src.replace(location.host, '').replace('http://', '').replace('.aspx', '') : '';
        };

        if (tab) {
            src = fnSrc(tab);
            if (src) window.location.hash = '!' + src;   //如果src没有，就不设置，case在f5刷新时出现
            if (src == homeUrl) window.location.hash = '';
        }
        else {
            src = fnSrc(tabs[tabs.length - 1]); //关闭tabs时，当前tab为空
            window.location.hash = '!' + src;
        }
    }
    catch (e) { }
}
 
wrapper.createFrame = function (url) {
    return '<iframe scrolling="auto" frameborder="0"  style="width:100%;height:100%;" src="' + url + '" ></iframe>';
}

wrapper.openTabHandler = function ($tab,hasTab, subtitle,url,icon) {
    if (!hasTab) {
        $tab.tabs('add', {title: subtitle,content: wrapper.createFrame(url),closable: true,icon: icon});
    } else {
        $tab.tabs('select', subtitle);
        wrapper.tabRefresh(url);   //选择TAB时刷新页面
    }
    wrapper.setLocationHash();
};
 
wrapper.getTabTitles = function ($tab) {
    var titles = [];
    var tabs = $tab.tabs('tabs');
    $.each(tabs, function () { titles.push($(this).panel('options').title); });
    return titles;
};

wrapper.addTab = function (subtitle, url, icon) {
    if (!url|| url == '#') return false;
    var $tab = $('#tabs');
    var tabCount = $tab.tabs('tabs').length;
    var hasTab = $tab.tabs('exists', subtitle);
    if ((tabCount <= wrapper.settings.maxTabCount) || hasTab) 
        wrapper.openTabHandler($tab, hasTab, subtitle, url, icon);
    else
        $.messager.confirm("系统提示", '<b>您当前打开了太多的页面，如果继续打开，会造成程序运行缓慢，无法流畅操作！</b>', function (b) {
            if (b)
                wrapper.openTabHandler($tab, hasTab, subtitle, url, icon);
        });
};

//菜单生成
wrapper.menuAccordion = function (menus) {
    var $obj = $('#wnav');
    $obj.accordion({ animate: false, fit: true, border: false });
    $.each(menus, function () {
        var html = '<ul>';
        var temple = '<li><div><a ref="{0}" href="javascript:void(0)" rel="{1}"><span class="icon {2}">&nbsp;</span><span class="nav">{3}</span></a></div></li>';
        $.each(this.children || [], function () {
            html += utils.formatString(temple, this.MenuCode, this.URL, this.IconClass, this.MenuName);
        });
        html += '</ul>';

        $obj.accordion('add', {
            title: this.MenuName,
            content: html,
            iconCls: 'icon ' + this.IconClass,
            border: false
        });
    });

    var panels = $obj.accordion('panels');
    $obj.accordion('select', panels[0].panel('options').title);

    $obj.find('li').click(function () {
        $obj.find('li div').removeClass("selected");
        $(this).children('div').addClass("selected");

        var link = $(this).find('a');
        var title = link.children('.nav').text();
        var url = link.attr("rel");
        var code = link.attr("ref");
        var icon = link.children('.icon').attr('class');

        wrapper.addTab(title, url, icon);
    }).hover(function () {
        $(this).children('div').addClass("hover");
    }, function () {
        $(this).children('div').removeClass("hover");
    });
};

wrapper.menuTree = function (menus) {
    var settings = { data: { key: { name: "MenuName", url: "URL" } }, callback: { onClick: function (event, treeId, node) { wrapper.addTab(node.MenuName, node.URL, node.IconClass); } } };
    var $obj = $('#wnav').addClass("ztree");
    if (menus.length > 0) menus[0].open = true;
    $.fn.zTree.init($obj, settings, menus);
};

wrapper.menuButtonChild = function (n) {
    var str = '';
    $.each(n.children, function (j, o) {
        if (o.children) {
            str += '<div>';
            str += '<span iconCls="' + o.IconClass + '">' + o.MenuName + '</span><div style="width:120px;">';
            str = wrapper.menuButtonChild(o);
            str += '</div></div>';
        } else
            str += '<div iconCls="' + o.IconClass + '" id="' + o.URL + '">' + o.MenuName + '</div>';
    });
    return str;
}

wrapper.menuButton = function (menus) {
    var menulist = "";
    var childMenu = '';
    $.each(menus, function (i, n) {
        menulist += utils.formatString('<a href="javascript:void(0)" id="mb{0}" class="easyui-menubutton" menu="#mm{0}" iconCls="{1}">{2}</a>',
            (i + 1), n.IconClass, n.MenuName);
 
        if ((n.children||[]).length > 0) {
            childMenu += '<div id="mm' + (i + 1) + '" style="width:120px;">';
            childMenu += wrapper.menuButtonChild(n);
            childMenu += '</div>';
        }
    });

    $('#wnav').append(menulist).append(childMenu);

    $('#wnav').css({ 'float': 'left', 'width': '100%', 'height': '30px', 'padding': '3px 0px 0px 20px', 'background': '#6ABEFA url(/common/images/datagrid_title_bg.png)' });

    if (wrapper.settings.theme == 'gray') {
        $('#wnav').css('background', 'url(/Content/js/jquery-easyui-1.3.1/themes/gray/images/tabs_enabled.gif)');
    }
 
    var northPanel = $('body').layout('panel', 'north');
    northPanel.panel('resize', { height: 103 });

    $('body').layout('resize');

    var mb = $('#wnav .easyui-menubutton').menubutton();
    $.each(mb, function (i, n) {
        $($(n).menubutton('options').menu).menu({
            onClick: function (item) {
                var tabTitle = item.text;
                var url = item.id;
                var icon = item.iconCls;
                wrapper.addTab(tabTitle, url, icon);
                return false;
            }
        });
    });
};
  
$(wrapper.init);