$(document).ready(function () {
 
    // Filtrar
    $("#btnFiltrar").click(function (event) {
        event.preventDefault();

        var de = $("#txtDataDe").val();
        var ate = $("#txtDataAte").val();

        if (!validaData(de)) {
            alerta("Erro", "Preencha uma data inicial válida.");
            return false;
        } else if (!validaData(ate)) {
            alerta("Erro", "Preencha uma data final válida.");
            return false;
        }

        if (de !== "") {
            var aux = de.split('/');
            de = aux[1] + "%2f" + aux[0] + "%2f" + aux[2];
        }

        if (ate !== "") {
            var aux = ate.split('/');
            ate = aux[1] + "%2f" + aux[0] + "%2f" + aux[2];
        }

        $("#loading").fadeIn();
        window.location.href = "/Analytics/Index?de=" + de + "&ate=" + ate;
        return false;
    });
});