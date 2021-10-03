$('th').click(function () {
    var thText = $(this).children('span').text()
    var icon = $(this).children('icon')

    switch (thText) {
        case "Tytuł": {
            toggleArrow(icon)
            break;
        }
        case "Cena": {
            toggleArrow(icon)
            break;
        }
        default: {
            toggleArrow(icon)
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

function toggleArrow(icon) {
    $("icon").hide()
    icon.show()
    if (icon.hasClass('fa-arrow-down')) {
        $(icon).removeClass('fa-arrow-down')
        $(icon).addClass('fa-arrow-up')
    }
    else {
        $(icon).removeClass('fa-arrow-up')
        $(icon).addClass('fa-arrow-down')
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

$(".filterAdvert").on("keyup focus", async function () {
    const filterValue = $(this).val();
    if (filterValue !== "" && filterValue != " ") {
        if ($("#pending-tab").hasClass("active")) {
            const result = await $.get("/Advert/UserAdverts", $.param({
                filterValue: filterValue,
                advertType: "Pending"
            }));
            $(".pendingAdvertsTableData").html(result);
        }
        else if ($("#active-tab").hasClass("active")) {
            const result = await $.get("/Advert/UserAdverts", $.param({
                filterValue: filterValue,
                advertType: "Active"
            }));
            $(".activeAdvertsTableData").html(result);
        }
        else {
            const result = await $.get("/Advert/UserAdverts", $.param({
                filterValue: filterValue,
                advertType: "Ended"
            }));
            $(".endedAdvertsTableData").html(result);
        }
    }
    else {
        if ($("#pending-tab").hasClass("active")) {
            const result = await $.get("/Advert/GetUserSpecificAdverts", $.param({
                advertType: "Pending"
            }));
            $(".pendingAdvertsTableData").html(result);
        }
        else if ($("#active-tab").hasClass("active")) {
            const result = await $.get("/Advert/GetUserSpecificAdverts", $.param({
                advertType: "Active"
            }));
            $(".activeAdvertsTableData").html(result);
        }
        else {
            const result = await $.get("/Advert/GetUserSpecificAdverts", $.param({
                advertType: "Ended"
            }));
            $(".endedAdvertsTableData").html(result);
        }
    }
})
