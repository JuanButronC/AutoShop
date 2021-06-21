//Para Create Paquetería

const $inputImageCreate = document.querySelector("#inputImageCreateP");
const $imagenPrevisualizacionCreate = document.querySelector("#ImageCreateP");

$inputImageCreate.addEventListener("change", () => {
    const archivos = $inputImageCreate.files;
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacionCreate.src = "";
        return;
    }
    const primerArchivo = archivos[0];
    const objectURL = URL.createObjectURL(primerArchivo);
    $imagenPrevisualizacionCreate.src = objectURL;
});