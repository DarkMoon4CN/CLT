$(function () {
    $('.helpList li a').click(function () {
        if ($(this).children('span').hasClass('icon-down')) {
            $(this).children('span').removeClass('icon-down').addClass('icon-up');
            $(this).siblings('p').slideDown(200);
            $(this).parent('li').siblings('li').find('span').removeClass('icon-up').addClass('icon-down');
            $(this).parent('li').siblings('li').find('p').slideUp(200);
        } else if ($(this).children('span').hasClass('icon-up')) {
            $(this).children('span').removeClass('icon-up').addClass('icon-down');
            $(this).siblings('p').slideUp(200);
            $(this).parent('li').siblings('li').find('span').removeClass('icon-up').addClass('icon-down');
            $(this).parent('li').siblings('li').find('p').slideUp(200);
        } else {
            $(this).children('span').removeClass('icon-up').addClass('icon-down');
            $(this).siblings('p').slideUp(200);
            $(this).parent('li').siblings('li').find('span').removeClass('icon-down').addClass('icon-up');
            $(this).parent('li').siblings('li').find('p').slideDown(200);
        }
    })
})