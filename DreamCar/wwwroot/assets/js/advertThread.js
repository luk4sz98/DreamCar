$(document).ready(function() {
    $(".btn").click(function() {
        const icon = $(this).parent().children('button').last().children('i').first();
        const buttonText = $(this).parent().children('button').first().children('span').first();
        icon.toggleClass('rotate');
        icon.toggleClass('fw-bolder');
        buttonText.toggleClass('fw-bolder');
    });
})
