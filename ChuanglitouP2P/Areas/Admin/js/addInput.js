// JavaScript Document
window.onload=function(){
	
	var oBtn1=document.getElementById('z_ap01');
	var oBtn2=document.getElementById('z_ap02');
	var oAddEle=document.getElementById('tj_01');
	var oAddEle2=document.getElementById('tj_02');
	
	

	oBtn1.onclick=function(){
		var inner1=document.createElement('li');
		oAddEle.insertBefore(inner1,oAddEle2);
		inner1.innerHTML = '<span>投资金额<input type="text"><i>-</i><input type="text">元</span><span class="z_span">赠送现金百分比<input class="z_num" type="text">%</span><a href="javascript:void(0)" id="z_ap02"><img src="/Areas/Admin/img/icon_04.png"></a>';
		inner1.className="ad_01";
	};
	$("#z_ap02").live("click",function(){
		$(this).parent().remove();
	});

	var oBtn3=document.getElementById("z_ap03");
	var oAd_ul=document.getElementById("z_ad_ul01");
	var oAd_li=document.getElementById("z_ad_li01");
	
	oBtn3.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul.insertBefore(inner1,oAd_li);
		inner1.innerHTML = '<span>投资金额<input type="text"><i>-</i><input type="text">元</span><span class="z_span">赠送抵扣券<input type="text">张，分别为<input type="text" class="z_41">元、<input type="text" class="z_41">元不等</span><a href="javascript:void(0)" class="z_ap04"><img src="/Areas/Admin/img/icon_04.png"></a>';
		inner1.style.marginBottom='41px';
		$(".z_ap04").live("click",function(){
			$(this).parent().remove();
		});
	}
	
	var oBtn4=document.getElementById("z_ap05");
	var oAd_ul1=document.getElementById("z_ad_ul02");
	var oAd_li1=document.getElementById("z_ad_li02");
	oBtn4.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul1.insertBefore(inner1,oAd_li1);
		inner1.innerHTML = '<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time jcDate"></span><a href="javascript:void(0)" class="z_ap06"><img src="/Areas/Admin/img/icon_04.png"></a>';
		inner1.style.marginBottom='28px';
		inner1.style.paddingLeft='138px';
		$(".z_ap06").live("click",function(){
			$(this).parent().remove();
		});
	}
	
	var oBtn5=document.getElementById("z_ap07");
	var oAd_ul2=document.getElementById("z_ad_ul03");
	var oAd_li2=document.getElementById("z_ad_li03");
	oBtn5.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul2.insertBefore(inner1,oAd_li2);
		inner1.innerHTML = '<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time jcDate"></span><a href="javascript:void(0)" class="z_ap08"><img src="/Areas/Admin/img/icon_04.png" id="z_ap09"></a>';
		$(".z_ap08").live("click",function(){
			$(this).parent().remove();
		});
	};
	
	var oBtn6=document.getElementById("z_ap_01");
	var oAd_ul3=document.getElementById("z_aPul");
	var oAd_li3=document.getElementById("z_aPLi");
	oBtn6.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul3.insertBefore(inner1,oAd_li3);
		inner1.innerHTML = '<span>投资金额<input type="text"><i>-</i><input type="text">元</span><span class="z_span">赠送加息券百分比<input class="z_num" type="text">%</span><a id="z_ap_02" href="javascript:void(0)"><img src="/Areas/Admin/img/icon_04.png"></a>';
		inner1.className="zTab_li2";
		inner1.style.paddingLeft="138px";
		$("#z_ap_02").live("click",function(){
			$(this).parent().remove();
		});
		
	};
	
	var oBtn7=document.getElementById("z_ad_03");
	var oAd_ul4=document.getElementById("z_aPul2");
	var oAd_li4=document.getElementById("z_aPli2");
	oBtn7.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul4.insertBefore(inner1,oAd_li4);
		inner1.innerHTML = '<span>投资金额<input type="text"><i>-</i><input type="text">元</span><span class="z_span">赠送加息券<input type="text">张，分别为<input type="text">%、<input type="text">%、<br><input type="text">%不等<a href="javascript:void(0)" id="z_ad_04"><img src="/Areas/Admin/img/icon_04.png"></a></span>';
		inner1.style.marginBottom="14px";
		$("#z_ad_04").live("click",function(){
			$(this).parent().parent().remove();
		});
		
	};
	var oBtn8=document.getElementById("z_ad_05");
	var oAd_ul5=document.getElementById("z_aPul3");
	var oAd_li5=document.getElementById("z_aPli3");
	oBtn8.onclick=function(){
		var inner1=document.createElement('li');
		oAd_ul5.insertBefore(inner1,oAd_li5);
		inner1.innerHTML = '<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time"></span><a id="z_ad_06" href="javascript:void(0)"><img src="/Areas/Admin/img/icon_04.png"></a>';
		inner1.style.marginBottom="14px";
		$("#z_ad_06").live("click",function(){
			$(this).parent().remove();
		});
		
	};
	
	
}