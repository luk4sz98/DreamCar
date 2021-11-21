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
            jQuery('#lblMessage').html(message);
        }
        else {
            message = ''
            jQuery('#lblMessage').html(message);
            $("#form").submit()
        }
    }
});
