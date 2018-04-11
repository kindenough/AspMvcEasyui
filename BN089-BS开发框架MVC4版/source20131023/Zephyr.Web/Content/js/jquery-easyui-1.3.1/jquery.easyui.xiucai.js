(function ($) {
    $.fn.hWindow = function (options) {
        var self = this;
        var defaults = {
            width: 500, 			//宽度
            height: 400, 		//高度
            iconCls: '', 		//图标class
            collapsible: false, 	//折叠
            closable: true, //显示右上角关闭按钮
            minimizable: false, 	//最小化
            maximizable: false, 	//最大化
            resizable: false, 	//改变窗口大小
            title: '窗口标题', 	//窗口标题
            modal: true, 		//模态	
            draggable: true, //允许拖动
            submit: function () {
                alert('写入执行的代码。');
            },
            html: '',
            center: true,         //每次弹出窗口居中
            url: '',          //要加载文件的地址
            showclosebtn: true, //显示关闭按钮
            closeText: '取消', // 默认关闭按钮显示文本
            okText: '确定', //默认提交按钮显示文本
            onload: function () {
                //加载文件完成后，执行的函数
            },
            max: false //是否最大化窗口
        }
        var options = $.extend(defaults, options);
        var win_width = $(window).width();
        var win_height = $(window).height();

        if (options.max) {
            options.width = win_width - 20;
            options.height = win_height - 20;
        }

        var _top = (win_height - options.height) / 2;
        var _left = (win_width - options.width) / 2;



        var html = options.html;
        $.extend(options, {
            top: _top, left: _left, content: buildWindowContent(html, options.submit, options.url), onBeforeClose: function () {
                $(this).find(".combo-f").each(function () {
                    var panel = $(this).data().combo.panel;
                    panel.panel("destroy");
                });
                $(this).empty();

                $('body .validatebox-tip').remove();
            }
        });
        $(self).window(options).window('open');

        //$(self).keyup(function (e) {
        //    if (e.keyCode == 27) {
        //        $(self).window('close'); return false;
        //    }
        //}).focus();

        function buildWindowContent(contentHTML, fn, url) {
            var centerDIV = $('<div region="center" border="false" style="padding:5px;"></div>').html(contentHTML);
            if (url && url != '')
                centerDIV.empty().load(url, options.onload);
            $('<div class="easyui-layout" fit="true"></div>')
			.append(centerDIV)
			.append('<div region="south" border="false" style="padding-top:5px;height:40px; overflow:hidden; text-align:center;background:#fafafa;border-top:#eee 1px solid;"> <button id="AB" class="sexybutton"><span><span><span class="ok">' + options.okText + '</span></span></span></button> ' +
            (options.showclosebtn ? '&nbsp; <button title="ESC 关闭" id="AC" class="sexybutton"><span><span><span class="cancel">' + options.closeText + '</span></span></span></button>' : '')
            + '</div>')
			.appendTo($(self).empty())
			.layout();

            $('button[id="AC"]').click(function () {
                $(self).window('close'); return false;
            });

            $('#AB', self).unbind('click').click(fn);
        }
    }

    //Dialog
    $.fn.hDialog = function (options) {
        var defaults = {
            width: 300,
            height: 200,
            title: '此处标题',
            html: '',
            iconCls: '',
            modal: true,
            showbtn: true,
            btns: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: options.submit
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#' + id).dialog('close'); return false;
                }
            }],
            submit: function () { alert('可执行代码.'); }
        }
        var id = $(this).attr('id');

        var self = this;

        options = $.extend(defaults, options);
        $(self).dialog({
            href: options.href,
            title: options.title,
            height: options.height,
            width: options.width,
            iconCls: options.iconCls,
            onLoad: options.onload,
            modal: options.modal,
            buttons: (function () {
                if (options.showbtn)
                    return options.btns;
                else
                    return null;
            })()
        });

        function createContent() {
            $('.dialog-content', $(self)).empty().append('<div id="' + id + '_content" style="padding:5px;"></div>');
            if (options.html != '')
                $('#' + id + "_content").html(options.html);
        }
        createContent();
    }

    function createtip(el) {
        var box = $(el);
        var msg = box.attr('tip');
        var tip = $("<div class=\"validatebox-tip\">" + "<span class=\"validatebox-tip-content\">" + "</span>" + "<span class=\"validatebox-tip-pointer\">" + "</span>" + "</div>").appendTo("body");
        tip.find(".validatebox-tip-content").html(msg);
        el.data("tip", tip);
        tip.css({ display: "block", left: box.offset().left + box.outerWidth(), top: box.offset().top });
    }

    function removetip(el) {
        var tip = el.data("tip");
        if (tip) {
            tip.remove();
            $(el).removeData("tip");
        }
    }

    $.fn.tip = function (options) {
        return this.each(function () {
            var msg = $(this).attr('tip');
            if (msg) {
                switch (options.trigger) {
                    case "hover":
                        $(this).hover(function () { createtip($(this)); }, function () { removetip($(this)) });
                        break;
                    default:
                        $(this).focus(function () {
                            createtip($(this));
                        }).blur(function () {
                            removetip($(this));
                        });
                        break;
                }
            }
        })
    }

    //扩展datagrid 方法 getSelectedIndex
    if ($.fn.datagrid)
        $.extend($.fn.datagrid.methods, {
            getSelectedIndex: function (jq) {
                var row = $(jq).datagrid('getSelected');
                if (row)
                    return $(jq).datagrid('getRowIndex', row);
                else
                    return -1;
            }
        });

    //扩展 combobox 方法 selectedIndex
    if ($.fn.combobox)
        $.extend($.fn.combobox.methods, {
            selectedIndex: function (jq, index) {
                if (!index)
                    index = 0;
                var data = $(jq).combobox('options').data;
                var vf = $(jq).combobox('options').valueField;
                $(jq).combobox('setValue', eval('data[index].' + vf));
            }
        });

    if ($.fn.datagrid)
        $.extend($.fn.datagrid.defaults.editors, {
            numberspinner: {
                init: function (container, options) {
                    var input = $('<input type="text">').appendTo(container);
                    return input.numberspinner(options);
                },
                destroy: function (target) {
                    $(target).numberspinner('destroy');
                },
                getValue: function (target) {
                    return $(target).numberspinner('getValue');
                },
                setValue: function (target, value) {
                    $(target).numberspinner('setValue', value);
                },
                resize: function (target, width) {
                    $(target).numberspinner('resize', width);
                }
            }
        });
})(jQuery)



