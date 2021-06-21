$(".btn-ver").click(function () {
    $("#div-info").html("");
    var id = $(this).attr("id-empleado");
    $.ajax({
        method: "GET",
        url: "Empleado/DetailsPartial",
        data: { id: id },
        success: function (respuesta) {
            $("#div-info").html(respuesta);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            bootBoxAlert("Error", errorThrown);

        },

    });
});

$("#imagen_fk").on("change", function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#img-preview').attr('src', e.target.result);
        }
        reader.readAsDataURL(this.files[0]);
    }
});

$("#imagen").on("change", function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#img-preview').attr('src', e.target.result);
        }
        reader.readAsDataURL(this.files[0]);
    }
});

$("#form-edit").submit(function (event) {

    if ($('#imagen_fk').get(0).files.length !== 0) {
        return;
    }
    event.preventDefault();
    bootBoxAlert("Aviso", "Debe seleccionar una imagen.");

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
