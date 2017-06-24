$(function(){
	/*滑过qq微信的效果*/
	$(".hea_p2").mouseover(function(){
		$(this).delay(250).css("background","#02BB00")
		$(this).find("em").stop().animate({top:-24},250)
		
	})
	$(".hea_p2").mouseout(function(){
		$(this).delay(250).css("background","#eee")
		$(this).find("em").stop().animate({top:0},250)
	})
	$(".hea_p3").mouseover(function(){
		$(this).delay(250).css("background","#3E91D5")
	})
	
	$("#hea_p2").mouseover(function(){

		$(".hea_dw").css("display","block")
	})
	$("#hea_p2").mouseout(function(){
		
		$(".hea_dw").css("display","none")
	})
	$("#hea_p3").mouseover(function(){

		$(".hea_qq").css("display","block")
	})
	$("#hea_p3").mouseout(function(){
		
		$(".hea_qq").css("display","none")
	})
	
	/*登陆注册效果*/
	/*$(".hea_em4").mouseover(function(){
		$(".hea_p4 i").stop().animate({left:-37},250)
	})
	$(".hea_em4").mouseout(function(){
		$(".hea_p4 i").stop().animate({left:0},250)
	})*/
	//var index1 = 0;
	
	//document.getElementById("tar").onmouseover = function(e){
	//	if( !e ) e = window.event;
	//	var reltg = e.relatedTarget ? e.relatedTarget : e.fromElement;
	//	while( reltg && reltg != this ) reltg = reltg.parentNode;
	//	if( reltg != this ){
	//	// 这里可以编写 onmouseenter 事件的处理代码
	//	$(".hea_p4 i").stop().animate({left:-37},250)
	//	index1 = 1;
	//	}
	//}
	
	//document.getElementById("tar").onmouseout = function(e){
	//	if( !e ) e = window.event;
	//	var reltg = e.relatedTarget ? e.relatedTarget : e.toElement;
	//	while( reltg && reltg != this ) reltg = reltg.parentNode;
	//	if( reltg != this ){
	//	// 这里可以编写 onmouseleave 事件的处理代码
	//	index1 = 0;
	//	}
	//}
	
	//document.getElementById("gaizi").onmouseout = function(e){
	//	if( !e ) e = window.event;
	//	var reltg = e.relatedTarget ? e.relatedTarget : e.toElement;
	//	while( reltg && reltg != this ) reltg = reltg.parentNode;
	//	if( reltg != this ){
	//	// 这里可以编写 onmouseleave 事件的处理代码
	//		if (index1 == 0) {
	//			$(".hea_p4 i").stop().animate({left:0},250)
	//		}
			
	//	}
	//} 
	
	/*标的投资进度百分比*/
	//var sz = document.getElementById("shuzhi");
	//var sz1 = document.getElementById("shuzhi1");
	//var a = $(".div2_p3_i").width()
	//var b = parseFloat($(".bfb").html())/100
	//c=a*b
	//sz.style.width = c+'px'
	//sz1.style.width = c+'px'
	/*logo内容*/
	$(".logoA").mouseover(function(){
		$(this).find("img").attr("src","/images/logo2.png")
	})
	$(".logoA").mouseout(function(){
		$(this).find("img").attr("src","/images/logo_man.jpg")
	})
	
	/*content内容*/
		
		/* 滚动样式js*/
	 var index = 0;
    var adtimer;
    var _wrap = $("#container ol"); //
    var len = $("#container ol li").length;
    //len=1;
    if (len > 1) {
        $("#container").hover(function() {
            clearInterval(adtimer);
        },
        function() {
            adtimer = setInterval(function() {

                var _field = _wrap.find('li:first'); 
                var _h = _field.height();
                _field.animate({
                    marginTop: -_h + 'px'
                },
                500,
                function() { 
                    _field.css('marginTop', 0).appendTo(_wrap); 
                })

            },
            5000);
        }).trigger("mouseleave");
        function showImg(index) {
            var Height = $("#container").height();
            $("#container ol").stop().animate({
                marginTop: -_h + 'px'
            },
            1000);
        }
    }

    $("#container").find(".mouse_top").click(function() {
        var _field = _wrap.find('li:first');
        var last = _wrap.find('li:last'); 
        var _h = last.height();
        $("#container ol").css('marginTop', -_h + "px");
        last.prependTo(_wrap);
        $("#container ol").animate({
            marginTop: 0
        },
        500,
        function() { 
            //$("#container ol").css('marginTop',0).prependTo(_wrap);//���غ�,�����е�marginֵ����,�����뵽���,ʵ���޷����
        })
    });
    $("#container").find(".mouse_bottom").click(function() {
        var _field = _wrap.find('li:first'); 
        var _h = _field.height();
        _field.animate({
            marginTop: -_h + 'px'
        },
        500,
        function() { 
            _field.css('marginTop', 0).appendTo(_wrap); 
        })
    });
	
})
