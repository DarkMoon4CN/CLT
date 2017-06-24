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

    $(".zp_rotate").on("click", function () {
        if (EndTime2 < NowTime) {
            return false;
        }
        else {
            choujiangAction();
        }
    });

    //关闭按钮
    $('.tanchuang_main_guan').click(function () {
        $('.tanchuang').fadeOut();
    })
    //关闭按钮
    $('#ingBtn2').click(function () {
        $('.tanchuang').fadeOut();
    })
    //关闭按钮2
    $('.tanchuang2_main_guan').click(function () {
        $('.tanchuang2').fadeOut();
    })
    $("#ingBtn").click(function () {
        $('.tanchuang2').fadeOut();
    });
    //滚动效果
    $(function () {
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
        var timer = setInterval(scrolling, 30);
        scrollingBox.onmouseover = function () { clearInterval(timer); }
        scrollingBox.onmouseout = function () { timer = setInterval(scrolling, 30) }
    });
});

function rotateFn(awards, angles, txt) {
    $('.zhuanpan_jiang').stopRotate();
    $('.zhuanpan_jiang').rotate({
        angle: 0,
        animateTo: angles + 1800 + 22.5,
        duration: 8000,
        callback: function () {
            $(".tanchuang").fadeIn();
            $(".tanchuang_main1").fadeIn();
            $(".Prize").html(txt);

            $(".zp_rotate").on("click", function () {
                choujiangAction();
            });
        }
    })
};


function rnd(n, m) {
    return Math.floor(Math.random() * (m - n + 1) + n)
}

function getIosDate(strDate) {
    var arr = strDate.split(/[- :]/);
    return new Date(arr[0], arr[1] - 1, arr[2], arr[3], arr[4], arr[5]);
}
function choujiangAction() {
    $(".zp_rotate").unbind("click");
    var item = rnd(0, 7);
    var scount = $("#count").html();
    if (parseInt(scount) > 0) {
        $("#count").html(parseInt(scount) - 1);
    }

    function luckDrawAward(msg) {

        var res = $.parseJSON(msg);
        if (res.code == "0") {
            switch (res.data) {
                case "0":
                    rotateFn(0, 360, '20元抵扣券');
                    break;
                case "1":
                    rotateFn(1, 45, '5元现金');
                    break;
                case "2":
                    rotateFn(2, 90, '2%加息券');
                    break;
                case "3":
                    rotateFn(3, 135, '华为Mete9');
                    break;
                case "4":
                    rotateFn(4, 180, '50元抵扣券');
                    break;
                case "5":
                    rotateFn(5, 225, '小米手环(2代)');
                    break;
                case "6":
                    rotateFn(6, 270, '100元京东卡');
                    break;
                case "7":
                    rotateFn(7, 315, '1%加息券');
                    break;
            }
        }
        else {
            if (res.code == "2") {
                $(".tanchuang2").fadeIn();
            }
            else {
                alert(res.data);
            }
            $(".zp_rotate").on("click", function () {
                choujiangAction();
            });
        }
    }
    $.post("/activity/W20161212/LuckDrawAward", {}, function (result) { luckDrawAward(result); });

    console.log(item);
}
var NowTime;
var EndTime1;//= new Date('2016/12/8 19:15:00');
var EndTime2;//= new Date('2016/12/14 23:59:59');
var t;
(function () {
    EndTime1 = getIosDate($("#startTime").val());
    EndTime2 = getIosDate($("#endTime").val());
    NowTime = getIosDate($("#nowTime").val());
    t = EndTime1.getTime() - NowTime.getTime();
})();
var autoBind = false;
var timeone = setInterval(GetRTime, 1000);
function GetRTime() {
    var et = EndTime2;
    if (et >= NowTime) {
        $("#Btn1").hide();
        $("#loockA").show();
        $("#showBig").show();
        $(".zp_rotate").css("cursor", "pointer");
        $("#showBig").show();
        if ($("#LoginState").val() == 0 && $("#AppLoginState").val() == 0) {
            $("#Btn3").css("display", "block");
        }
        else { $("#Btn2").css("display", "block"); }
    }
    t = et.getTime() - NowTime.setSeconds(NowTime.getSeconds() + 1);
    timeCot(t);
    if (t < 0) {
        clearInterval(timeone);
        t = 0;
        timeCot(t);
        $(".zp_rotate").unbind();
        $("#Btn1").show();
        $("#loockA").hide();
        $("#showBig").hide();
        $("#Btn2").hide();
        $("#Btn3").hide();
    } 
}
function timeCot(t) {
    var r = Math.floor(t / 1000 / 60 / 60 / 24);
    var h = Math.floor(t / 1000 / 60 / 60 % 24);
    var m = Math.floor(t / 1000 / 60 % 60);
    var s = Math.floor(t / 1000 % 60);

    document.getElementById("t_r").innerHTML = r;
    document.getElementById("t_h").innerHTML = h;
    document.getElementById("t_m").innerHTML = m;
    document.getElementById("t_s").innerHTML = s;
    if (document.getElementById("t_r").innerHTML < 10) {
        document.getElementById("t_r").innerHTML = "0" + document.getElementById("t_r").innerHTML;
    }
    if (document.getElementById("t_h").innerHTML < 10) {
        document.getElementById("t_h").innerHTML = "0" + document.getElementById("t_h").innerHTML;
    }
    if (document.getElementById("t_m").innerHTML < 10) {
        document.getElementById("t_m").innerHTML = "0" + document.getElementById("t_m").innerHTML;
    }
    if (document.getElementById("t_s").innerHTML < 10) {
        document.getElementById("t_s").innerHTML = "0" + document.getElementById("t_s").innerHTML;
    }
    if (document.getElementById("t_r").innerHTML < 0) {
        document.getElementById("t_r").innerHTML == 0;
    } else if (document.getElementById("t_h").innerHTML < 0) {
        document.getElementById("t_h").innerHTML == 0;
    } else if (document.getElementById("t_m").innerHTML < 0) {
        document.getElementById("t_m").innerHTML == 0;
    } else if (document.getElementById("t_s").innerHTML < 0) {
        document.getElementById("t_s").innerHTML == 0;
    }
}
GetRTime();
//setInterval(GetRTime, 1000);


//***************************************以下是跑马灯
var flag = true;
function aw() {
    if (flag) {
        $(".dengDode").show();
        $(".dengUp").hide();
        flag = false;
    } else {
        $(".dengUp").show();
        $(".dengDode").hide();
        flag = true;
    }
}
var timeTwo = setInterval(aw, 700);
