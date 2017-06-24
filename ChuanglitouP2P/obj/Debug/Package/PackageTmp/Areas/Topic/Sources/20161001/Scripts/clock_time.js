// JavaScript Document
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
        return false
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
