$(document).ready(function () {
    // Initialize based on model value
    const isAccount = $('#ShowAccounts').val() === 'True';
    setAccountType(isAccount);

    // Button click handlers
    $('#showAccounts').click(function () {
        setAccountType(true);
        $('#ShowAccounts').val('True');
    });

    $('#showCards').click(function () {
        setAccountType(false);
        $('#ShowAccounts').val('False');
    });

    function setAccountType(isAccount) {
        $('#accountSection').toggle(isAccount);
        $('#cardSection, #cardDetailsSection').toggle(!isAccount);

        // Update button styles
        $('#showAccounts')
            .toggleClass('btn-primary', isAccount)
            .toggleClass('btn-outline-dark', !isAccount);
        $('#showCards')
            .toggleClass('btn-primary', !isAccount)
            .toggleClass('btn-outline-dark', isAccount);

        // Clear fields when switching
        if (isAccount) {
            $('#SelectedCardNumber, #VisaCVV, #VisaExpDate').val('');
        } else {
            $('#SelectedAccountNumber').val('');
        }
    }
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