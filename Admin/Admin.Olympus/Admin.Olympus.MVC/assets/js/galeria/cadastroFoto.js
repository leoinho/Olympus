$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                   Foto                  ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################
    
    // Cancelar edição
    $("div.galeriaFoto #btnCancelar").click(function (event) {
        event.preventDefault();
        history.back(1);
    });

    // Salvar edição
    $("div.galeriaFoto #btnEditar").click(function (event) {
        event.preventDefault();

        var id = $("div.galeriaFoto #hddIdFoto").val();
        var imagem = $("div.galeriaFoto #hddDsArquivo").val();
        var album = $("div.galeriaFoto #ddlAlbum").val();
        var albumNome = $("div.galeriaFoto #ddlAlbum option:selected").text();
        var nome = $("div.galeriaFoto #txtNome").val();
        var ativo = $("div.galeriaFoto #ckbAtivo:checked").length > 0;

        if (album === "" || album == 0) {
            alerta("Erro", "Preencha corretamente o Álbum e o Nome");
            return false;
        } 

        var files1 = $("div.galeriaFoto #fupImagem").get(0).files;
        if (imagem === "" && files1.length == 0) {
            alerta("Erro", "Escolha uma imagem.");
            return false;
        }
        
        var url = "/Galeria/EditarFoto";
        $.post(url, { idFoto: id, idAlbum: album, dsAlbum: albumNome, dsNome: nome, dsArquivo: imagem, btAtivo: ativo }, function (response) {
            if (response.id > 0) {
                if (files1.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files1.length; i++) {
                        data.append("file" + i, files1[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/Galeria/CadastrarImagem?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Foto atualizada, mas ocorreu um erro ao chamar o método de gravação da imagem: " + res.dsMensagem, true);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Foto atualizada, mas ocorreu um erro ao chamar o método de gravação da imagem: " + res.dsMensagem, true);
                            return false;
                        }
                    });
                }
                alerta("Sucesso", "Foto atualizada com sucesso.", true);
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
    $("div.galeriaFoto #btnCadastro").click(function (event) {
        event.preventDefault();

        var album = $("div.galeriaFoto #ddlAlbum").val();
        var albumNome = $("div.galeriaFoto #ddlAlbum option:selected").text();
        var nome = $("div.galeriaFoto #txtNome").val();
        var ativo = $("div.galeriaFoto #ckbAtivo:checked").length > 0;

        if (album === "" || album == 0) {
            alerta("Erro", "Preencha corretamente o Álbum.");
            return false;
        }

        var files1 = $("div.galeriaFoto #fupImagem").get(0).files;
        if (files1.length == 0) {
            alerta("Erro", "Escolha uma imagem.");
            return false;
        }

        var url = "/Galeria/CadastrarFoto";
        $.post(url, { idFoto: 0, idAlbum: album, dsAlbum: albumNome, dsNome: nome, dsArquivo: '', btAtivo: ativo }, function (response) {
            if (response.id > 0) {
                
                if (files1.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files1.length; i++) {
                        data.append("file" + i, files1[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/Galeria/CadastrarImagem?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Foto cadastrada, mas ocorreu um erro ao chamar o método de gravação da imagem: "+res.dsMensagem, false, "/Galeria/EditarFoto?foto=" + response.id);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Foto cadastrada, mas ocorreu um erro ao chamar o método de gravação da imagem: " + res.dsMensagem, false, "/Galeria/EditarFoto?foto=" + response.id);
                            return false;
                        }
                    });
                }

                alerta("Sucesso", "Foto cadastrada com sucesso.", false, "/Galeria/EditarFoto?foto=" + response.id);
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