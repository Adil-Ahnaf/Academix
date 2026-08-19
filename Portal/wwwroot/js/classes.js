var table;

$(document).ready(function () {
    table = $("#classes_table").DataTable({
        // Design Assets
        stateSave: true,
        autoWidth: true,
        // ServerSide Setups
        processing: true,
        serverSide: true,
        // Paging Setups
        paging: true,
        // Custom Export Buttons
        dom: 'Bfrtip',
        // Searching Setups
        searching: { regex: true },
        // Ajax Filter
        ajax: {
            url: "/Classes/LoadTable",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        // Columns Setups
        columns: [
            { data: "id" },
			{ data: "className" },
			{ data: "section" },
			{ data: "academicYear" },
			{ data: "maxCapacity" },
			{ data: "classGuid" },
			{ data: "isActive" }
        ],
        //TODO: Need to check if we need [Column Definitions] section
        // Column Definitions
        columnDefs: [
            { targets: "no-sort", orderable: false },
            { targets: "no-search", searchable: false },
            {
                targets: "trim",
                render: function (data, type, full, meta) {
                    if (type === "display") {
                        data = strtrunc(data, 10);
                    }
                    return data;
                }
            },
            //{ targets: "date-type", type: "date-eu" },
            {
                targets: 7,
                data: null,
                defaultContent: "<a class='btn btn-link' role='button' href='#' onclick='edit(this)'>Edit</a>",
                orderable: false
            },
        ]
    });
});

function strtrunc(str, num) {
    if (str.length > num) {
        return str.slice(0, num) + "...";
    }
    else {
        return str;
    }
}

function edit(rowContext) {
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        //alert("Example showing row edit with id: " + data["id"] + ", name: " + data["name"]);
	    window.location.href = '/Classes/Edit/' + data["id"];
    }
}