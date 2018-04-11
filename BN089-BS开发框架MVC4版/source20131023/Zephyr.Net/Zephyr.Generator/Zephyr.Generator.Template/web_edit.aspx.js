//一、定义变量
var grid, form, crud, id;


//二、文档加载完事件
$(function () {
    //1.给变量赋值
    form = $("#master"), grid = $("#list"), crud = zdefaults.datagrid.crud(grid), id = request[0];

    //2.初始化页面
    using(['combobox', 'datagrid'], Page_Load);

    //3.绑定按钮事件
    $('#a_add').click(add);
    $('#a_edit').click(crud.edit);
    $('#a_del').click(crud.del);
    $('#a_save').click(save);
    $('#a_cancel').click(cancel);
    $('#a_xls').click(download);
    $('#a_xlsx').click(download);
    $('#a_doc').click(download);
});


//三、被调用函数方法

//新增
function add(obj) {
    $(obj.delegateTarget).zdata("{TableName}/Key").data("field", "RowId").success(function (d) {
        crud.add({ BillNo: id, RowId: d.data });
    }).ajax({ loading: false });
}

//下载
function download(obj) {
    var suffix = obj.delegateTarget.id.split('_')[1]
    grid.zfile(suffix).download();
}

//取消
function cancel() {
    $('#master,#list').zdata().reload({ loading: false, success: function () { $.zmsg('success', '已取消修改！'); } });
}

//保存
function save() {
    if (!crud.changed() && form.zform('getChanges').length == 0) return;
    $("a_save").zdata("{TableName}/e").data(form).detail(grid).setdetail("{DetailTableName}")
               .success(function () { $('#master,#list').zdata().ajax({ loading: false }); }).ajax();
}

//页面初始化
function Page_Load() {

    //设置输入区表单信息
    form.zdata("{TableName}/q3").data("{0:ColumnName}", id, "equal").success(function (d) {
        var data = d.data ? JSON.parse(d.data) : { BillNo: id };
        form.zform(data).zform('resize', '90%');
    });
 
    //设置表格列
    var cols = [[{DetailCols}]];

    //设置明细表格的属性
    var opt = zdefaults.datagrid.winsize(-4, -146).extend({
        pagination: true,
        columns: cols,
        onDblClickRow: crud.edit,
        onClickRow: crud.end
    });

    //设置编辑属性,如 设置验证 设置关联//Todo
    $.extend(crud, {
        begin: function (index) {
            var editors = grid.datagrid('getEditors', index);
            editors[0].target.validatebox({ required: true });
        }
    });

    //设置明细表请求的数据
    grid.datagrid(opt)
        .zdata("{TableName}/q2")
        .data("{0:ColumnName}", id, "equal")
        .sort("{1:ColumnName}")
        .successLoad();
 
    //访问数据库取得数据
    $('#master,#list').zdata().ajax();
}


 