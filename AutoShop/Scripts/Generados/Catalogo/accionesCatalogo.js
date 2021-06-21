$(document).ready(function () {
    showPage("1");
});

function showPage(num) {
    clean();
    var cards = $(".div-prod");
    cards.each(function () {
        var page = $(this).attr("pagina");
        if (page === num) {
            $(this).addClass("pagSelected");
        } else {
            $(this).removeClass("pagSelected");
            $(this).prop("hidden", true);
        }
    });
    $(".btn-c-selected").addClass("btn-c");
    $(".btn-c-selected").removeClass("btn-c-selected");
    $("#page-link-" + num).addClass("btn-c-selected");
    $("#page-link-" + num).removeClass("btn-c");
}
$(".a-categoria").click(function () {
    var idCat = $(this).attr("id-categoria");
    $.ajax({
        method: "GET",
        data: { id: idCat },
        url: "../../catalogo/productosCategoria",
        success: function (respuesta) {
            $("#main-catalog").html(respuesta);
            showPage("1");

        },
        error: function (jqXHR, textStatus, errorThrown) {
            bootBoxAlert("Error", errorThrown);

        },

    });
    $.ajax({
        method: "GET",
        data: { id: idCat },
        url: "../../catalogo/getCategoria",
        success: function (respuesta) {
            $("#div-search").html(respuesta);

        },
        error: function (jqXHR, textStatus, errorThrown) {
            bootBoxAlert("Error", errorThrown);

        },

    });
});


$("#a-todos").click(function () {
    $("#div-search").html("");
    $.ajax({
        method: "GET",
        url: "../../catalogo/todosProductos",
        success: function (respuesta) {
            $("#main-catalog").html(respuesta);
            showPage("1");

        },
        error: function (jqXHR, textStatus, errorThrown) {
            bootBoxAlert("Error", errorThrown);

        },

    });
});
$(".filtro-precio").change(function () {
    var max = $("#input-max").val().trim();
    var min = $("#input-min").val().trim();
    var valMax = ((max === "" || isNaN(max)) ? -1 : parseInt(max, "10"));
    var valMin = ((min === "" || isNaN(min)) ? -1 : parseInt(min, "10"));
    if (valMax > 0 && valMin > 0) {
        var cards = $(".div-prod");
        cards.each(function () {
            var precio = parseInt($(this).attr("precio"), "10");
            if (valMin <= precio && precio <= valMax) {
                $(this).addClass("precioSelected");

            } else {
                $(this).removeClass("precioSelected");
            }
        });
    } else if (valMax > 0) {
        var cards = $(".div-prod");
        cards.each(function () {
            var precio = parseInt($(this).attr("precio"), "10");
            if (precio <= valMax) {
                $(this).addClass("precioSelected");

            } else {
                $(this).removeClass("precioSelected");
            }
        });

    } else {
        var cards = $(".div-prod");
        cards.each(function () {
            var precio = parseInt($(this).attr("precio"), "10");
            if (valMin <= precio) {
                $(this).addClass("precioSelected");

            } else {
                $(this).removeClass("precioSelected");
            }
        });
    }
    if (!showSelected()) {
        bootBoxAlert("Error", "Sin coincidencias");
    }

});


$("#div-descuentos").on("click", ".btn-descuento", function () {
    var cards = $(".div-prod");
    var descuentoB = parseInt($(this).attr("descuento"), "10");
    cards.each(function () {
        var descuentoP = parseInt($(this).attr("descuento"), "10");
        if (descuentoP === descuentoB) {
            $(this).addClass("descSelected");

        } else {
            $(this).removeClass("descSelected");
        }
    });
    if (!showSelected()) {
        bootBoxAlert("Aviso", "Sin coincidencias");
    }
});
$("#main-catalog").on("click", ".page-link", function () {
    var pag = $(this).attr("pag");
    if (pag === "prev") {
        var prev = parseInt($(".btn-c-selected").attr("pag"), "10") - 1;
        if (prev > 0) {
            showPage(prev + "");
        }
    } else if (pag === "next") {
        var next = parseInt($(".btn-c-selected").attr("pag"), "10") + 1;
        var lastIndex = $(".page-link").length - 2;
        var last = parseInt($(".page-link:eq(" + lastIndex + ")").attr("pag"), "10");
        if (next <= last) {
            showPage(next + "");
        }
    } else {
        showPage(pag);
    }


});
$("#btn-todos-descuento").click(function () {
    var cards = $(".div-prod");
    cards.each(function () {
        var descuento = parseInt($(this).attr("descuento"), "10");
        if (descuento > 0) {
            $(this).addClass("descSelected");

        } else {
            $(this).removeClass("descSelected");
        }
    });
    if (!showSelected()) {
        bootBoxAlert("Error", "Sin coincidencias");
    }
});

$("#btn-limpiar-filtros").click(function () {
    clean();
});



function hideAll() {
    var cards = $(".div-prod");
    cards.each(function () {

        $(this).prop("hidden", true);

    });
}
function showAll() {
    var cards = $(".div-prod");
    cards.each(function () {
        $(this).prop("hidden", false);
    });
}
function clean() {
    var cards = $(".div-prod");
    cards.each(function () {
        $(this).prop("hidden", false);
        $(this).removeClass("descSelected");
        $(this).remove("precioSelected");
    });
    $("#input-max").val("");
    $("#input-min").val("");
}
function showSelected() {
    var cards = $(".div-prod");
    var selected = false;
    var filtradoPrecio = (($(".precioSelected").length > 0) ? true : false);
    var filtradoDescuento = (($(".descSelected").length > 0) ? true : false);
    if (filtradoPrecio && filtradoDescuento) {
        cards.each(function () {
            if ($(this).hasClass("pagSelected") &&
                $(this).hasClass("descSelected") &&
                $(this).hasClass("precioSelected")) {
                $(this).prop("hidden", false);

                selected = true;

            } else {
                $(this).prop("hidden", true);
            }
        });

    } else {
        cards.each(function () {
            if ($(this).hasClass("pagSelected") &&
                $(this).hasClass("descSelected") ||
                $(this).hasClass("precioSelected")) {
                $(this).prop("hidden", false);

                selected = true;

            } else {
                $(this).prop("hidden", true);
            }
        });
    }

    return selected;
}



function bootBoxAlert(titulo, mensaje) {
    bootbox.dialog({
        title: titulo,
        message: '<div class="row" >' +
            '<div class=col-md-12>' +
            '<p>' + mensaje + '</p>' +
            '</div>' +
            '</div>',
        buttons: {
            main: {
                label: '<i class="fa fa-check-circle"></i> Aceptar',
                className: "btn-c text-white"
            }
        }
    });
}