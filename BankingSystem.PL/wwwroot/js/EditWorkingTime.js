$('.edit-btn').click(function (e) {
    const $btn = $(this);
    const isEditMode = $btn.find('.btn-text').text().trim() === 'Edit Business Hours';

    if (isEditMode) {
        // Enable editing
        $('#Opens, #Closes').prop('readonly', false);
        $btn.html('<i class="bi bi-save me-2"></i><span class="btn-text">Save Business Hours</span>')
            .removeClass('btn-outline-primary')
            .addClass('btn-success');
    } else {
        e.preventDefault();
        const form = $('#businessHoursForm'); // Target the correct form
        $.ajax({
            url: form.attr('action'),
            method: 'POST',
            data: form.serialize(), // Includes anti-forgery token
            success: () => location.reload(),
            error: (xhr) => {
                try {
                    const error = JSON.parse(xhr.responseText);
                    alert(error.errors ? error.errors.join('\n') : error.error);
                } catch {
                    alert('An unexpected error occurred.');
                }
            }
        });
    }
});