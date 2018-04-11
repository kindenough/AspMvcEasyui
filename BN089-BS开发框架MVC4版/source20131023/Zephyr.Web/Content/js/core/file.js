/**
上传下载文件

//1 下载datagrid treegrid data  取数据为$("grid").zdata().ajax();
$("grid").zfile("xls").begin().end().error().page().compress('rar').download();


//2 下载url
$("down").zfile(url).download();

//3 下载文件打包
$("down").zfile(url[]).downlad();
{
    downloadType:   "generate url method", 
    dataGenerate: "#grid"
    dataUrls:[]
    dataMethod:"SysUser/GetFile"
    generateType: "xls,doc,pdf"
    generateTitle: "["key:icon,value:'图标'",""]"
    compressType:  "zip,rar,none"   
}
**/

(function ($) {
    $.zGetOptoins = function (target, name, defaults) {
        if (!target) return null;
        if (!$.data(target, name))
            $(target).addClass(name).data(name, $.extend(true, {}, defaults));
        return $.data(target, name);
    };

    $.fn.zfile = function () {
        if (this.length == 0) throw "选择器错误";

        var that = this, defaults, current, objfn;
        if (arguments[0] == 'upload') {
            defaults = $.extend(true, {}, $.fn.zfile.upload.defaults.options);
            current = $.zGetOptoins(that[0], "zfile_upload", defaults);
            objfn = zfile_upload;
        }
        else {
            defaults = $.extend(true, {}, $.fn.zfile.download.defaults.options);
            current = $.zGetOptoins(that[0], "zfile_download", defaults);
            objfn = zfile_download;
        }

        var options = {
            $this: that
            , defaults: defaults
            , current: current
            , arg0: arguments[0]
        };

        return new objfn(options);
    };

    $.fn.zfile.download = {};
    $.fn.zfile.download.defaults = {
        ext: ['xls', 'xlsx', 'doc', 'docx', 'pdf', 'rar']
       , options: {
           downloadType: "generate" //"generate url method"
          , dataGenerate: ""
          , dataUrls: []
          , dataMethod: ""
          , generateType: "xls"
          , generateTitle: []
          , generateAll: true
          , compressType: "none"
          , begin: function () { }
          , error: function () { }
          , end: function () { }
       }
    };

    $.fn.zfile.upload = {};
    $.fn.zfile.upload.defaults = {
        options: {
            text: '上传'
          , listSelector: ''
          , type: 'import' //数据导入import，项目附件project，临时文件temp，材料图片picture...
          , billno: '' //业务id
          , params: []
          , method: 'SysUser/import'
          , importParams: { map: [{ 'UserCode': '用户名称' }], keys: ['UserCode'], errorContinue: false }
          , progress: true
          , begin: function () { }
          , success: function () { }
          , error: function () { }
            //, progress: function () { }
        }
    };

})(jQuery);

var zfile_upload = function (options) {
    var defaults = {}, current = {}, arg0, $that, that = this;

    if (options) {
        $that = options.$this;
        defaults = options.defaults;
        current = options.current;
        arg0 = options.arg0;
    }

    that.text = function (text) {
        current.text = text;
        return that;
    };

    that.type = function (type) {
        current.type = type;
        return that;
    };

    that.method = function (method) {
        current.method = method;
        return that;
    };

    that.upload = function () {
        //        using('../common/js/uploader/fileuploader.js', function () {
        //            var uploader = new qq.FileUploader({
        //                element: $that[0],
        //                action: '/common/page/upload.aspx',
        //                uploadButtonText: '上传',
        //                onComplete: function (id, fileName, responseJSON) {
        //                    alert(responseJSON.success)
        //                },
        //                onProgress: function () { alert('onProgress') },
        //                onError: function (id, name, reason) { alert(reason); }
        //            });
        //        });

        using('messager', function () {
            //动态添加kissy.js无效，所以必须自动添加kiss.js引用
            KISSY.config({ packages: [{ name: "gallery", path: "/Content/js/kissy/", charset: "utf-8" }] });
            KISSY.use('gallery/form/1.3/uploader/index', function (S, RenderUploader) {
                var ru = new RenderUploader('#' + $that.attr("id"), "#list", {
                    serverConfig: { action: "/Service/File/Upload", data: { test: '123' } },
                    type: 'auto',
                    name: "Filedata", // 文件域
                    urlsInputName: "fileUrls"  //用于放服务器端返回的url的隐藏域
                });

                ru.on("init", function (ev) {
                    var uploader = ev.uploader;

                    uploader.on('render', function (ev) {
                        //alert('上传组件准备就绪！');
                    });
                    uploader.on('select', function (ev) {
                        var files = ev.files;
                        //alert('选择了' + files.length + '个文件');
                    });
                    uploader.on('start', function (ev) {
                        var index = ev.index, file = ev.file;
                        //alert('开始上传,文件名：' + file.name + '，队列索引为：' + index);
                        $.messager.progress({ title: '请稍等', msg: '正在上传...', interval: 0 });
                    });
                    uploader.on('progress', function (ev) {
                        var file = ev.file, loaded = ev.loaded, total = ev.total;
                        //alert('正在上传,文件名：' + file.name + '，大小：' + total + '，已经上传：' + loaded);
                        $.messager.progress('bar').progressbar('setValue', Math.ceil(loaded * 100 / total));
                        //if (loaded == total) $.messager.progress('close');
                    });
                    uploader.on('success', function (ev) {
                        var index = ev.index, file = ev.file;
                        //服务器端返回的结果集
                        var result = ev.result;
                        //alert('上传成功,服务器端返回上传方式：' + result.type);
                        //$.messager.progress('close')
                    });
                    uploader.on('complete', function (ev) {
                        var index = ev.index, file = ev.file;
                        //服务器端返回的结果集
                        var result = ev.result;

                        $.messager.progress('close')
                        //alert('上传结束,服务器端返回上传状态：' + result.status);
                    });
                    uploader.on('error', function (ev) {
                        var index = ev.index, file = ev.file;
                        //服务器端返回的结果集
                        var result = ev.result;
                        alert('上传失败,错误消息为：' + result.msg);
                    });
                    uploader.on('add', function (ev) {
                        var queue = ev.queue;
                        var file = ev.file;
                        //alert('队列添加文件！文件名为：' + file.name);
                    });
                    uploader.on('remove', function (ev) {
                        var queue = ev.queue;
                        //alert('队列删除文件！文件索引值：' + ev.index);
                        //alert('队列中的文件数为：' + queue.get('files').length);
                    });
                });
            });
        });
    };
}

var zfile_download = function (options) {
    var defaults = {}, current = {}, arg0, $that, that = this;

    if (options) {
        $that = options.$this;
        defaults = options.defaults;
        current = options.current;
        arg0 = options.arg0;
    }

    //不调用时导出全部页，page() 无参数 当前页，page(1,50)参数对应页
    that.paging = function () {
        if (arguments.length == 0)
            current.dataGenerate.data.query.paging = $.extend(true, {}, $that.zdata().currentOptions().data.query.paging);
        else if (arguments.length == 2)
            current.dataGenerate.data.query.paging = { page: arguments[0], pagesize: arguments[1] };

        return that;
    };

    // .title('code','编码'); 追加
    // .title([{key:'code',value:'编码'},{key:'xx',value:'yy'}],displayAll);
    that.title = function () {
        if (arguments[0] instanceof Array) {
            current.generateTitle = arguments[0];
            current.generateAll = (arguments[1] == true);
        } else if (typeof arguments[0] == 'string' && typeof arguments[1] == 'string') {
            current.generateTitle.push({ Key: arguments[0], Value: arguments[1] });
        }

        return that;
    };

    //不传参数时默认是rar,有参数中用参数中值 none rar zip 7z...
    that.zip = that.compress = function () {
        current.compressType = "zip";
        return that;
    };

    //事件 未实现
    that.begin = function (fn) {
        if ($.isFunction(fn))
            current.begin = fn;
    }

    that.end = function (fn) {
        if ($.isFunction(fn))
            current.end = fn;
    }

    that.error = function (fn) {
        if ($.isFunction(fn))
            current.error = fn;
    }

    //下载处理
    that.download = function () {
        //前台检查  //todo
        //if (_isDownloading) return;

        var params = $.extend(true, {}, current);
        params.dataGenerate = JSON.stringify(current.dataGenerate);
        params.generateTitle = JSON.stringify(current.generateTitle);
        params.dataUrls = JSON.stringify(params.dataUrls);

        //创建iframe
        var downloadHelper = $('<iframe style="height: 0px; visibility: hidden;" id="downloadHelper"></iframe>').appendTo('body')[0];

        var doc = downloadHelper.contentWindow.document;
        if (doc) {
            doc.open();
            doc.write('')//微软为doc.clear();
            doc.writeln('<html><body><form id="downloadForm" name="downloadForm" method="post" action="/Service/File/Download">');
            for (var key in params) doc.writeln(String.format("<input type='hidden' name='{0}' value='{1}'>", key, params[key]));
            doc.writeln('<\/form><\/body><\/html>');
            doc.close();
            var form = doc.forms[0];
            if (form) {
                current.begin();
                form.submit();
                this._isDownloading = true;
            }
        }
    };

    var init = function () {
        //init zfile
        var _isDownloading = false;
        if (arg0) current = defaults; //只有$("#grid").zfile()访问时，才返回原来的设置

        if (arg0 instanceof Array) {  //打包下载
            current.downloadType = "url";
            current.dataUrls = arg0;
        }
        else if (typeof arg0 == 'string') //下载单个文件
        {
            if ($.inArray(arg0, $.fn.zfile.download.defaults.ext) > -1) { //下载画面列表数据
                //中文题头设置
                var dg = $that.data('datagrid');
                if (dg && dg.options && dg.options.columns) {
                    var titles = [];
                    $.each(dg.options.columns, function (i, item) { $.each(item, function (i, col) { if (!col.hidden) titles.push({ Key: col.field, Value: col.title }); }); });
                    that.title(titles);
                }
                current.downloadType = "generate";
                current.generateType = arg0;
                current.dataGenerate = $.extend(true, {}, $that.zdata().currentOptions()); //默认当前zdata
                that.paging(1, 0); //默认导出全部
            }
            else {
                current.downloadType = "url";
                current.dataUrls.push(arg0);;
            }
        }
        return that;
    }

    return init();

};
