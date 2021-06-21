$("#form-add").submit(function (event) {

    var cantidad = $("#cantidad-prod").val().trim();
    var cantidadVal = ((cantidad === "" || isNaN(cantidad)) ? -1 : parseInt(cantidad, "10"));

    if (cantidadVal >= 1) {
        return;
    }
    event.preventDefault();

    bootBoxAlert("Aviso", "La cantidad no es valida, elegir un número mayor a cero");

});
$("#form-comment").submit(function (event) {

    var comen = $("#comentario").val().trim();
    var resultado = ((comen === "" || comen.length > 100) ? -1 : 0);
    if (resultado == 0) {
        return;
    }
    event.preventDefault();
    bootBoxAlert("Aviso", "El comentario no es valido, no puede estar vacio ni tener más de 100 caracteres");

});


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