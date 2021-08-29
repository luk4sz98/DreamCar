jQuery(document).ready(function () {
    $("#reCaptcha").hide();
});

jQuery("#confirmPassword").focusin(function () {
    $("#reCaptcha").show(500);
})

jQuery("#message").focusin(function () {
    $("#reCaptcha").show(500);
})

function reCaptchaCallback() {
    var response = grecaptcha.getResponse();
    if (response.length !== 0) {
        $("#lblMessage").hide(500)
        jQuery("#lblMessage").html('');
    }
}

jQuery('button[type="button"]').click(function (e) {
    var message = 'Dokonaj walidacji reCaptcha';
    if (typeof (grecaptcha) != 'undefined') {
        var response = grecaptcha.getResponse();
        if (response.length === 0) {
            message = 'Dokonaj walidacji reCaptcha'
            jQuery('#lblMessage').html(message);
        }
        else {
            message = ''
            jQuery('#lblMessage').html(message);
            $("#form").submit()
        }
    }
});