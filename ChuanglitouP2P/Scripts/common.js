/*公共js*/
$(function(){
	/*var a = $(".conLeft").find("a");
	a.click(function(){
		$(this).addClass("add").siblings().removeClass("add")
		console.log($(this).index())
		$(".choose").eq($(this).index()).css("display","block").siblings().css("display","none")
	})*/
	/* 滚动样式js*/
	 var index = 0;
    var adtimer;
    var _wrap = $("#container ol"); //
    var len = $("#container ol li").length;
    //len=1;
    if (len > 1) {
        $("#container").hover(function() {
            clearInterval(adtimer);
        },
        function() {
            adtimer = setInterval(function() {
                var _field = _wrap.find('li:first'); 
                var _h = _field.height();
                _field.animate({
                    marginTop: -_h + 'px'
                },
                500,
                function() { 
                    _field.css('marginTop', 0).appendTo(_wrap); 
                })
            },
            5000);
        }).trigger("mouseleave");
        function showImg(index) {
            var Height = $("#container").height();
            $("#container ol").stop().animate({
                marginTop: -_h + 'px'
            },
            1000);
        }
    }

    $("#container").find(".mouse_top").click(function() {
        var _field = _wrap.find('li:first');
        var last = _wrap.find('li:last'); 
        var _h = last.height();
        $("#container ol").css('marginTop', -_h + "px");
        last.prependTo(_wrap);
        $("#container ol").animate({
            marginTop: 0
        },
        500,
        function() { 
            //$("#container ol").css('marginTop',0).prependTo(_wrap);//���غ�,�����е�marginֵ����,�����뵽���,ʵ���޷����
        })
    });
    $("#container").find(".mouse_bottom").click(function() {
        var _field = _wrap.find('li:first'); 
        var _h = _field.height();
        _field.animate({
            marginTop: -_h + 'px'
        },
        500,
        function() { 
            _field.css('marginTop', 0).appendTo(_wrap); 
        })
    });
	/*----获取银行----------------------------
	
	$('.ad_oP').click(function(){
		$('.recOther').css('display','block');
		isBolck();
	});
	$('.recOther ul li a').click(function(){
		$('.ad_span').css('display','inline-block');
		$('.ad_oP').css('display','none')	
		$('.dImg1').attr('src', ($(this).children('img').eq(0).attr('src')));
		$('#blankName').val($(this).children('img').eq(0).attr('name'));
		//alert($(this).children('img').eq(0).attr('name'));
		$('#OpenBankId').val($(this).children('img').eq(0).attr('name'));

		$('#UsrBindCardID').val($(this).children('img').eq(0).attr('id'));
		
		$('.recOther').css('display','none');
		isBolck();	
	});
	
	$('.bankBtn_img').click(function(){
		$('.recOther').css('display','block');
		isBolck();	
	});
	function isBolck(){
		if($('.recOther').css('display')=='block'){
			$('.bankBtn_img').css('display','none')
		}else{
			$('.bankBtn_img').css('display','inline-block')	
		}	
	}*/
	
});

function canvas(a_canvas,data,n){
		//a_canvas  canvas元素
		//data  坐标Y轴高度
		//n  灰色图标个数
        window.addEventListener("load", function(){

         
		  
          // 获取上下文
          
          var context = a_canvas.getContext("2d");
		  

          // 绘制背景
          var gradient = context.createLinearGradient(0,0,0,600);


          gradient.addColorStop(0,"#fff");
          gradient.addColorStop(1,"#fff");
		  

          context.fillStyle = gradient;

          context.fillRect(0,0,a_canvas.width,a_canvas.height);

         
          // 描绘边框
          var grid_cols = data.length;/*1111*/
          var grid_rows = 3;
          var cell_height = a_canvas.height / grid_rows;
          var cell_width = a_canvas.width / grid_cols;
          context.lineWidth = 1;
          context.strokeStyle = "#999";

          // 结束边框描绘
          context.beginPath();
          // 准备画横线
          for (var col = 0; col <= grid_cols; col++) {
            var x = col * cell_width;
            context.moveTo(x,0);
            context.lineTo(x,a_canvas.height);
          }
		  context.lineWidth = 1;
          context.strokeStyle = "#e8e8e8";
          context.stroke();
		  
          // 准备画竖线
		  context.beginPath();
          for(var row = 0; row <= grid_rows; row++){
            var y = row * cell_height;
            context.moveTo(0,y);
            context.lineTo(a_canvas.width, y);
          }
          context.lineWidth = 0.5;
          context.strokeStyle = "#e8e8e8";
          context.stroke();

          var max_v = 0;
          for(var i = 0; i<data.length; i++){
            if (data[i] > max_v) { max_v = data[i]};
          }

          max_v = max_v * 1.1;
          // 将数据换算为坐标
          var points = [];
          for( var i=0; i < data.length; i++){
            var v= data[i];
            var px = cell_width *　(i);/*1111*/
            var py = a_canvas.height - a_canvas.height*(v / max_v);
            points.push({"x":px,"y":py});
          }
          // 绘制折现
          context.beginPath();
          context.moveTo(points[3].x, points[3].y);
          
		  for(var i= 3; i< points.length; i++){
            context.lineTo(points[i].x,points[i].y);
          }


          context.lineWidth = 1;
          context.strokeStyle = "#0f91dd";
          context.stroke();
		  
		  
		  context.beginPath();
		  context.moveTo(points[0].x, points[0].y);
		  for(var i= 1; i< points.length-6; i++){
            context.lineTo(points[i].x,points[i].y);
          }
		  context.lineWidth = 1;
          context.strokeStyle = "#999";
          context.stroke();

          //绘制坐标图形
          for(var i in points){
            var p = points[i];
			context.beginPath();
            
			
			if(i==0){
				context.arc(p.x,p.y,0,0,2*Math.PI,false);
			}else{
				context.arc(p.x,p.y,3,0,2*Math.PI);	
			};
			if(i<n){
				
				 context.fillStyle = "#999";
			}else{
				context.fillStyle = "#0f91dd";
			}
			
            context.fill();
          }
        },false);
      };
