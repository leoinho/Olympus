using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Repositorio;
using Admin.Olympus.Dominio.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Admin.Olympus.MVC.Controllers
{
    public class DashboardController : Controller
    {
        public RepositorioDashboard _dashboard = new RepositorioDashboard();

        public ActionResult Index()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            var dashboard = _dashboard.Resumo(idIdioma);

            //Verifica perfil
            if (usuario.idTipo == 1)
            {
                //OBL
                ViewBag.CadastroSite = _dashboard.RelatorioCadastroSite();
                ViewBag.InscricaoSite = _dashboard.RelatorioInscricaoSite();

                ViewBag.Mes = DdlMes(null);
                ViewBag.inMes = System.DateTime.Now.Month;
                ViewBag.GraficoTreinamentoInscricao = _dashboard.GraficoTreinamentoInscricao(System.DateTime.Now.Month);
            }
            else
            {
                //OLA e OMS
                ViewBag.GraficoCliquesTreinamento = GraficoCliquesTreinamento(_dashboard.GraficoTreinamentoExterno(idIdioma));
            }

            ViewBag.TodosUsuarios = dashboard.FirstOrDefault(x => x.inTipo == 1).inQtd;
            ViewBag.UsuariosPorSite = dashboard.FirstOrDefault(x => x.inTipo == 2).inQtd;
            ViewBag.InscritosPorSite = dashboard.FirstOrDefault(x => x.inTipo == 3).inQtd;

            ViewBag.GraficoAcessosDiarios = GraficoAcessosDiarios(_dashboard.GraficoAcessosDiarios(idIdioma));

            return View();
        }
        public ActionResult GraficoTreinamentoInscricaoMes(int inMes)
        {
            ViewBag.Mes = DdlMes(inMes);
            ViewBag.inMes = inMes;

            var listInscricao = _dashboard.GraficoTreinamentoInscricao(inMes);

            return PartialView("_GraficoTreinamentoInscricao", listInscricao);
        }
        
        public string GraficoCliquesTreinamento(List<Dashboard> lista)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            sb.Append("<script type='text/javascript'>");
            sb.Append("google.setOnLoadCallback(drawGraficoTreinamentoInscricao);");
            sb.Append("function drawGraficoTreinamentoInscricao() {");
            sb.Append("var data = google.visualization.arrayToDataTable([");
            sb.Append("['" + Geral.RetornaTexto("DashboardGraficoCliquesTreinamentoColunaCliques") + "', '" + Geral.RetornaTexto("DashboardGraficoCliquesTreinamentoColunaCliques") + "'],");

            foreach (var item in lista)
            {
                row.Append("['" + item.dsCodigoTreinamento + "', " + item.inQtd + "],");
            }

            sb.Append(row);
            sb.Append("]);");
            sb.Append(@"var options = {
                    title: '',
                    hAxis: {
                      title: '" + Geral.RetornaTexto("DashboardGraficoCliquesTreinamentoEixoX") + @"'
                    },
                    vAxis: {
                      title: '" + Geral.RetornaTexto("DashboardGraficoCliquesTreinamentoEixoY") + @"'
                    }
                  };");

            sb.Append(" var chart = new google.visualization.ColumnChart(document.getElementById('GraficoTreinamentoInscricao'));");

            sb.Append("chart.draw(data, options);");
            sb.Append(" }");
            sb.Append("</script>");
            sb.Append("<div id='GraficoTreinamentoInscricao' style='width: 100%; height: 429px;'></div>");

            return sb.ToString();
        }

        public string GraficoAcessosDiarios(List<Dashboard> acessos)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder row = new StringBuilder();

            sb.Append("<script type='text/javascript'>");
            sb.Append("google.setOnLoadCallback(drawGraficoAcessoDiario);");
            sb.Append("function drawGraficoAcessoDiario() {");
            sb.Append("var data = google.visualization.arrayToDataTable([");
            //sb.Append("['" + Geral.RetornaTexto("DashboardGraficoAcessoDiarioColunaData") + "', '" + Geral.RetornaTexto("DashboardGraficoAcessoDiarioColunaAcesso") + "'],");
            sb.Append("['" + Geral.RetornaTexto("DashboardGraficoAcessoDiarioColunaData") + "', '" + Geral.RetornaTexto("DashboardGraficoAcessoDiarioColunaAcesso") + "', { role: 'annotation' }],");

            if (acessos != null && acessos.Count > 0)
            {
                foreach (var item in acessos)
                {
                    row.Append(" ['" + item.dsData + "' ," + item.inQtd.ToString() + " , '" + item.inQtd.ToString() + "'],");
                    //row.Append(" ['" + DateTime.Now.ToShortDateString() + "' ," + item.inQtd.ToString() + " , '" + item.inQtd.ToString() + "'],");
                    //row.Append(" ['" + item.dsData  + "' ," + item.inQtd + "],");
                }
            }
            else
            {
                row.Append(" ['-' , 0],");
            }

            sb.Append(row.ToString().Trim(','));
            sb.Append("]);");

            sb.Append("var options = {");
            sb.Append("title: '',");
            sb.Append("annotations: {");
            sb.Append("     textStyle: {");
            sb.Append("         color: '#000',");
            sb.Append("         auraColor: 'none'");
            sb.Append("     }");
            sb.Append("},");
            //sb.Append("isStacked: true,");
            sb.Append("bar: { groupWidth: '70%' },");
            sb.Append("legend: { position: 'none' },");
            sb.Append("hAxis: { title: '' },");
            sb.Append("vAxis: { title: 'Acessos' },");
            //sb.Append("pieHole: 0.4,");
            sb.Append(" };");

            sb.Append("var chart = new google.visualization.LineChart(document.getElementById('AcessoDiario'));");
            sb.Append("  chart.draw(data, options);");
            sb.Append("}");
            sb.Append("</script>");
            sb.Append(" <div id='AcessoDiario' style='width: 100%; height: 429px;'></div>");


            return sb.ToString();
        }

        public SelectList DdlMes(int? inMes)
        {
            var listMes = new List<Dashboard>();
            listMes.Add(new Dashboard { inMes = 1, dsMes = "Janeiro" });
            listMes.Add(new Dashboard { inMes = 2, dsMes = "Fevereiro" });
            listMes.Add(new Dashboard { inMes = 3, dsMes = "Março" });
            listMes.Add(new Dashboard { inMes = 4, dsMes = "Abril" });
            listMes.Add(new Dashboard { inMes = 5, dsMes = "Maio" });
            listMes.Add(new Dashboard { inMes = 6, dsMes = "Junho" });
            listMes.Add(new Dashboard { inMes = 7, dsMes = "Julho" });
            listMes.Add(new Dashboard { inMes = 8, dsMes = "Agosto" });
            listMes.Add(new Dashboard { inMes = 9, dsMes = "Setembro" });
            listMes.Add(new Dashboard { inMes = 10, dsMes = "Outubro" });
            listMes.Add(new Dashboard { inMes = 11, dsMes = "Novembro" });
            listMes.Add(new Dashboard { inMes = 12, dsMes = "Dezembro" });

            return new SelectList(listMes, "inMes", "dsMes", (inMes == null) ? System.DateTime.Now.Month : inMes);
        }
	}
}