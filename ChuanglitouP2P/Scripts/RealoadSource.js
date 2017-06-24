//$(function () {
//    var version = $("#hidVersion").val();
//    function replaceURL(srcName,obj){
//        var thisSrc = obj.prop(srcName);
//        if ($.trim(thisSrc) == "") { return;}
//        obj.prop(srcName, thisSrc + (thisSrc.indexOf("?") > -1 ? "&" : "?") + version);
//    }
//    $("img,script").each(function () {
//        replaceURL("src", $(this));
//    });
//    $("link").each(function () {
//        replaceURL("href", $(this));
//    });
//});