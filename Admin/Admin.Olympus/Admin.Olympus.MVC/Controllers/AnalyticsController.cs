using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Repositorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Admin.Olympus.MVC.Controllers
{
    public class AnalyticsController : Controller
    {
        public async Task<ActionResult> Index(DateTime? de, DateTime? ate)
        {
            if (Session["Administrador"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var usuario = (Administrador)Session["Administrador"];

            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            RepositorioAnalytics objAnalytics = new RepositorioAnalytics(usuario.idTipo);

            var dtInicial = WebConfigurationManager.AppSettings["dtInicial"].ToString();
            DateTime iniAux = DateTime.Now;
            DateTime.TryParse(dtInicial, out iniAux);

            if (de == null || de.Value < iniAux)
                de = iniAux;

            if (ate == null || ate > DateTime.Now)
                ate = DateTime.Now;

            if (ate < de || ate < iniAux)
                ate = de;

            var idProjeto = string.Empty;

            if (usuario.idTipo == 1)
                idProjeto = WebConfigurationManager.AppSettings["idProjetoOBL"].ToString();
            else if (usuario.idTipo == 2)
                idProjeto = WebConfigurationManager.AppSettings["idProjetoOLA"].ToString();
            else if (usuario.idTipo == 3)
                idProjeto = WebConfigurationManager.AppSettings["idProjetoOMS"].ToString();

            Analytics analytics = new Analytics();

            analytics.idProjeto = idProjeto;
            analytics.dtInicial = de.Value.ToString("yyyy-MM-dd");
            analytics.dtFinal = ate.Value.ToString("yyyy-MM-dd");

            ViewBag.filtroDataDe = de.Value.ToString("dd/MM/yyyy");
            ViewBag.filtroDataAte = ate.Value.ToString("dd/MM/yyyy");

            /* Quantidade de Sessao */
            analytics.dimensoes = "ga:userType";
            analytics.metricas = "ga:sessions";
            analytics.ordernacao = "-ga:sessions";

            var quantidadeSesso = await AnalyticsAsync(analytics);

            ViewBag.quantidadeSesso = ResultadoAnalytics(quantidadeSesso);

            /*Quantidade de Vizualização*/
            analytics.metricas = "ga:pageviews";
            analytics.ordernacao = "-ga:pageviews";

            var quantidadeVizualizacao = await AnalyticsAsync(analytics);

            ViewBag.quantidadeVizualizacao = ResultadoAnalytics(quantidadeVizualizacao);

            /* Quantidade de Pagina/Sessao */
            analytics.dimensoes = "ga:userDefinedValue";
            analytics.metricas = "ga:pageviewsPerSession";
            analytics.ordernacao = "-ga:pageviewsPerSession";

            var paginaSessao = await AnalyticsAsync(analytics);

            ViewBag.paginaSessao = ResultadoAnalyticsPaginaSessao(paginaSessao); //RetornaDecimal(ResultadoAnalyticsPaginaSessao(paginaSessao));

            /*Duração de Sessão*/
            analytics.dimensoes = "ga:userDefinedValue";
            analytics.metricas = "ga:avgSessionDuration";
            analytics.ordernacao = "ga:avgSessionDuration";

            var duracaoSessao = await AnalyticsAsync(analytics);

            ViewBag.duracaoSessao = RetornaDuracaoMediaSessao(duracaoSessao);

            /*Carrega Grafico Visitante*/

            analytics.dimensoes = "ga:userType";
            analytics.metricas = "ga:pageviews";
            analytics.ordernacao = "ga:userType";

            var graficoVisitante = await AnalyticsAsync(analytics);


            /* Graficos */
            ViewBag.graficoVisitante = objAnalytics.CarregaGraficoVisitante(graficoVisitante);

            analytics.dimensoes = "ga:city";
            analytics.metricas = "ga:sessions";
            analytics.ordernacao = "-ga:sessions";


            var cidadesAtivas = await AnalyticsAsync(analytics);

            ViewBag.cidadesAtivasGrafico = objAnalytics.CarregaGraficoCidade(cidadesAtivas);

            ViewBag.cidadesAtivasTabela = objAnalytics.CarregaCidadesAtivas(cidadesAtivas);


            analytics.dimensoes = "ga:pagePath";

            var pagina = await AnalyticsAsync(analytics);

            var graficoPagina = objAnalytics.CarregaGraficoPagina(pagina);
            ViewBag.paginasVisitadasGrafico = graficoPagina;

            ViewBag.paginasVisitadasTabela = objAnalytics.CarregaPaginasAtivas(pagina);


            //analytics.dimensoes = "ga:browser";


            //var browser = await AnalyticsAsync(analytics);

            //ViewBag.browser = objAnalytics.CarregaGraficoBrowser(browser);

            //analytics.dimensoes = "ga:operatingSystem";


            //var sistemaOperacional = await AnalyticsAsync(analytics);

            //ViewBag.sistemaOperacional = objAnalytics.CarregaGraficoSistemaOperacional(sistemaOperacional);

            //analytics.dimensoes = "ga:hour";

            //var periodoAcesso = await AnalyticsAsync(analytics);

            //ViewBag.periodoGrafico = objAnalytics.CarregaGraficoPeriodoAcesso(periodoAcesso);


            analytics.dimensoes = "ga:date";
            analytics.metricas = "ga:sessions,ga:pageviews";
            analytics.ordernacao = "ga:date";

            var performance = await AnalyticsAsync(analytics);

            ViewBag.performance = objAnalytics.CarregaGrafico(performance);

            return View();
        }

        public string RetornaDecimal(string value)
        {
            if (value.Contains("."))
            {
                value += "00";
            }
            else
            {
                value += ".00";
            }
            return value.Substring(0, value.IndexOf(".") + 3);
        }

        static async Task<DataTable> AnalyticsAsync(Analytics analytics)
        {
            var resultado = new DataTable();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://ga.fullbarhomologa.com.br/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = await client.GetAsync(string.Format("api/Analytics?idProjeto={0}&dtInicial={1}&dtFinal={2}&dimensoes={3}&metricas={4}&ordenacao={5}"
                                                                                                  , analytics.idProjeto
                                                                                                  , analytics.dtInicial
                                                                                                  , analytics.dtFinal
                                                                                                  , analytics.dimensoes
                                                                                                  , analytics.metricas
                                                                                                  , analytics.ordernacao));

                if (response.IsSuccessStatusCode)
                {
                    resultado = await response.Content.ReadAsAsync<DataTable>();
                }
            }

            return resultado;
        }

        public string ResultadoAnalytics(DataTable resultadoAnalytics)
        {
            var count = 0;

            if (resultadoAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < resultadoAnalytics.Rows.Count; i++)
                {
                    count += (i == 0) ? Convert.ToInt32(resultadoAnalytics.Rows[i][1]) : Convert.ToInt32(resultadoAnalytics.Rows[i][i]);
                }
                return count.ToString();
            }
            else
                return "0";
        }

        public string ResultadoAnalyticsPaginaSessao(DataTable resultadoAnalytics)
        {
            var sessao = "";
            if (resultadoAnalytics.Rows.Count > 0) // Incluso
            {
                for (int i = 0; i < resultadoAnalytics.Rows.Count; i++)
                {
                    sessao = resultadoAnalytics.Rows[i][1].ToString();
                }

                return RetornaDecimal(sessao);
            }
            else
                return "0";
        }

        public string RetornaDuracaoMediaSessao(DataTable resultadoAnalytics)
        {
            var retorno = string.Empty;

            if (resultadoAnalytics.Rows.Count > 0) // Incluso
            {
                var segundos = resultadoAnalytics.Rows[0][1].ToString().Split('.');

                int hor = (int)(Convert.ToInt16(segundos[0].ToString()) / (60 * 60));

                int min = (int)((Convert.ToInt16(segundos[0].ToString()) - (hor * 60 * 60)) / 60);

                int seg = (int)(Convert.ToInt16(segundos[0].ToString()) - (hor * 60 * 60) - (min * 60));

                retorno = String.Format("{0}:{1}:{2}", hor.ToString().PadLeft(2, '0'), min.ToString().PadLeft(2, '0'), seg.ToString().PadLeft(2, '0'));

                return retorno;
            }
            else
                return "0";
        }
	}
}