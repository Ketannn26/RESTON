// Populate modal with menu item details
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');

    deleteButtons.forEach(button => {
        button.addEventListener('click', function () {
            const menuId = button.getAttribute('data-id'); // Get the item's ID
            const menuName = button.getAttribute('data-name'); // Get the item's name

            // Set modal content
            document.getElementById('deleteMessage').textContent = `Are you sure you want to delete "${menuName}"?`;
            document.getElementById('menuIdToDelete').value = menuId; // Set the hidden input
        });
    });
});
});    function showToast(message) {
        const toastElement = document.getElementById('toast');
        toastElement.querySelector('.toast-body').textContent = message; // Set the message
        const toast = new bootstrap.Toast(toastElement);
        toast.show(); // Display the toast
    }

});
