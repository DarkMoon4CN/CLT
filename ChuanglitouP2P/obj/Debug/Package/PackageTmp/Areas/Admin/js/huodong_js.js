window.onload = function () {
    var aBtn1 = document.getElementById('z_ap11');
    var aAddEle = document.getElementById('tj_11');
    var aAddEle2 = document.getElementById('tj_12');
    
    if (aBtn1 != null && aBtn1 != 'undifined') {
        aBtn1.onclick = function () {
            var inner1 = document.createElement('li');
            aAddEle.insertBefore(inner1, aAddEle2);
            inner1.innerHTML = '<span>投资金额<input type="text" name="startAmt" id="startAmt" datatype="float"  nullmsg="金额不能为空" errormsg="金额格式不对! 如100.00"><i>-</i><input type="text" name="endAmt" id="endAmt" datatype="float"  nullmsg="金额不能为空" errormsg="金额格式不对! 如100.00">元</span><span class="z_span">赠送现金百分比<input type="text"   name="percent" id="percent"  datatype="float"  nullmsg="赠送现金百分比不能为空" errormsg="赠送现金百分比格式不对! 如5% 请输入0.50">%</span><span class="z_span">项目借款期限：<select style="width:60px;height: 18px;" name="LifeLoan" id="LifeLoan"><option value="0" selected="selected">不限</option><option value="1">1个月</option><option value="3">3个月</option><option value="6">6个月</option></select></span><a style="margin-left: 19px;" href="javascript:void(0)" id="z_ap12"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.style.marginBottom = "11px";
            inner1.style.marginLeft = "138px";
        };
        $("#z_ap12").live("click", function () {
            $(this).parent().remove();
        });
    }

    var aBtn2 = document.getElementById('z_ap13');
    if (aBtn2 != null && aBtn2 != 'undifined') {
        aBtn2.onclick = function () {
            var inner1 = document.createElement('li');
            aAddEle.appendChild(inner1);
            inner1.innerHTML = '<span>投资金额<input type="text" name="startAmt1"  id="startAmt1" datatype="float"  nullmsg="金额不能为空" errormsg="金额格式不对! 如100.00" ><i>-</i><input type="text"  name="endAmt1" id="endAmt1" datatype="float"  nullmsg="金额不能为空" errormsg="金额格式不对! 如100.00">元</span><span class="z_span">赠送现金百分比<input class="z_num" type="text" name="percent1" id="percent1"  datatype="float"  nullmsg="赠送现金百分比不能为空" errormsg="赠送现金百分比格式不对! 如5% 请输入0.50">%</span><a href="javascript:void(0)" id="z_ap14"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.className = 'zTab_li2';
            inner1.style.marginBottom = "11px";
            inner1.style.marginLeft = "138px";
        };
        $("#z_ap14").live("click", function () {
            $(this).parent().remove();
        });
    }

    var aBtn3 = document.getElementById('z_ap18');
    if (aBtn3 != null && aBtn3 != 'undifined') {
        aBtn3.onclick = function () {
            var inner1 = document.createElement('li');
            aAddEle.appendChild(inner1);
            inner1.innerHTML = '<span>投资金额<input type="text" name="startAmt1" id="startAmt1" datatype="float" nullmsg="金额不能为空" errormsg="金额格式不对! 如100.00"><i>-</i><input type="text" name="endAmt1" id="endAmt1" datatype="float" nullmsg="金额不能为空" errormsg="金额格式不对! 如100.00">元</span><span class="z_span">赠送现金<input class="z_num" type="text" name="percent1" id="percent1" datatype="float" nullmsg="赠送现金不能为空" errormsg="赠送现金格式不对! 如100.00">元</span> <span class="z_span">项目借款期限：<select style="width:60px;height: 18px;" name="LifeLoan1" id="LifeLoan1"><option value="0" selected="selected">不限</option><option value="1">1个月</option><option value="3">3个月</option><option value="6">6个月</option></select></span><a style="margin-left: 19px;" href="javascript:void(0)" id="z_ap19"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.className = 'zTab_li2';
            inner1.style.marginBottom = "11px";
            inner1.style.marginLeft = "138px";
        };
        $("#z_ap19").live("click", function () {
            $(this).parent().remove();
        });
    }


    var oBtn1 = document.getElementById('z_ap01');
    var oBtn2 = document.getElementById('z_ap02');
    var oAddEle = document.getElementById('tj_01');
    var oAddEle2 = document.getElementById('tj_02');


    if (oBtn1 != null && oBtn1 != 'undifined') {
        oBtn1.onclick = function () {
            var inner1 = document.createElement('li');
            oAddEle.insertBefore(inner1, oAddEle2);
            inner1.innerHTML = '<span>投资金额<input type="text" name="startAmt2_0" id="startAmt2_0" ><i>-</i><input type="text" name="endAmt2_0"  id="endAmt2_0 >元</span><span class="z_span">        赠送抵扣券<input class="z_num" type="text" name="percent2_0" id="percent2_0" >元</span><a href="javascript:void(0)" id="z_ap02"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.style.marginBottom = "11px";
            inner1.style.marginLeft = "138px";
            inner1.className = 'zTab_li2';
        };
        $("#z_ap02").live("click", function () {
            $(this).parent().remove();
        });
    }

    var oBtn3 = document.getElementById("z_ap03");
    var oAd_ul = document.getElementById("z_ad_ul01");
    var oAd_li = document.getElementById("z_ad_li01");

    if (oBtn3 != null && oBtn3 != 'undifined') {
        oBtn3.onclick = function () {
            var inner1 = document.createElement('li');
            oAd_ul.insertBefore(inner1, oAd_li);
            inner1.innerHTML = '<span>投资金额<input type="text" name="startAmt3_0" id="startAmt3_0"><i>-</i><input type="text" name="endAmt3_0" id="endAmt3_0">元</span><span class="z_span">赠送抵扣券<input type="text" name="num3_0" id="num3_0" >张，分别为<input type="text" class="z_41" name="Amtstr3_01" id="Amtstr3_01" >元、<input type="text" class="z_41" name="Amtstr3_02" id="Amtstr3_02">元</span><a href="javascript:void(0)" class="z_ap04" name="Amtstr3_0" id="Amtstr3_0"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.style.marginBottom = '11px';
            $(".z_ap04").live("click", function () {
                $(this).parent().remove();
            });
        }
    }

    var oBtn4 = document.getElementById("z_ap05");
    var oAd_ul1 = document.getElementById("z_ad_ul02");
    var oAd_li1 = document.getElementById("z_ad_li02");
    if (oBtn4 != null && oBtn4 != 'undifined') {
        oBtn4.onclick = function () {
            var inner1 = document.createElement('li');
            oAd_ul1.insertBefore(inner1, oAd_li1);
            inner1.innerHTML = '<span><input type="text"  name="cashAmtsplit1" id="cashAmtsplit1" >元抵扣券，投资金额<input type="text"  name="startAmtsplit1" id="startAmtsplit1"><i>-</i><input type="text" name="endAmtsplit1" id="endAmtsplit1" >元可用，过期时间<input type="text"  name="endTimesplit1" id="endTimesplit1"  class="z_time jcDate">或有效天数<input type="text" name="validity" id="validity"></span><span class="z_span">项目期限：<input name="startLife" style="width:1em;" /> - <input name="endLife" style="width:1em;" />个月</span><a href="javascript:void(0)" class="z_ap06"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.style.marginBottom = '28px';
            inner1.style.paddingLeft = '67px';

            $(".jcDate").jcDate({
                IcoClass: "jcDateIco",
                Event: "click",
                Speed: 100,
                Left: 0,
                Top: 28,
                format: "-",
                Timeout: 100
            });

            $(".z_ap06").live("click", function () {
                $(this).parent().remove();
            });
        }
    }


    //var oBtn5=document.getElementById("z_ap07");
    var oAd_ul2 = document.getElementById("z_ad_ul03");
    var oAd_li2 = document.getElementById("z_ad_li03");
    /*
	oBtn5.onclick = function () {
		var inner1=document.createElement('li');
		oAd_ul2.insertBefore(inner1,oAd_li2);
		inner1.innerHTML='<span><input type="text">元抵扣券，投资金额<input type="text"><i>-</i><input type="text">元可用，过期时间<input type="text" class="z_time jcDate"></span><a href="javascript:void(0)" class="z_ap08"><img src="/Areas/admin/img/icon_04.png" id="z_ap09"></a>';
		$(".z_ap08").live("click",function(){
			$(this).parent().remove();
		});
	};*/

    var oBtn6 = document.getElementById("z_ap_01");
    var oAd_ul3 = document.getElementById("z_aPul");
    var oAd_li3 = document.getElementById("z_aPLi");

    if (oBtn6 != null && oBtn6 != 'undifined') {
        oBtn6.onclick = function () {
            var inner1 = document.createElement('li');
            oAd_ul3.insertBefore(inner1, oAd_li3);
            inner1.innerHTML = '<span>投资金额<input type="text" name="startAmt4_0" id="startAmt4_0"><i>-</i><input type="text" name="endAmt4_0"  id="endAmt4_0">元</span><span class="z_span">赠送加息券百分比<input class="z_num" type="text" name="percent4" id="percent4">%</span><a id="z_ap_02" href="javascript:void(0)"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.className = "zTab_li2";
            inner1.style.paddingLeft = "138px";
            $("#z_ap_02").live("click", function () {
                $(this).parent().remove();
            });

        };
    }

    var oBtn7 = document.getElementById("z_ad_03");
    var oAd_ul4 = document.getElementById("z_aPul2");
    var oAd_li4 = document.getElementById("z_aPli2");
    if (oBtn7 != null && oBtn7 != 'undifined') {
        oBtn7.onclick = function () {
            var inner1 = document.createElement('li');
            oAd_ul4.insertBefore(inner1, oAd_li4);
            inner1.innerHTML = '<span>投资金额<input type="text"  name="startAmt5_0" id="startAmt5_0"><i>-</i><input type="text" name="endAmt5_0"  id="endAmt5_0">元</span><span class="z_span">赠送加息券<input type="text" name="num5_0" id="num5_0" >张，分别为<input type="text"  name="Amtstr5_01" id="Amtstr5_01">%、<input type="text" name="Amtstr5_02" id="Amtstr5_02">%、<input type="text" name="Amtstr5_03" id="Amtstr5_03" >%不等<a href="javascript:void(0)" id="z_ad_04"><img src="/Areas/admin/img/icon_04.png"></a></span>';
            inner1.style.marginBottom = "14px";
            $("#z_ad_04").live("click", function () {
                $(this).parent().parent().remove();
            });

        };
    }
    var oBtn8 = document.getElementById("z_ad_05");
    var oAd_ul5 = document.getElementById("z_aPul3");
    var oAd_li5 = document.getElementById("z_aPli3");
    if (oBtn8 != null && oBtn8 != 'undifined') {
        oBtn8.onclick = function () {
            var inner1 = document.createElement('li');
            oAd_ul5.insertBefore(inner1, oAd_li5);
            inner1.innerHTML = '<span><input type="text" name="cashAmtsplit3" id="cashAmtsplit3"> %加息券，投资金额<input type="text" name="startAmtsplit3"  id="startAmtsplit3"><i>-</i><input type="text" name="endAmtsplit3" id="endAmtsplit3">元可用，过期时间<input type="text" class="z_time jcDate" name="endTimesplit3" id="endTimesplit3">或有效天数<input type="text" name="validity2" id="validity2"></span><span class="z_span">项目期限：<input name="startLife2" style="width:1em;" /> - <input name="endLife2" style="width:1em;" />个月</span><a id="z_ad_06" href="javascript:void(0)"><img src="/Areas/admin/img/icon_04.png"></a>';
            inner1.style.marginBottom = "14px";

            $(".jcDate").jcDate({
                IcoClass: "jcDateIco",
                Event: "click",
                Speed: 100,
                Left: 0,
                Top: 28,
                format: "-",
                Timeout: 100
            });

            $("#z_ad_06").live("click", function () {
                $(this).parent().remove();
            });

        };
    }




}