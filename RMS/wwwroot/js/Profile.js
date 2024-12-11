// JavaScript to toggle the main content visibility
document.getElementById("toggleContentButton").addEventListener("click", function () {
    const mainContent = document.getElementById("mainContent");
    if (mainContent.style.display === "none" || mainContent.style.display === "") {
        mainContent.style.display = "block"; // Show the content
    } else {
        mainContent.style.display = "none"; // Hide the content
    }
});

document.getElementById("signOutBtn").addEventListener("click", function (event) {
    event.preventDefault(); // Prevent the default link behavior

    // Trigger your first action: Sign out (clear session)
    fetch('/Account/SignOut', {
        method: 'POST', // Assuming the SignOut method is a POST request
        headers: {
            'Content-Type': 'application/json'
        },
    })
        .then(response => {
            if (response.ok) {
                // Redirect to another action, e.g., the home page
                window.location.href = "/Home/Index";
            } else {
                alert("Error signing out");
            }
        });
});
  function validatePrice() {
        const priceInput = document.getElementById('Price');
    const priceError = document.getElementById('priceError');

    // Check if the price is valid and positive
    if (parseFloat(priceInput.value) < 0) {
        priceError.classList.remove('d-none');
    priceInput.setCustomValidity('Price must be a positive value.');
        } else {
        priceError.classList.add('d-none');
    priceInput.setCustomValidity(''); // Reset custom validation message
        }
    }
