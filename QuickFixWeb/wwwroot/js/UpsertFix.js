$(document).ready(function () {
    // Select the first item under #stringList when the page is loaded
    var firstListItem = $('#stringList li:first-child');
    if (firstListItem.length > 0) {
        firstListItem.addClass('selected');
    }

    //update listContentInput
    //listContentInput();
});

// Function to collect the content of the ul element and include it in the form submission
function includeListContentInForm() {
    var listContent = [];
    $('#stringList li').each(function () {
        listContent.push($(this).text().trim());
    });
    $('#listContentInput').val(JSON.stringify(listContent));
}

// Function to enable/disable Add button based on input value
$('#newStringInput').on('input', function () {
    var trimmedValue = $(this).val().trim();
    if (trimmedValue.length > 0) {
        $('#addButton').prop('disabled', false);
    } else {
        $('#addButton').prop('disabled', true);
    }
});

// JavaScript function to add a new string to the list
$('#addButton').click(function (e) {
    e.preventDefault();
    var newString = $('#newStringInput').val().trim();
    if (newString.length > 0) {
        // Check if the newString is already listed
        var isAlreadyListed = $('#stringList li').filter(function () {
            return $(this).text().trim() === newString;
        }).length > 0;

        if (isAlreadyListed) {
            // Show modal indicating that the tag is already listed
            showModal("The tag is already listed.");
        } else {
            // Add the newString to the list
            $('#stringList').append(`<li class="list-group-item" data-item="${newString}">${newString}</li>`);
            includeListContentInForm(); // Include the updated list content in the form
            $('#newStringInput').val(''); // Clear input field after adding
            $('#addButton').prop('disabled', true); // Disable Add button again
        }
    }
    return false;
});

// JavaScript function to handle delete button click
$('#deleteButton').click(function (e) {
    e.preventDefault();
    var selectedString = $('#stringList li.selected').text().trim();
    if (selectedString) {
        $('#stringList li.selected').remove();
        includeListContentInForm(); // Include the updated list content in the form
    } else {
        showModal("Please select a tag to delete.");
    }
    return false;
});

// JavaScript function to handle click on list items
$('#stringList').on('click', 'li', function () {
    $('#stringList li').removeClass('selected');
    $(this).addClass('selected');
});

// Function to show modal with message
function showModal(message) {
    $('#modalMessage').text(message);
    $('#myModal').css('display', 'block');
}

// Close the modal when the user clicks on <span> (x)
$('.close').click(function () {
    $('#myModal').css('display', 'none');
});

// Close the modal when the user clicks anywhere outside of the modal
window.onclick = function (event) {
    var modal = document.getElementById('myModal');
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
