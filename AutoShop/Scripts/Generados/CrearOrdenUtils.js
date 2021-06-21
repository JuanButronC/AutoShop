$(".ck-direccion").click(function () {
    if ($(this).prop("checked")) {
        $(".selected-dir").prop("checked", false);
        $(".selected-dir").removeClass("selected-dir");
        $(this).addClass("selected-dir");
    } else {
        $(this).removeClass("selected-dir");
    }
});

$(".ck-tarjeta").click(function () {
    if ($(this).prop("checked")) {
        $(".selected-tar").prop("checked", false);
        $(".selected-tar").removeClass("selected-tar");
        $(this).addClass("selected-tar");
        var num = $(this).attr("no-tarj");
        $("#nombreTarjeta").html(num);

        num = num.substring(0, 1);
        switch (num) {
            case "4":
                $("#imgTarjeta").attr("src", "../Content/img/recursos/Visa.png");
                break;
            case "5":
                $("#imgTarjeta").attr("src", "../Content/img/recursos/Master.png");
                break;
            case "6":
                $("#imgTarjeta").attr("src", "../Content/img/recursos/American.png");
                break;
        }
    } else {
        $("#imgTarjeta").attr("src", "../Content/img/recursos/TarjetaIcon.png");
        $(this).removeClass("selected-tar");
        $("#nombreTarjeta").html("");
    }
});

$("#a-pago-tarj").click(function () {
    var selectedDir = $(".selected-dir").val();
    var selectedTarj = $(".selected-tar").val();
    if (selectedDir !== undefined) {
        if (selectedTarj !== undefined) {
            var data = { tipo: "T", dir: selectedDir, tar: selectedTarj };
            pagar(data);
        } else {
            bootboxMsg("Por favor, seleccione una tarjeta para continuar.", "Advertencia");
        }
    } else {
        if (selectedDir == undefined && selectedTarj == undefined) {
            bootboxMsg("Por favor, seleccione una dirección y tarjeta para continuar", "Advertencia");
        } else {
            bootboxMsg("Por favor, seleccione una dirección para continuar", "Advertencia");
        }
    }

});

$("#a-pago-paypal").click(function () {
    var selectedDir = $(".selected-dir").val();
    if (selectedDir !== undefined) {
        //llamad
        var data = { tipo: "P", dir: selectedDir, tar: 0 };
        pagar(data);
    } else {
        bootboxMsg("Por favor, seleccione una dirección para continuar", "Advertencia");
    }

});

function pagar(data) {
    var form = document.createElement('form');
    document.body.appendChild(form);
    form.method = 'post';
    form.action = "/Pago/Pagar";
    for (var nombre in data) {
        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = nombre;
        input.value = data[nombre];
        form.appendChild(input);
    }
    form.submit();
}

function bootboxMsg(message, title) {
    bootbox.dialog({
        title: "<h4>" + title + "</h4>",
        message: message,
        buttons: {
            confirm: {
                label: '<i class="fa fa-check"></i> Aceptar'
            }
        },
        callback: function (result) {
        }
    });
}