$(function () {
    //点击h2  显示隐藏
    $(".left_nav h2").click(function () {
        if ($(this).next('div').hasClass("show")) {
            $(this).next('div').removeClass("show");
            $(this).next('div').addClass("hide");

            $(this).children().removeClass('hover');
            $(this).children().addClass('leave');
            $(this).children('a').attr('href', 'javascript:void(0)');
        }
        else {
            if ($(this).next('div').hasClass("hide")) {
                $(this).next('div').removeClass("hide"); 
                $(this).siblings('div').removeClass("show").addClass("hide");
            };
            $(this).children('a').attr('href', $(this).next('div').children('ul').children('li').children('a').eq(0).attr('href'));
            $(this).next('div').addClass("show");
            $(this).children().addClass('hover');
            $(this).children().removeClass('leave');
        }
    });

    if ($('.show_01').hasClass('show')) {
        $('.show_01').siblings('.home').children('a').addClass('hover');
    };
    if ($('.show_02').hasClass('show')) {
        $('.show_02').siblings('.home_01').children('a').addClass('hover');
    };

    $(".left_nav h3").click(function () {
        if ($(this).next('ul').hasClass("hide")) {
            $(this).next('ul').removeClass("hide"); 
            $(this).siblings('ul').removeClass("show").addClass("hide");
            $(this).next('ul').addClass("show");
            $(this).children().addClass('hover');
            $(this).children().removeClass('leave');
        }
        else if ($(this).next('ul').hasClass("show")) {
            $(this).next('ul').removeClass("show")
            $(this).next('ul').addClass("hide")

            $(this).children().removeClass('hover');
            $(this).children().addClass('leave')
        }
    });


    /*列表点击事件 */
    $(".left_nav1 ul li a").click(function () {

        if ($(this).hasClass('a_checked')) {
            return;
        } else {
            $(".left_nav1 ul li a").removeClass('a_checked');
            $(this).addClass('a_checked');
            $(".left_nav2 h3 a").removeClass('a_checked');
        }
    });

    $(".left_nav2 h3 a").click(function () {

        if ($(this).hasClass('a_checked')) {
            return;
        } else {
            $(".left_nav2 h3 a").removeClass('a_checked');
            $(".left_nav1 ul li a").removeClass('a_checked');
            $(this).addClass('a_checked');
        }
    });



})