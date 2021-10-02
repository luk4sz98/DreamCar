$('th').click(function () {

    var thText = $(this).children('span').text()

    switch (thText) {
        case "Tytuł": {
            display('title')
            displayNone('price')
            displayNone('date')
            toggleArrow('title')
            break;
        }
        case "Cena": {
            display('price')
            displayNone('title')
            displayNone('date')
            toggleArrow('price')
            break;
        }
        default: {
            displayNone('title')
            displayNone('price')
            display('date')
            toggleArrow('date')
            break;
        }
    }

    var table = $(this).parents('table').eq(0)
    var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()))
    this.asc = !this.asc
    if (!this.asc) {
        rows = rows.reverse()
    }
    for (var i = 0; i < rows.length; i++) {
        table.append(rows[i])
    }
})

function displayNone(iconId) {
    var id = "#" + iconId
    if ($(`${id}`).is(':visible')) {
        $(`${id}`).hide()
    }
}

function display(iconId) {
    var id = "#" + iconId
    if (!$(`${id}`).is(':visible')) {
        $(`${id}`).show()
    }
}

function toggleArrow(iconId) {
    var id = "#" + iconId

    if ($(`${id}`).hasClass('fa-arrow-down')) {
        $(`${id}`).removeClass('fa-arrow-down')
        $(`${id}`).addClass('fa-arrow-up')
    }
    else {
        $(`${id}`).removeClass('fa-arrow-up')
        $(`${id}`).addClass('fa-arrow-down')
    }
}

function comparer(index) {
    return function (a, b) {
        var valA = getCellValue(a, index),
            valB = getCellValue(b, index)

        if (valA.match(/[0-9]{2}\.[0-9]{2}\.[0-9]{4}/)) {
            valA = new Date(
                        reverseStringToValidDateFormat(
                            valA.replace(/\./g, '-')
                        ))
            valB = new Date(
                        reverseStringToValidDateFormat(
                            valB.replace(/\./g, '-')
                        ))
        }

        if ($.isNumeric(valA) && $.isNumeric(valB)) {
            return valA - valB;
        }
        else if (valA instanceof Date) {
            return valA - valB;
        }
        else {
            return valA.toString().localeCompare(valB)
        }

    }
}

function reverseStringToValidDateFormat(str) {
    return str.split("-").reverse().join("-");
}

function getCellValue(row, index) {
    return $(row).children('td').eq(index).text()
}

$("#filterAdvert").on("keyup focus", async function () {
    const filterValue = $("#filterAdvert").val();
    if (filterValue !== "" && filterValue != " ") {
        const result = await $.get("/Advert/UserAdverts", $.param({
            filterValue: filterValue
        }));

        $(".advertsTableData").html(result);
    }
    else {
        const result = await $.get("/Advert/UserPendingAdverts");
        $(".advertsTableData").html(result);
    }

})