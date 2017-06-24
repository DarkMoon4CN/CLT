$(function(){
//------------------------视频分享

$(".gy_show").click(function(){
	$(".video_show").show();
	$(".gy_v02").show();
	var _offsetTop = $(".gy_v02").offset().top;
	$(".gy_v02").css('margin-top',(-_offsetTop)+'px');
});

$('.gy_btn').click(function(){
	$(".video_show").hide();
	$(".gy_v02").hide();	
	$(".gy_v02").css('margin-top',-275+'px');
});


//-----------------------媒体报道  图片切换

var aSpan=$('.gy_mt_left span');
var aCont=$('.gy_mt_mid a');
	for(var i=0;i<aSpan.length;i++){
		aSpan[i].index=i;
		$(aSpan[i]).mouseover(function(){
			for(var i=0;i<aSpan.length;i++){
				$(aSpan[i]).removeClass('gy_hover');
				$(aCont[i]).css('display','none');
			}
			$(this).addClass('gy_hover');
			$(aCont[this.index]).css('display','block');
		})
	}


/*---招贤纳士 last虚线-------------------------------*/

$('.gy_zx01').last().css('border','none');


/*----招贤纳士  -----------------------------------*/
$('.gy_zxCont').eq(0).css('display','block');
$('.gy_zx01 h2').eq(0).addClass('tabHover');
$('.gy_zx01 h2').click(function(){	
	
	if($(this).hasClass("tabHover")){
		$(this).removeClass("tabHover");
		$(this).next('div').css('display','none');
	}
	else{
		$(this).addClass("tabHover");
		$(this).next('div').css('display','block');
	}	
	
});

/*----  -----------------------------------*/

$('.gy_tact_bot dl:eq(0)').css('margin-right','150px');


/*----顶部下拉--------------------------*/

 
	
//alert($(document.body).height())
var top=$(document).height();
$('.video_show').css('height',top);

////导航跳转
//$('.gy_a1').click(function(){
//    $(window).scrollTop(0);
//    $(document).attr("title", "创利投--公司介绍");
//})
//$('.gy_a2').click(function(){
//    $(window).scrollTop(1340);
//    $(document).attr("title", "创利投--媒体报道");
//})
//$('.gy_a3').click(function(){
//    $(window).scrollTop(1905);
//    $(document).attr("title", "创利投--网站公告");
//})
//$('.gy_a4').click(function(){
//    $(window).scrollTop(3630);
//    $(document).attr("title", "创利投--常见问题");
//})
//$('.gy_a5').click(function(){
//    $(window).scrollTop(2788);
//    $(document).attr("title", "创利投--行业新闻");

//})
//$('.gy_a6').click(function(){
//    $(window).scrollTop(4765);
//    $(document).attr("title", "创利投--招贤纳士");
//})
//$('.gy_a7').click(function(){
//    $(window).scrollTop(5800);
//    $(document).attr("title", "创利投--联系我们");
//})


//alert($('.gy_tact').offset().top)  //1594//2360  3868  3043  5004  5987


})

