/**
* 模块名：共通脚本
* 程序名: 通用工具函数
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var utils = {};
 
/**
* 格式化字符串
* 用法:
.formatString("{0}-{1}","a","b");
*/
utils.formatString = function () {
    for (var i = 1; i < arguments.length; i++) {
        var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        arguments[0] = arguments[0].replace(exp, arguments[i]);
    }
    return arguments[0];
};

/**
* 格式化时间显示方式
* 用法:format="yyyy-MM-dd hh:mm:ss";
*/
utils.formatDate = function (v, format) {
    if (!v) return "";
    var d = v;
    if (typeof v === 'string') {
        if (v.indexOf("/Date(") > -1)
            d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
        else
            d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0]));//.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
    }
    var o = {
        "M+": d.getMonth() + 1,  //month
        "d+": d.getDate(),       //day
        "h+": d.getHours(),      //hour
        "m+": d.getMinutes(),    //minute
        "s+": d.getSeconds(),    //second
        "q+": Math.floor((d.getMonth() + 3) / 3),  //quarter
        "S": d.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};

/**  
* 格式化数字显示方式   
* 用法  
* formatNumber(12345.999,'#,##0.00');  
* formatNumber(12345.999,'#,##0.##');  
* formatNumber(123,'000000');
*/
utils.formatNumber = function (v, pattern) {
    if (v == null)
        return v;
    var strarr = v ? v.toString().split('.') : ['0'];
    var fmtarr = pattern ? pattern.split('.') : [''];
    var retstr = '';
    // 整数部分   
    var str = strarr[0];
    var fmt = fmtarr[0];
    var i = str.length - 1;
    var comma = false;
    for (var f = fmt.length - 1; f >= 0; f--) {
        switch (fmt.substr(f, 1)) {
            case '#':
                if (i >= 0) retstr = str.substr(i--, 1) + retstr;
                break;
            case '0':
                if (i >= 0) retstr = str.substr(i--, 1) + retstr;
                else retstr = '0' + retstr;
                break;
            case ',':
                comma = true;
                retstr = ',' + retstr;
                break;
        }
    }
    if (i >= 0) {
        if (comma) {
            var l = str.length;
            for (; i >= 0; i--) {
                retstr = str.substr(i, 1) + retstr;
                if (i > 0 && ((l - i) % 3) == 0) retstr = ',' + retstr;
            }
        }
        else retstr = str.substr(0, i + 1) + retstr;
    }
    retstr = retstr + '.';
    // 处理小数部分   
    str = strarr.length > 1 ? strarr[1] : '';
    fmt = fmtarr.length > 1 ? fmtarr[1] : '';
    i = 0;
    for (var f = 0; f < fmt.length; f++) {
        switch (fmt.substr(f, 1)) {
            case '#':
                if (i < str.length) retstr += str.substr(i++, 1);
                break;
            case '0':
                if (i < str.length) retstr += str.substr(i++, 1);
                else retstr += '0';
                break;
        }
    }
    return retstr.replace(/^,+/, '').replace(/\.$/, '');
};
 
/** 
* json格式转树状结构 
* @param   {json}      json数据 
* @param   {String}    id的字符串 
* @param   {String}    父id的字符串 
* @param   {String}    children的字符串 
* @return  {Array}     数组 
*/
utils.toTreeData = function (a, idStr, pidStr, childrenStr) {
    var r = [], hash = {},len = (a||[]).length;
    for (var i=0; i < len; i++) {
        hash[a[i][idStr]] = a[i];
    }
    for (var j=0; j < len; j++) {
        var aVal = a[j], hashVP = hash[aVal[pidStr]];
        if (hashVP) {
            !hashVP[childrenStr] && (hashVP[childrenStr] = []);
            hashVP[childrenStr].push(aVal);
        } else {
            r.push(aVal);
        }
    }
    return r;
};
 
utils.eachTreeRow = function(treeData,eachHandler) {
    for (var i in treeData) {
        if (eachHandler(treeData[i]) == false) break;
        if (treeData[i].children)
            utils.eachTreeRow(treeData[i].children, eachHandler);
    }
};

utils.isInChild = function (treeData,pid,id) {
    var isChild = false;
    utils.eachTreeRow(treeData, function (curNode) {
        if (curNode.id == pid) {
            utils.eachTreeRow([curNode], function (row) {
                if (row.id == id) {
                    isChild = true;
                    return false;
                }
            });
            return false;
        }
    });
    return isChild;
};
 
utils.fnValueToText = function (list, value, text) {
    var map = {};
    for (var i in list) {
        map[list[i][value || 'value']] = list[i][text || 'text'];
    }
    var fnConvert = function (v, r) {
        return map[v];
    };
    return fnConvert;
};

utils.compareObject = function (v1, v2) {
    var countProps = function (obj) {
        var count = 0;
        for (k in obj) if (obj.hasOwnProperty(k)) count++;
        return count;
    };

    if (typeof (v1) !== typeof (v2)) {
        return false;
    }

    if (typeof (v1) === "function") {
        return v1.toString() === v2.toString();
    }

    if (v1 instanceof Object && v2 instanceof Object) {
        if (countProps(v1) !== countProps(v2)) {
            return false;
        }
        var r = true;
        for (k in v1) {
            r = utils.compareObject(v1[k], v2[k]);
            if (!r) {
                return false;
            }
        }
        return true;
    } else {
        return v1 === v2;
    }
};

utils.minusArray = function (arr1, arr2) {
    var arr = [];
    for (var i in arr1) {
        var b = true;
        for (var j in arr2) {
            if (utils.compareObject(arr1[i],arr2[j])) {
                b = false;
                break;
            }
        }
        if (b) {
            arr.push(arr1[i]);
        }
    }
    return arr;
};

utils.diffrence = function (obj1, obj2, reserve,ignore) {
    var obj = {}, reserve = reserve || [], ignore = ignore || [], reserveMap = {}, ignoreMap = {};
    for (var i in reserve)
        reserveMap[reserve[i]] = true;

    for (var i in ignore)
        ignoreMap[ignore[i]] = true;

    for (var k in obj1) {
        if (!ignoreMap[k] && (obj1[k] != obj2[k] || reserveMap[k])) {
            obj[k] = obj1[k];
        }
    }
    return obj;
};

utils.filterProperties = function (obj, props, ignore) {
    var ret;
    if (obj instanceof Array || Object.prototype.toString.call(obj) === "[object Array]") {
        ret = [];
        for (var k in obj) {
            ret.push(utils.filterProperties(obj[k], props, ignore));
        }
    }
    else if (typeof obj === 'object') {
        ret = {};
        if (ignore) {
            var map = {};
            for (var k in props)
                map[props[k]] = true;

            for (var i in obj) {
                if (!map[i]) ret[i] = obj[i];
            }
        }
        else {
            for (var i in props) {
                var arr = props[i].split(" as ");
                ret[arr[1] || arr[0]] = obj[arr[0]];
            }
        }
    }
    else {
        ret = obj;
    }
    return ret;
};


utils.copyProperty = function (obj, sourcePropertyName, newPropertyName, overWrite) {
    if (obj instanceof Array || Object.prototype.toString.call(obj) === "[object Array]") {
        for (var k in obj) 
            utils.copyProperty(obj[k], sourcePropertyName, newPropertyName);
    }
    else if (typeof obj === 'object') {
        if (sourcePropertyName instanceof Array || Object.prototype.toString.call(sourcePropertyName) === "[object Array]") {
            for (var i in sourcePropertyName) {
                utils.copyProperty(obj, sourcePropertyName[i], newPropertyName[i]);
            }
        }
        else if (typeof sourcePropertyName === 'string') {
            if ((obj[newPropertyName] && overWrite) || (!obj[newPropertyName]))
                obj[newPropertyName] = obj[sourcePropertyName];
        }
    }
    return obj;
};

utils.clearIframe = function (context) {
    var frame = $('iframe', context).add(parent.$('iframe', context));
    if (frame.length > 0) {
        frame[0].contentWindow.document.write('');
        frame[0].contentWindow.close();
        frame.remove();
        if ($.browser.msie) {
            CollectGarbage();
        }
    }
};

utils.getThisIframe = function () {
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

utils.functionComment = function(fn){
    return fn.toString().replace(/^.*\r?\n?.*\/\*|\*\/([.\r\n]*).+?$/gm,'');
};

utils.uuid = (function () { var a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".split(""); return function (b, f) { var h = a, e = [], d = Math.random; f = f || h.length; if (b) { for (var c = 0; c < b; c++) { e[c] = h[0 | d() * f]; } } else { var g; e[8] = e[13] = e[18] = e[23] = "-"; e[14] = "4"; for (var c = 0; c < 36; c++) { if (!e[c]) { g = 0 | d() * 16; e[c] = h[(c == 19) ? (g & 3) | 8 : g & 15]; } } } return e.join("").toLowerCase(); }; })();

utils.getRequest = function (name, url) {
    var url = url|| window.location.href;
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.split("?")[1];
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest[name];
};
