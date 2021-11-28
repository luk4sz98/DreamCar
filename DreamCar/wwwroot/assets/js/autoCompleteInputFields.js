$("#brandCar").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/AutoCompleteHelper/AutoComplete',
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
            failure: function (failure) {
                alert("failure " + failure.responseText);
            }
        })
    }
})

$("#modelCar").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/AutoCompleteHelper/AutoComplete',
            data: { "brand": $("#brandCar").val(), "prefix": request.term },
            type: "POST",
            success: function (data) {
                response($.map(data, function (item) {
                    return item
                }))
            },
            error: function (xhr, textStatus, error) {
                alert(xhr.statusText);
            },
            failure: function (failure) {
                alert("failure " + failure.responseText);
            }
        })
    }
})

$("#generationCar").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/AutoCompleteHelper/AutoComplete',
            data: { "brand": $("#brandCar").val(), "model": $("#modelCar").val(), "prefix": request.term },
            type: "POST",
            success: function (data) {
                response($.map(data, function (item) {
                    return item
                }))
            },
            error: function (xhr, textStatus, error) {
                alert(xhr.statusText);
            },
            failure: function (failure) {
                alert("failure " + failure.responseText);
            }
        })
    }
})
