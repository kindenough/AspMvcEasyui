$(function () {
    using(['layout', 'datagrid', 'tree'], function () {
        //获取window信息
        var iframe = getThisIframe();
        var thiswin = parent.$(iframe).parent();
        var options = thiswin.window("options");
        var param = options.paramater;

        //初始化layout
        var box = $("#layoutbox"), right = $('#right').layout();
        box.width($(window).width()).height($(window).height()).layout();
        $(window).resize(function () { box.width($(window).width()).height($(window).height()).layout('resize'); });

        //调整layout时自动调整grid
        var panels = $('#right').data('layout').panels;
        panels.north.panel({
            onResize: function (w, h) {
                $('#list').datagrid('resize', { width: w, height: h - 38 });
            }
        });
        panels.center.panel({
            onResize: function (w, h) {
                $('#selectlist').datagrid('resize', { width: w, height: h - 36 });
            }
        });

        //设置grid列
        var cols = [[
                { title: '材料编码', field: 'MaterialCode', sortable: true, align: 'left', width: 100},
                { title: '材料名称', field: 'MaterialName', sortable: true, align: 'left', width: 120 },
                { title: '规格型号', field: 'Model', sortable: true, align: 'left', width: 80},
                //{ title: '材质', field: 'Material', sortable: true, align: 'center', width: 60 },
                { title: '剩余数量', field: 'RemainNum', sortable: true, align: 'right', width: 60 }
        ]];

        if (param._xml == "mms.material_send" || param._xml == "mms.material_dict")
            cols[0].pop();

        if (param._xml == "mms.material_check") {
            cols[0][3] = { title: '账面数量', field: 'BookNum', sortable: true, align: 'right', width: 60 };
        }

        if (param._xml == "mms.material_batches" || param._xml == "mms.material_send" || param._xml == "mms.material_rentin") {
            cols[0].push({ title: '来源类型', field: 'SrcBillType', sortable: true, align: 'left', width: 60 ,formatter:mms.com.formatSrcBillType});
            cols[0].push({ title: '来源单号', field: 'SrcBillNo', sortable: true, align: 'left', width: 85 });
        }

        //定义返回值
        var selected = { total: 0, rows: [] };
        var grid1 = $('#list');
        var grid2 = $('#selectlist');

        var defaults = {
            iconCls: 'icon icon-list',
            nowrap: true,           //折行
            rownumbers: true,       //行号
            striped: true,          //隔行变色
            singleSelect: true,     //单选
            remoteSort: true,       //后台排序
            pagination: false,      //翻页
            pageSize: com.getSetting("gridrows", 20),
            contentType: "application/json",
            method: "GET"
        };

        //设置明细表格的属性
        var opt = $.extend({},defaults,{
            height: 310,
            pagination: true,
            url: '/api/mms/home/GetMaterial',
            queryParams: param,
            pageSize: 10,
            columns: cols,
            onDblClickRow: function (index, row) {
                for (var i in selected.rows) {
                    if (row.MaterialCode == selected.rows[i].MaterialCode) {
                        grid2.datagrid('selectRow', i);
                        return;
                    }
                }
                selected.total = selected.rows.push(row);
                grid2.datagrid('loadData', selected);
                grid2.datagrid('selectRow', grid2.datagrid('getRowIndex', row));
                $('#total').html(selected.total);
            }
        });

        //已选择的grid
        var opt2 = $.extend({}, defaults, {
            pagination: false,
            remoteSort: false,
            columns: cols,
            height: panels.center.panel('options').height - 36,
            onDblClickRow: function (index, row) {
                for (var i in selected.rows) {
                    if (row.MaterialCode == selected.rows[i].MaterialCode) {
                        selected.rows.pop(row);
                        selected.total -= 1;
                        grid2.datagrid('loadData', selected);
                        $('#total').html(selected.total);
                        break;
                    }
                }
            }
        });

        grid2.datagrid(opt2).datagrid('loaded');
        grid1.datagrid(opt);

        var typeid = '';
        var clickType = function (node) {
            typeid = node.id;
            search();
        };

        var search = function () {
            var queryParams = $.extend({},param,{
                MaterialType: typeid,
                MaterialCode: $('#id').val(),
                MaterialName: $('#text').val()
            });
            grid1.datagrid('reload', queryParams);
        };

        var paramStr="";
        for (var key in param)
            paramStr += (paramStr ? "&" : "?") + key + "=" + param[key];

        $('#btnSearch').click(search);
        $('#btnClear').click(function () { $('#master').find("input").val(""); search(); });
        $('#typetree').tree({
            method:'GET',
            url: '/api/mms/home/GetMaterialType' + paramStr,
            onClick: clickType,
            loadFilter:function(d){
                var data = utils.toTreeData(d.rows || d, 'id', 'pid', "children");
                return [{ id: '', text: '所有类别', children: data }];
            },
        });
  
        $('#btnConfirm').click(function () {
            options.onSelect(selected);
            destroyIframe(iframe);
            thiswin.window('destroy');
        });

        $('#btnCancel').click(function () {
            destroyIframe(iframe);
            thiswin.window('destroy');
        });
    });
});

function getThisIframe() {
    if (!parent) return null;
    var iframes = parent.document.getElementsByTagName('iframe');
    if (iframes.length == 0) return null;
    for (var i = 0; i < iframes.length; ++i) {
        var iframe = iframes[i];
        if (iframe.contentWindow === self) {
            return iframe;
        }
    }
    return null;
}

function destroyIframe(iframeEl) {
    if (!iframeEl) return;
    iframeEl.parentNode.removeChild(iframeEl);
    iframeEl = null;
};