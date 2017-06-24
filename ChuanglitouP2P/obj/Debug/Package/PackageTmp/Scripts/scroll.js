// JavaScript Document
$('.ddw').val(0);
$('.ddw2').val(0);
//缁欏尯鍩熸爣棰樿缃姩浣滃欢杩�
/*
setTimeout(function(){
$('.num').eq(0).find('p').stop().animate({'top':'50%'},500)
	},500);
*/	
//榧犳爣婊氬姩浜嬩欢
$(function(){
$('.num_box').mousewheel(function(event, delta) {
	var aaaa=$('.ddw2').val();
	if (aaaa == 1){
		return;	
	}
	qpgd(delta);
});
});
function qpgd(a){
	var z =$('.ddw').val();
    b = parseInt(z);
	c = $('.num').length;
if(a<0){
	if(-b==c-1){
		return;
	}
	b-=1;
	$('.ddw2').val(1);
	}else if(a>0){
		if(-b==0){
			return;
	}
	b+=1;
	$('.ddw2').val(1);
	}

//鍙充晶鎸夐挳瀵艰埅鍖哄煙鏍峰紡

$('.ddw').val(b);
$('.fixed_r li').eq(-b).addClass('on').siblings('li').removeClass('on');
$('.num').eq(-b).children('span').addClass('m2-help-animate');
var single_hh = $(window).height();
click_hh =-single_hh*b;
$('.num_box').animate({'bottom': click_hh},1000);
setTimeout(function(){
	$('.ddw2').val(0);
	},1400);
}
	$('.fixed_r li').eq(0).addClass('on');
	$('.num').eq(0).children('span').addClass('m2-help-animate');
	$('.fixed_r li').click(function(){
		var b = $(this).index();
		$('.num').eq(b).children('span').addClass('m2-help-animate');
		$(this).addClass('on').siblings('li').removeClass('on');
		$('.ddw').val(-b);
var single_hh = $(window).height();
click_hh =single_hh*b;
$('.num_box').animate({'bottom': click_hh});
		})
function quanp(){
var single_hh = $(window).height();
var single_ww = $(window).width();
$('.num').height(single_hh);
$('.num li').width(single_ww);
}
quanp();
$(window).resize(function(){
	if (typeof indexSlides != 'undefined' && indexSlides.reformat) 
		indexSlides.reformat();
		quanp();
});