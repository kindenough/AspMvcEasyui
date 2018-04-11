/// <reference path="knockout-2.2.1.js" />
/**
* 模块名：共通脚本
* 程序名: knockoutjs自定义绑定
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

(function ($, ko) {
    var jqElement = function (element) {
        var jq = $(element);
        if ($(document).find(element).length == 0) {  //处理元素在父页面执行的情况
            if ($(parent.document).find(element).length > 0)
                jq = parent.$(element);
        }
        return jq;
    };
    //value
    ko.creatEasyuiValueBindings = function (o) {
        o = $.extend({ type: '', event: '', getter: 'getValue', setter: 'setValue', fix: $.noop,formatter: function (v) { return v; }}, o);

        var customBinding = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var jq = jqElement(element), handler = jq[o.type]('options')[o.event], opt = {};

                //handle the field changing
                opt[o.event] = function () {
                    handler.apply(element, arguments);
                    var value = jq[o.type](o.getter);
                    if (valueAccessor() == null) throw "viewModel中没有页面绑定的字段";
                    valueAccessor()(value);
                };

                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    jq[o.type]("destroy");
                });

                o.fix(element, valueAccessor);
                jq[o.type](opt);
            },
            update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                value = ko.utils.unwrapObservable(valueAccessor());
                jqElement(element)[o.type](o.setter, o.formatter(value));
            }
        };
        ko.bindingHandlers[o.type + 'Value'] = customBinding;
    };

    ko.creatEasyuiValueBindings({ type: 'combobox', event: 'onSelect' });
    ko.creatEasyuiValueBindings({ type: 'combotree', event: 'onChange' });
    ko.creatEasyuiValueBindings({ type: 'datebox'       , event: 'onSelect' , formatter: com.formatDate });
    ko.creatEasyuiValueBindings({ type: 'lookup'        , event: 'onChange' });
    ko.creatEasyuiValueBindings({ type: 'numberbox'     , event: 'onChange' });
    ko.creatEasyuiValueBindings({ type: 'numberspinner' , event: 'onChange' ,fix: function (element) { $(element).width($(element).width() + 20); } });

    //enable
    ko.creatEasyuiEnableBindings = function (o) {
        o = $.extend({ type: '',method:'Enable', options:function(v){return 'disabled';}, value: function (v) { return !v; } }, o);
        var customBinding = {
            update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var enable = ko.toJS(valueAccessor());
                $(element)[o.type](o.options(enable), o.value(enable));
            }
        };
        ko.bindingHandlers[o.type + o.method] = customBinding;
    };
    ko.creatEasyuiEnableBindings({ type: 'linkbutton', options: function (b) { return {disabled:!b}; } });
    ko.creatEasyuiEnableBindings({ type: 'linkbutton',method:'Disable', options: function (b) { return { disabled: b }; } });

    //readonly
    _readOnlyHandles = {};
    _readOnlyHandles.defaults = function (obj, b) {
        b ? obj.addClass("readonly").attr("readonly", true) : obj.removeClass("readonly").removeAttr("readonly");
    };
    _readOnlyHandles.combo = function (obj, b) {
        obj.combo(b ? "disable" : "enable");
    };
    _readOnlyHandles.spinner = function (obj, b) {
        obj.spinner(b ? "disable" : "enable");
    };

    ko.creatEasyuiReadOnlyBindings = function (o) {
        o = $.extend({ type: '', handler: _readOnlyHandles.defaults }, o);
        var customBinding = {
            update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                o.handler($(element),ko.toJS(valueAccessor()));
            }
        };
        ko.bindingHandlers[o.type ?  (o.type + 'ReadOnly'): 'readOnly'] = customBinding;
    };

    ko.creatEasyuiReadOnlyBindings(); //default readonly
    ko.creatEasyuiReadOnlyBindings({ type: 'numberbox' }); 
    ko.creatEasyuiReadOnlyBindings({ type: 'combobox', handler: _readOnlyHandles.combo });
    ko.creatEasyuiReadOnlyBindings({ type: 'datebox', handler: _readOnlyHandles.combo });
    ko.creatEasyuiReadOnlyBindings({ type: 'lookup', handler: _readOnlyHandles.combo });
    ko.creatEasyuiReadOnlyBindings({ type: 'combotree', handler: _readOnlyHandles.combo });
    ko.creatEasyuiReadOnlyBindings({ type: 'numberspinner', handler: _readOnlyHandles.spinner });
 
    //datagrid
    ko.bindingHandlers.datasource = {
        init: function (element, valueAccessor) {
            var jq = jqElement(element);
            var ds = ko.toJS(valueAccessor());
            if ($.isFunction(ds)) ds = ds.call();
            if (jq.data('treegrid'))
                jq.treegrid('loadData', ds);
            else if (jq.data('datagrid'))
                jq.datagrid('loadData', ds);
            else if (jq.data('combotree'))
                jq.combotree('loadData', ds);
            else if (jq.data('combobox')) {
                var val = jq.combobox('getValue'), ds = ds.rows || ds;
                jq.combobox('clear').combobox('loadData',ds).combobox('setValue', val);
            }
            else if (jq.data('tree'))
                jq.tree('loadData', ds.rows || ds);
        }
    };

    ko.creatEasyuiGridBindings = function (gridType) {
        var gridBinding = {
            update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                var grid = jqElement(element), opts = ko.toJS(valueAccessor()), url = opts.url;
                var customLoader = function (queryParams) {
                    if (opts.pagination) {
                        var paging = grid.datagrid('getPager').pagination('options');
                        queryParams = $.extend(queryParams, { page: paging.pageNumber, rows: paging.pageSize });
                    }
                    com.ajax({
                        type: 'get',
                        url: url,
                        data: queryParams,
                        success: function (d) {
                            if ($.isFunction(opts.afterCustomSuccess)) opts.afterCustomSuccess(d);
                            grid[gridType]('loadData', d);
                        }
                    });
                };

                if (opts.treeField && opts.parentField) { //only for treegrid
                    opts.loadFilter = function (data) {
                        return utils.toTreeData(data.rows || data, opts.idField||opts.treeField, opts.parentField, "children");
                    };
                }

                var handler = function () {
                    if (grid.data(gridType)) {
                        if (opts.customLoad)
                            customLoader(opts.queryParams);
                        else
                            grid[gridType](opts);
                    } else {
                        var defaults = {
                            iconCls: 'icon icon-list',
                            nowrap: true,           //折行
                            rownumbers: true,       //行号
                            striped: true,          //隔行变色
                            singleSelect: true,     //单选
                            remoteSort: true,       //后台排序
                            pagination: false,      //翻页
                            pageSize: com.getSetting("gridrows",20),
                            contentType: "application/json",
                            method: "GET"
                        };
                        var winsize = function (size) {
                            var ret = {};
                            if (size && size.w) ret.width = jqElement(window).width() - size.w;
                            if (size && size.h) ret.height = jqElement(window).height() - size.h;
                            return ret;
                        };

                        opts = $.extend(defaults, opts, winsize(opts.size));
                        if (opts.customLoad) {
                            if (opts.remoteSort)
                                opts.onSortColumn = function (sort, order) { customLoader($.extend(opts.queryParams, { sort: sort, order: order })); };

                            grid[gridType](opts);
                            customLoader(opts.queryParams);

                            if (opts.pagination)
                                grid[gridType]('getPager').pagination({ onSelectPage: customLoader });
                        }
                        else
                            grid[gridType](opts);

                        valueAccessor()[gridType] = function () { return grid[gridType].apply(grid, arguments); }
                        valueAccessor()['$element'] = function () { return grid; };
                        if (opts.size) jqElement(window).resize(function () { grid[gridType]('resize', winsize(opts.size)); });
                    }
                };

                grid[gridType] ? handler() : using([gridType], function () {
                    handler();
                    if (gridType=='treegrid') com.loadCss('/Content/css/icon/icon.css', document, true);//解决图标被tree.css覆盖的问题
                });
            }
        };

        return gridBinding;
    };

    ko.bindingHandlers.datagrid = ko.creatEasyuiGridBindings('datagrid');
    ko.bindingHandlers.treegrid = ko.creatEasyuiGridBindings('treegrid');

    //初始化控件
    ko.createEasyuiInitBindings = function (o) {
        o = $.extend({ type: '' },o);
        var customBinding = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var jq = jqElement(element), opt = ko.mapping.toJS(valueAccessor());
                var handler = function () {
                    jq[o.type](opt);
                };
                jq[o.type] ? handler() : using(o.type, handler);

                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    jq[o.type]("destroy");
                });
                valueAccessor()["$element"] = function () { return jq };
            },
            update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var jq = jqElement(element), opt = ko.mapping.toJS(valueAccessor());
                if (jq[o.type]) jq[o.type](opt);
            }
        };
        var bindName = 'easyui' + o.type.replace(/(^|\s+)\w/g, function (s) {return s.toUpperCase();});
        ko.bindingHandlers[bindName] = customBinding;
    };

    ko.createEasyuiInitBindings({ type: 'tabs' });
    ko.createEasyuiInitBindings({ type: 'tree' });
    ko.createEasyuiInitBindings({ type: 'combotree' });
    ko.createEasyuiInitBindings({ type: 'combobox' });
    ko.createEasyuiInitBindings({ type: 'linkbutton' });

    //width height
    ko.bindingHandlers.inputwidth = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var that = $(element), widthStr = ko.toJS(valueAccessor());
            var calcWidth = function (w) {
                var rate = 1
                if (typeof w == 'string') {
                    if (w.indexOf("px") > -1 || w.indexOf("PX") > -1) w = w.replace("px", "").replace("PX", "");
                    if (w.indexOf("%") > -1) w = w.replace("%", "") / 100;
                }
                if (w > 0 && w <= 1) {
                    rate = w;
                    w = $(".val").width();
                }
                return w * rate;
            };
        
            var resizeWidth = function () {
                var width = calcWidth(widthStr);
                $.each([
                    { selector: 'input.z-txt', handler: function (jq) { jq.width(width - 8); } },            //pading3*2 + border1*2 = 8
                    { selector: 'input[comboname],input.combo-f', handler: function (jq) { jq.combo('resize', width); } },
                    { selector: 'input[numberboxname].spinner-text', handler: function (jq) { jq.spinner('resize', width); } }
                ], function () {
                    var jq = that.find(this.selector);
                    if (jq.length > 0) this.handler(jq);
                    if (that.is(this.selector)) this.handler(that);
                });
            };

            resizeWidth();
            $(window).resize(resizeWidth);
        }
    };

    ko.bindingHandlers.autoheight = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var that = $(element), obserable = valueAccessor(),
               height = $.isFunction(obserable) ? obserable() : obserable;
            that.height($(window).height() - height);
            $(window).resize(function () { that.height($(window).height() - height); });
        }
    };
    ko.bindingHandlers.autowidth = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var that = $(element), obserable = valueAccessor(),
               width = $.isFunction(obserable) ? obserable() : obserable;
            that.width($(window).width() - width);
            $(window).resize(function () { that.width($(window).width() - width); });
        }
    };

    //bindingViewModel
    ko.bindingViewModel = function (viewModelInstance,node) {
        using('parser', function () {
            $.parser.onComplete = function () {
                ko.applyBindings(viewModelInstance, node || document.body);
            };
        });
    };
 
})(jQuery, ko);