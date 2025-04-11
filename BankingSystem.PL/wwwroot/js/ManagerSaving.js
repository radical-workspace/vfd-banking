$(function () {
    $('.edit-btn').click(function () {
        const $row = $(this).closest('tr');
        const isEditMode = $(this).text() === 'Edit';

        if (isEditMode) {
            // Enter edit mode
            $row.find('td:not(:last)').each(function () {
                const value = $(this).text().replace('$', '').trim();
                $(this).html(`<input type="text" class="form-control" value="${value}">`);
            });
            $(this).text('Save').removeClass('btn-primary').addClass('btn-success');
        } else {
            // Prepare form submission
            $('#savingId').val($row.data('saving-id'));
            $('#savingCurrency').val($row.find('td:eq(0) input').val());
            $('#savingBalance').val($row.find('td:eq(1) input').val());
            $('#updateForm').submit();
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const addBtn = document.querySelector('.Add-btn');
    const addFormRow = document.getElementById('addFormRow');

    if (addBtn && addFormRow) {
        addBtn.addEventListener('click', function (e) {
            e.preventDefault(); // Prevent default button action
            addFormRow.classList.toggle('d-none'); // Toggle visibility
            if (addFormRow.classList.contains('d-none')) addBtn.textContent = 'Add Currency';
            else addBtn.textContent = 'Cancel';

        });
    }
});