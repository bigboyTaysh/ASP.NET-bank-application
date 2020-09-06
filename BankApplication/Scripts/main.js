$(document).ready(function ($) {
    $(".bankAccountNumber").on("keyup", function (e) {
        $(this).inputmask("99 9999 9999 9999 9999 9999 9999");
    });
});

$(document).on("click", ".clickable-tr", function (e) {
    var target = $(e.target);
    if (target.closest('.clickable-tr').attr("clicked") === "false") {
        $(target).closest('.clickable-tr').next('.openable-tr').show();
        $(target).closest('.clickable-tr').attr("clicked", "true");
    } else if (target.closest('.clickable-tr').attr("clicked") === "true") {
        $(target).closest('.clickable-tr').next('.openable-tr').hide();
        $(target).closest('.clickable-tr').attr("clicked", "false");
    }
});