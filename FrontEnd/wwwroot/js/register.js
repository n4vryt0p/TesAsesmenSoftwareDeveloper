"use strict"

var input = '', correct = 3, isDaftar = false, antiForgeryToken;
function subsmits(e) {
    e.preventDefault();
    $.ajax({
        url: '../auth/register?handler=Reg',
        type: 'post',
        data: $("#lpsLogin").serialize(),
        headers: {
            "RequestVerificationToken": antiForgeryToken
        },
        success: function (data) {
            $.unblockUI();
            Swal.fire("Success!", "Your account created", "success").then((result) => {
                window.location.href = "../auth/login";
            });
        },
        error: function (data) {
            $.unblockUI();
            Swal.fire("Server Error!", data.responseText, "error");
        },
    });
    return false;
}

$(document).ready(function () {
    antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
});
