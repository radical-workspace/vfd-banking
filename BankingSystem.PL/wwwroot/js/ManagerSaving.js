$(function () {
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
            const savingId = $row.data('saving-id');
            const newCurrency = $row.find('.currency-cell input').val();
            const newBalance = $row.find('.balance-cell input').val();

            console.log("Saving edited row:", { savingId, newCurrency, newBalance });

            // Check if form exists
            if ($('#updateForm').length === 0) {
                console.error("Update form not found in the DOM");
                return;
            }

            $('#savingId').val(savingId);
            $('#savingCurrency').val(newCurrency);
            $('#savingBalance').val(newBalance);
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
                // Clear previous inputs
                $('#addCurrency').val('');
                $('#addBalance').val('');
                // Scroll to form
                $('html, body').animate({
                    scrollTop: addFormRow.offset().top - 100
                }, 200);
            }
        });
    }

    // Save new item
    $('#saveNewBtn').click(function () {
        const currency = $('#addCurrency').val();
        const balance = $('#addBalance').val();

        if (!currency || !balance) {
            alert('Please fill in all fields');
            return;
        }

        // Set values in the hidden form
        $('#formCurrency').val(currency);
        $('#formBalance').val(balance);

        // Submit the form
        $('#addForm').submit();
    });
});