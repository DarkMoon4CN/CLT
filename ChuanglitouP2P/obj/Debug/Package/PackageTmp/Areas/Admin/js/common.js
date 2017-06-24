$(function(){
	$(".jcDate").jcDate({					       
			IcoClass : "jcDateIco",
			Event : "click",
			Speed : 100,
			Left : 0,
			Top : 28,
			format : "-",
			Timeout : 100
	});
	/*$(".le_h3").mouseover(function(){
		$(this).css("backgroundColor","#01467D")
	});
	$(".le_h3").mouseout(function(){
		$(this).css("backgroundColor","#003664")
	});
	$(".le_h3").click(function(){
		if($(this).hasClass("go")){
			$(".xianshi").css("display","none");
			$(".xianshi").animate({height:0},250);
			$(this).css("backgroundColor","#003664")
			$(this).siblings().addClass("go");
			$(this).next().animate({height:200},250);
			$(this).next().css("display","block")
			$(this).removeClass("go")
			$(this).addClass("back")
			$(this).find("img").attr("src","img/xx.png")
		}
		else if($(this).hasClass("back"))
		{	
			$(this).css("backgroundColor","#01467D");
			$(this).next().animate({height:0},250)
			$(this).next().css("display","none")
			$(this).removeClass("back")
			$(this).addClass("go")
			$(this).find("img").animate(1000).attr("src","img/xy.png")
			$(this).siblings().find("img").attr("src","img/xy.png")
		}
	});
	$(".le_p1").mouseover(function(){
		$(this).css("color","#FB6530").siblings().css("color","#fff");
	})
	$(".le_p1").click(function(event){
		$(this).css("color","#FB6530").siblings().css("color","#fff");
		$(".le_p1").find("img").css("display","none");
		$(this).find("img").css("display","inline-block");
		var e = window.event || event;
		e.stopPropagation();
	});*/
	
	
	/*左侧的js样式*/
	
	$(".le_h3").click(function(){
		/*$(this).next().toggle()*/
		if($(this).next().hasClass("go")){
			$(this).next().removeClass("go")
			$(this).next().addClass("back")
			$(this).find("img").attr("src","img/xx.png");
			$(this).addClass('h3_hover');
			
		}
		else if($(this).next().hasClass("back")){
			$(this).next().removeClass("back")
			$(this).next().addClass("go")
			$(this).find("img").attr("src","img/xy.png");
			$(this).removeClass('h3_hover');
			
		}
	});
	
	/*$(".xianshi a").click(function(){
		$(this).css("color","#FB6530").siblings().css("color","#fff");
		$(".xianshi a").find("img").css("display","none");
		$(this).find("img").css("display","inline-block");
	})*/
	//当前菜单效果

	if($('.le_h4').hasClass('h3_hover')){
		$('.le_h4').find("img").attr("src","img/xx.png");
	};
	if($('.le_h5').hasClass('h3_hover')){
		$('.le_h5').find("img").attr("src","img/xx.png");
	}
	
	
	$(".tP4 span").click(function(){
		$(this).addClass("add").siblings().removeClass("add")
	});
	
	$(".tz_td").mouseover(function(){
		$(this).find("div").css("display","block")
	})
	$(".tz_td").mouseout(function(){
		$(this).find("div").css("display","none")
	});
	
	$(".tz_td div a").click(function(){
		$(this).css({"color":"#388EF0","background-image":"url(img/dian1.gif)"}).siblings().css({"color":"#53555E","background-image":"url(img/dian.jpg)"})
	});
	$('.z_tabEle ul a').click(function(){
		$(this).css({"color":"#388EF0","background-image":"url(img/dian1.gif)"}).siblings().css({"color":"#53555E","background-image":"url(img/dian.jpg)"})
	});
	$(".cz_td1 div a").click(function(){
		$(this).css({"color":"#388EF0","background-image":"url(img/dian1.gif)"}).siblings().css({"color":"#53555E","background-image":"url(img/dian.jpg)"})
	});
	
	$(".tzElse").mouseover(function(){
		$(this).find("div").css("display","block")
	})
	$(".tzElse").mouseout(function(){
		$(this).find("div").css("display","none")
	});
	
	//全选
	var oDiv = document.getElementById("checkbox")
	$(".checkbox").click(function(){		
		if(oDiv.checked){
			for(var i=0;i<$(".one").length;i++){
				$(".one")[i].checked=true
			}
		}
		else{
			for(var i=0;i<$(".one").length;i++){
				$(".one")[i].checked=false
			}
		}
	});
	
	/*奖励活动的js*/
	function tog(a){
		a.click(function(){
		
		if($(this).parent().next().hasClass("jl_go")){
			$(this).parent().next().removeClass("jl_go");
			$(this).parent().next().addClass("jl_back");
		}
		else if($(this).parent().next().hasClass("jl_back")){
			$(this).parent().next().removeClass("jl_back");
			$(this).parent().next().addClass("jl_go");
		}
	})
	}
	tog($(".jl_js1"));

	/*表单间隔颜色*/
var tabTr=document.getElementsByTagName('tr');

for(var i=0;i<tabTr.length;i++){
	tabTr[0].style.background="#e6e6e6";
	if(i%2!=0){
		tabTr[i].style.background='#fff'
	}else{
		tabTr[i].style.background='#f0f0f0'
	}
}
	
})

