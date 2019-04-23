$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                   Album                 ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################
    
    // Cancelar edição
    $("div.galeriaAlbum #btnCancelar").click(function (event) {
        event.preventDefault();
        history.back(1);
    });

    // Salvar edição
    $("div.galeriaAlbum #btnEditar").click(function (event) {
        event.preventDefault();

        var id = $("div.galeriaAlbum #hddIdAlbum").val();
        var nome = $("div.galeriaAlbum #txtNome").val();
        var ativo = $("div.galeriaAlbum #ckbAtivo:checked").length > 0;

        if (nome === "") {
            alerta("Erro", "Preencha corretamente o Nome");
            return false;
        } 
        var url = "/Galeria/EditarAlbum";
        $.post(url, { idAlbum: id,dsNome: nome, btAtivo: ativo }, function (response) {
            if (response.id > 0) {
                alerta("Sucesso", "Álbum atualizado com sucesso.", true);
            }
            else {
                alerta("Erro", response.dsMensagem);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alerta("Erro", "Ocorreu um erro ao chamar o método: " + errorThrown);
        });
        return false;
    });

    // Salvar cadastro
    $("div.galeriaAlbum #btnCadastro").click(function (event) {
        event.preventDefault();

        var nome = $("div.galeriaAlbum #txtNome").val();
        var ativo = $("div.galeriaAlbum #ckbAtivo:checked").length > 0;

        if (nome === "" ) {
            alerta("Erro", "Preencha corretamente o Nome.");
            return false;
        } 
        
        var url = "/Galeria/CadastrarAlbum";
        $.post(url, { idAlbum: 0, dsNome: nome, btAtivo: ativo }, function (response) {
            if (response.id > 0) {     
                alerta("Sucesso", "Álbum cadastrado com sucesso.", false, "/Galeria/EditarAlbum?album=" + response.id);
            }
            else {
                alerta("Erro", response.dsMensagem);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alerta("Erro", "Ocorreu um erro ao chamar o método: " + errorThrown);
        });
        return false;
    });
});