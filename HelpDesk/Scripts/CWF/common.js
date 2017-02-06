
$(function () {

    // Toggle Function
    $('.toggle').click(function () {
        // Switches the Icon
        $(this).children('i').toggleClass('fa-pencil');
        // Switches the forms  
        $('.form').animate({
            height: "toggle",
            'padding-top': 'toggle',
            'padding-bottom': 'toggle',
            opacity: "toggle"
        }, "slow");
    });

    //$(document).ready(function () {
    //    $('.dropdown-submenu a.test').on("click", function (e) {
    //        $(this).next('ul').toggle();
    //        e.stopPropagation();
    //        e.preventDefault();
    //    });
    //});
});