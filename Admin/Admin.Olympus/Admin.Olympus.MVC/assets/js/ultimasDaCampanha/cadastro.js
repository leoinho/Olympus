$(document).ready(function () {
    // #################################################################
    // #################################################################
    // #################################################################
    // ############										    ############
    // ############          Ultimas da Campanha            ############
    // ############                                         ############
    // #################################################################
    // #################################################################
    // #################################################################
    
    $('#txtData').mask('00/00/0000');

    // Cancelar edição
    $("div.noticias #btnCancelar").click(function (event) {
        event.preventDefault();
        history.back(1);
    });

    // Salvar edição
    $("div.noticias #btnEditar").click(function (event) {
        event.preventDefault();

        var id = $("div.noticias #hddIdCampanha").val();
        var thumb = $("div.noticias #hddDsThumb").val();
        var imagem = $("div.noticias #hddDsImagem").val();
        var titulo = $("div.noticias #txtTitulo").val();
        var subtitulo = $("div.noticias #txtSubtitulo").val();
        var data = $("div.noticias #txtData").val();
        var ativo = $("div.noticias #ckbAtivo:checked").length > 0;
        var conteudo = tinyMCE.activeEditor.getContent();//$("div.noticias #txtConteudo").val();


        if (titulo === "" || subtitulo === "" || data === "" || conteudo === "") {
            alerta("Erro", "Preencha corretamente o Título, Subtitulo, Data e Conteúdo da notícia.");
            return false;
        } else if (!validaData(data)) {
            alerta("Erro", "Preencha uma data válida.");
            return false;
        }

        var files1 = $("div.noticias #fupThumb").get(0).files;
        var files2 = $("div.noticias #fupImagem").get(0).files;
        if ((thumb === "" && files1.length == 0) || (imagem === "" && files2.length == 0)) {
            alerta("Erro", "Escolha uma imagem de thumb e cabeçalho para a notícia.");
            return false;
        }

        var url = "/UltimasDaCampanha/EditarNoticia";
        $.post(url, { idCampanha: id, dsTitulo: titulo, dsSubtitulo: subtitulo, dtData: data, dsThumb: thumb, dsImagem: imagem, dsConteudo: conteudo, btAtivo: ativo }, function (response) {
            if (response.id > 0) {
                if (files1.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files1.length; i++) {
                        data.append("file" + i, files1[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/UltimasDaCampanha/CadastrarThumb?id="+response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Noticia atualizada, mas ocorreu um erro ao chamar o método de gravação da imagem de thumb: " + res.dsMensagem, true);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Noticia atualizada, mas ocorreu um erro ao chamar o método de gravação da imagem de thumb.", true);
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
                        url: "/UltimasDaCampanha/CadastrarCabecalho?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Noticia atualizada, mas ocorreu um erro ao chamar o método de gravação da imagem de cabeçalho: " + res.dsMensagem, true);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Noticia atualizada, mas ocorreu um erro ao chamar o método de gravação da imagem de cabeçalho.", true);
                            return false;
                        }
                    });
                }

                alerta("Sucesso", "Notícia atualizada com sucesso.", true);
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
    $("div.noticias #btnCadastro").click(function (event) {
        event.preventDefault();

        var titulo = $("div.noticias #txtTitulo").val();
        var subtitulo = $("div.noticias #txtSubtitulo").val();
        var data = $("div.noticias #txtData").val();
        var ativo = $("div.noticias #ckbAtivo:checked").length > 0;
        var conteudo = tinyMCE.activeEditor.getContent();//$("div.noticias #txtConteudo").val();


        if (titulo === "" || subtitulo === "" || data === "" || conteudo === "") {
            alerta("Erro", "Preencha corretamente o Título, Subtitulo, Data e Conteúdo da notícia.");
            return false;
        } else if (!validaData(data)) {
            alerta("Erro", "Preencha uma data válida.");
            return false;
        }

        var files1 = $("div.noticias #fupThumb").get(0).files;
        var files2 = $("div.noticias #fupImagem").get(0).files;
        if (files1.length == 0 || files2.length == 0) {
            alerta("Erro", "Escolha uma imagem de thumb e cabeçalho para a notícia.");
            return false;
        }

        var url = "/UltimasDaCampanha/CadastrarNoticia";
        $.post(url, { idCampanha: 0, dsTitulo: titulo, dsSubtitulo: subtitulo, dtData: data, dsThumb: '', dsImagem: '', dsConteudo: conteudo, btAtivo: ativo }, function (response) {
            if (response.id > 0) {
                if (files1.length > 0) {
                    var data = new FormData();

                    for (i = 0; i < files1.length; i++) {
                        data.append("file" + i, files1[i]);
                    }

                    $.ajax({
                        type: "POST",
                        url: "/UltimasDaCampanha/CadastrarThumb?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Noticia cadastrada, mas ocorreu um erro ao chamar o método de gravação da imagem de thumb: " + res.dsMensagem, false, "/UltimasDaCampanha/Editar?campanha=" + response.id);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Noticia cadastrada, mas ocorreu um erro ao chamar o método de gravação da imagem de thumb.", false, "/UltimasDaCampanha/Editar?campanha=" + response.id);
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
                        url: "/UltimasDaCampanha/CadastrarCabecalho?id=" + response.id,
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (res) {
                            if (res.id <= 0) {
                                alerta("Erro", "Noticia cadastrada, mas ocorreu um erro ao chamar o método de gravação da imagem de cabeçalho: " + res.dsMensagem, false, "/UltimasDaCampanha/Editar?campanha=" + response.id);
                                return false;
                            }
                        },
                        error: function () {
                            alerta("Erro", "Noticia cadastrada, mas ocorreu um erro ao chamar o método de gravação da imagem de cabeçalho.", false, "/UltimasDaCampanha/Editar?campanha=" + response.id);
                            return false;
                        }
                    });
                }

                alerta("Sucesso", "Notícia cadastrada com sucesso.", false, "/UltimasDaCampanha/Editar?campanha=" + response.id);
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