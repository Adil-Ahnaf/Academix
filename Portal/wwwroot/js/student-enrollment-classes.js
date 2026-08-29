// ============================================================
// ENROLL CLASS
// Available Classes -> Enrolled Classes
// ============================================================

$(document).on('click', '.enroll-btn', function () {

    var button = $(this);
    var row = button.closest('tr');

    var studentGuid = $('#studentGuid').val();
    var classGuid = button.attr('data-class-guid');

    // Prevent double click
    if (button.prop('disabled')) {
        return;
    }

    button.prop('disabled', true);

    // Show loading
    $('#enrollmentLoading').removeClass('d-none');

    var ajaxCompleted = false;
    var ajaxSuccess = false;
    var ajaxResponse = null;

    // Start AJAX immediately
    $.ajax({
        url: window.studentEnrollmentUrls.insert,
        type: 'POST',
        data: {
            classGuid: classGuid,
            studentGuid: studentGuid
        },
        success: function (response) {
            ajaxCompleted = true;
            ajaxSuccess = true;
            ajaxResponse = response;

        },
        error: function (xhr) {
            ajaxCompleted = true;
            ajaxSuccess = false;

            console.error(xhr);
        }
    });

    // Keep loading for minimum 2 seconds
    setTimeout(function () {
        if (ajaxCompleted) {
            if (ajaxSuccess && ajaxResponse.success) {

                // Remove empty row
                $('#enrolledClassesTable tbody .empty-row').remove();

                // Get enrolled class data from controller JSON
                var academicYear = ajaxResponse.academicYear;
                var className = ajaxResponse.className;
                var subjectName = ajaxResponse.subject;
                var section = ajaxResponse.section;
                var maxCapacity = ajaxResponse.maxCapacity;
                var totalEnrolled = ajaxResponse.totalEnrolled;
                var enrolledClassGuid = ajaxResponse.classGuid;


                // Create enrolled row
                var enrolledRow = `
                    <tr>
                        <td>${academicYear}</td>
                        <td>${className}</td>
                        <td>${subjectName}</td>
                        <td>${section}</td>
                        <td>${maxCapacity}</td>
                        <td>${totalEnrolled}</td>
                        <td>
                            <button type="button"
                                    class="btn btn-sm btn-danger remove-btn"
                                    data-class-guid="${enrolledClassGuid}">
                                Remove
                            </button>
                        </td>
                    </tr>
                `;

                // Add to enrolled table
                $('#enrolledClassesTable tbody').append(enrolledRow);

                // Remove from available table
                row.remove();

                // Check available table
                if ($('#availableClassesTable tbody tr').length === 0) {

                    $('#availableClassesTable tbody').append(`
                        <tr class="empty-row">
                            <td colspan="7"
                                class="text-center text-muted py-4">
                                No available classes found.
                            </td>
                        </tr>
                    `);
                }
            }
            else {

                alert(
                    (ajaxResponse && ajaxResponse.message) ||
                    'Unable to enroll in this class.'
                );

                button.prop('disabled', false);
            }

            // Hide loading
            $('#enrollmentLoading').addClass('d-none');

        }
    }, 1000);
});

// ============================================================
// REMOVE CLASS
// Enrolled Classes -> Available Classes
// ============================================================

$(document).on('click', '.remove-btn', function () {

    var button = $(this);
    var row = button.closest('tr');

    var studentGuid = $('#studentGuid').val();
    var classGuid = button.attr('data-class-guid');

    // Prevent double click
    if (button.prop('disabled')) {
        return;
    }

    button.prop('disabled', true);

    // Show loading
    $('#enrollmentRemoving').removeClass('d-none');

    var ajaxCompleted = false;
    var ajaxSuccess = false;
    var ajaxResponse = null;

    // Start AJAX immediately
    $.ajax({
        url: window.studentEnrollmentUrls.delete,
        type: 'POST',
        data: {
            classGuid: classGuid,
            studentGuid: studentGuid
        },

        success: function (response) {

            ajaxCompleted = true;
            ajaxSuccess = true;
            ajaxResponse = response;
        },

        error: function (xhr) {

            ajaxCompleted = true;
            ajaxSuccess = false;

            console.error(xhr);
        }
    });

    // Keep loading for minimum 1 second
    setTimeout(function () {

        if (ajaxCompleted) {

            if (ajaxSuccess && ajaxResponse.success) {

                // Remove empty row
                $('#availableClassesTable tbody .empty-row').remove();

                // Get available class data from controller JSON
                var academicYear = ajaxResponse.academicYear;
                var className = ajaxResponse.className;
                var subjectName = ajaxResponse.subject;
                var section = ajaxResponse.section;
                var maxCapacity = ajaxResponse.maxCapacity;
                var totalEnrolled = ajaxResponse.totalEnrolled;
                var availableClassGuid = ajaxResponse.classGuid;

                // Create available class row
                var availableRow = `
                    <tr>
                        <td>${academicYear}</td>
                        <td>${className}</td>
                        <td>${subjectName}</td>
                        <td>${section}</td>
                        <td>${maxCapacity}</td>
                        <td>${totalEnrolled}</td>
                        <td>
                            <button type="button"
                                    class="btn btn-sm btn-primary enroll-btn"
                                    data-class-guid="${availableClassGuid}">
                                Enroll
                            </button>
                        </td>
                    </tr>
                `;

                // Add to available table
                $('#availableClassesTable tbody').append(availableRow);

                // Remove from enrolled table
                row.remove();

                // Check if enrolled table is empty
                if ($('#enrolledClassesTable tbody tr').length === 0) {

                    $('#enrolledClassesTable tbody').append(`
                        <tr class="empty-row">
                            <td colspan="7"
                                class="text-center text-muted py-4">
                                No enrolled classes found.
                            </td>
                        </tr>
                    `);
                }
            }
            else {

                alert(
                    (ajaxResponse && ajaxResponse.message) ||
                    'Unable to remove this class.'
                );

                button.prop('disabled', false);
            }

            // Hide loading
            $('#enrollmentRemoving').addClass('d-none');
        }

    }, 1000);
});