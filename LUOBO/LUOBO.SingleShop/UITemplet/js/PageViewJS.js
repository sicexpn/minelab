function ShowPage(obj) {
    _PageEvents = obj.PageEvents;

    var strResult = "";
    var size = Math.floor(obj.PageShowSize / 2);
    var maxSize = obj.CurrentPage + size > obj.MaxPageSize ? obj.MaxPageSize : obj.CurrentPage + size;
    var minSize = obj.CurrentPage - size < 1 ? 1 : obj.CurrentPage - size;

    if (maxSize == obj.MaxPageSize)
        minSize = maxSize - obj.PageShowSize + 1;
    if (minSize == 1)
        maxSize = minSize + obj.PageShowSize - 1;

    for (var i = 0; i < obj.MaxPageSize; i++) {

        var curPage = i + 1;

        if (curPage == 1 || (curPage >= minSize && curPage <= maxSize) || curPage == obj.MaxPageSize) {
            var strPage = "";
            if (curPage == minSize && (obj.CurrentPage > obj.PageShowSize || minSize > 2))
                strPage += "...&nbsp;";
            if (obj.CurrentPage == curPage)
                strPage += "<li class='active'><a href='javascript:void(0)'>" + curPage + "</a></li>";
            else
                strPage += "<li href=\"javascript:void(0);\" onclick=\"_PageEvents(" + curPage + ");\"><a>" + curPage + "</a></li>";
            if (curPage == maxSize && obj.MaxPageSize - obj.CurrentPage - 1 > size)
                strPage += "&nbsp;...";

            strResult += strPage;
        }
    }

    if (obj.IsUpDown) {
        if (obj.CurrentPage == 1)
            strResult = "<li class='disabled'><a href='javascript:void(0)'>&laquo;</a></li>" + strResult;
        else
            strResult = "<li><a href=\"javascript:void(0);\" onclick=\"_PageEvents(" + (obj.CurrentPage - 1) + ");\">&laquo;</a></li>" + strResult;
        if (obj.CurrentPage == obj.MaxPageSize)
            strResult = strResult + "<li class='disabled'><a href='javascript:void(0)'>&raquo;</a></li>";
        else
            strResult = strResult + "<li><a href=\"javascript:void(0);\" onclick=\"_PageEvents(" + (obj.CurrentPage + 1) + ");\">&raquo;</a></li>";
    }

    obj.ShowElement.innerHTML = strResult;
}