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

(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
}(jQuery));

$(document).ready(function () {
    $(".currencyTextBox").inputFilter(function (value) {
        return /^-?\d*[.,]?\d{0,2}$/.test(value);
    });
});