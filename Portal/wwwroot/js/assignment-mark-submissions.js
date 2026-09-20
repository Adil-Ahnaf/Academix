var table;
var editMode = false;
var editedSubmissions = {};

$(document).ready(function () {

    table = $("#students_table").DataTable({

        stateSave: true,
        autoWidth: true,

        processing: true,
        serverSide: true,

        // Disable pagination
        paging: false,

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
                           class="submission-file text-primary"
                           target="_blank">
                            <i class="fa-solid fa-download me-1"></i>
                            ${data.split('/').pop()}
                        </a>
                    `;
                }
            },
            {
                data: "marks",
                name: "marks",
                render: function (data, type, row) {

                    return `
                        <input type="number"
                               class="form-control form-control-sm marks-input"
                               value="${data ?? ''}"
                               min="0"
                               max="${row.totalMarks}"
                               disabled />
                    `;
                }
            },
            {
                data: "feedback",
                name: "feedback",
                render: function (data, type, row) {

                    return `
                        <textarea class="form-control form-control-sm feedback-input"
                                  rows="1"
                                  disabled>${data ?? ''}</textarea>
                    `;
                }
            },
            {
                data: null,
                name: "Actions",
                orderable: false,
                searchable: false,
                render: function (data, type, row) {

                    return `
                        <button type="button"
                                class="btn btn-sm btn-outline-primary btn-edit-row">
                            <i class="fa-solid fa-pen me-1"></i>
                            Edit
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
    window.location.href = `/Submissions/DownloadAllSubmissions?assignmentGuid=${assignmentGuid}`;
});

// Store edited marks / feedback
$(document).on("input", ".marks-input, .feedback-input", function () {

    var node = $(this).closest("tr");

    var row = table.row(node).data();

    if (!row) {
        return;
    }

    var marks = $(node).find(".marks-input").val();
    var feedback = $(node).find(".feedback-input").val();

    editedSubmissions[row.submissionGuid] = {

        submissionGuid: row.submissionGuid,

        marks: marks === ""
            ? null
            : parseFloat(marks),

        feedback: feedback
    };
});


// Edit All / Save All
$("#btnEditAll").on("click", function () {

    editMode = !editMode;

    if (editMode) {

        $(this).removeClass("btn-outline-primary").addClass("btn-primary");

        $(this).find("i").removeClass("fa-pen").addClass("fa-save");

        $(this).find("span").text("Save All");

        $("#students_table tbody").find(".marks-input, .feedback-input").prop("disabled", false);
    }
    else {

        saveAllSubmissions();

        $(this).removeClass("btn-primary").addClass("btn-outline-primary");

        $(this).find("i").removeClass("fa-save").addClass("fa-pen");

        $(this).find("span").text("Edit All");

        $("#students_table tbody").find(".marks-input, .feedback-input").prop("disabled", true);
    }
});

// Row Edit
$(document).on("click", ".btn-edit-row", function () {

    var button = $(this);
    var rowNode = button.closest("tr");
    var row = table.row(rowNode).data();

    if (!row) {
        return;
    }

    var marksInput = rowNode.find(".marks-input");
    var feedbackInput = rowNode.find(".feedback-input");

    // Start editing
    if (!button.hasClass("editing")) {

        // Store original values in the row
        rowNode.data("original-marks", marksInput.val());
        rowNode.data("original-feedback", feedbackInput.val());

        // Enable fields
        marksInput.prop("disabled", false);
        feedbackInput.prop("disabled", false);

        // Change button appearance
        button
            .addClass("editing btn-warning")
            .removeClass("btn-outline-primary")
            .html('<i class="fa-solid fa-save me-1"></i> Save');
        return;
    }

    // Save row
    saveSingleSubmission(rowNode, row);
});


// Save All
function saveAllSubmissions() {

    var submissions = Object.values(editedSubmissions);

    if (submissions.length === 0) {

        alert("No changes to save.");
        return;
    }

    $.ajax({
        url: "/Submissions/SaveAllMarks",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(submissions),

        success: function (response) {

            if (response.success) {
                alert(response.message);
                editedSubmissions = {};
            }
            else {
                alert(response.message);
            }
        },

        error: function (xhr) {
            alert("Something went wrong while saving.");
        }
    });
}


function saveSingleSubmission(rowNode, row) {

    var marksInput = rowNode.find(".marks-input");
    var feedbackInput = rowNode.find(".feedback-input");

    var marksValue = marksInput.val();
    var feedbackValue = feedbackInput.val();

    var submission = { submissionGuid: row.submissionGuid,

        marks: marksValue === ""
            ? null
            : parseFloat(marksValue),

        feedback: feedbackValue
    };

    $.ajax({
        url: "/Submissions/SaveAllMarks",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify([submission]),

        beforeSend: function () {

            rowNode.find(".btn-edit-row")
                .prop("disabled", true)
                .html('<i class="fa-solid fa-spinner fa-spin me-1"></i> Saving...');
        },

        success: function (response) {

            if (response.success) {

                // Update DataTables row data
                row.marks = submission.marks;
                row.feedback = submission.feedback;

                table.row(rowNode).data(row).invalidate();

                // Remove from unsaved changes
                delete editedSubmissions[row.submissionGuid];

                // Disable inputs again
                rowNode.find(".marks-input, .feedback-input")
                    .prop("disabled", true);

                // Reset button
                rowNode.find(".btn-edit-row")
                    .removeClass("editing btn-primary")
                    .addClass("btn-outline-primary")
                    .prop("disabled", false)
                    .html('<i class="fa-solid fa-pen me-1"></i> Edit');

                // Optional success message
                alert(response.message);
            }
            else {

                rowNode.find(".btn-edit-row")
                    .prop("disabled", false)
                    .html('<i class="fa-solid fa-save me-1"></i> Save');

                alert(response.message);
            }
        },

        error: function (xhr) {

            rowNode.find(".btn-edit-row")
                .prop("disabled", false)
                .html('<i class="fa-solid fa-save me-1"></i> Save');

            alert("Something went wrong while saving.");
        }
    });
}