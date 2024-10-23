document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('purchaseForm');
    const clientRadios = document.querySelectorAll('input[name="selectedClient"]');
    const bookRadios = document.querySelectorAll('input[name="selectedBook"]');
    const submitBtn = document.getElementById('submitBtn');
    const quantityInput = document.getElementById('quantity');
    const stockInfo = document.getElementById('stockInfo');

    // Handle client selection
    clientRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            // Remove selected class from all items
            document.querySelectorAll('#clientsList .list-group-item').forEach(item => {
                item.classList.remove('selected');
            });

            // Add selected class to parent
            this.closest('.list-group-item').classList.add('selected');

            // Get client details
            const label = this.closest('label');
            const clientName = label.querySelector('strong').textContent;

            // Update hidden input and display
            document.getElementById('userId').value = this.value;
            document.getElementById('selectedClientDisplay').textContent = clientName;

            updateSubmitButton();
        });
    });

    // Handle book selection
    bookRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            // Remove selected class from all items
            document.querySelectorAll('#booksList .list-group-item').forEach(item => {
                item.classList.remove('selected');
            });

            // Add selected class to parent
            this.closest('.list-group-item').classList.add('selected');

            // Get book details
            const label = this.closest('label');
            const bookTitle = label.querySelector('strong').textContent;
            const maxQty = parseInt(this.dataset.maxQty);
            const price = parseFloat(this.dataset.price);

            // Update hidden inputs and displays
            document.getElementById('bookId').value = this.value;
            document.getElementById('pricePerPC').value = price;
            document.getElementById('selectedBookDisplay').textContent = bookTitle;

            // Update quantity constraints
            quantityInput.max = maxQty;
            quantityInput.value = Math.min(parseInt(quantityInput.value) || 1, maxQty);
            stockInfo.textContent = `Maximum available: ${maxQty}`;

            updateSubmitButton();
        });
    });

    // Handle quantity changes
    quantityInput.addEventListener('input', function () {
        const max = parseInt(this.max) || 1;
        const value = parseInt(this.value) || 1;

        if (value > max) {
            this.value = max;
        } else if (value < 1) {
            this.value = 1;
        }
    });

    function updateSubmitButton() {
        const hasClient = document.getElementById('userId').value !== '';
        const hasBook = document.getElementById('bookId').value !== '';
        submitBtn.disabled = !(hasClient && hasBook);
    }

    // Auto-dismiss alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            const closeButton = alert.querySelector('.btn-close');
            if (closeButton) {
                closeButton.click();
            }
        }, 5000);
    });
});