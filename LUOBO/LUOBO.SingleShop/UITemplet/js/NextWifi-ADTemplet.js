


function getNextADTemplet(item) {
    var templethtml = "";
    var tmpvalue = "";
    for (var j = 0; j < item.File_Templet.length; j++) {
        tmpvalue = "";
        for (var k = 0; k < item.File_Content.length; k++) {
            if (item.File_Content[k].TKey == ("Templet_" + j)) {
                tmpvalue = item.File_Content[k].TValue;
                break;
            }
        }
        if (item.File_Templet[j].Unit_Type == "txt" || item.File_Templet[j].Unit_Type == "parameter") {
            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ：</td><td colspan=2><input class='form-control' id='Templet_" + j + "' name='Templet_" + j + "' type='text' value='" + tmpvalue + "' /></div></td></tr>";
        } else if (item.File_Templet[j].Unit_Type == "pic") {
            templethtml += "<tr><td><div >" + item.File_Templet[j].Unit_Name + " ：</td><td> " + tmpvalue + " </div><div id='Templet_" + j + "_div'> <input class='pull-left' id='Templet_" + j + "' name='Templet_" + j + "' type='file' /> <a class='pull-right' href='javascript:ChangePicSelect(\"Templet_" + j + "_div\",1,\"Templet_" + j + "\")'>选择已上传图片</a></div></td></tr>";
        }
        if (item.File_Templet[j].Unit_Link == "true") {
            tmpvalue = "";
            for (var k = 0; k < item.File_Content.length; k++) {
                if (item.File_Content[k].TKey == ("Templet_" + j + "_link")) {
                    tmpvalue = item.File_Content[k].TValue;
                    break;
                }
            }
            templethtml += "<div><td>连接地址：</td><td colspan=2><input class='form-control' id='Templet_" + j + "_link' name='Templet_" + j + "_link' type='text' value='" + tmpvalue + "' /></div>";
        }
    }
    return templethtml;
}


function ChangePicSelect(mydiv, p, n) {
    $("#" + mydiv).empty();
    if (p == 0) {
        $("#" + mydiv).append("<input class='pull-left' id='" + n + "' name='" + n + "' type='file' /> <a class='pull-right' href='javascript:ChangePicSelect(\"" + mydiv + "\",1,\"" + n + "\")'>选择已上传图片</a>");
    } else {
        $("#" + mydiv).append("<input id='" + n + "' name='" + n + "' type='text'/> <input  type='button' onclick='javascript:SelectServerPic(\"" + n + "\")' value='选择' /> <a class='pull-right' href='javascript:ChangePicSelect(\"" + mydiv + "\",0,\"" + n + "\")'>选择本地文件上传</a>");
    }
}