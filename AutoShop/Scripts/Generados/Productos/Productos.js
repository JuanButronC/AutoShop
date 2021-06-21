$(document).ready(function () {
    $.ajax({
        url: "producto/getProductos",
        success: function (result) {
            $("#tablaProductos").html(result);
        }
    });

    $.ajax({
        url: "producto/getExistencias",
        success: function (result) {
            $("#tablaExistencias").html(result);
        }
    });

    $.ajax({
        url: "producto/getProdMayor",
        success: function (result) {
            $("#producto").html(result);
        }
    });

    const $seleccionArchivos = document.querySelector("#seleccionArchivos"),
        $imagenPrevisualizacion = document.querySelector("#imagenPrevisualizacion");

        $seleccionArchivos.addEventListener("change", () => {
        const archivos = $seleccionArchivos.files;
        if (!archivos || !archivos.length) {
            $imagenPrevisualizacion.src = "";
            return;
        }
        const primerArchivo = archivos[0];
        const objectURL = URL.createObjectURL(primerArchivo);
        $imagenPrevisualizacion.src = objectURL;
    });

});