
$(function () {
    var allFun = {};
    allFun.shortAjax = function (destStr, dataAll, succFun, errFun) {//Ajax简化
        $.ajax({
            url: destStr,
            type: "Post",
            data: dataAll,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (msg) {
                succFun(msg);
            },
            error: function (err) {
                if (errFun != undefined) {
                    errFun();
                }
                alert("通讯失败，请稍后重试！");
            }
        });
    }
    var rotateTimeOut = function () {
        $('#rotate').rotate({
            angle: 0,
            animateTo: 2160,
            duration: 8000,
            callback: function () {
                alert('网络超时，请检查您的网络设置！');
            }
        });
    };
    var bRotate = false;

    var rotateFn = function (awards, angles, txt) {
        bRotate = !bRotate;
        var shuy = awards;
        $('.pointer').stopRotate();
        $('.pointer').rotate({
            angle: 0,
            animateTo: angles + 1800 + 22.5,
            duration: 8000,
            callback: function () {
                if (shuy == "1") {
                    $('.tc_02').fadeIn();
                    $('.c_bg').fadeIn();
                    bRotate = !bRotate;
                } else if (shuy == "3" || shuy == "5") {
                    $('.tc_05').fadeIn();
                    $('.c_bg').fadeIn();
                    $('.tc_05 dt').html(txt);
                    bRotate = !bRotate;
                }
                else {
                    $('.tc_01').fadeIn();
                    $('.c_bg').fadeIn();
                    $('.tc_01 dt').html(txt);
                    if (shuy = "0" || shuy == "2") {
                        $('a.chakan').prop("href", '/Reward/xianjin');
                        $('a.shiyong').prop("href", '/Investlist.html');
                    }
                    if (shuy = "4" || shuy == "6" || shuy == "7") {
                        $('a.chakan').prop("href", '/Reward/jiaxi')
                        $('a.shiyong').prop("href", '/Investlist.html');
                    }
                    bRotate = !bRotate;
                }
            }
        })
    };
    function choujiangAction() {
        // if ($("#userid").val() =="0") {
        //    //$('.tc_01').fadeIn();
        //    //$('.c_bg').fadeIn();
        //    //$('.tc_01 dt').html('您还没有登录');
        //    alert('您还没有登录！');
        //    return;
        // }
        $(".zp_rotate").unbind("click");
        if (bRotate) return;
        var item = rnd(0, 7);
        var scount = $("#count").html();
        if (parseInt(scount) > 0) {
            $("#count").html(parseInt(scount) - 1);
        }

        function luckDrawAward(msg) {
            var res = $.parseJSON(msg);
            $(".zp_rotate").bind("click", choujiangAction);
            if (res.code == "0") {
                switch (res.data) {
                    case "0":
                        rotateFn(0, 360, '1%加息券');
                        break;
                    case "1":
                        rotateFn(1, 45, '谢谢参与');
                        break;
                    case "2":
                        rotateFn(2, 90, '2%加息券');
                        break;
                    case "3":
                        rotateFn(3, 135, '50元现金');
                        break;
                    case "4":
                        rotateFn(4, 180, '20元代金券');
                        break;
                    case "5":
                        rotateFn(5, 225, '50元现金');
                        break;
                    case "6":
                        rotateFn(6, 270, '10元代金券');
                        break;
                    case "7":
                        rotateFn(7, 315, '50元代金券');
                        break;
                }
            }
            else {
                //$('.tc_01').fadeIn();
                //$('.c_bg').fadeIn();
                //$('.tc_01 dt').html(res.data);
                if (res.code == "1") {
                    alert("您还没有登录");
                    return;
                }
                if (res.code == "2") {
                    $('.c_bg').fadeIn();
                    $(".tc_03").fadeIn();
                    return;
                }
                alert(res.data);
                return;
            }
        }
        //allFun.shortAjax("/Topic/T20160901/LuckDrawAward", "{}", luckDrawAward);
        $.post("/Topic/T20160901/LuckDrawAward", {}, function (result) { luckDrawAward(result); });

        console.log(item);
    }
    $(".zp_rotate").on("click", function () {
        choujiangAction();
    });

    (function () {
        //滚动效果

        var scrollingBox = document.getElementById("xstCont");
        function scrolling() {
            var bottom;
            var reachedBottom = false;
            var timer = null;
            var origin = scrollingBox.scrollTop++;
            if (origin >= scrollingBox.scrollTop && scrollingBox.scrollTop != 0) {
                if (!reachedBottom) {
                    scrollingBox.innerHTML += scrollingBox.innerHTML;
                    reachedBottom = true;
                    bottom = origin;
                } else {
                    scrollingBox.scrollTop = bottom;
                }
            }

            scrollingBox.style.overflow = "hidden";
        }
        var timer = setInterval(scrolling, 120);
        scrollingBox.onmouseover = function () { clearInterval(timer); }
        scrollingBox.onmouseout = function () { timer = setInterval(scrolling, 120) }
    })();


    //关闭按钮

    $('.guanbi').click(function () {
        $('.tc_cont').fadeOut();
        $('.c_bg').fadeOut();
    })
    $('.tc_cont .tc_close').click(function () {
        $('.tc_cont').fadeOut();
        $('.c_bg').fadeOut();
    })

    //规则弹窗  1
    $('.lucky_02 .c_right').click(function () {
        $('.c_bg').fadeIn();
        $('.gz_tc01').fadeIn();
    });
    $('.gz_tc01 .gz_btn').click(function () {
        $('.c_bg').fadeOut();
        $('.gz_tc01').fadeOut();
    })

    //规则弹窗 2
    $('.gz_btn2').click(function () {
        $('.c_bg').fadeIn();
        $('.gz_tc02').fadeIn();
    })
    $('.gz_tc02 .gz_btn').click(function () {
        $('.c_bg').fadeOut();
        $('.gz_tc02').fadeOut();
    })

    //规则弹窗 3
    $('.lucky_03 .c_right').click(function () {
        $('.c_bg').fadeIn();
        $('.gz_tc03').fadeIn();
    });
    $('.gz_tc03 .gz_btn').click(function () {
        $('.c_bg').fadeOut();
        $('.gz_tc03').fadeOut();
    })

    //规则弹窗 4
    $('.lucky_04 .c_right').click(function () {
        $('.c_bg').fadeIn();
        $('.gz_tc04').fadeIn();
    });
    $('.gz_tc04 .gz_btn').click(function () {
        $('.c_bg').fadeOut();
        $('.gz_tc04').fadeOut();
    })


});
function rnd(n, m) {
    return Math.floor(Math.random() * (m - n + 1) + n)
}


//滚动效果
//window.onload = function () {
//    //滚动效果

//    var scrollingBox = document.getElementById("xstCont");
//    alert(scrollingBox);
//    function scrolling() {
//        var bottom;
//        var reachedBottom = false;
//        var timer = null;
//        var origin = scrollingBox.scrollTop++;
//        if (origin >= scrollingBox.scrollTop) {
//            if (!reachedBottom) {
//                scrollingBox.innerHTML += scrollingBox.innerHTML;
//                reachedBottom = true;
//                bottom = origin;
//            } else {
//                scrollingBox.scrollTop = bottom;
//            }
//        }

//        scrollingBox.style.overflow = "hidden";
//    }
//    var timer = setInterval(scrolling, 120);
//    scrollingBox.onmouseover = function () { clearInterval(timer);}
//    scrollingBox.onmouseout = function () { timer = setInterval(scrolling, 120) }
//}