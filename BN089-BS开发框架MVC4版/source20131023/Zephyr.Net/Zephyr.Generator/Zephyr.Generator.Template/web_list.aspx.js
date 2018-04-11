//一、定义变量
var grid, form;


//二、文档加载完事件
$(function() {
    //1.给变量赋值
    grid = $("#gridlist"), form = $("#condition");

    //2.加载相应脚本初始化页面
    using(['datagrid', 'autocomplete'], Page_Load);

    //2.绑定画面按钮事件
    $('#a_search').click(search);
    $('#a_reset').click(clear);
    $('#a_add').click(add);
    $('#a_edit').click(edit);
    $('#a_del').click(del);
    $('#a_xls').click(download);
    $('#a_xlsx').click(download);
    $('#a_doc').click(download);
});


//三、被调用函数方法

//查询
function search() {
    grid.zdata().data(form).ajax();
}

//清空
function clear() {
    grid.zdata().clearData().ajax();
    form.zform('clear');
}

//新增
function add(obj) {
    $(obj.delegateTarget).zdata("{TableName}/Key").data("rule", "dateplus").success(function (d) {
        edit(0, { BillNo: d.data });
    }).ajax({ loading: false });
}

//编辑
function edit(index, row) {
    row = row || grid.datagrid('getSelected');
    if (!row) return $.zmsg('warning', '请先选择一条{BillName}！');
    parent.addTab("{BillName}明细", "{FileName}/" + row.BillNo);
}

//删除
function del(obj) {
    var row = grid.datagrid('getSelected');
    if (!row) return $.zmsg('warning', '请先选择一条{BillName}！');
    $.zmsg('confirm', '确实要删除选中的{BillName}吗？', function (b) {
        if (b) $(obj.delegateTarget).zdata("{TableName}/d").data("BillNo", row.BillNo, 'equal').success(search).ajax();
    });
}

//下载
function download(obj) {
    var suffix = obj.delegateTarget.id.split('_')[1];
    grid.zfile(suffix).download();
}

//初始化页面

function Page_Load() {

    //设置grid数据列
    var cols = [[{cols}]];

    //取得框架默认的grid属性
    var opt = zdefaults.datagrid.winsize(-4, -94).extend({
        pagination: true,
        onDblClickRow: edit,
        columns: cols
    });

    //设置grid请求的数据
    grid.datagrid(opt)
        .zdata("{TableName}/q2")
        .sort("{0:ColumnName}")
        .successLoad();
 
    //设置第一个字段自动完成功能
    $("#{0:ColumnName}").autocomplete({
        action: '{TableName}/q2',
        filed: '{0:ColumnName}'
    });
     
    //访问数据库取得数据
    $("#gridlist").zdata().ajax();
}

 
