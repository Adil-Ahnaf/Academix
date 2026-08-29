// ============================================================
// ENROLL CLASS
// Available Classes -> Enrolled Classes
// ============================================================

$(document).on('click', '.enroll-btn', function () {

    var button = $(this);
    var row = button.closest('tr');

    var classGuid = button.attr('data-class-guid');
    var studentGuid = $('#studentGuid').val();

    if (!classGuid || !studentGuid) {
        alert('Class or student information is missing.');
        return;
    }

    // Prevent double click
    button.prop('disabled', true);
    button.text('Enrolling...');

    // Remove empty message from enrolled table
    $('#enrolledClassesTable tbody').find('.empty-row').remove();

    $.ajax({
        url: window.studentEnrollmentUrls.insert,
        type: 'POST',
        data: {
            classGuid: classGuid,
            studentGuid: studentGuid
        },
        success: function (response) {

            if (response.success) {
                // Change Enroll -> Remove
                button
                    .removeClass('btn-primary enroll-btn')
                    .addClass('btn-danger remove-btn')
                    .text('Remove')
                    .prop('disabled', false);

                // Move row:
                $('#enrolledClassesTable tbody').append(row);

                // If no available classes remain
                if ($('#availableClassesTable tbody tr').not('.empty-row').length === 0)
                {
                    $('#availableClassesTable tbody').find('.empty-row').remove();
                    $('#availableClassesTable tbody')
                        .append(`
                            <tr class="empty-row">
                                <td colspan="6"
                                    class="text-center text-muted py-4">
                                    No available classes found.
                                </td>
                            </tr>
                    `);
                }

            } else {
                alert(response.message || 'Unable to enroll in this class.');
                button.prop('disabled', false).text('Enroll');
            }
        },

        error: function (xhr) {

            console.error('AJAX Error');
            console.error('Status:', xhr.status);
            console.error('Response:', xhr.responseText);

            alert('An error occurred while enrolling the class.\n' + 'HTTP Status: ' + xhr.status);
            button.prop('disabled', false).text('Enroll');
        }
    });
});


// ============================================================
// REMOVE CLASS
// Enrolled Classes -> Available Classes
// ============================================================

$(document).on('click', '.remove-btn', function () {

    var button = $(this);
    var row = button.closest('tr');

    var classGuid = button.attr('data-class-guid');
    var studentGuid = $('#studentGuid').val();

    console.log(classGuid);

    if (!classGuid || !studentGuid) {
        alert('Class or student information is missing.');
        return;
    }

    // Prevent double click
    button.prop('disabled', true);
    button.text('Removing...');

    // Remove empty message from available table
    $('#availableClassesTable tbody').find('.empty-row').remove();

    $.ajax({
        url: window.studentEnrollmentUrls.delete,
        type: 'POST',
        data: {
            classGuid: classGuid,
            studentGuid: studentGuid
        },
        success: function (response) {

            if (response.success) {
                // Change Remove -> Enroll
                button
                    .removeClass('btn-danger remove-btn')
                    .addClass('btn-primary enroll-btn')
                    .text('Enroll')
                    .prop('disabled', false);

                // Increase Total Enrolled by 1
                var totalEnrolledCell = row.find('td').eq(5);
                var totalEnrolled = parseInt(totalEnrolledCell.text(), 10) || 0;
                totalEnrolled++;
                totalEnrolledCell.text(totalEnrolled);

                // Move row:
                $('#availableClassesTable tbody').append(row);

                // If no enrolled classes remain
                if ($('#enrolledClassesTable tbody tr').not('.empty-row').length === 0)
                {
                    $('#enrolledClassesTable tbody').find('.empty-row').remove();
                    $('#enrolledClassesTable tbody')
                        .append(`
                        <tr class="empty-row">
                            <td colspan="6"
                                class="text-center text-muted py-4">
                                No enrolled classes found.
                            </td>
                        </tr>
                    `);
                }

            } else {

                alert(response.message || 'Unable to remove this class.');
                button.prop('disabled', false).text('Remove');
            }
        },

        error: function (xhr) {

            console.error('AJAX Error');
            console.error('Status:', xhr.status);
            console.error('Response:', xhr.responseText);

            alert('An error occurred while removing the class.\n' + 'HTTP Status: ' + xhr.status);
            button.prop('disabled', false).text('Remove');
        }
    });
});
