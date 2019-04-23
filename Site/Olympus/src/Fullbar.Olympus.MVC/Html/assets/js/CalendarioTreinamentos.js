
var dataSelecionada = new Date();

$(function () {


    var d = new Date();
    var n = d.getDate();


    $.get('/Treinamento/Calendario', function (resultado) {

        if (resultado[0].Idioma == 'pt-BR') {

            $("#datepickerTreinamento").datepicker({

                numberOfMonths: 3,
                //minDate: -n + 1,
                //maxDate: "+1M" + " -" + n,
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],

                beforeShowDay: function (date) {

                    return [true, Background(date.getTime(), resultado)];
                }
            });
        }
        else if (resultado[0].Idioma == 'en') {

            $("#datepickerTreinamento").datepicker({

                numberOfMonths: 3,
                beforeShowDay: function (date) {

                    return [true, Background(date.getTime(), resultado)];
                }
            });
        }
        else {


            $("#datepickerTreinamento").datepicker({

                numberOfMonths: 3,
                //minDate: -n + 1,
                //maxDate: "+1M" + " -" + n,
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],

                beforeShowDay: function (date) {

                    return [true, Background(date.getTime(), resultado)];

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

                /* Status Confirmado */
                if (resultado[i].idStatus == 2) {

                    return "dp-highlight2";
                }

                /* Status lista de espera */
                if (resultado[i].idStatus == 3 || resultado[i].idStatus == 4) {

                    return "dp-highlight3";
                }
            }
        }
    }
}