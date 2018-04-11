/**
* jquery.autocomplete.js
* create by liuhuisheng 2012-11-18
*/
(function ($) {

    $.fn.autocomplete = function (arg0, arg1) {
        if (typeof arg0 == "string") {
            var methods = $.fn.autocomplete.methods[arg0];
            if (methods)
                return methods(this, arg1);
            else
                return this.validatebox(this, arg1);
        }
        arg0 = arg0 || {};
        return this.each(function () {
            var autocomplete = $(this).data("autocomplete");
            if (autocomplete)
                $.extend(true, autocomplete.options, arg0);
            else
                $(this).data("autocomplete", { options: $.extend(true, {}, $.fn.autocomplete.defaults, { field: $(this).attr("name") }, $.fn.autocomplete.parseOptions(this), arg0) });

            create(this);
            bindEvents(this);
            $(this).validatebox($(this).data("autocomplete").options);
        });
    };

    $.fn.autocomplete.methods = {
        destroy: function () {


        },
        disable: function (jq) {
            return jq.each(function () {
                disableThis(this);
            });
        }
    };

    $.fn.autocomplete.parseOptions = function (target) {
        var t = $(target);
        return $.extend({},
		$.fn.combo.parseOptions(target), $.parser.parseOptions(target, ["valueField", "textField", "url", "grid", "url", 'window', 'valueTitle', 'textTitle']));
    };

    $.fn.autocomplete.defaults = $.extend({}, $.fn.validatebox.defaults, {
        url: "",
        field: "a,b",
        data: [],
        top: 10,
        width: null,
        both: false,
        pinyin: false,
        disabled: false,
        delay: 0,
        onselect: function (d) { }
    });

    function disableThis(jq) {
        var options = $.data(jq, "autocomplete").options;
        options.disable = true;
        $(jq).attr('disabled', true);
    }

    function bindEvents(jq) {
        var data = $.data(jq, "autocomplete"), that = $(jq);
        var options = data.options;
        var panel = data.panel;

        $(document).unbind(".autocomplete").bind("mousedown.autocomplete", function (e) {
            $("div.autocomplete-panel").panel("close"); //鼠标点击combo外，关闭选择面板
        });
        //移除事件处理器
        panel.unbind(".autocomplete");

        //若组件未禁用，添加以下事件处理器
        if (!options.disabled) {
            panel.bind("mousedown.autocomplete", function (e) {
                return false;
            });
            that.bind("mousedown.autocomplete", function (e) {
                $("div.autocomplete-panel[target!='" + $(jq).attr("id") + "']").panel("close"); //鼠标点击combo外，关闭选择面板
                e.stopPropagation(); //该方法将停止事件的传播，阻止它被分派到其他 Document节点，详情参考http://www.w3school.com.cn/xmldom/met_event_stoppropagation.asp
            }).bind("keydown.autocomplete", function (e) {
                switch (e.keyCode) {
                    case 38: //小键盘上箭头
                        selectPrev(this); //小键盘向上选择
                        break;
                    case 40: //小键盘下箭头
                        selectNext(this); //小键盘向下选择
                        break;
                    case 13: //Enter键
                        e.preventDefault();
                        hidePanel(jq);
                        return false;
                    case 9: //Tab键
                    case 27: //Esc键
                        hidePanel(jq);
                        break;
                    default:
                        if (!options.disabled) {
                            if (data.timer) {
                                clearTimeout(data.timer);
                            }
                            data.timer = setTimeout(function () {
                                var q = that.val().replace(/(^\s*)|(\s*$)/g, "");
                                //$.zmsg('success', q);
                                search(jq, q);
                            }, options.delay);
                        }
                }
            });
        }
    };

    function search(jq, q) {
        var that = $(jq), data = $.data(jq, "autocomplete"), options = data.options;
        if (q) {
            $.get(options.url +"?q=" + escape(q), function (d) {
                loadData(jq, d);
                if (d.length) showPanel(jq);
            });
        }
        else {
            data.panel.empty();
        }
    }

    function create(jq) {
        var that = $(jq);
        that.addClass("autocomplete");
        var panel = $("<div class=\"autocomplete-panel combo-panel\" target=\"" + that.attr("id") + "\"></div>").appendTo("body"); //将下来面板添加到body
        //设置下拉面板
        panel.panel({
            doSize: false,
            closed: true,
            style: {
                position: "absolute",
                zIndex: 10
            },
            onOpen: function () {
                $(this).panel("resize"); //打开时重置大小
            },
            width: $(jq).outerWidth()
            , height: 198
        });
        $.data(jq, "autocomplete").panel = panel;

        //        that.blur(function () {
        //            var item = $(".combobox-item", panel).filter('[value=' + that.val() + ']');
        //            var row = item.length > 0 ? item.data('row') : [that.val(), '', ''];
        //            $.data(jq, "autocomplete").options.onselect(row);
        //        });
    };

    function showPanel(jq) {
        var data = $.data(jq, "autocomplete");
        var options = data.options;
        var panel = data.panel;
        var that = $(jq);

        if ($.fn.window) {
            panel.panel("panel").css("z-index", $.fn.window.defaults.zIndex++); //若放在窗口里面，则显示在窗口之上
        }
        panel.panel("move", {
            left: that.offset().left,
            top: getOffsetTop()
        });
        panel.panel("open");
        //options.onShowPanel.call(jq);
        (function () {
            if (panel.is(":visible")) {
                panel.panel("move", {
                    left: getOffsetLeft(),
                    top: getOffsetTop()
                });
                setTimeout(arguments.callee, 200);
            }
        })();
        /**
        * 获取Left位置
        * @return {TypeName} 
        */
        function getOffsetLeft() {
            var left = that.offset().left;
            if (left + panel.outerWidth() > $(window).width()
					+ $(document).scrollLeft()) {
                left = $(window).width() + $(document).scrollLeft()
						- panel.outerWidth();
            }
            if (left < 0) {
                left = 0;
            }
            return left;
        };
        /**
        * 获取TOP位置
        * @return {TypeName} 
        */
        function getOffsetTop() {
            var top = that.offset().top + that.outerHeight();
            if (top + panel.outerHeight() > $(window).height()
					+ $(document).scrollTop()) {
                top = that.offset().top - panel.outerHeight();
            }
            if (top < $(document).scrollTop()) {
                top = that.offset().top + that.outerHeight();
            }
            return top;
        };
    };

    function hidePanel(jq) {
        var data = $.data(jq, "autocomplete");
        var options = data.options;
        var panel = data.panel;

        panel.panel("close");
        //options.onHidePanel.call(jq);
    };

    function formatItem(cols, count, q, w) {
        var html = '';
        switch (count) {
            case 1:
                html = utils.formatString('{0}<strong>{1}</strong>', cols[0].substr(0, q.length), cols[0].substr(q.length, cols[0].length));
                break;
            default:
                var temple = '<div style="{1}">{0}</div>';
                for (var m in cols) {
                    var style = 'float:' + (count == 2 && m == 1 ? 'right;' : 'left;') + (count > 2 ? 'float:left; width:' + Math.floor(w * 0.95 / count) + 'px' : '');
                    var strong = (cols[m].substr(0, q.length) == q) ? "{0}<strong>{1}</strong>" : "{0}{1}";
                    var text = utils.formatString(strong, cols[m].substr(0, q.length), cols[m].substr(q.length, cols[m].length));
                    html += utils.formatString(temple, text, style);
                }
                break;
        }

        return html;
    }

    function loadData(jq, data) {
        var autocomplete = $.data(jq, "autocomplete");
        var options = autocomplete.options;
        var panel = autocomplete.panel;
        var q = $(jq).val();
        if (options.width) panel.panel('resize', { width: options.width });
        var w = panel.width();

        panel.empty(); //清空下拉面板所有选项
        //循环数据，给下拉面板添加选项
        for (var i = 0; i < data.length; i++) {
            var row = data[i], j = 0, cols = {};
            for (var key in row) if (key != 'FLUENTDATA_ROWNUMBER') cols[j++] = utils.formatString('{0}', row[key]);
            var item = $("<div class=\"combobox-item\" " + (j > 1 ? "style=\"height:18px;\"" : "") + "></div>").appendTo(panel).attr("value", cols[0]).data("row", cols); //添加选项
            item.html(options.formatter ? options.formatter.call(jq, data[i]) : formatItem(cols, j, q, w));
        }

        //给下拉面板选项注册hover、click事件
        $(".combobox-item", panel).hover(function () {
            $(this).addClass("combobox-item-hover");
        }, function () {
            $(this).removeClass("combobox-item-hover");
        }).click(function () {
            var selectItem = $(this); //单击选中的选项
            $(jq).val(selectItem.attr("value"));
            $(jq).trigger('change');
            options.onselect(selectItem.data("row"));
            hidePanel(jq); //单选时，选中一次就隐藏下拉面板
        });
    };

    /**
    * 小键盘向上选择操作
    * @param {Object} jq
    */
    function selectPrev(jq) {
        var data = $.data(jq, "autocomplete");
        var panel = data.panel;
        var that = $(jq);

        //获取到combo值在下拉面板panel里面对应的item项的前一选项
        var item = panel.find("div.combobox-item[value=" + that.val() + "]");
        if (item.length) {
            var prevItem = item.prev(":visible"); //获取前一项
            if (prevItem.length) {
                item = prevItem; //若有前一项，则item指向前一个选项
            }
        } else {
            item = panel.find("div.combobox-item:visible:last"); //若不存在前一项，则直接指向最后一个选项
        }
        var value = item.val(); //获取上一选项的值
        //selectByValue(jq, value);
        that.val(value);
        $(jq).trigger('change');
        data.options.onselect(item.data("row"));
        panel.find("div.combobox-item-selected").removeClass("combobox-item-selected");
        item.addClass("combobox-item-selected");
        scrollTo(jq, value);
    };
    /**
    * 小键盘向下选择操作
    * @param {Object} jq
    */
    function selectNext(jq) {
        var data = $.data(jq, "autocomplete");
        var panel = data.panel;
        var that = $(jq);

        //获取到combo值在下拉面板panel里面对应的item项的下一选项
        var item = panel.find("div.combobox-item[value=" + that.val() + "]");
        if (item.length) {
            var nextItem = item.next(":visible"); //获取下一项
            if (nextItem.length) {
                item = nextItem; //若有下一项，则item指向下一个选项
            }
        } else {
            item = panel.find("div.combobox-item:visible:first"); //若不存在下一项，则直接指向第一个选项
        }
        var value = item.val();
        //selectByValue(jq, value);
        that.val(value);
        $(jq).trigger('change');
        data.options.onselect(item.data("row"));
        panel.find("div.combobox-item-selected").removeClass("combobox-item-selected");
        item.addClass("combobox-item-selected");
        scrollTo(jq, value);
    };

    function scrollTo(jq, value) {
        var panel = $.data(jq, 'autocomplete').panel;
        var item = panel.find("div.combobox-item[value=" + value + "]");
        if (item.length) {
            if (item.position().top <= 0) {
                var h = panel.scrollTop() + item.position().top;
                panel.scrollTop(h);
            } else {
                if (item.position().top + item.outerHeight() > panel.height()) {
                    var h = panel.scrollTop() + item.position().top
							+ item.outerHeight() - panel.height();
                    panel.scrollTop(h);
                }
            }
        }
    };
})(jQuery);