var currentTab = 0;
$(document).ready(function () {
    showTab(currentTab);
})

function checkFields() {
    var x = $(".step")
    var required = $('input,textarea,select').filter('[required]:visible');
    var allRequired = true;
    required.each(function () {
        if ($(this).val() === '' || $(this).children("option:selected").val() === '') {
            allRequired = false;
        }
    })
    if (!allRequired) {
        $('#errorMessageModal').modal('show');
    }
    else {
        if (!$('#fileUpload-error').is(":visible")) {
            nextPrev(1)
            $(x[currentTab - 1]).addClass("finish");
        }
    }
}

function showTab(n) {
    var x = $(".tab")

    $(x[n]).slideToggle(800)
    if (n != 0) {
        $('html,body').animate({
            scrollTop: $(x[n]).offset().top
        }, 'slow');
    }

    if (n == 0) {
        $("#prevBtn").hide()
        $('html,body').animate({
            scrollTop: $('.infoHeader').offset().top
        }, 'slow');
    } else {
        $("#prevBtn").show()
    }

    if (n == (x.length - 1)) {
        $("#nextBtn").text("Dodaj ogłoszenie")
    } else {
        $("#nextBtn").text("Przejdź dalej");
    }

    fixStepIndicator(n)
}

function nextPrev(n) {
    var x = $(".tab")

    currentTab = currentTab + n;

    if (currentTab >= x.length) {
        $("#addAdvertForm").submit()
        return;
    }

    if (n == 1) {
        $(x[currentTab - 1]).slideToggle(800)
    }
    else {
        $(x[currentTab + 1]).slideToggle(800)
    }

    showTab(currentTab);
}

function fixStepIndicator(n) {
    var i, x = $(".step");
    for (i = 0; i < x.length; i++) {
        $(x[i]).removeClass("active");
    }
    $(x[n]).addClass("active");
    $(x[n]).removeClass("finish")

    var number = 25;
    if (n != 0) {
        number = (100 / x.length) * (n + 1);
    }

    $(".progress-bar").css("width", number.toString() + "%");
}

(function ($, window, document) {
    $('.inputfile').each(function () {
        var $input = $(this),
            $label = $input.next('label'),
            labelVal = $label.html();

        $input.on('change', function (e) {
            var fileName = '';

            if (this.files && this.files.length > 1)
                fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
            else if (e.target.value)
                fileName = e.target.value.split('\\').pop();

            if (fileName)
                $label.find('span').html(fileName);
            else
                $label.html(labelVal);
        });

        $input
            .on('focus', function () { $input.addClass('has-focus'); })
            .on('blur', function () { $input.removeClass('has-focus'); });
    });
})(jQuery, window, document);
