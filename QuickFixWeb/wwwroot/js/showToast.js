
$(document).ready(function () {
    console.log("Document ready");
    //show toast message
    showMessage();
});

function showMessage() {
    var message = $('#message').data('message');
    var errorIndicator = "[!]";
    if (message) {
        if (message.includes(errorIndicator))
            toastr.error(message.replace(errorIndicator, ""));
        else 
            toastr.success(message);
    }    
}