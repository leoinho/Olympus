
$(document).ready(function () {
    //$('#myModal').on('shown.bs.modal', function () {
    //    $('#myInput').focus()
    //})

    $("#carregando").hide();
    //$(".esconde-cadastro").hide();

    $('#drop-submenu').on('show.bs.dropdown', function () {
        $().dropdown('toggle');
    })

    $(".selec-cadastro").click(function () {
        $(".esconde-cadastro").toggle("slow", function () {
            
        });
    });

    $(".deslize").click(function () {
        $(".deslize-aviso img").hide();
    });

    

    $('#quote-carousel').carousel({
        pause: true,
        interval: 40000,
    });


    $(".ui-corner-all").hide();
    
    
});


$(function () {
    if (window.innerWidth > 992) {
        //$('.bar-lateral').css('min-height', $('html').outerHeight() + $('nav.navbar').outerHeight())
        $('.conteudo').css('min-height', $('html').outerHeight() + $('nav.navbar').outerHeight() - $('.footer').outerHeight() - $('.bg-menu').outerHeight());
        //$('.conteudo.fila-espera').css('min-height', $('html').outerHeight() + $('nav.navbar').outerHeight() - $('.footer').outerHeight() - $('.bg-menu').outerHeight());

        $('.bar-lateral').css('height', $('.conteudo').outerHeight());
        $('.bar-lateral.conteudo-fila-espera').css('height', $('.conteudo.fila-espera').outerHeight());

        $('.treinamento-tipo').css('min-height', $('.treinamento-endereco').outerHeight());
        $('.treinamento-data').css('min-height', $('.treinamento-endereco').outerHeight());
    }

    console.log(window.innerWidth);
   
})

//$(window).bind('resize', function () {
//    $('.bar-lateral').css('min-height', $('.conteudo').outerHeight() + $('nav.navbar').outerHeight())
    
//});

function alerta(txt, title, reload, link) {

    $('.contentModal').html(txt);
    if (title) {
        $('.titleModal').html(title);
    };

    $('#myModal').modal(function () {
        $("#myModal").show();
    });

    if (reload == true) {
        $('#myModal .close, #myModal').on('click', function (event) {
            event.preventDefault();
            window.location.reload();
        });       

    };

    if (link != false && link != undefined && link != "") {
        

        $('#myModal .close, #myModal').on('click', function (event) {
            event.preventDefault();
            
            window.location.href = link;
        });
    }

};



function confirma(titulo, mensagem) {

    $('#myModalConfirmLabel').text(titulo);
    $('#myModalConfirmTexto').html(mensagem);
    $('#myModalConfirm').modal('show');

}

function cadastro(titulo, mensagem) {

    $('#myModalCadastroLabel').text(titulo);
    $('#myModalCadastroTexto').html(mensagem);
    $('#myModalCadastro').modal('show');
}