//Para Edit Paquetería

const $inputImageEdit = document.querySelector("#inputImageEditP");
const $imagenPrevisualizacionEdit = document.querySelector("#ImageEditP");

$inputImageEdit.addEventListener("change", () => {
    const archivos = $inputImageEdit.files;
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacionEdit.src = "";
        return;
    }
    const primerArchivo = archivos[0];
    const objectURL = URL.createObjectURL(primerArchivo);
    $imagenPrevisualizacionEdit.src = objectURL;
});