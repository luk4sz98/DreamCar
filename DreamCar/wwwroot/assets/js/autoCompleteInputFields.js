$("#brandCar").autocomplete({
    source: function (request, response) {
        $.ajax({
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: '/AutoCompleteHelper/AutoCompleteBrand',
            data: { "prefix": request.term },
            type: "POST",
            success: function (data) {
                response($.map(data, function (item) {
                    return item
                }))
            },
            error: function (xhr, textStatus, error) {
                alert(xhr.statusText);
            },
            failure: function (response) {
                alert("failure " + response.responseText);
            }
        })
    }
})

$("#modelCar").autocomplete({
    source: function (request, response) {
        $.ajax({
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            url: '/AutoCompleteHelper/AutoCompleteModel',
            data: { "brand": $("#brandCar").val(), "prefixModel": request.term },
            type: "POST",
            success: function (data) {
                response($.map(data, function (item) {
                    return item
                }))
            },
            error: function (xhr, textStatus, error) {
                alert(xhr.statusText);
            },
            failure: function (response) {
                alert("failure " + response.responseText);
            }
        })
    }
})