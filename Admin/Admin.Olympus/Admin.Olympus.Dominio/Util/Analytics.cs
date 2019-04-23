using Google.GData.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Admin.Olympus.Dominio.Util
{
    public class Analytics
    {
        private AnalyticsService _service = new AnalyticsService(WebConfigurationManager.AppSettings["_service"].ToString());
        private readonly string _feedURL = WebConfigurationManager.AppSettings["_feedURL"].ToString();
        private readonly string _usuario = WebConfigurationManager.AppSettings["_usuario"].ToString();
        private readonly string _senha = WebConfigurationManager.AppSettings["_senha"].ToString();
        private readonly string _ids = string.Empty;

        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }

        public Analytics(string _id)
        {
            _ids = _id;
            _service.setUserCredentials(_usuario, _senha);
        }

        public List<object[]> ObterDados(string dimensao, string metrica, string ordernarPor, int? limiteResultados = null)
        {

            var query = new DataQuery(_feedURL);
            
            query.Ids = _ids;
            query.GAStartDate = DataInicio.ToString("yyyy-MM-dd");
            query.GAEndDate = DataTermino.ToString("yyyy-MM-dd");

            query.Dimensions = dimensao;
            query.Metrics = metrica;
            query.Sort = ordernarPor;



            if (limiteResultados != null)
                query.ExtraParameters = "max-results=" + limiteResultados;

            var dataFeed = _service.Query(query);
            var resultados = new List<object[]>();

            foreach (DataEntry item in dataFeed.Entries)
            {
                var intCount = metrica.Split(',').Count();

                var resultado = new object[intCount + 1];

                resultado[0] = item.Dimensions[0].Value;

                for (int i = 1; i < resultado.Count(); i++)
                {
                    resultado[i] = item.Metrics[i - 1].Value;
                }

                resultados.Add(resultado);
            }

            return resultados;
        }
    }
}
