$(document).ready(function () {
    $('#showAccounts').click(function () {
        $('#accountSection').show();
        $('#cardSection').hide();
        $(this).removeClass('btn-outline-primary').addClass('btn-primary');
        $('#showCards').removeClass('btn-primary').addClass('btn-outline-dark');
    });

    $('#showCards').click(function () {
        $('#accountSection').hide();
        $('#cardSection').show();
        $(this).removeClass('btn-outline-primary').addClass('btn-primary');
        $('#showAccounts').removeClass('btn-primary').addClass('btn-outline-dark');
    });
});

// Format IBAN as user types
document.getElementById('DestinationIban').addEventListener('input', function (e) {
    // Remove all non-alphanumeric characters
    let value = e.target.value.replace(/[^a-zA-Z0-9]/g, '');

    // Add space every 4 characters
    value = value.replace(/(.{4})/g, '$1 ').trim();

    // Update the input value
    e.target.value = value;
});