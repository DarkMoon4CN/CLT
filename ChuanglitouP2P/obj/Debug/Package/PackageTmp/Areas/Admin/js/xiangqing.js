$(function(){
	$(".xq_js1").click(function(){
		$(this).parent().next().toggle()
	})
	$(".xq_cz_td").mouseover(function(){
		$(this).find("div").show()
	})
	$(".xq_cz_td").mouseout(function(){
		$(this).find("div").hide()
	})
	/*点击切换上下箭头*/
	/*资金的tab*/
	function zijin(a,b){
		a.click(function(){
		$(this).css("background","#F95151").siblings().css("background","#7E7E7E")
		/*b.eq($(this).index()).css("display","block").siblings().css("display","none")*/
		$(this).parent().next().find(b).eq($(this).index()).css("display","block").siblings().css("display","none")
	})
	}
	zijin($(".xq_zj_con>ul>li"),".wsh_tab")
	zijin($(".xq_jl_con>ul>li"),".wsh_tab1")
	
	/*日历左边的tab切换*/
	/*$(".xq_cz1 em").click(function(){
		
		$(this).css("color","#003663").siblings().css("color","#333");
		$(".xq_cz_tab table").eq($(this).index()).show().siblings().hide()
	})*/
	 function tab(c,d){
	 	c.click(function(){
		$(this).css("color","#003663").siblings().css("color","#333");
		d.eq($(this).index()).show().siblings().hide();
		/*alert(d.length())*/
	})
	 }
	 tab($(".xq_cz2 em"),$(".xq_cz_tab1 table"))
	 tab($(".xq_cz3 em"),$(".xq_cz_tab2 table"))
	 tab($(".xq_cz5 em"),$(".xq_cz_tab5 table"))
	 
	 
	 $(".hy_cash_tab_td").mouseover(function(){
	 	$(this).find("div").show()
	 })
	  $(".hy_cash_tab_td").mouseout(function(){
	 	$(this).find("div").hide()
	 })
	  
	  /*点击详情的a*/
	 $(".gt_mid_p2 a").click(function(){
	 	$(this).css("color","#388EF1").siblings().css("color","#333")
	 	$(this).css("border-color","#388EF1").siblings().css("border-color","#fff")
	 })
})
