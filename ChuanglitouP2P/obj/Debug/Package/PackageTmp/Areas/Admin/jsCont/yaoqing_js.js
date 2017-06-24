// JavaScript Document
//邀请好友  添加页面
window.onload=function(){
	
	var oBtn1=document.getElementById('z_ap01');
	var oBtn2=document.getElementById('z_ap02');
	var oAddEle=document.getElementById('tj_01');
	var oAddEle2=document.getElementById('tj_02');
	
	

	oBtn1.onclick=function(){
		var inner1=document.createElement('li');
		oAddEle.insertBefore(inner1,oAddEle2);
		inner1.innerHTML='受邀好友投资金额<input type="text"><i>-</i><input type="text">，邀请人获得现金为投资额的<input type="text">%<a href="javascript:void(0)" id="z_ap02"><img src="img/icon_04.png"></a>';
	};
	$("#z_ap02").live("click",function(){
		$(this).parent().remove();
	});
	
	var oBtn4=document.getElementById("z_ap05");
	var oAd_ul1=document.getElementById("z_ad_ul02");
	var oAd_li1=document.getElementById("z_ad_li02");
	oBtn4.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul1.insertBefore(inner1,oAd_li1);
		inner1.innerHTML='<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time jcDate"></span><a href="javascriprt:void(0)" class="z_ap06"><img src="img/icon_04.png"></a>';
		inner1.style.marginBottom='28px';
		inner1.style.paddingLeft='138px';
		$(".z_ap06").live("click",function(){
			$(this).parent().remove();
		});
	};
	
	
	var oBtn5=document.getElementById("z_ap07");
	var oAd_ul2=document.getElementById("z_ad_ul03");
	var oAd_li2=document.getElementById("z_ad_li03");
	oBtn5.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul2.insertBefore(inner1,oAd_li2);
		inner1.innerHTML='<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time jcDate"></span><a href="javascriprt:void(0)" class="z_ap08"><img src="img/icon_04.png" id="z_ap09"></a>';
		$(".z_ap08").live("click",function(){
			$(this).parent().remove();
		});
	};
	
	
	var oBtn6=document.getElementById("z_ap15");
	var oAd_ul3=document.getElementById("z_ad_ul12");
	var oAd_li3=document.getElementById("z_ad_li12");
	oBtn6.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul3.insertBefore(inner1,oAd_li3);
		inner1.innerHTML='<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time jcDate"></span><a href="javascriprt:void(0)" class="z_ap16"><img src="img/icon_04.png"></a>';
		inner1.style.marginBottom='28px';
		inner1.style.paddingLeft='138px';
		$(".z_ap16").live("click",function(){
			$(this).parent().remove();
		});
	};
	
	
	var oBtn7=document.getElementById("z_ap17");
	var oAd_ul4=document.getElementById("z_ad_ul13");
	var oAd_li4=document.getElementById("z_ad_li13");
	oBtn7.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul4.insertBefore(inner1,oAd_li4);
		inner1.innerHTML='<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time jcDate"></span><a href="javascriprt:void(0)" class="z_ap18"><img src="img/icon_04.png" id="z_ap09"></a>';
		$(".z_ap18").live("click",function(){
			$(this).parent().remove();
		});
	};
	
}
