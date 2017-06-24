function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return decodeURI(r[2]); return null; //返回参数值
}
window.onload = function () {
    var paramVal = getUrlParam("app") == null ? "" : getUrlParam("app").toLowerCase();
    var paramP = getUrlParam("p") == null ? "" : getUrlParam("p").toLowerCase();
    function getUrlDestination(url) {
        var res = "";
        if (url.indexOf("register") > 0) { res = "register"; }
        if (url.indexOf("login") > 0) { res = "login"; }
        if (url.indexOf("usercenter") > 0) { res = paramP == "ios" ? "login" : "usercenter"; }
        if (url.indexOf("ProjectDetail") > 0) { res = "ProjectDetail"; }
        if (url.indexOf("invite") > 0) { res = "invite"; }
        if (res == "") { res = "home"; }
        return res;
    }
    if (paramVal == "clt") {
        $(".fixed_Bar").hide();
        $("header.header").hide();
        $("nav.susnav").hide();
        $("a").each(function () {
            var url = $(this).attr("href");
            var key = $(this).attr("tag"); //tag==key 表示不用移动端执行
            if (key != "key") {
                if (url != "#" && $.trim(url).length > 0) {
                    var dest = getUrlDestination(url);
                    if (dest.length > 0) {
                        var newUrl = "AppRedirect://" + dest;
                        $(this).prop("href", newUrl);
                    }
                }
            }
        });
    }
};