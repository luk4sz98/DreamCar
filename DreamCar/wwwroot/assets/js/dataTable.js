var advertId
$(".dropdownAdvertManageButton").click(function () {
    advertId = $(this).closest("tr").children("td:first").text()
})
$("#endAdvertButton").click(function () {
    $.ajax({
        url: "/Advert/EndAdvert",
        data: {
            advertId: advertId
        },
        type: 'POST',
        success: function (response) {
            window.location.href = response.redirectToUrl
            $("#endConfirmationModal").modal('hide');
        },
        error: function () {
            $("#endConfirmationModal").modal('hide');
        }
    })
})
$("#deleteAdvertButton").click(function () {
    $.ajax({
        url: "/Advert/DeleteAdvert",
        data: {
            advertId: advertId
        },
        type: 'POST',
        success: function (response) {
            window.location.href = response.redirectToUrl
            $("#deleteConfirmationModal").modal('hide');
        },
        error: function () {
            $("#deleteConfirmationModal").modal('hide');
        }
    })
})
$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');
    $("table").DataTable({
        clientSide: true,
        filter: true,
        paging: true,
        pagingType: "full_numbers",
        "autoWidth": false,
        columnDefs: [
            { targets: 1, orderable: false },
            { targets: 2, orderable: true },
            { targets: 3, orderable: true },
            { targets: 4, orderable: false },
            { targets: 5, orderable: true },
            { targets: 6, orderable: false },
        ],
        language: {
            buttons: {
                copySuccess: {
                    1: "Skopiowano jeden wiersz do schowka",
                    _: "Skopiowano %d wiersze do schowka"
                },
                copyTitle: 'Skopiowano',
            },
            "paginate": {
                "first": "Pierwsza",
                "last": "Ostatnia",
                "previous": "<i class='fas fa-chevron-left'></i>",
                "next": "<i class='fas fa-chevron-right'></i>"
            },
            "zeroRecords": "Nie znaleziono pasujących danych",
            "emptyTable": "Brak danych",
            "info": "Pozycje od _START_ do _END_ z _TOTAL_ łącznie",
            "infoEmpty": "Pozycji 0 z 0 dostępnych",
            "infoFiltered": "(filtrowanie spośród _MAX_ dostepnych pozycji)",
            "lengthMenu": "Wyświetl _MENU_ pozycji/-e",
            "loadingRecords": "Wczytywanie danych...",
            "processing": "Przetwarzanie...",
            "search": "Szukaj:",
        },
        dom: "<'row'<'col-12'B>>" +
            "<'row g-0'<'col-12 col-md-6'l><'col-12 col-md-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5 d-flex justify-content-start align-items-start mb-3 mb-md-0'i><'col-sm-12 col-md-7 d-flex justify-content-center justify-content-md-end'p>>",
        buttons: [
            {
                extend: 'copyHtml5',
                text: '<i class="fas fa-copy"></i>',
                exportOptions: {
                    columns: [2, 3, 4, 5]
                },
                className: 'shadow-none',
                title: 'Ogłoszenia oczekujące',

            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [2, 3, 4, 5]
                },
                className: 'shadow-none',
                download: "download",
                title: 'Ogłoszenia oczekujące',
                text: '<i class="fas fa-file-excel"></i>',
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [2, 3, 4, 5]
                },
                text: '<i class="fas fa-file-pdf"></i>',
                className: 'shadow-none',
                download: "download",
                title: 'Ogłoszenia oczekujące'
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>',
                autoPrint: false,
                exportOptions: {
                    modifier: {
                        page: 'current'
                    },
                    columns: [2, 3, 4, 5]
                },
                className: 'shadow-none',
                title: 'Ogłoszenia oczekujące'
            }
        ],
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, "wszystkie"]
        ]
    })
})
