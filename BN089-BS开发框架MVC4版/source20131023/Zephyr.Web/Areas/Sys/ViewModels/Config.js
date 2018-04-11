/**
* 模块名：mms viewModel
* 程序名: config.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
 
function viewModel(d) {
    this.form = ko.mapping.fromJS(d.form);
    delete this.form.__ko_mapping__;
    this.save = function () {
        $.ajax({
            url: '/api/sys/config',
            type: 'POST',
            contentType: "application/json",
            data: ko.toJSON(this.form),
            success: function (r) {
                com.message('success', '恭喜，全局设置保存成功,按F5看效果');
            },
            error: function (e) {
                com.message('error', e.responseText);
            }
        });
    };
    return ko.mapping.fromJS(this);
}