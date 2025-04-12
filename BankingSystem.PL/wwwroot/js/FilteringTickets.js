document.addEventListener('DOMContentLoaded', function () {
    // Filtering functionality
    const buttons = document.querySelectorAll('.filter-btn');
    const rows = document.querySelectorAll('tbody tr');

    buttons.forEach(button => {
        button.addEventListener('click', function () {
            const filterType = this.dataset.filterType;
            const filterValue = this.dataset.filterValue;
            const groupButtons = Array.from(document.querySelectorAll(`.filter-btn[data-filter-type="${filterType}"]`));
            const allButton = groupButtons.find(btn => btn.dataset.filterValue === 'all');

            if (filterValue === 'all') {
                groupButtons.forEach(btn => btn.classList.remove('active'));
                allButton.classList.add('active');
            } else {
                this.classList.toggle('active');
                allButton?.classList.remove('active');

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
        const ticketTypes = Array.from(document.querySelectorAll('.filter-btn[data-filter-type="ticket-type"].active:not([data-filter-value="all"])'))
            .map(btn => btn.dataset.filterValue);
        const ticketStatuses = Array.from(document.querySelectorAll('.filter-btn[data-filter-type="ticket-status"].active:not([data-filter-value="all"])'))
            .map(btn => btn.dataset.filterValue);

        rows.forEach(row => {
            const rowType = row.dataset.ticketType;
            const rowStatus = row.dataset.ticketStatus;

            const typeMatch = ticketTypes.length === 0 || ticketTypes.includes(rowType);
            const statusMatch = ticketStatuses.length === 0 || ticketStatuses.includes(rowStatus);

            row.style.display = typeMatch && statusMatch ? '' : 'none';
        });
    }

    // Search functionality
    const searchInput = document.getElementById('searchInput');
    searchInput.addEventListener('input', function () {
        const searchValue = this.value.trim().toLowerCase();

        rows.forEach(row => {
            const customerName = row.cells[0].textContent.trim().toLowerCase();
            const accountNumber = row.cells[1].textContent.trim().toLowerCase();

            if (searchValue === "" ||
                customerName.includes(searchValue) ||
                accountNumber.includes(searchValue)) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        });
    });
});