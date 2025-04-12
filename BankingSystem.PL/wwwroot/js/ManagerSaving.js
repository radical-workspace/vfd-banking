$(function () {
    // Edit/Save functionality
    $('.edit-btn').click(function () {
        const $row = $(this).closest('tr');
        const isEditMode = $(this).find('i').hasClass('bi-pencil-square');

        if (isEditMode) {
            // Enter edit mode
            $row.find('.currency-cell').html(`
                <input type="text" class="form-control form-control-sm" 
                       value="${$row.find('.currency-cell').text().trim()}">
            `);

            $row.find('.balance-cell').html(`
                <input type="number" class="form-control form-control-sm" 
                       value="${$row.find('.balance-cell').text().replace(/[^0-9.-]+/g, "")}" step="0.01">
            `);

            $(this).html('<i class="bi bi-check-circle"></i> Save')
                .removeClass('btn-secondary')
                .addClass('btn-success');
        } else {
            // Prepare form submission
            $('#savingId').val($row.data('saving-id'));
            $('#savingCurrency').val($row.find('.currency-cell input').val());
            $('#savingBalance').val($row.find('.balance-cell input').val());
            $('#updateForm').submit();
        }
    });

    // Add/Cancel functionality
    const addBtn = $('.Add-btn');
    const addFormRow = $('#addFormRow');

    if (addBtn.length && addFormRow.length) {
        addBtn.click(function (e) {
            e.preventDefault();
            addFormRow.toggleClass('d-none');

            if (addFormRow.hasClass('d-none')) {
                $(this).html('<i class="bi bi-plus-circle me-2"></i>Add Currency');
            } else {
                $(this).html('<i class="bi bi-x-circle me-2"></i>Cancel');
                // Scroll to form if it's long table
                $('html, body').animate({
                    scrollTop: addFormRow.offset().top - 100
                }, 200);
            }
        });
    }

    // Form submission handling
    $('#addForm').submit(function (e) {
        e.preventDefault();
        // Add your form submission logic here
        // You might want to use AJAX for better UX
    });
});