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
	
	
	$(".hea_dw").mouseover(function(){
		$('#hea_p2').delay(250).css("background","#02BB00")
		$('#hea_p2').find("em").stop().animate({top:-24},250)
		
	})
	$(".hea_dw").mouseout(function(){
		$('#hea_p2').delay(250).css("background","#eee")
		$('#hea_p2').find("em").stop().animate({top:0},250)
	})
	$(".hea_qq").mouseover(function(){
		$('#hea_p3').delay(250).css("background","#3E91D5")
		$('#hea_p3').find("em").stop().animate({top:-24},250)
	})
	$(".hea_qq").mouseout(function(){
		$('#hea_p3').delay(250).css("background","#eee")
		$('#hea_p3').find("em").stop().animate({top:0},250)
	})
	var timer1=null;
	var timer2=null;
	$("#hea_p2").mouseover(function(){
		clearTimeout(timer1);
		$(".hea_dw").css("display","block")
	})
	$("#hea_p2").mouseout(function(){
		timer1=setTimeout(function(){
			$(".hea_dw").css("display","none")	
		},200)
		
	})
	$(".hea_dw").mouseover(function(){
		clearTimeout(timer1);
	})
	$(".hea_dw").mouseout(function(){
		timer1=setTimeout(function(){
			$(".hea_dw").css("display","none")	
		},200)
	})
	$("#hea_p3").mouseover(function(){
		clearTimeout(timer2);
		$(".hea_qq").css("display","block")
	})
	$("#hea_p3").mouseout(function(){
		timer2=setTimeout(function(){
			$(".hea_qq").css("display","none")	
		},200)
	})
	$(".hea_qq").mouseover(function(){
		clearTimeout(timer2);
	})
	$(".hea_qq").mouseout(function(){
		timer2=setTimeout(function(){
			$(".hea_qq").css("display","none")	
		},200)
	})
	
;

/*----顶部下拉--------------------------*/

$(window).scroll(function () {
      if ($(window).scrollTop() >=500) {
		  $(".gy_top").addClass('gy_fixed')
 	}else{
  		 $(".gy_top").removeClass('gy_fixed')
 	}
})

/*---新手指引---------------------------*/
$(".gud_btn li").hover(function(){
	$(this).find(".gud_a02").css('display','block');
},function(){
	$(this).find(".gud_a02").css('display','none');
})


/*--------我要投资   更改投资金额---------------------------------------------*/

$('.ct_change').click(function(){
	$('.in_change').attr('disabled',false);
	$('.in_change').focus();
})
$('.in_change').blur(function(){
	$('.in_change').attr('disabled',true);
});
/*--------充值并投资---------------------------------------------*/
var isTrue=true;
$('.ct3_top').click(function(){
	
	if(isTrue){
		$('.ct3_cont').css('display','block');
		$(this).css("background-image","url(images/ct_icon2.jpg)");
		isTrue=false;
	}else{
		$('.ct3_cont').css('display','none');
		$(this).css("background-image","url(images/ct_icon04.jpg)");
		isTrue=true;
	}
})

})



