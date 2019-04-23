using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.Dominio.Repositorio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Fullbar.Olympus.ServicoSMS
{
    class Program
    {

        static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            kernel.Bind<ISMS>().To<RepositorioSMS>();
            var appSMS = kernel.Get<ISMS>();

            var listaSMSPendente = appSMS.Lista();

            string url = "http://api.akna.com.br/emkt/int/integracao.php";
            string user = "integracaosms.olympus@fullbar.com.br";
            //string senha = "fullbar!@#";
            string senha = "0d45b007761536289420d9b791b7fdc7";
            
            int statusValida = 4;

            foreach (var item in listaSMSPendente.Where(x => x.dsTelefone != string.Empty))
            {
                RetornoSMS retorno = new RetornoSMS();
                var celular = item.dsTelefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").TrimEnd().TrimStart().Trim();
                var msg = "Sua presença no treinamento Olympus foi confirmada. Apresente a ficha de inscrição enviada para seu e-mail no dia do evento.";
                var remetente = "OLYMPUS";

                retorno = SMSAkna.EnvioConfirmacao(url, user, senha, celular, remetente, msg);

                appSMS.AtualizaStatus(item.idAgendamento, statusValida, retorno.Xml, retorno.Codigo);

            }


            var listaSMSValida = appSMS.ValidacaoLista();

            foreach (var item in listaSMSValida)
            {
                var idStatus = 0;
                var statusEnvio = new StatusEnvioSMS();
                statusEnvio = SMSAkna.VerificaEnvio(url, user, senha, item.dsCodigo);
                switch(statusEnvio.Status.Replace(" ", ""))
                {
                    case "MENSAGEMENVIADA":
                    case "MENSAGEMENVIADASEMCONFIRMAÇÃO":
                        idStatus = (int)StatusEnvioSMS.StatusEnvio.Enviado;
                        break;
                    case "AGUARDANDORETORNODAOPERADORA":
                        idStatus = (int)StatusEnvioSMS.StatusEnvio.Validando;
                        break;
                    default:
                        idStatus = (int)StatusEnvioSMS.StatusEnvio.ErroAoEnviar;
                        break;
                }

                appSMS.AtualizaStatusValidacao(item.idLog, idStatus);

            }

            //foreach (var item in listaSMSPendente.Where(x => x.dsTelefone != string.Empty))
            //{

            //    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api-rest.zenvia360.com.br/services/send-sms");
            //    var usuario = "full.olympus";
            //    var senha = "uVymk2UiFS";
            //    var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(usuario + ":" + senha));

            //    httpWebRequest.Accept = "application/json";
            //    httpWebRequest.ContentType = "application/json";
            //    httpWebRequest.Method = "POST";
            //    httpWebRequest.Headers.Add("Authorization", "Basic " + encoded);

            //    var celular = item.dsTelefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").TrimEnd().TrimStart().Trim();

            //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //    {
            //        var json = "{ \"sendSmsRequest\": { \"from\":\"Olympus Educação\", \"to\":\"55" + celular + "\" , \"msg\":\"Sua presença no treinamento Olympus foi confirmada. Apresente a ficha de inscrição enviada para seu e-mail no dia do evento.\", \"aggregateId\":\"13423\"}}";

            //        streamWriter.Write(json);
            //        streamWriter.Flush();
            //        streamWriter.Close();
            //    }

            //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //    var result = string.Empty;

            //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //    {
            //        result = streamReader.ReadToEnd();
            //    }


            //    var retornoDeserilize = JsonConvert.DeserializeObject<dynamic>(result);

            //    var retornoRequest = new sendSmsResponse()
            //    {
            //        detailCode = retornoDeserilize.sendSmsResponse.detailCode.Value,
            //        detailDescription = retornoDeserilize.sendSmsResponse.detailDescription.Value,
            //        statusCode = retornoDeserilize.sendSmsResponse.statusCode.Value,
            //        statusDescription = retornoDeserilize.sendSmsResponse.statusDescription.Value

            //    };

            //    var idStatus = (retornoRequest.detailCode == "000") ? 2 : 3;
            //    var xml = GetXML(retornoRequest);

            //    appSMS.AtualizaStatus(item.idAgendamento, idStatus, xml);
            //}

        }

        //public static string GetXML(object o)
        //{
        //    StringWriter sw = new StringWriter();
        //    XmlTextWriter tw = new XmlTextWriter(sw);

        //    try
        //    {
        //        XmlSerializer serializer = new XmlSerializer(o.GetType());
        //        serializer.Serialize(tw, o);
        //        return sw.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Handle Exception Code
        //    }
        //    return null;
        //}
    }
}
