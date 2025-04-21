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

            // Set values in the hidden form
            $('#savingId').val(savingId);
            $('#savingCurrency').val(newCurrency);
            $('#savingBalance').val(newBalance);
            $('#updateForm').submit();
        }
    });
});
