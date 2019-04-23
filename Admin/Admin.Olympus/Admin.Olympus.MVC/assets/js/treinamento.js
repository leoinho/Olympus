

$(function () {
    
    $("#dtTreinamento").mask("99/99/9999");
    $("#dsHorario").mask("99:99");



    //$("#btnAddAgenda").click(function (e) {
        
    //    return false;
    //});
    
    $(".btnDelPalestrante").click(function (e) {

        e.preventDefault();

        $("#loading").fadeIn();
            
        var dsPalestrante = $(this).attr('data-dsPalestrante');

        window.location.href = "ExcluiPalestrante?dsPalestrante=" + dsPalestrante;

    });

    $(".btnDelAgenda").click(function (e) {

        e.preventDefault();

        $("#loading").fadeIn();

        var dsIdAgenda = $(this).attr('data-dsidagenda');
        var idTreinamento = $(this).attr('data-idTreinamento');

        window.location.href = "ExcluiAgenda?dsIdAgenda=" + dsIdAgenda + "&idTreinamento=" + idTreinamento;

    });


    /* EDITAR */

    $(".btnDelPalestranteEditar").click(function (e) {

        e.preventDefault();

        $("#loading").fadeIn();

        var dsPalestrante = $(this).attr('data-dsPalestrante');

        window.location.href = "/Treinamento/ExcluiPalestranteEditar?dsPalestrante=" + dsPalestrante;

    });
    
    $(".btnDelAgendaEditar").click(function (e) {

        e.preventDefault();

        $("#loading").fadeIn();

        var dsIdAgenda = $(this).attr('data-dsidagenda');
        var idTreinamento = $(this).attr('data-idTreinamento');

        window.location.href = "/Treinamento/ExcluiAgendaEditar?dsIdAgenda=" + dsIdAgenda + "&idTreinamento=" + idTreinamento;

    });

});