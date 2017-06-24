
//滚动效果
window.onload = function () {
    //滚动效果

    var scrollingBox = document.getElementById("xstCont");
    function scrolling() {
        var bottom;
        var reachedBottom = false;
        var timer = null;
        var origin = scrollingBox.scrollTop++;
        if (origin == scrollingBox.scrollTop) {
            if (!reachedBottom) {
                scrollingBox.innerHTML += scrollingBox.innerHTML;
                reachedBottom = true;
                bottom = origin;
            } else {
                scrollingBox.scrollTop = bottom;
            }
        }

        scrollingBox.style.overflow = "hidden";
    }
    var timer = setInterval(scrolling, 120);
    scrollingBox.onmouseover = function () { clearInterval(timer);}
    scrollingBox.onmouseout = function () { timer = setInterval(scrolling, 120) }
}