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
            $("#t_d").html(PrefixInteger(day1, 2) + "天");
            $("#t_h").html(PrefixInteger(hour, 2) + "时");
            $("#t_m").html(PrefixInteger(minute,2) + "分");
            $("#t_s").hide();
            $("#t_s1").hide();
        } else {
            $("#t_d").hide();
            $("#t_d1").hide();
            $("#t_h").html(PrefixInteger(hour, 2) + "时");
            $("#t_m").html(PrefixInteger(minute,2) + "分");
            $("#t_s").html(PrefixInteger(second,2) + "秒");
        }
    }
}
function StartRtime(datetimes) {

    setInterval(function () { getRTime(datetimes); }, 1000);
}


//num传入的数字，n需要的字符长度
function PrefixInteger(num, n) {
    return (Array(n).join(0) + num).slice(-n);
}