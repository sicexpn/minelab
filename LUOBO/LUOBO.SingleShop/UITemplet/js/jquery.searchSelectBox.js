/*
* Date:2014-07-04
* Plugin name:jQuery.searchSelectBox
* Version:0.01
* Description: 

*/

(function ($) {
    $.fn.searchSelectBox = function (datas, options) {
        var isInDropdownList = false;
        var opts = $.extend({
            key: "key",
            value: "value",
            boxWidth: '200px',
            itemWidth: '150px',
            listWidth: '360px',
            listColnum: 4,
            slideDownTime: 250,
            slideUpTime: 250
        }, options);

        return this.each(function () {
            var $this = $(this);

            $this.prop("class", "form-control input-sm");
            $this.wrapAll("<div class='input-group' style='width:" + opts.boxWidth + ";float: left;' />");
            $this.parent().append("<span class='input-group-addon dropdown-caret-down' style='padding:0px 6px;cursor: pointer;'></span>");
            var ul = $("<ul class='dropdownlist clearfix' style='display:none; z-index:100000000; overflow:auto; max-height:200px'></ul>");

            for (var i = 0; i < datas.length; i++) {
                if (i == 0) {
                    $this.prop("key", datas[i][opts.key]);
                    $this.val(datas[i][opts.value]);
                }
                ul.append("<li><label for='" + datas[i][opts.key] + "'>" + datas[i][opts.value] + "</label></li>");
            }
            $this.parent().append(ul);

            $this.parent().find("span").bind('click', function () {
                $this.focus();
            });

            ul.find("li").bind('click', function () {
                $this.prop("key", $(this).find("label:eq(0)").prop("for"));
                $this.val($(this).find("label:eq(0)").html());
                ul.slideUp(opts.slideUpTime);
                $this.change();
            });

            $this.bind('focus', function () {
                if (navigator.platform.indexOf("Win") > -1 || navigator.platform.indexOf("Mac") > -1) {
                    ul.width(($(".dropdownlist").find("li").width() + 5) * opts.listColnum + 17);
                }
                else
                    ul.width($(this).width() + 12);
                ul.slideDown(opts.slideDownTime);
            });
            $this.bind('blur', function () {
                if (isInDropdownList)
                    return;
                ul.slideUp(opts.slideUpTime);
            });
            $this.bind('keyup', function () {
                ul.empty();
                for (var i = 0; i < datas.length; i++) {
                    if (datas[i][opts.value].indexOf($this.val()) > -1) {
                        ul.append("<li><label for='" + datas[i][opts.key] + "'>" + datas[i][opts.value] + "</label></li>");
                    }
                }
            });

            $(ul).bind('mouseenter', function () {
                isInDropdownList = true;
            });
            $(ul).bind('mouseleave', function () {
                isInDropdownList = false;
            });
        });
    };
})(jQuery);