var table;

$(document).ready(function () {

    table = $("#assignments_table").DataTable({

        stateSave: true,
        autoWidth: true,

        processing: true,
        serverSide: true,

        paging: true,

        dom: "Bfrtip",

        searching: true,

        ajax: {
            url: "/Assignments/LoadTable",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            data: function (d) {
                return JSON.stringify(d);
            },

            error: function (xhr, error, thrown) {
                console.error("DataTables AJAX Error");
                console.error("Status:", xhr.status);
                console.error("Response:", xhr.responseText);
                console.error("Error:", error);
                console.error("Thrown:", thrown);
            }
        },

        columns: [
            {
                data: "title",
                name: "title"
            },
            {
                data: "className",
                name: "className"
            },
            {
                data: "section",
                name: "section"
            },
            {
                data: "marks",
                name: "marks"
            },
            {
                data: "deadline",
                name: "deadline",
                render: function (data, type) {
                    if (type === "display" && data) {
                        return new Date(data).toLocaleDateString();
                    }
                    return data;
                }
            },
            {
                data: null,
                name: "Actions",
                orderable: false,
                searchable: false,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-primary"
                            onclick="location.href='/Submissions/MarkSubmissions?assignmentGuid=${row.assignmentGuid}'">
                            View Details
                        </button>
                    `;
                }
            }
        ],

        columnDefs: [
            {
                targets: "no-sort",
                orderable: false
            },
            {
                targets: "no-search",
                searchable: false
            },
            {
                targets: "trim",
                render: function (data, type) {
                    if (type === "display" && data) {
                        return strtrunc(data, 10);
                    }

                    return data;
                }
            }
        ]
    });
});

function strtrunc(str, num) {

    if (!str) {
        return "";
    }

    return str.length > num
        ? str.substring(0, num) + "..."
        : str;
}