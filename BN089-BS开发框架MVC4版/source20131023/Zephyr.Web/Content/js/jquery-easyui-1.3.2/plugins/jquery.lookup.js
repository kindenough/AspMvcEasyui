/**
* jquery.lookup.js
* create by liuhuisheng 2012-11-07
*/
(function ($) {
    $.fn.lookup = function (arg0, arg1) {
        if (typeof arg0 == "string") {
            var methods = $.fn.lookup.methods[arg0];
            if (methods) {
                return methods(this, arg1);
            } else {
                return this.combo(arg0, arg1);
            }
        }
        arg0 = arg0 || {};
        return this.each(function () {
            var lookup = $(this).data("lookup");
            if (lookup) {
                $.extend(true, lookup.options, arg0);
                initlookup(this);
            } else {
                lookup = $(this).data("lookup", { options: $.extend(true, {}, $.fn.lookup.defaults, $.fn.lookup.parseOptions(this), arg0) });
                initlookup(this);
            }
        });
    };

    $.fn.lookup.methods = {
        setValue: function (jq, value) {
            return jq.each(function () {
                setValuesHandle(this, value);
            });
        }
    };

    function setValuesHandle(target, value) {
        var that = $(target), options = that.data('lookup').options, text;
        that.combo('setValue', value);
        if (value) {
            that.zdata(options.action).success(function (d) {
                if (d.data) {
                    try {
                        var json = JSON.parse(d.data);
                        if (json.length > 0)
                            text = $.map(json, function (v) { return v[options.textField] }).join(',');
                    }
                    catch (e) { }
                }
                that.combo('setText', text || value);
            });

            if (typeof value == 'string' && value.indexOf(',') > -1)
                that.zdata().data([{ name: options.valueField, value: "'" + value.replaceAll(",", "','") + "'", cp: 'invalue'}]);
            else
                that.zdata().data([{ name: options.valueField, value: value, cp: 'equal'}]);

            that.zdata().ajax({ loading: false });
        }
        else {
            that.combo('setText', value);
        }
    }

    $.fn.lookup.parseOptions = function (target) {
        var t = $(target);
        return $.extend({},
		$.fn.combo.parseOptions(target), $.parser.parseOptions(target, ["valueField", "textField", "action", "grid", "url", 'window', 'valueTitle', 'textTitle']));
    };

    $.fn.lookup.defaults = $.extend({}, $.fn.combo.defaults, {
        url: '/common/page/lookup.aspx'
        , textField: 'textField'
        , textTitle: '名称'
        , valueField: 'valueField'
        , valueTitle: '编码'
        , parentField: ''
        , multiple: false
        , action: ''
        , grid: {}
        , window: {
            title: '弹出选择'
            , width: 600
            , height: 420
            , modal: true
            , collapsible: false
            , minimizable: false
            , maximizable: true
            , closable: true
        }
    });

    function setSelection(jq, selectionStart, selectionEnd) {
        if (jq.lengh == 0) return jq;
        input = jq[0];

        if (input.createTextRange) {
            var range = input.createTextRange();
            range.collapse(true);
            range.moveEnd('character', selectionEnd);
            range.moveStart('character', selectionStart);
            range.select();
        } else if (input.setSelectionRange) {
            input.focus();
            input.setSelectionRange(selectionStart, selectionEnd);
        }
        return jq;
    }

    function initlookup(target) {
        var that = $(target), options = that.data('lookup').options;
        var fnShow = function () {
            that.combo('hidePanel');
            var panel = that.data('combo').panel.remove('style').addClass('lookup-win');
            var pPanel = parent.$(panel);
            options.window.content = "<iframe id='frm_win_" + options.valueField + "' src='" + options.url + (options.url.indexOf('?') > -1 ? '&' : '?') + "r=" + Math.random() + "' style='height:100%;width:100%;border:0;' frameborder='0'></iframe>" //frameborder="0" for ie7
            options.window.onClose = function () {
                var rtnValue = pPanel.data("returnValue");
                if (rtnValue) {
                    $(target).combo('setText', rtnValue.text);
                    $(target).combo('setValue', rtnValue.value);
                }
                pPanel.window('destroy');
                var txt = that.data("combo").combo.find(".combo-text");
                var len = txt.val().length;
                setSelection(txt, len, len);
            };

            options.text = $(target).combo('getText');
            options.value = $(target).combo('getValue');
            pPanel.window(options.window);
            pPanel.data("lookup", options);
        };
        that.addClass("lookup-f");
        that.combo($.extend({}, options, { onShowPanel: fnShow }));
        that.data('combo').combo.addClass("lookup");
        that.lookup('setValue', that.val());
    }
})(jQuery);