"use strict"

$(document).ready(function () {
    localStorage.removeItem('picProfile');
    $('html').block({ message: null });
    $.ajax({
        url: '../auth/logout/challenge',
        type: 'post',
        data: $("#lpsLogout").serialize(),
        headers: {
            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
        },
        success: function (data) {
            //$('html').unblock();
            if (data == null) {
                window.location.href = "../auth/login";
            } else {
                window.location.href = "../auth/login?ReturnUrl=" + encodeURIComponent(data);
            }
        },
        error: function (data) {
            $('html').unblock();
            Swal.fire({
                title: 'To completely logout press Continue',
                showDenyButton: false,
                showCancelButton: false,
                confirmButtonText: 'Continue',
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {
                    window.location.reload(true);
                }
            })
        },
    });
});

function logoutBro() {
    $('html').block({ message: null });
    let dataX = document.getElementById("returnUrl").value
    if (dataX == null) {
        window.location.href = "../auth/login";
    } else {
        window.location.href = "../auth/login?ReturnUrl=" + encodeURIComponent(data);
    }
}
