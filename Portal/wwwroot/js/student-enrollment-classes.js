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

    // Get row data
    var academicYear = row.find('td:eq(0)').text().trim();
    var className = row.find('td:eq(1)').text().trim();
    var subjectName = row.find('td:eq(2)').text().trim();
    var section = row.find('td:eq(3)').text().trim();
    var maxCapacity = row.find('td:eq(4)').text().trim();
    var totalEnrolled = row.find('td:eq(5)').text().trim();

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
                                    data-class-guid="${classGuid}">
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

                alert(
                    ajaxResponse.message ||
                    'Class enrolled successfully.'
                );

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
    }, 2000);
});