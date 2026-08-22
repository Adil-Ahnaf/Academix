var table;

$(document).ready(function () {

    table = $("#classes_table").DataTable({

        stateSave: true,
        autoWidth: true,

        processing: true,
        serverSide: true,

        paging: true,

        dom: "Bfrtip",

        searching: true,

        ajax: {
            url: "/Home/LoadTable",
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
                data: "academicYear",
                name: "AcademicYear"
            },
            {
                data: "className",
                name: "ClassName"
            },
            {
                data: "subjectName",
                name: "SubjectName"
            },
            {
                data: "section",
                name: "Section"
            },
            {
                data: "maxCapacity",
                name: "MaxCapacity"
            },
            {
                data: "isActive",
                name: "IsActive",
                render: function (data) {
                    return data
                        ? '<span class="badge bg-success">Active</span>'
                        : '<span class="badge bg-danger">Inactive</span>';
                }
            },
            {
                data: null,
                name: "Actions",
                orderable: false,
                searchable: false,
                render: function (data, type, row) {
                    return `
                        <a class="btn btn-sm btn-primary"
                           href="/Classes/Edit/${row.classGuid}">
                            Edit
                        </a>`;
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