/*滑过显示效果*/

$(function(){
	/*点击账户中心效果*/
	/*$(".wsh_zhzx").click(fuction(){
		alert(1)
		$(".conRig").css("display","block").siblings().css("display","none")
	})*/
	var qus = $(".qus");
	qus.mouseover(function(){
		$(".explain").css("display","block")
	})
	qus.mouseout(function(){
		$(".explain").css("display","none")
	})
	
	/*$(".commonTable tr:even").css("backgroundColor","#f00");*/
	/*$("table tr:eq(0)").css("backgroundColor","#f00");*/
	$('table').each(function(){
$(this).find('tr:even').css("background","#f9f9f9");
$(this).find('tr:odd').css("background","#fff");
});
	
/*本金  收益的TAB切换*/
	var b=$(".chart p") 
	b.click(function(){
		$("#box .tubiao").eq($(this).index()).css("display","block").siblings().css("display","none")
		$(this).addClass("pChange").siblings().removeClass("pChange")
	});
	
		/*提现  和提现记录的TAB切换*/
	$(".dTab p").click(function(){
	    $(this).addClass("dAdd").siblings().removeClass("dAdd")

		$(".dContent .hh").eq($(this).index()).css("display", "block").siblings().css("display", "none")

		$(".dContent>div").eq($(this).index()).css("display", "block").siblings().css("display", "none")

		$(".TContent>div").eq($(this).index()).css("display","block").siblings().css("display","none")
	});
function soto(){
	/*判断输入金额是否大于提现金额*/ 
	$(".dP4 input").focus(function(){
		$(this).css("color","#666")
	});
    /*
	$(".dP4 input").blur(function(){	

       
	    if (parsefloat($(this).val()) > 0)
	    {
	        if ($(this).val() <= parsefloat($(".dEm1").text()) && $(this).val() > 0) {
	            $(".dEm").css("display", "none")
	        }
	        else {
	            $(this).attr("value", " ")
	            $(".dEm").css("display", "inline-block")
	        }
	       
	    } else
	    {

	        alert("请正确输入提现金额");	       

	    }
        
		  
		
	});*/
	$(".dP6 input").focus(function(){
				$(this).css("color","#666")
				$(".dP6 input").blur(function(){ 
					var a = $(this).val();		
					var arr = [];
					$(".dEm6 i").each(function(){
						arr.push($(this).text())	
					})
					var b = arr.join("")
					console.log(b+"+"+a)
					if(a == b){
						$(".dEm5").css("display","none")
					}else{
						$(".dEm5").css("display","inline-block")
					}
				})
			})
}
soto()
/*点击提现===================
	$(".dBtn").click(function(){
            
	        $(".dP6 input").focus()
			$(".dP6 input").blur()
			$(".dP4 input").focus()
			$(".dP4 input").blur()
			
	});*/
//日历左边的点击TAB切换
var b=$(".dRl em") 
	b.click(function(){
		//$(".dRl table").eq($(this).index()).css("display","block").siblings().css("display","none")
		//$(".dRl_wsh table").eq($(this).index()).css("display","block").siblings().css("display","none")
		$(this).addClass("dEmAdd").siblings().removeClass("dEmAdd")

		var n = 0;
		switch($(this).html())
		{
            case "全部":
		        n = 0;
		        break;
		    case "今天":
		        n = 1;
		        break;
		    case "近一个月":
                n = 30;
		        break;
		    case "近三个月":
		        n = 90;
		        break;
		    default:
		        n = 0;		       
		}
		
		$('#timeday').val(n);
	});
	var c = $("#dRl em")
	c.click(function () {
	    //$(".dRl table").eq($(this).index()).css("display","block").siblings().css("display","none")
	    //$(".dRl_wsh table").eq($(this).index()).css("display","block").siblings().css("display","none")
	    $(this).addClass("dEmAdd").siblings().removeClass("dEmAdd")

	    var n = 0;
	    switch ($(this).html()) {
	        case "最近3天":
	            n = 3;
	            break;
	        case "最近7天":
	            n = 7;
	            break;
	        case "1个月":
	            n = 30;
	            break;
	        case "3个月":
	            n = 90;
	            break;
	        default:
	            n = 0;
	    }

	    $('#timeday').val(n);
	});

	var d = $("#dRlACT em")
	d.click(function () {
	    //$(".dRl table").eq($(this).index()).css("display","block").siblings().css("display","none")
	    //$(".dRl_wsh table").eq($(this).index()).css("display","block").siblings().css("display","none")
	    $(this).addClass("dEmAdd").siblings().removeClass("dEmAdd")

	    var n = 0;
	    switch ($(this).html()) {
	        case "最近3天":
	            n = 3;
	            break;
	        case "最近7天":
	            n = 7;
	            break;
	        case "1个月":
	            n = 30;
	            break;
	        case "3个月":
	            n = 90;
	            break;
	        default:
	            n = 0;
	    }

	    $('#timedayACT').val(n);
	});

	
//日历部分
$(".jcDate").jcDate({					       
			IcoClass : "jcDateIco",
			Event : "click",
			Speed : 100,
			Left : 0,
			Top : 28,
			format : "-",
			Timeout : 100
	});
//点击提现状态
$(".dState").mouseover(function(){
	$(".dZz").css("display","block")
	$(".dSh").css("display","block")
	$(this).addClass("dStateElse")
})
$(".dSh").mouseover(function(){
	$(".dZz").css("display","block")
	$(".dSh").css("display","block")
	$(this).addClass("dStateElse")
})
$(".dSh").mouseout(function(){
	$(".dZz").css("display","none")
	$(".dSh").css("display","none")
	$(this).removeClass("dStateElse")
});
$(".dState").mouseout(function(){
	$(".dZz").css("display","none")
	$(".dSh").css("display","none")
	$(this).removeClass("dStateElse")
});

$(".dSh span").click(function () { 
    if ($(this).hasClass("dSh1")) {
        $(this).removeClass("dSh1");
    } else {
        $(this).addClass("dSh1")
    }    
}) 


//页数的改变class
$(".dFy>i").click(function(){
	$(this).addClass("addBak").siblings().removeClass("addBak")
});

//抵扣券全部未使用的分类切换
$(".cTab p").click(function(){
		$(this).addClass("cPE").siblings().removeClass("cPE");
		$(".cTXs .dfs").eq($(this).index()).css("display", "block").siblings().css("display", "none")
	});
    //滑过抵扣券的效果


$(".xs .emP").mouseover(function () {
	$(".cCom").eq($(this).index()).css("display","block")
})
$(".xs .emP").mouseout(function(){
	$(".cCom").eq($(this).index()).css("display","none")
})
$(".xs1 .emP").mouseover(function(){
	$(".cCom1").eq($(this).index()).css("display","block")
})
$(".xs1 .emP").mouseout(function(){
	$(".cCom1").eq($(this).index()).css("display","none")
})
$(".xs2 .emP").mouseover(function(){
	$(".cCom2").eq($(this).index()).css("display","block")
})
$(".xs2 .emP").mouseout(function(){
	$(".cCom2").eq($(this).index()).css("display","none")
})
//抵扣券全部未使用的分类切换
$(".cTab1 p").click(function(){
		$(this).addClass("cPE").siblings().removeClass("cPE");
		$(".cTXs .wsh_changeP").eq($(this).index()).css("display","block").siblings().css("display","none")
	});
//滑过抵扣券的效果
$(".wsh_changeP .emP").mouseover(function(){
	$(this).find(".wsh_cCom").css("display","block")
})
$(".wsh_changeP .emP").mouseout(function(){
	$(this).find(".wsh_cCom").css("display","none")
})
//投资记录里面的TAB
$(".tSuces>em").click(function(){
	$(".tTab>table").eq($(this).index()-1).css("display","block").siblings().css("display","none")
})
//常用地址的input  点击文字消失
$(".pCom3 input").focus(function(){
	//$(this).attr("value"," ") ;
});
//修改头像
$("#pCha").click(function(){
	$(".pAdd").css("display","block")
})
$(".pImg").click(function(){
	$(".pAdd").css("display","none")
});
//修改账号
$(".pA").click(function(){
	$(".pC").removeAttr("disabled");
	/*$(".pC").css("outline","none")*/
	$(".pC").css("borderColor","#999");
	$(".pC").attr("value"," ");
	$(".pC").focus()
});
//修改账号的表单验证
$(".pC").blur(function(){
	var re = /^[A-Za-z0-9_-]{4,30}$/
	if(!re.test($(this).val())){
		
		/*$(".pHide").css("display","block")*/
	}
	else{
		$(".pHide").css("display","none")
	}
})
//添加地址
$(".pAdd1").click(function(){
	$(".pTxt").focus()
})
$(".pAdd2").click(function(){
	$(".pTxt2").focus()
})

/*-----身份证认证-----*/
function renzheng(){
	if($(".Renzheng").html()=="已认证"){
		$(".Renzheng").attr("href","javascript:;");
		$(".Renzheng").attr('disabled',"true");
	}else{
		$(".Renzheng").attr("href","javascript:;");
	}
}
renzheng();

//QQ号码的表单验证
$(".pTxt2").blur(function(){
	var re = /^[1-9][0-9]{5,10}$/
	if(!re.test($(this).val())){

		/*$(".pHide").css("display","block")*/
	}
	else{
		
	}
});
//修改手机和邮箱
$(".pSelect1 .pSI2").mouseover(function(){
	$(this).attr("src","/images/pXg.jpg");
	$(".pSelect1 strong").css("color","white")
})
$(".pSelect1 strong").mouseover(function(){
	$(".pSelect1 .pSI2").attr("src","/images/pXg.jpg");
	$(".pSelect1 strong").css("color","white")
})
$(".pSelect1 .pSI2").mouseout(function(){
	$(this).attr("src","/images/pXg1.jpg");
	$(".pSelect1 strong").css("color","#666")
})
$(".pSelect5 .pSI2").mouseover(function(){
	$(this).attr("src","/images/pXg.jpg");
	$(".pSelect5 strong").css("color","white")
})
$(".pSelect5 strong").mouseover(function(){
	$(".pSelect5 .pSI2").attr("src","/images/pXg.jpg");
	$(".pSelect5 strong").css("color","white")
})
$(".pSelect5 .pSI2").mouseout(function(){
	$(this).attr("src","/images/pXg1.jpg");
	$(".pSelect5 strong").css("color","#666")
})
//手机号的正则

function tel(){
$(".pTxt4").blur(function(){
	function top(){
	var re = /^1[3|4|5|8][0-9]{9}$/
	if(!re.test($(this).val())){
		$("#pTelyz").css("display","inline-block")
		return false;
	}
	else{
		$("#pTelyz").css("display","none")
		return true;
	}
}
	top()
})
};
tel()
/*console.log(top())*/
//验证码的正则

function yz(){
$(".pTxt5").blur(function(){
	function jugo(){
	var re = /^[0-9]{6}$/
	if(!re.test($(this).val())){
		$("#pTelyz1").css("display","inline-block")
		return false;
	}
	else{
		$("#pTelyz1").css("display","none")
		return true;
	}
}
})
};
yz()
/*alert(jugo())*/

//点击下一步
$(".pBtn3").click(function(){
	$(".pFir").css("display","none");
	$(".pSec").css("display","block")
})
$(".pBtn4").click(function(){
	$(".pSec").css("display","none");
	$(".pThr").css("display","block")
})
/*个人资料页面==================================================点击修改验证手机*/
$(".pSI2_else").click(function(){
	$(".pTel").css("display","block");
	$(".pYz").css("display","block");
	$(".pFir").css("display","block");
})

	/*点击修改邮箱*/
	$(".pSelect5").click(function(){
		$(".pEmail").css("display","block");
		$(".pYz").css("display","block");
		$(".pEFir1").css("display","block");
	})
/*回款计划页面=================================================点击Tab切换*/
$(".bAll i").click(function(){
	$(this).css("color","#0F91DD").siblings().css("color","#666");
	console.log($(this).index())
	$('#timeday').val($(this).index());
	//alert($(this).index());

	//$(".bTab table").eq($(this).index()).css("display","block").siblings().css("display","none"); 
	//$(".bTab1 table").eq($(this).index()).css("display","block").siblings().css("display","none"); 
})


    /*站内消息==================================================Tab*/
$(".nTab p").click(function(){
	$(this).addClass("nP").siblings().removeClass("nP");
	//$(".nMassage .nAll").eq($(this).index()).removeClass("nNo").siblings().addClass("nNo");
});

$(".nCommon b").live("click",function(){
	$(this).next().next().toggleClass("nNo")
});


//$(".nAdel").click(function () {
//    if (confirm("真的要删除该条记录吗1?")) {
//        alert("删除成功！");
//        var a = $(this).next().find(".nAll")
//        if (a.hasClass("nNo")) {
//            a.children().css("display", "none");
//        }
//        else {
//            a.children().css("display", "block");
//        }
//        return true;
//    } else {
//        return false;
//    }

//})

/*我要充值*/
$(".oTab p").click(function(){
	$(this).addClass("oAdd").siblings().removeClass("oAdd")
	$(".oChange>div").eq($(this).index()).css("display","block").siblings().css("display","none")
})
	/*账户余额相加*/
$(".oP3 input").focus(function(){
	//$(this).attr("value"," ") ;
	$(this).css("color","#333")
})
$(".oP3 input").blur(function(){
	if($(this).val()<100){
		$(".ochange1").css("display","block")
		console.log($(".oP2 em").html())
	}
	else {
	    var value = parseFloat($(this).val()) + parseFloat($(".oP2 em").html());
	    if (value.toString().indexOf('.') == -1) {
	        $(".ochange i").html(value.toString() + ".00");
	    } else 
	    {
	        $(".ochange i").html(value.toFixed(2));
	    }
	    $(".ochange1").css("display", "none");
	    //$(".ochange i").html(parseFloat($(this).val()) + parseFloat($(".oP2 em").html()));
	    $(".ochange2").css("display", "block");
	}
})
	/*点击充值按钮*/
	$(".oBtn").click(function(){
		$(".oP3 input").focus()
	});
$(".wsh_zhzx").click(function(){
		$(".conRig").css("display","block").siblings().css("display","none")
		$(".wsh_zhzx").siblings().removeClass("add")
	})
	
	/*加息券点击弹出框*/
	$(".jx_tc01").click(function(){
		$('.zhezhaoceng').css('display','block');
		$('#use_rules').css('display','block');
	})
	$(".jx_tCont .close").click(function(){
		$('.zhezhaoceng').css('display','none');
		$('#use_rules').css('display','none');
	})
	
	/*加息券点击弹出框*/
	$(".cEmE").click(function(){
		$('.zhezhaoceng').css('display','block');
		$('#get_jxq').css('display','block');
	})
	$("#get_jxq .close").click(function(){
		$('.zhezhaoceng').css('display','none');
		$('#get_jxq').css('display','none');
	})

	
 
	
})









