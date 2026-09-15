var table;

$(document).ready(function () {

    table = $("#students_table").DataTable({

        stateSave: true,
        autoWidth: true,

        processing: true,
        serverSide: true,

        paging: true,

        dom: "Bfrtip",

        searching: true,

        ajax: {
            url: "/Submissions/LoadTable",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            data: function (d) {
                d.assignmentGuid = $('#assignmentGuid').val();
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
                data: "studentCode",
                name: "studentCode"
            },
            {
                data: "fullName",
                name: "fullName"
            },
            {
                data: "fileName",
                name: "fileName",
                render: function (data, type, row) {

                    if (!data) {
                        return '<span class="text-muted fst-italic">Not submitted</span>';
                    }

                    return `
                        <a href="/Submissions/ViewSubmissionFile?submissionGuid=${row.submissionGuid}"
                           class="submission-file text-primary" target="_blank">
                            <i class="fa-solid fa-download me-1"></i>
                            ${data.split('/').pop()}
                        </a>
                    `;
                }
            },
            {
                data: "marks",
                name: "marks"
            },
            {
                data: "feedback",
                name: "feedback"
            },
            {
                data: null,
                name: "Actions",
                orderable: false,
                searchable: false,
                render: function (data, type, row) {
                    return `
                        <a class="btn btn-sm btn-primary"
                           href="#">
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

function renderDownloadForm(format) {
    $('#export-to-file-form').attr('action', '/Submissions/ExportTable?format=' + format);

    // Get jQuery DataTables AJAX params
    var datatableParams = $('#students_table').DataTable().ajax.params();

    // Set DataTables parameters
    $('#dtParametersJson').val(JSON.stringify(datatableParams));

    // Set Assignment Guid
    $('#exportAssignmentGuid').val($('#assignmentGuid').val());

    // If the input exists, replace value, if not create the input and append to form
    if ($("#export-to-file-form input[name=dtParametersJson]").val()) {
        $('#export-to-file-form input[name=dtParametersJson]').val(datatableParams);
    } else {
        var searchModelInput = $("<input>")
            .attr("type", "hidden")
            .attr("name", "dtParametersJson")
            .val(datatableParams);

        $('#export-to-file-form').append(searchModelInput);
    }
}


$('#btnExportList').on('click', function () {

    renderDownloadForm('excel');

    $('#export-to-file-form').submit();

});

$("#btnDownloadAll").on("click", function () {

    const assignmentGuid = $("#assignmentGuid").val();

    window.location.href = `/Assignments/DownloadAllSubmissions?assignmentGuid=${assignmentGuid}`;
});
