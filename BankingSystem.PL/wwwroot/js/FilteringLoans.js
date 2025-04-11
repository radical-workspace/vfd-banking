// filteringLoans based on loan type and status
document.addEventListener('DOMContentLoaded', function () {
    const buttons = document.querySelectorAll('.filter-btn');
    const rows = document.querySelectorAll('tbody tr');

    buttons.forEach(button => {
        button.addEventListener('click', function () {
            const filterType = this.dataset.filterType;
            const filterValue = this.dataset.filterValue;
            const groupButtons = Array.from(document.querySelectorAll(`.filter-btn[data-filter-type="${filterType}"]`));
            const allButton = groupButtons.find(btn => btn.dataset.filterValue === 'all');

            if (filterValue === 'all') {
                // Deactivate all other buttons and activate 'all'
                groupButtons.forEach(btn => btn.classList.remove('active'));
                allButton.classList.add('active');
            } else {
                // Toggle clicked button and manage 'all' state
                this.classList.toggle('active');
                allButton?.classList.remove('active');

                // Check if all specific buttons are active to switch to 'all'
                const specificButtons = groupButtons.filter(btn => btn !== allButton);
                const allActive = specificButtons.every(btn => btn.classList.contains('active'));
                if (allActive) {
                    specificButtons.forEach(btn => btn.classList.remove('active'));
                    allButton.classList.add('active');
                }
            }

            applyFilters();
        });
    });

    function applyFilters() {
        // Get active filters for each category (excluding 'all')
        const loanTypes = Array.from(document.querySelectorAll('.filter-btn[data-filter-type="loan-type"].active:not([data-filter-value="all"])'))
            .map(btn => btn.dataset.filterValue);
        const loanStatuses = Array.from(document.querySelectorAll('.filter-btn[data-filter-type="loan-status"].active:not([data-filter-value="all"])'))
            .map(btn => btn.dataset.filterValue);

        rows.forEach(row => {
            const rowType = row.dataset.loanType;
            const rowStatus = row.dataset.loanStatus;

            // Check against active filters (show if matches any)
            const typeMatch = loanTypes.length === 0 || loanTypes.includes(rowType);
            const statusMatch = loanStatuses.length === 0 || loanStatuses.includes(rowStatus);

            row.style.display = typeMatch && statusMatch ? '' : 'none';
        });
    }
});

// Search functionality

document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInput');
    const rows = document.querySelectorAll('tbody tr');

    searchInput.addEventListener('input', function () {
        const searchValue = searchInput.value.trim().toLowerCase();

        for (const row of rows) {
            const accountNumberCell = row.cells[1];
            const SSNCell= row.cells[9];

            if (!accountNumberCell) continue;
            if (!SSNCell) continue;

            const accountNumber = accountNumberCell.textContent.trim().toLowerCase();
            const accountSSN = SSNCell.textContent.trim().toLowerCase();
            if (searchValue === "") {
                row.style.display = ''; // Show all rows
            } else {
                if (accountNumber.includes(searchValue) || accountSSN.includes(searchValue)) {
                    row.style.display = ''; // Show the row
                } else {
                    row.style.display = 'none'; // Hide the row
                }
            }
        }
    });
});