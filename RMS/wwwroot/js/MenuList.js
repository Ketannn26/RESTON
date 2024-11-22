document.addEventListener('DOMContentLoaded', () => {
    const deleteModal = document.getElementById('deleteModal'); // Modal element
    const confirmDeleteButton = document.getElementById('confirmDelete'); // Confirm delete button
    let deleteMenuID = null;

    // Trigger modal and set up delete ID
    deleteModal.addEventListener('show.bs.modal', function (event) {
        const button = event.relatedTarget; // Button that triggered the modal
        deleteMenuID = button.getAttribute('data-id'); // Extract menu ID
        const itemName = button.getAttribute('data-name'); // Extract item name

        // Update modal content with item name
        document.getElementById('deleteItemName').textContent = itemName;
    });

    // Handle delete confirmation with AJAX
    confirmDeleteButton.addEventListener('click', function () {
        if (deleteMenuID) {
            // Retrieve Anti-Forgery Token
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            fetch(`/AdminDashboard/DeleteMenu/${deleteMenuID}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token // Add token to headers
                }
            })
                .then(response => {
                    if (response.ok) {
                        // Hide modal
                        const modalInstance = bootstrap.Modal.getInstance(deleteModal);
                        modalInstance.hide();

                        // Remove the row from the table
                        const row = document.querySelector(`button[data-id="${deleteMenuID}"]`).closest('tr');
                        if (row) row.remove();

                        // Optional: Show success message
                        alert('Menu item deleted successfully!');
                    } else {
                        alert('Failed to delete the menu item. Please try again.');
                    }
                })
                .catch(error => console.error('Error:', error));
        }
    });
    function showToast(message) {
        const toastElement = document.getElementById('toast');
        toastElement.querySelector('.toast-body').textContent = message; // Set the message
        const toast = new bootstrap.Toast(toastElement);
        toast.show(); // Display the toast
    }

});
