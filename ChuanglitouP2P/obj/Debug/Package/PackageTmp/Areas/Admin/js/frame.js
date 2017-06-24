<!--
var $ = jQuery;
var thespeed = 5;
var navIE = document.all && navigator.userAgent.indexOf("Firefox")==-1;
var myspeed=0;
$(function(){
		
		//��ݲ˵�
		bindQuickMenu();
		
		//���˵�����
		LeftMenuToggle();
		
		//ȫ�����ܿ���
		AllMenuToggle();

		//ȡ��˵���������
		$(".head").find("a").click(function(){$(this).blur()});
		$(".menu").find("a").click(function(){$(this).blur()});
		
		/*
		//���������Ϣ
		$.get('getdedesysmsg.php',function(data){
			if(data != ''){
				$(".scroll").html(data);
				$(".scroll").Scroll({line:1,speed:500,timer:3000});
			}
			else
			{
				$(".scroll").html("�޷���ȡ��ʯ�ڹ����ҹٷ���Ϣ");
			}
		});
		*/
		
		
	}).keydown(function(event){//��ݼ�
		if(event.keyCode ==116 ){
			//url = $("#main").attr("src");
			//main.location.href = url;
			//return false;	
		}
		if(event.keyCode ==27 ){
			$("#qucikmenu").slideToggle("fast")
		}
});
	
function bindQuickMenu(){//��ݲ˵�
		$("#ac_qucikmenu").bind("mouseenter",function(){
			$("#qucikmenu").slideDown("fast");
		}).dblclick(function(){
			$("#qucikmenu").slideToggle("fast");
		}).bind("mouseleave",function(){
			hidequcikmenu=setTimeout('$("#qucikmenu").slideUp("fast");',700);
			$(this).bind("mouseenter",function(){clearTimeout(hidequcikmenu);});
		});
		$("#qucikmenu").bind("mouseleave",function(){
			hidequcikmenu=setTimeout('$("#qucikmenu").slideUp("fast");',700);
			$(this).bind("mouseenter",function(){clearTimeout(hidequcikmenu);});
		}).find("a").click(function(){
			$(this).blur();
			$("#qucikmenu").slideUp("fast");
			//$("#ac_qucikmenu").text($(this).text());
		});
}
	
function LeftMenuToggle(){//���˵�����
		$("#togglemenu").click(function(){
			if($("body").attr("class")=="showmenu"){
				$("body").attr("class","hidemenu");
				$(this).html("<img src='/admin/images/b2.png' border='0'>");
			}else{
				$("body").attr("class","showmenu");
				$(this).html("<img src='/admin/images/b1.png' border='0'>");
			}
		});
	}
	
	
function AllMenuToggle(){//ȫ�����ܿ���
		mask = $(".pagemask,.iframemask,.allmenu");
		$("#allmenu").click(function(){
				mask.show();
		});
		//mask.mousedown(function(){alert("123");});
		mask.click(function(){mask.hide();});
}
	
function AC(act){	
		//alert(act);
		mlink = $("a[id='"+act+"']");	
		if(mlink.size()>0){
			box = mlink.parents("div[id^='menu_']");
			boxid = box.attr("id").substring(5,128);
			if($("body").attr("class")!="showmenu")$("#togglemenu").click();
			if(mlink.attr("_url")){
				$("#menu").find("div[id^=menu]").hide();
				box.show();
				mlink.addClass("thisclass").blur().parents("#menu").find("ul li a").not(mlink).removeClass("thisclass");
				if($("#mod_"+boxid).attr("class")==""){
					$("#nav").find("a").removeClass("thisclass");
					$("#nav").find("a[id='mod_"+boxid+"']").addClass("thisclass").blur();
				}
				main.location.href = mlink.attr("_url");
			}else if(mlink.attr("_open") && mlink.attr("_open")!=undefined){
				window.open(mlink.attr("_open"));
			}
		}
}

/*********************
 * ������ť����
*********************/

function scrollwindow()
{
	parent.frames['menu'].scrollBy(0,myspeed);
}

function initializeIT()
{
	if (myspeed!=0) {
		scrollwindow();
	}
}


//�������
/*
(function($){
	$.fn.extend({
		Scroll:function(opt,callback){
			//�����ʼ��
			if(!opt) var opt={};
			var _this=this.eq(0).find("ul:first");
			var	lineH=_this.find("li:first").height(), //��ȡ�и�
				line=opt.line?parseInt(opt.line,10):parseInt(this.height()/lineH,10), //ÿ�ι���������Ĭ��Ϊһ�������������߶�
				speed=opt.speed?parseInt(opt.speed,10):500, //�?�ٶȣ���ֵԽ���ٶ�Խ����룩
				timer=opt.timer?parseInt(opt.timer,10):3000; //������ʱ���������룩
				if(line==0) line=1;
				var upHeight=0-line*lineH;
				//��������
				scrollUp=function(){
					_this.animate({
						marginTop:upHeight
					},speed,function(){
						for(i=1;i<=line;i++){
							_this.find("li:first").appendTo(_this);
						}
						_this.css({marginTop:0});
					});
				}
				//����¼���
				var timerID;
				timerID=setInterval("scrollUp()",timer);
				_this.mouseover(function(){
					clearInterval(timerID);			 
				}).mouseout(function(){
					timerID=setInterval("scrollUp()",timer);
				});
		}
	})
})(jQuery);
*/

-->
	

	
