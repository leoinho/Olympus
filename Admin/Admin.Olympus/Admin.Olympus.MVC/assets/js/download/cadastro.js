$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############                  Download               ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################
    
    // Cancelar edição
    $("div.download #btnCancelar").click(function (event) {
        event.preventDefault();
        history.back(1);
    });

    // Salvar edição
    $("div.download #btnEditar").click(function (event) {
        event.preventDefault();

        var id = $("div.download #hddIdDownload").val();
        var imagem = $("div.download #hddDsImagem").val();
        var arquivo = $("div.download #hddDsArquivo").val();
        var titulo = $("div.download #txtTitulo").val();
        var ativo = $("div.download #ckbAtivo:checked").length > 0;
        var equipeComercial = $("div.download #ckbEquipeComercial:checked").length > 0;
        var unidadeOperacional = $("div.download #ckbUnidadeOperacional:checked").length > 0;
        var revenda = $("div.download #ckbRevenda:checked").length > 0;
        var gerenteRevenda = $("div.download #ckbGerenteRevenda:checked").length > 0;
        var equipeRevenda = $("div.download #ckbEquipeRevenda:checked").length > 0;

        if (titulo === "") {
            alerta("Erro", "Preencha corretamente o Título");
            return false;
        } 

        var files1 = $("div.download #fupImagem").get(0).files;
        if (imagem === "" && files1.length == 0) {
            alerta("Erro", "Escolha uma imagem.");
            return false;
        }

        var files2 = $("div.download #fupArquivo").get(0).files;
        if (arquivo === "" && files2.length == 0) {
            alerta("Erro", "Escolha um arquivo.");
            return false;
        }

        var url = "/Download/EditarDownload";
        $.post(url, { idDownload: id, dsImagem: imagem, dsTitulo: titulo, dsArquivo: arquivo, btEquipeComercial: equipeComercial, btUnidadeOperacional: unidadeOperacional, btRevenda: revenda, btGerenteRevenda: gerenteRevenda, btEquipeRevenda: equipeRevenda, btAtivo: ativo }, function (response) {
            if (response.id > 0) {
                if (files1.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files1.length; i++) {
                        data.append("file" + i, files1[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/Download/CadastrarImagem?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Download atualizado, mas ocorreu um erro ao chamar o método de gravação da imagem: " + res.dsMensagem, true);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Download atualizado, mas ocorreu um erro ao chamar o método de gravação da imagem.", true);
                            return false;
                        }
                    });
                }

                if (files2.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files2.length; i++) {
                        data.append("file" + i, files2[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/Download/CadastrarArquivo?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Download atualizado, mas ocorreu um erro ao chamar o método de gravação do arquivo: " + res.dsMensagem, true);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Download atualizado, mas ocorreu um erro ao chamar o método de gravação do arquivo.", true);
                            return false;
                        }
                    });
                }

                alerta("Sucesso", "Download atualizado com sucesso.", true);
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
    $("div.download #btnCadastro").click(function (event) {
        event.preventDefault();

        var titulo = $("div.download #txtTitulo").val();
        var ativo = $("div.download #ckbAtivo:checked").length > 0;
        var equipeComercial = $("div.download #ckbEquipeComercial:checked").length > 0;
        var unidadeOperacional = $("div.download #ckbUnidadeOperacional:checked").length > 0;
        var revenda = $("div.download #ckbRevenda:checked").length > 0;
        var gerenteRevenda = $("div.download #ckbGerenteRevenda:checked").length > 0;
        var equipeRevenda = $("div.download #ckbEquipeRevenda:checked").length > 0;

        if (titulo === "" ) {
            alerta("Erro", "Preencha corretamente o Título.");
            return false;
        } 

        var files1 = $("div.download #fupImagem").get(0).files;
        if (files1.length == 0) {
            alerta("Erro", "Escolha uma imagem.");
            return false;
        }


        var files2 = $("div.download #fupArquivo").get(0).files;
        if (files2.length == 0) {
            alerta("Erro", "Escolha um arquivo.");
            return false;
        }

        var url = "/Download/CadastrarDownload";
        $.post(url, { idDownload: 0, dsImagem: '', dsTitulo: titulo, dsArquivo: '', btEquipeComercial: equipeComercial, btUnidadeOperacional: unidadeOperacional, btRevenda: revenda, btGerenteRevenda: gerenteRevenda, btEquipeRevenda: equipeRevenda, btAtivo: ativo }, function (response) {
            if (response.id > 0) {
                
                if (files1.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files1.length; i++) {
                        data.append("file" + i, files1[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/Download/CadastrarImagem?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Download cadastrado, mas ocorreu um erro ao chamar o método de gravação da imagem: " + res.dsMensagem, false, "/Download/Editar?download=" + response.id);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Download cadastrado, mas ocorreu um erro ao chamar o método de gravação da imagem.", false, "/Download/Editar?download=" + response.id);
                            return false;
                        }
                    });
                }

                if (files2.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files2.length; i++) {
                        data.append("file" + i, files2[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/Download/CadastrarArquivo?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Download atualizado, mas ocorreu um erro ao chamar o método de gravação do arquivo: " + res.dsMensagem, false, "/Download/Editar?download=" + response.id);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Download atualizado, mas ocorreu um erro ao chamar o método de gravação do arquivo.", false, "/Download/Editar?download=" + response.id);
                            return false;
                        }
                    });
                }

                alerta("Sucesso", "Download cadastrado com sucesso.", false, "/Download/Editar?download=" + response.id);
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