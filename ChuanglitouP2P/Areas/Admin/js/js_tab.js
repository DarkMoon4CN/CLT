function g(o) { return document.getElementById(o); }
function Hovertab(num, counts, tag) {
    for (i = 1; i <= counts; i++) {
        g(tag + 'div0' + i).style.display = 'none';
    }
    g(tag + 'div0' + num).style.display = 'block';
}

$(function () {
    var groupItem = $(".z_span2");
    groupItem.each(function (index, element) {
        $(this).on("click", function () {
            var _this = $(this);
            if (!_this.parent().next().hasClass("off")) {
                _this.parent().next().addClass("off");
            } else {
                _this.parent().next().removeClass("off");
            }

        });
    });

    $("#checkAll").click(function () {
        var cAll = $(this).prop("checked");
        $(this).siblings("input[type='checkbox']").prop("checked", cAll);
        getTPIDs(this);
    }).siblings("input[type='checkbox']").click(function () {
        getTPIDs(this);
    });;
    function getTPIDs(obj) {
        var ids = "";
        $(obj).parent().children("input[type='checkbox'][actionData]").each(function () {
            if ($(this).prop("checked")) {
                ids += "1";
            } else {
                ids += "0";
            }
        });
        $("#hidTargetPlatform").val(ids);
    }

    var showText = $(".z_aShow");
    showText.each(function (index, element) {
        $(this).on("click", function () {
            var _this = $(this);
            if (!_this.parent().next().hasClass("on")) {
                _this.parent().next().addClass("on");
                _this.html('保存编辑<i>-</i>');
                _this.parent().siblings('.z_pText1').css('display', 'none')
            } else {
                _this.parent().next().removeClass("on");

                var inText = _this.parent().next().val();
                if (inText == "") {
                    _this.parent().siblings('.z_pText1').css('display', 'none')
                } else {
                    _this.parent().siblings('.z_pText1').css('display', 'block');
                    _this.parent().nextAll('.z_pText1').html(inText)
                }

                _this.html('编辑内容<i>+</i>');
            }

        });
    });

    var showTD = $(".z_tdShow");
    showTD.each(function (index, element) {
        $(this).on("mouseover", function () {
            var _this = $(this);
            _this.addClass("z_show");
            _this.children().css('display', 'block')
        });
        $(this).on("mouseout", function () {
            var _this = $(this);
            _this.removeClass("z_show");
            _this.children().css('display', 'none')
        });
    });

    //var aInDiv = $("#tab_01");
    //var aDDiv = $("#cheDele");
    var aInputs = $("#tab_01 input");
    var ChBtn = $("#cheAll");
    var aBtn = $("#cheAll input")[0];
    //var aTr = $("#tab_01 tr");

    ChBtn.click(function () {
        aInputs.prop("checked", aBtn.prop("checked"));
    });

    //var aInDiv = document.getElementById('tab_01');
    //if (aInDiv != undefined) {
    //    var aDDiv = document.getElementById('cheDele');
    //    var aInputs = aInDiv.getElementsByTagName('input');

    //    var ChBtn = document.getElementById('cheAll');
    //    var aBtn = ChBtn.getElementsByTagName('input')[0];
    //    var aTr = aInDiv.getElementsByTagName('tr')
    //    ChBtn.onclick = function () {
    //        if (aBtn.checked == true) {
    //            for (var i = 0; i < aInputs.length; i++) {
    //                aInputs[i].checked = true;
    //            }
    //        } else {
    //            for (var i = 0; i < aInputs.length; i++) {
    //                aInputs[i].checked = false;
    //            }
    //        }

    //    };
    //}

    $(".jx_link_01").each(function (index) {
        $(this).on("click", function () {
            if ($('#show_01').hasClass('off')) {
                $('#show_01').removeClass('off');
                $('#show_01').addClass('on');
            }
        })
    });
});
