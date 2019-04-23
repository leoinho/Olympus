
var dataSelecionada = new Date();

$(function () {


    var d = new Date();
    var n = d.getDate();




    $.get('/Home/Calendario', function (resultado) {
        
        if (resultado[0].Idioma == 'pt-BR') {

            var dataSelecionada = {};
            var textoSelecionado = {};
            var linkSelecionado = {};

            if (resultado[0].idTreinamento != -1) {
                for (var i = 0; i < resultado.length; i++) {

                    var data = resultado[i].DataTreinamento.split('/');
                    var date1 = new Date(parseInt(data[2]), parseInt(data[1]) - 1, parseInt(data[0]));

                    dataSelecionada[new Date(date1)] = new Date(date1);
                    textoSelecionado[new Date(date1)] = resultado[i].dsNome;
                    linkSelecionado[new Date(date1)] = resultado[i].dsUrl;
                }
            }

                       
            $("#datepicker").datepicker({

                numberOfMonths: 1,
                minDate: -n + 1,
                maxDate: "+1M" + " -" + n,
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],

                beforeShowDay: function (date) {

                    var dataCelula = dataSelecionada[date];
                    var textoCelula = textoSelecionado[date];

                    if (dataCelula) {

                        return [true, Background(date.getTime(), resultado), textoCelula];
                    }
                    else {

                        return [true, '', ''];
                    }

                }
                ,

                onSelect: function (date) {

                    var dataLink = date.split('/');
                    var date1Link = new Date(parseInt(dataLink[2]), parseInt(dataLink[1]) - 1, parseInt(dataLink[0]));

                    var dataCelula = dataSelecionada[date1Link];
                    var linkTreinamento = linkSelecionado[date1Link];

                    if (dataCelula) {

                        return window.location.href = linkTreinamento;

                    }
                    else {

                        return [true, '', ''];
                    }

                }
            });
        }
        else if (resultado[0].Idioma == 'en') {


            var dataSelecionada = {};
            var textoSelecionado = {};
            var linkSelecionado = {};


            if (resultado[0].idTreinamento != -1) {
                for (var i = 0; i < resultado.length; i++) {

                    var data = resultado[i].DataTreinamento.split('/');
                    var date1 = new Date(parseInt(data[2]), parseInt(data[1]) - 1, parseInt(data[0]));

                    dataSelecionada[new Date(date1)] = new Date(date1);
                    textoSelecionado[new Date(date1)] = resultado[i].dsNome;
                    linkSelecionado[new Date(date1)] = resultado[i].dsUrl;
                }
            }

            $("#datepicker").datepicker({

                numberOfMonths: 1,
                minDate: -n + 1,
                maxDate: "+1M" + " -" + n,
                beforeShowDay: function (date) {

                    var dataCelula = dataSelecionada[date];
                    var textoCelula = textoSelecionado[date];

                    if (dataCelula) {

                        return [true, Background(date.getTime(), resultado), textoCelula];
                    }
                    else {

                        return [true, '', ''];
                    }
                },

                onSelect: function (date) {

                    var dataLink = date.split('/');
                    var date1Link = new Date(parseInt(dataLink[2]), parseInt(dataLink[0]) - 1, parseInt(dataLink[1]));

                    var dataCelula = dataSelecionada[date1Link];
                    var linkTreinamento = linkSelecionado[date1Link];

                    if (dataCelula) {

                        return window.location.href = linkTreinamento;
                    }
                    else {

                        return [true, '', ''];
                    }

                }

            });
        }
        else {

            var dataSelecionada = {};
            var textoSelecionado = {};
            var linkSelecionado = {};

            if (resultado[0].idTreinamento != -1) {
                for (var i = 0; i < resultado.length; i++) {

                    var data = resultado[i].DataTreinamento.split('/');
                    var date1 = new Date(parseInt(data[2]), parseInt(data[1]) - 1, parseInt(data[0]));

                    dataSelecionada[new Date(date1)] = new Date(date1);
                    textoSelecionado[new Date(date1)] = resultado[i].dsNome;
                    linkSelecionado[new Date(date1)] = resultado[i].dsUrl;
                }
            }


            $("#datepicker").datepicker({

                numberOfMonths: 1,
                minDate: -n + 1,
                maxDate: "+1M" + " -" + n,
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],

                beforeShowDay: function (date) {



                    var dataCelula = dataSelecionada[date];
                    var textoCelula = textoSelecionado[date];

                    if (dataCelula) {

                        return [true, Background(date.getTime(), resultado), textoCelula];
                    }
                    else {

                        return [true, '', ''];
                    }


                },

                onSelect: function (date) {

                    var dataLink = date.split('/');
                    var date1Link = new Date(parseInt(dataLink[2]), parseInt(dataLink[1]) - 1, parseInt(dataLink[0]));

                    var dataCelula = dataSelecionada[date1Link];
                    var linkTreinamento = linkSelecionado[date1Link];

                    if (dataCelula) {

                        return window.location.href = linkTreinamento;
                    }
                    else {

                        return [true, '', ''];
                    }

                }
            });
        }
    });
});


function Background(date, resultado) {

    for (var i = 0; i < resultado.length; i++) {

        var data = resultado[i].DataTreinamento.split('/');
        var date1 = new Date(parseInt(data[2]), parseInt(data[1]) - 1, parseInt(data[0]));

        if (dataSelecionada !== date1.getDate()) {

            if (date1 && (date == date1.getTime())) {

                dataSelecionada = date1.getDate();

                
                /* Status Disponivel */
                if (resultado[i].idStatus == 1) {

                    return "dp-highlight";
                }

                /* Status Cancelado */
                if (resultado[i].idStatus == 5 || resultado[i].idStatus == 6) {

                    return "dp-highlight3";
                }

                /* Status Confirmado - Se usuário logado */
                if (resultado[i].idStatus == 2 && resultado[i].dsStatusInscricao == "Confirmada") {

                    return "dp-highlight2";
                }

                /* Status 2 (lista de espera) neste caso é do treinamento, não da inscrição, por isso verifico se o texto é Confirmada */
                if (resultado[i].idStatus == 2 && resultado[i].dsStatusInscricao != "Confirmada") {

                    return "listaEspera";
                }

                /* Status lista de espera */
                if (resultado[i].idStatus == 4 || resultado[i].idStatus == 3) {

                    return "listaEspera";
                }

            }
        }
    }
}

function RetornaUrl(date, resultado) {
    for (var i = 0; i < resultado.length; i++) {

        var data = resultado[i].DataTreinamento.split('/');
        var date1 = new Date(parseInt(data[2]), parseInt(data[1]) - 1, parseInt(data[0]));

        if (dataSelecionada !== date1.getDate()) {

            if (date1 && (date == date1.getTime())) {

                dataSelecionada = date1.getDate();

                return window.location.href = '/Treinamento/Detalhes/' + resultado.idTreinamento;

            }
        }
    }

}

function RetornaTreinamentoHome(date, resultado) {

    for (var i = 0; i < resultado.length; i++) {

        var data = resultado[i].DataTreinamento.split('/');
        var date1 = new Date(parseInt(data[2]), parseInt(data[1]) - 1, parseInt(data[0]));

        if (dataSelecionada !== date1.getDate()) {

            if (date1 && (date == date1.getTime())) {

                dataSelecionada = date1.getDate();

                return resultado[i].dsNome

            }
        }

    }
}