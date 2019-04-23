using Admin.Olympus.Dominio.Entidade;
//using Admin.Olympus.Dominio.Interface;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioAnalytics
    {
        private readonly string _ids = string.Empty;

        public RepositorioAnalytics(int idTipo)
        {
            if (idTipo == 1)
                _ids = WebConfigurationManager.AppSettings["_idsOBL"].ToString();
            else if (idTipo == 2)
                _ids = WebConfigurationManager.AppSettings["_idsOLA"].ToString();
            else if (idTipo == 3)
                _ids = WebConfigurationManager.AppSettings["_idsOMS"].ToString();
        }

        public string CarregaGraficoPaginaSessao(DateTime dtInicial, DateTime dtFinal)
        {
            var objAnalytics = new Util.Analytics(_ids);

            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            objAnalytics.DataInicio = (dtInicial == DateTime.MinValue) ? DateTime.Now.AddDays(-30) : Convert.ToDateTime(dtInicial);
            objAnalytics.DataTermino = (dtFinal == DateTime.MinValue) ? DateTime.Now.AddDays(-1) : Convert.ToDateTime(dtFinal);

            var dadosAnalytics = new List<List<object[]>>();

            dadosAnalytics.Add(objAnalytics.ObterDados("ga:userDefinedValue", "ga:pageviewsPerSession", "-ga:pageviewsPerSession"));

            var resultado = dadosAnalytics[0][0][1];

            return resultado.ToString();
        }

        /* Graficos novos*/
        public string CarregaGraficoVisitante(DataTable dadosAnalytics)
        {

            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            sb.Append("<script type='text/javascript' src='https://www.google.com/jsapi'></script>");

            sb.Append("<script type='text/javascript'>");

            sb.Append("google.load('visualization', '1', {packages:['corechart']});");

            sb.Append("google.setOnLoadCallback(drawChart3);");

            sb.Append("function drawChart3() {");

            sb.Append("var data = google.visualization.arrayToDataTable([");

            sb.Append("['Nova', 'Cadastros'],");


            for (int i = 0; i < dadosAnalytics.Rows.Count; i++)
            {
                var count = 0;
                var strInformacao = (dadosAnalytics.Rows[i][count].ToString().Contains("New")) ? "Novo Visitante" : "Retorno Visitante";

                count += 1;
                row.Append(" ['" + strInformacao + "' ," + dadosAnalytics.Rows[i][count].ToString() + "],");
            }

            sb.Append(row.ToString().Trim(','));
            sb.Append("]);");

            sb.Append("var options = {");
            sb.Append("title: '',");

            sb.Append("pieHole: 0.4,");
            sb.Append(" };");

            sb.Append("var chart = new google.visualization.PieChart(document.getElementById('chart_visitante'));");
            sb.Append("chart.draw(data, options);");
            sb.Append(" }");
            sb.Append("</script>");

            sb.Append("<div id='chart_visitante' style='width: 100%; height: 400px;'></div>");

            return sb.ToString();



        }

        public string CarregaGraficoCidade(DataTable dadosAnalytics)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            sb.Append("<script type='text/javascript' src='https://www.google.com/jsapi'></script>");
            sb.Append("<script type='text/javascript'>");
            sb.Append("google.load('visualization', '1', {packages:['corechart']});");
            sb.Append("google.setOnLoadCallback(drawChart);");
            sb.Append("function drawChart() {");
            sb.Append("var data = google.visualization.arrayToDataTable([");
            sb.Append("['Cidade', 'Sess\u00e3o'],");

            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < 10; i++)
                {
                    row.Append("['" + dadosAnalytics.Rows[i][0].ToString() + "' ," + dadosAnalytics.Rows[i][1] + "],");
                }
            }
            else
            {
                return "<div aria-hidden='true' style='position: relative; padding-top: 189px; left: 189px; white-space: nowrap; font-family: Arial; font-size: 12px; height: 400px;'>Nenhum dado</div>";
            }

            sb.Append(row.ToString().Trim(','));

            sb.Append("]);");

            sb.Append("var options = {};");

            sb.Append("var chart = new google.visualization.BarChart(document.getElementById('chart_Cidade'));");

            sb.Append("chart.draw(data, options);");

            sb.Append("}");

            sb.Append("</script>");

            sb.Append("<div id='chart_Cidade' style='width: 100%; height:429px;'></div>");

            return sb.ToString();
        }

        public List<Cidades> CarregaCidadesAtivas(DataTable dadosAnalytics)
        {

            var lista = new List<Cidades>();
            var count = 0;

            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < 10; i++)
                {
                    var mCidade = new Cidades();

                    count += 1;

                    mCidade.Posicao = count;
                    mCidade.Cidade = dadosAnalytics.Rows[i][0].ToString();
                    mCidade.Sessao = dadosAnalytics.Rows[i][1].ToString();

                    lista.Add(mCidade);
                }
            }


            return lista;
        }

        public string CarregaGraficoPagina(DataTable dadosAnalytics)
        {

            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            var Lista = new List<Home>();
            var resultado = new List<Home>();

            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < dadosAnalytics.Rows.Count; i++)
                {
                    var mPagina = new Home();
                    var retorno = dadosAnalytics.Rows[i][0].ToString().Split('.');
                    var pagina = retorno[0].ToString().ToUpper();
                    var nomePagina = (pagina.Replace("/", "") == string.Empty || pagina.Replace("/", "") == "DEFAULT") ? "HOME" : pagina;

                    mPagina.Pagina = nomePagina;
                    mPagina.Sessao = Convert.ToInt16(dadosAnalytics.Rows[i][1]);

                    Lista.Add(mPagina);
                }
            }


            resultado = Lista.GroupBy(g => g.Pagina)
                                     .Select(i => new Home()
                                     {
                                         Pagina = i.Key,
                                         Sessao = i.Sum(s => s.Sessao)
                                     }).OrderByDescending(s => s.Sessao).ToList();




            sb.Append("<script type='text/javascript' src='https:sb.Append(www.google.com/jsapi'></script>");
            sb.Append("<script type='text/javascript'>");
            sb.Append("google.load('visualization', '1', {packages:['corechart']});");
            sb.Append("google.setOnLoadCallback(drawChart);");
            sb.Append("function drawChart() {");

            sb.Append("var data = google.visualization.arrayToDataTable([");

            sb.Append("['Página', 'Sessões'],");



            foreach (var item in resultado)
            {
                if (!item.Pagina.StartsWith("/?FROM="))
                    row.Append("['" + item.Pagina + "' ," + item.Sessao + "],");
            }

            sb.Append(row.ToString().Trim(','));

            sb.Append("]);");

            sb.Append("var options = {");
            sb.Append("title: ''");
            sb.Append("};");

            sb.Append("var chart = new google.visualization.PieChart(document.getElementById('chart_pagina'));");

            sb.Append("chart.draw(data, options);");
            sb.Append("}");
            sb.Append("</script>");

            sb.Append("<div id='chart_pagina' style='width: 100%; height:429px;'></div>");

            return sb.ToString();
        }

        public List<Home> CarregaPaginasAtivas(DataTable dadosAnalytics)
        {

            var lista = new List<Home>();
            var count = 0;

            var resultado = new List<Home>();

            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < 10 && i < dadosAnalytics.Rows.Count; i++)
                {
                    var mHome = new Home();

                    var retorno = dadosAnalytics.Rows[i][0].ToString().Split('.');
                    var pagina = retorno[0].ToString().ToUpper();
                    var nomePagina = (pagina.Replace("/", "") == string.Empty || pagina.Replace("/", "") == "DEFAULT") ? "HOME" : pagina;



                    mHome.Posicao = count;
                    mHome.Pagina = nomePagina;
                    mHome.Sessao = Convert.ToInt16(dadosAnalytics.Rows[i][1].ToString());

                    if (!nomePagina.StartsWith("/?FROM="))
                        lista.Add(mHome);
                }
            }

            resultado = lista.GroupBy(g => g.Pagina)
                                     .Select(i => new Home()
                                     {

                                         Pagina = i.Key,
                                         Sessao = i.Sum(s => s.Sessao)
                                     }).OrderByDescending(s => s.Sessao).ToList();

            foreach (var item in resultado)
            {
                count += 1;

                item.Posicao = count;
            }

            return resultado;
        }

        public string CarregaGraficoBrowser(DataTable dadosAnalytics)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();



            sb.Append("<script type='text/javascript' src='https://www.google.com/jsapi'></script>");
            sb.Append("<script type='text/javascript'>");
            sb.Append("google.load('visualization', '1', {packages:['corechart']});");
            sb.Append("google.setOnLoadCallback(drawChart);");
            sb.Append("function drawChart() {");
            sb.Append("var data = google.visualization.arrayToDataTable([");
            sb.Append("['Browser', 'Sess\u00f5es'],");


            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < dadosAnalytics.Rows.Count; i++)
                {
                    if (dadosAnalytics.Rows[i][0].ToString() == "(not set)")
                    {
                        row.Append("['não especificado' ," + dadosAnalytics.Rows[i][1] + "],");
                    }
                    else
                    {
                        row.Append("['" + dadosAnalytics.Rows[i][0].ToString() + "' ," + dadosAnalytics.Rows[i][1] + "],");
                    }
                }
            }


            sb.Append(row.ToString().Trim(','));

            sb.Append("]);");
            sb.Append("var options = {");

            sb.Append("title: '',");
            sb.Append("pieHole: 0.4,");
            sb.Append("};");

            sb.Append("var chart = new google.visualization.PieChart(document.getElementById('donutchart'));");
            sb.Append("chart.draw(data, options);");
            sb.Append("}");
            sb.Append("</script>");

            sb.Append("<div id='donutchart' style='width: 100%; height: 500px;'></div>");

            return sb.ToString();
        }

        public string CarregaGraficoSistemaOperacional(DataTable dadosAnalytics)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            sb.Append("<script type='text/javascript' src='https://www.google.com/jsapi'></script>");
            sb.Append("<script type='text/javascript'>");
            sb.Append("google.load('visualization', '1', {packages:['corechart']});");
            sb.Append("google.setOnLoadCallback(drawChart);");
            sb.Append("function drawChart() {");
            sb.Append("var data = google.visualization.arrayToDataTable([");
            sb.Append("['Sistema Operacional', 'Sess\u00f5es'],");

            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < dadosAnalytics.Rows.Count; i++)
                {
                    if (dadosAnalytics.Rows[i][0].ToString() == "(not set)")
                    {
                        row.Append("['não especificado' ," + dadosAnalytics.Rows[i][1] + "],");
                    }
                    else
                    {
                        row.Append("['" + dadosAnalytics.Rows[i][0].ToString() + "' ," + dadosAnalytics.Rows[i][1] + "],");
                    }
                }
            }

            sb.Append(row.ToString().Trim(','));

            sb.Append("]);");
            sb.Append("var options = {");

            sb.Append("title: '',");
            sb.Append("pieHole: 0.4,");
            sb.Append("};");

            sb.Append("var chart = new google.visualization.PieChart(document.getElementById('div_SistemaOperacional'));");
            sb.Append("chart.draw(data, options);");
            sb.Append("}");
            sb.Append("</script>");

            sb.Append("<div id='div_SistemaOperacional' style='width: 100%; height: 500px;'></div>");

            return sb.ToString();
        }

        public string CarregaGraficoPeriodoAcesso(DataTable dadosAnalytics)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            var listaPeriodo = new List<Periodo>();
            var resultado = new List<Periodo>();

            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < dadosAnalytics.Rows.Count; i++)
                {
                    var mPeriodo = new Periodo();

                    var hora = Convert.ToInt16(dadosAnalytics.Rows[i][0].ToString());
                    var intQuantidade = Convert.ToInt16(dadosAnalytics.Rows[i][1].ToString());

                    if (hora >= 00 && hora < 06)
                    {
                        mPeriodo.Tipo = "Madrugada";
                        mPeriodo.Quantidade = intQuantidade;
                    }
                    else if (hora >= 06 && hora < 12)
                    {
                        mPeriodo.Tipo = "Manhã";
                        mPeriodo.Quantidade = intQuantidade;
                    }
                    else if (hora >= 12 && hora < 18)
                    {
                        mPeriodo.Tipo = "Tarde";
                        mPeriodo.Quantidade = intQuantidade;
                    }
                    else
                    {
                        mPeriodo.Tipo = "Noite";
                        mPeriodo.Quantidade = intQuantidade;
                    }

                    listaPeriodo.Add(mPeriodo);
                }
            }

            resultado = listaPeriodo.GroupBy(g => g.Tipo)
                                     .Select(i => new Periodo()
                                     {
                                         Tipo = i.Key,
                                         Quantidade = i.Sum(s => s.Quantidade)
                                     }).OrderByDescending(s => s.Quantidade).ToList();


            sb.Append("<script type='text/javascript' src='https://www.google.com/jsapi'></script>");
            sb.Append("<script type='text/javascript'>");
            sb.Append("google.load('visualization', '1', {packages:['corechart']});");
            sb.Append("google.setOnLoadCallback(drawChart);");
            sb.Append("function drawChart() {");
            sb.Append("var data = google.visualization.arrayToDataTable([");
            sb.Append("['Periodo', 'Sess\u00f5es'],");

            foreach (var item in resultado)
            {
                row.Append("['" + item.Tipo.ToString() + "' ," + item.Quantidade + "],");
            }

            sb.Append(row.ToString().Trim(','));

            sb.Append("]);");
            sb.Append("var options = {");

            sb.Append("title: '',");
            sb.Append("pieHole: 0.4,");
            sb.Append("};");

            sb.Append("var chart = new google.visualization.PieChart(document.getElementById('div_PeriodoAcesso'));");
            sb.Append("chart.draw(data, options);");
            sb.Append("}");
            sb.Append("</script>");

            sb.Append("<div id='div_PeriodoAcesso' style='width: 100%; height: 500px;'></div>");

            return sb.ToString();
        }

        public string CarregaGrafico(DataTable dadosAnalytics)
        {

            var sVisitas = string.Empty;

            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            sb.Append("<script type='text/javascript' src='https://www.google.com/jsapi'></script>");
            sb.Append("<script type='text/javascript'>");

            sb.Append("var chart;");
            sb.Append("var data;");

            sb.Append("google.load('visualization', '1', { 'packages': ['corechart'] });");
            sb.Append("google.setOnLoadCallback(drawChart);");

            sb.Append("function drawChart() { ");

            sb.Append("data = new google.visualization.DataTable();");
            sb.Append("data.addColumn('string', 'Data');");
            sb.Append("data.addColumn('number', 'Sess\u00f5es');");
            sb.Append("data.addColumn('number', 'Vizualiza\u00e7\u00f5es');");

            sb.Append(" data.addRows([ ");

            if (dadosAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < dadosAnalytics.Rows.Count; i++)
                {
                    var sDate = dadosAnalytics.Rows[i][0].ToString();
                    sDate = sDate.Substring(6, 2);

                    row.Append("['" + sDate + "' ," + dadosAnalytics.Rows[i][1] + "," + dadosAnalytics.Rows[i][2] + "],");
                }
            }

            sb.Append(row.ToString().Trim(','));

            sb.Append("]);");

            sb.Append("chart = new google.visualization.LineChart(document.getElementById('chart_visitas'));");
            sb.Append("chart.draw(data, {height: 240, title: 'Vis\u00e3o Geral de Performance' });");
            sb.Append(" }");
            sb.Append("</script>");

            sb.Append("<div id='chart_visitas' style='width: 100%;'></div>");

            return sb.ToString();
        }
    }
}
