// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function hideMessageDiv() {
    var messageDiv = document.getElementById("message");
    if (messageDiv) {
        setTimeout(function () {
            messageDiv.style.display = "none";
        }, 3000);
    }
}
window.addEventListener("DOMContentLoaded", function () {
    hideMessageDiv();
});

function previewImage(input) {
    var imgPreview = document.getElementById('ImgPreview');
    var fileInput = input;

    if (fileInput.files && fileInput.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            imgPreview.src = e.target.result;
        };

        reader.readAsDataURL(fileInput.files[0]);
    }
}
