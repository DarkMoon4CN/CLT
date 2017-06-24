$(function(){
	
	//点击h2  显示隐藏
	$(".left_nav h2").click(function(){
		
		if($(this).next('div').hasClass("show")){
			$(this).next('div').removeClass("show");
			$(this).next('div').addClass("hide");
			
			$(this).children().removeClass('hover');
			$(this).children().addClass('leave')
		}
		else{
			if($(this).next('div').hasClass("hide")){
				$(this).next('div').removeClass("hide");
			};
			
			$(this).next('div').addClass("show");
			$(this).children().addClass('hover');
			
			$(this).next('div').children('ul').eq(0).children('li').eq(0).children('a').addClass('a_checked');

			/*$(this).next('div').children('h3').eq(0).children('a').addClass('a_checked');*/
		}
	});	
	
	if($('.show_01').hasClass('show')){
		$('.show_01').siblings('.home').children('a').addClass('hover');	
	};
	if($('.show_02').hasClass('show')){
		$('.show_02').siblings('.home_01').children('a').addClass('hover');	
	};
	
	$(".left_nav1 h3").click(function(){
		
		
		$(".left_nav2 h3 a").removeClass('a_checked');
		$(".left_nav1 ul a").removeClass('a_checked');
		
		$(this).next('ul').children('li:eq(0)').children('a').addClass('a_checked');
		
		if($(this).next('ul').hasClass("hide")){
			$(this).next('ul').removeClass("hide")
			$(this).next('ul').addClass("show")
			
			$(this).children().addClass('hover');
			$(this).children().removeClass('leave')
		}
		else if($(this).next('ul').hasClass("show")){
			$(this).next('ul').removeClass("show")
			$(this).next('ul').addClass("hide")
			
			$(this).children().removeClass('hover');
			$(this).children().addClass('leave')
		}
	});	
	
	
	/*列表点击事件 */
	
	
	$(".left_nav1 ul li a").click(function(){
		$(".left_nav2 h3 a").removeClass('a_checked');
		if($(this).hasClass('a_checked')){
			return ;
		}else{
			$(".left_nav1 ul li a").removeClass('a_checked');
			$(this).addClass('a_checked');
		}
	});
	
	
	$(".left_nav2 h3 a").click(function(){
		$(".left_nav1 ul li a").removeClass('a_checked');
		if($(this).hasClass('a_checked')){
			return ;
		}else{
			$(".left_nav2 h3 a").removeClass('a_checked');
			$(this).addClass('a_checked');
		}
	});
	
	
	
	
	
	
	
	
	
})