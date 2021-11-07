$(function () {
    var stt_is_shown = false;
    var width = $(window).width();
    $(window).scroll(function () {
        var win_height = 150;
        var scroll_top = $(document).scrollTop();
        if (scroll_top > win_height && !stt_is_shown && width > 576) {
            stt_is_shown = true;
            $("#scroll-to-top").fadeIn();
        } else if (scroll_top < win_height && stt_is_shown) {
            stt_is_shown = false;
            $("#scroll-to-top").fadeOut();
        }
    });
    $("#scroll-to-top").click(function (e) {
        e.preventDefault();
        $('html, body').animate({
            scrollTop: 0
        }, 1000);
    });
});
