document.write("<script language=\"javascript\" src=\"../../Scripts/jquery-1.8.2.min.js\"><\/script>");
document.write("<script language=\"javascript\" src=\"../../Scripts/PageViewJS.js\"><\/script>");

String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.LTrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.RTrim = function () {
    return this.replace(/(\s*$)/g, "");
}
/*格式化字符串*/
String.format = function (src) {
    if (arguments.length == 0) return null;
    var args = Array.prototype.slice.call(arguments, 1);
    return src.replace(/\{(\d+)\}/g, function (m, i) {
        return args[i];
    });
};
/*对象序列化为字符串*/
String.toSerialize = function (obj) {
    var ransferCharForJavascript = function (s) {
        var newStr = s.replace(
/[\x26\x27\x3C\x3E\x0D\x0A\x22\x2C\x5C\x00]/g,
function (c) {
    ascii = c.charCodeAt(0)
    return '\\u00' + (ascii < 16 ? '0' + ascii.toString(16) : ascii.toString(16))
}
);
        return newStr;
    }
    if (obj == null) {
        return null
    }
    else if (obj.constructor == Array) {
        var builder = [];
        builder.push("[");
        for (var index in obj) {
            if (typeof obj[index] == "function") continue;
            if (index > 0) builder.push(",");
            builder.push(String.toSerialize(obj[index]));
        }
        builder.push("]");
        return builder.join("");
    }
    else if (obj.constructor == Object) {
        var builder = [];
        builder.push("{");
        var index = 0;
        for (var key in obj) {
            if (typeof obj[key] == "function") continue;
            if (index > 0) builder.push(",");
            builder.push(String.format("\"{0}\":{1}", key, String.toSerialize(obj[key])));
            index++;
        }
        builder.push("}");
        return builder.join("");
    }
    else if (obj.constructor == Boolean) {
        return obj.toString();
    }
    else if (obj.constructor == Number) {
        return obj.toString();
    }
    else if (obj.constructor == String) {
        return String.format('"{0}"', ransferCharForJavascript(obj));
    }
    else if (obj.constructor == Date) {
        return String.format('"/Date({0})/"', obj.getTime() + 28800000);
    }
    else if (this.toString != undefined) {
        return String.toSerialize(obj);
    }
}
/*替换全部*/
String.prototype.replaceAll = function (reallyDo, replaceWith, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(reallyDo)) {
        return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi" : "g")), replaceWith);
    } else {
        return this.replace(reallyDo, replaceWith);
    }
}

/*日期格式化*/
function dateFormat(_date, _format) {
    return new Date(+/\d+/.exec(_date)).format(_format);
}

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

function clone(myObj) {
    if (typeof (myObj) != 'object') return myObj;
    if (myObj == null) return myObj;
    var myNewObj = new Object();
    for (var i in myObj)
        myNewObj[i] = clone(myObj[i]);
    return myNewObj;
}