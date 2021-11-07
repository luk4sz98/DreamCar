$(document).ready(function () {
    $("form").each(function () {
        $(this).find(':input').each(function () {
            if ($(this).val() != '') {
                $(this).removeAttr('disabled')
                $(this).next('label').removeClass('disabled')
            }               
        })
    })
})

$("#productionYearList").change(function () {
    $("#brandCar").removeAttr('disabled');
    $("#brandCarLabel").removeClass('disabled');
});

$("#brandCar").on('keyup change', function () {
    if ($("#brandCar").valid()) {
        $("#modelCar").removeAttr('disabled');
        $("#modelCarLabel").removeClass('disabled');
    }
});

$("#modelCar").on('keyup change', function () {
    if ($("#modelCar").valid()) {
        $("#fuelTypeList").removeAttr('disabled');
    }
});

$("#fuelTypeList").change(function () {
    $("#power").removeAttr('disabled');
    $("#powerLabel").removeClass('disabled');
});

$("#power").on('keyup change', function () {
    if ($("#power").valid()) {
        $("#engineCapacity").removeAttr('disabled');
        $("#engineCapacityLabel").removeClass('disabled');
    }
});

$("#engineCapacity").on('keyup change', function () {
    if ($("#engineCapacity").valid()) {
        $("#doorsList").removeAttr('disabled')
    }
})

$("#doorsList").change(function () {
    $("#gearboxList").removeAttr('disabled')
})

$("#gearboxList").change(function () {
    $("#version").removeAttr('disabled')
    $("#versionLabel").removeClass('disabled');
})

$("#version").on('keyup change', function () {
    if ($("#version").valid()) {
        $("#generation").removeAttr('disabled')
        $("#generationLabel").removeClass('disabled');
    }
})
