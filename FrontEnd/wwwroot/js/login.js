"use strict"

var input = '', correct = 3, isDaftar = false, antiForgeryToken;
function subsmits(e) {
    e.preventDefault();
    $('html').block();
    $.ajax({
        url: '../auth/login/challenge',
        type: 'post',
        data: $("#lpsLogin").serialize(),
        headers: {
            "RequestVerificationToken": antiForgeryToken
        },
        success: function (data) {
            if (data.pin === "Salah") {
                $('html').unblock();
                Swal.fire("Server Error!", "Harap hubungi helpdesk", "error");
                return;
            }
            if (data.tok != undefined) {
                localStorage.setItem("picProfile", data.pic);

                check_cookie_name(".AspNetCore.Culture");

                window.location.href = "../";

            } else {
                $('html').unblock();
                Swal.fire("Username tidak ada!", "Harap hubungi helpdesk", "warning");
            }
        },
        error: function (data) {
            //console.log(data);
            $('html').unblock();
            Swal.fire("Server Error!", data.responseText, "error");
        },
    });

    return false;
}

function check_cookie_name(name) {
    var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    if (match) {
        return false;
    }
    else {
        document.cookie = ".AspNetCore.Culture=c=en_US|uic=en_US;path=/"
        return false;
    }
}


$(document).ready(function () {
    antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
    localStorage.removeItem('picProfile');
    var dots = document.querySelectorAll('.dot'), numbers = document.querySelectorAll('.number');
    dots = Array.prototype.slice.call(dots);
    numbers = Array.prototype.slice.call(numbers);
    numbers.forEach(function (number, index) {
        number.addEventListener('click', function () {
            number.className += ' grow';
            if (index === 10) {
                if (input.length !== 0) {
                    dots[input.length - 1].classList.remove('active');
                    input = input.slice(0, -1);
                }
                ;
            }
            else if (index === 9) {
                input += 0;
                dots[input.length - 1].className += ' active';
            } else {
                input += (index + 1);
                dots[input.length - 1].className += ' active';
            }
            if (input.length >= 6) {
                $('#pin').block({ message: null });
                document.getElementById('pinCode').value = input;
                $.ajax({
                    url: '../auth/login/challengePin',
                    type: 'post',
                    data: $("#lpsLogin").serialize(),
                    headers: {
                        "RequestVerificationToken": antiForgeryToken
                    },
                    success: function (data) {
                        console.log(data);
                        if (data.pin === "Salah" || data.pin === -1) {
                            correct = correct - 1;
                            $('#pin').unblock();
                            if (correct <= 0) {
                                $("#pinModal").modal("hide");
                                Swal.fire("Your PIN is Locked!", "You may contact helpdesk to unlock your PIN.", "error");
                                return false;
                            }
                            document.getElementById('wordingPin2').innerHTML = `Wrong. You have ${correct} attempt(s)`;
                            dots.forEach(function (dot, index) {
                                dot.className += ' wrong';
                            });
                        } else {
                            document.getElementById('iconLock').innerHTML = "<i class='fa fa-unlock font-size-16pt text-warning'></i>";
                            dots.forEach(function (dot, index) {
                                dot.className += ' correct';
                            });
                            localStorage.setItem("picProfile", data.pic);
                            if (typeof data === 'string' && data.includes("DOCTYPE html")) {
                                document.open();
                                document.write(data);
                                document.close();
                            } else {
                                var parser = document.createElement('a');
                                parser.href = document.getElementById('returnUrl').value;
                                check_cookie_name(".AspNetCore.Culture");

                                const params = new Proxy(new URLSearchParams(window.location.search), {
                                    get: (searchParams, prop) => searchParams.get(prop),
                                });
                                if (location.host !== parser.host && params.ReturnUrl.includes("http") && params.SAMLRequest == null && params.RelayState == null && params.Token !== 'yes') {
                                    window.location.href = `${params.ReturnUrl}?jwt=${data.tok}`;
                                }
                                else if (location.host !== parser.host && params.ReturnUrl.includes("http") && params.SAMLRequest == null && params.RelayState == null && params.Token === 'yes') {
                                    let myEnc = encodeURIComponent(data.tik);
                                    window.location.href = `${params.ReturnUrl}&token=${myEnc}`;
                                }
                                else {
                                    let value = params.ReturnUrl || "/";
                                    window.location.href = value;
                                }
                            }
                        }
                    },
                    error: function (data) {
                        $('#pin').unblock();
                        $("#pinModal").modal("hide");
                        Swal.fire("Application Server currently is not available.", "Please, contact helpdesk", "error");
                    },
                });
                //$.post("/loged/challenged", $("#lpsLogin").serialize(), function (data, status) {
                //    if (status === "success") {

                //    }
                //})
                //    .fail(function () {

                //    });
                setTimeout(function () {
                    dots.forEach(function (dot, index) {
                        dot.className = 'dot';
                    });
                    input = '';
                }, 500);
            }
            setTimeout(function () {
                number.className = 'number';
            }, 1000);
        });
    });
});
