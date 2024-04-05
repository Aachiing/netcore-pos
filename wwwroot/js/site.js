// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ClearFields(div, obj) {

    $(`#${div} input[type='text']`).each(function () {
        var prop = $(this).attr("id");
        $(`#${div} #${prop}`).val(obj[prop]);
    });

    $(`#${div} input[type='number']`).each(function () {
        var prop = $(this).attr("id");
        $(`#${div} #${prop}`).val(obj[prop]);
    });

    $(`#${div} input[type='hidden']`).each(function () {
        var prop = $(this).attr("id");
        $(`#${div} #${prop}`).val(obj[prop]);
    });
}

$.fn.enterKey = function (fnc) {
    return this.each(function () {
        $(this).keypress(function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == '13') {
                fnc.call(this, ev);
            }
        })
    })
}
function ActiveMenu(activeNav) {

    if (!$(`#${activeNav}`).hasClass("active")) {
        $(".navbar-nav").find(".nav-item").removeClass("active");
        $(`#${activeNav}`).addClass("active");
    }
}

$('#tblOrders').on('click', 'tbody > tr', function () {
    if (!$(this).hasClass("active")) {
        $(this).parent().find("tr").removeClass("active");
        $(this).addClass("active");
    }
    else {
        $(this).removeClass("active");
    }
});

