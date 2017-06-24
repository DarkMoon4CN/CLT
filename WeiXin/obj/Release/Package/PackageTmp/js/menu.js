$(function(){
	var $divNav = $(".susnav ul li");
	$divNav.click(function () {
	    
	    $(this).addClass("menu_curr").siblings().removeClass("menu_curr");
		if ($(this).hasClass("cura")) {
			$(this).find(".subnav").slideUp(); //当前菜单下的二级菜单隐藏
			$divNav.removeClass("cura"); //同一级的菜单项
		} else {
			$divNav.removeClass("cura"); //移除所有的样式
			$(this).addClass("cura"); //给当前菜单添加特定样式
			$divNav.find(".subnav").slideUp("fast"); //隐藏所有的二级菜单
			$(this).find(".subnav").slideDown("fast"); //展示当前的二级菜单
		}
	});
});