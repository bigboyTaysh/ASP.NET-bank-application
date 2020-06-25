$(document).ready(function ($) {
    $(".bankAccountNumber").on("keyup", function (e) {
        $(this).inputmask("99 9999 9999 9999 9999 9999 9999");
    });
});

