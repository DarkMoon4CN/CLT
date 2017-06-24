var itime = 0;
function getRTime(datet) {
   
    var now = new Date();
    var endDate = new Date(datet);
    var leftTime = endDate.getTime() - now.getTime();
    var leftsecond = parseInt(leftTime / 1000);
    var day1 = Math.floor(leftsecond / (60 * 60 * 24));
    var hour = Math.floor((leftsecond - day1 * 24 * 60 * 60) / 3600);
    var minute = Math.floor((leftsecond - day1 * 24 * 60 * 60 - hour * 3600) / 60);
    var second = Math.floor(leftsecond - day1 * 24 * 60 * 60 - hour * 3600 - minute * 60);
    if (leftTime <= 0) {
        
       
          location.reload();
       
    }
    else {
        if (day1 > 0) {
            $("#t_d").html(day1 + "天");
            $("#t_h").html(hour + "时");
            $("#t_m").html(minute + "分");
            $("#t_s").hide();
        } else {
            $("#t_d").hide();
            $("#t_h").html(hour + "时");
            $("#t_m").html(minute + "分");
            $("#t_s").html(second + "秒");
        }
    }
}

function SetListRtime(datet,d,h,m,s)
{
   
    var now = new Date();
    var endDate = new Date(datet);
    var leftTime = endDate.getTime() - now.getTime();
    var leftsecond = parseInt(leftTime / 1000);
    var day1 = Math.floor(leftsecond / (60 * 60 * 24));
    var hour = Math.floor((leftsecond - day1 * 24 * 60 * 60) / 3600);
    var minute = Math.floor((leftsecond - day1 * 24 * 60 * 60 - hour * 3600) / 60);
    var second = Math.floor(leftsecond - day1 * 24 * 60 * 60 - hour * 3600 - minute * 60);
    if (leftTime <= 0) {
        location.reload();
    }
    else {
        if (day1 > 0) {
            d.html(day1 + "天");
            h.html(hour + "时");
            m.html(minute + "分");
            s.hide();
        } else {
            d.hide();
            h.html(hour + "时");
            m.html(minute + "分");
            s.html(second + "秒");
        }
    }
 

}

function SetListStartRtime(datet, d, h, m, s) {

    setInterval(function () { SetListRtime(datet, d, h, m, s); }, 1000);
}




function getREndTime(datet) {
    var now = new Date();
    var endDate = new Date(datet);
    var leftTime = endDate.getTime() - now.getTime();
    var leftsecond = parseInt(leftTime / 1000);
    var day1 = Math.floor(leftsecond / (60 * 60 * 24));
    var hour = Math.floor((leftsecond - day1 * 24 * 60 * 60) / 3600);
    var minute = Math.floor((leftsecond - day1 * 24 * 60 * 60 - hour * 3600) / 60);
    var second = Math.floor(leftsecond - day1 * 24 * 60 * 60 - hour * 3600 - minute * 60);
    if (leftTime <= 0) {
       // location.reload();
    }
    else {
        if (day1 > 0) {
          

            $("#detailed_end_timec").html("离项目竞标结束还有：" + day1 + " 天 " + hour + " 小时 " + minute + " 分 " + second + " 秒");


        } else {
            $("#detailed_end_timec").html("离项目竞标结束还有： " + hour + " 小时 " + minute + " 分 " + second + " 秒");
        }
    }
}

function StartREndTime(datetimes)
{
   
    setInterval(function () { getREndTime(datetimes); }, 1000);
}


function StartRtime(datetimes) {
  
    setInterval(function () { getRTime(datetimes); }, 1000);
}
function unbindbut() {
    $("#submit1").attr({ "disabled": "disabled" });
    $("#submit1").css("background", "#999")
    $("#submit1").val("还未开始");
    $("#investmentbtn").attr({ "disabled": "disabled" });
    $("#investmentbtn").css("background", "#999");
    $("#investmentbtn").val("还未开始");
}


function executeScript(html) {
    var reg = /<script[^>]*>([^\x00]+)$/i;
    //对整段HTML片段按<\/script>拆分
    var htmlBlock = html.split("<\/script>");
    for (var i in htmlBlock) {
        var blocks;//匹配正则表达式的内容数组，blocks[1]就是真正的一段脚本内容，因为前面reg定义我们用了括号进行了捕获分组
        if (blocks = htmlBlock[i].match(reg)) {
            //清除可能存在的注释标记，对于注释结尾-->可以忽略处理，eval一样能正常工作
            var code = blocks[1].replace(/<!--/, '');
            try {
                eval(code) //执行脚本
            }
            catch (e) {
            }
        }
    }
}