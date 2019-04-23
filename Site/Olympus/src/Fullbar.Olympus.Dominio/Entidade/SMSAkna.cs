using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml.Linq;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public static class SMSAkna
    {
        public static RetornoSMS EnvioConfirmacao(string _url, string _user, string _pass, string celular, string remetente, string msg)
        {
            //var url = WebConfigurationManager.AppSettings["UrlAkna"];
            //var user = WebConfigurationManager.AppSettings["User"];
            //var pass = WebConfigurationManager.AppSettings["Pass"];

            var url = _url;
            var user = _user;
            var pass = _pass;

            var mensagem = msg;

            var client = new RestClient(url);

            var request = new RestRequest(Method.POST);

            var xml = EncondingUtf8(String.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                    "<main>" +
                                    "<emkt trans=\"40.01\">" +
                                    "<remetente>OLYMPUS</remetente>" +
                                    "<sms>" +
                                    "<telefone>{0}</telefone>" +
                                    "<mensagem>{1}</mensagem>" +
                                    "</sms>" +
                                    "</emkt>" +
                                    "</main>", celular, mensagem));


            var postData = String.Format("User={0}&Pass={1}&XML={2}", user, pass, xml);
            request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);

            var response = client.Execute(request).Content;
            var objRetorno = XDocument.Parse(response).Descendants("RETURN").FirstOrDefault();
            var id = (objRetorno != null) ? objRetorno.Attribute("ID").Value : String.Empty;
            var codigo = (objRetorno != null) ? objRetorno.Value : String.Empty;

            return new RetornoSMS { Id = id, Codigo = codigo, Texto = mensagem, Xml = xml };

        }

        public static StatusEnvioSMS VerificaEnvio(string _url, string _user, string _pass, string codigoEnvio)
        {
            var url = _url;
            var user = _user;
            var pass = _pass;

            var client = new RestClient(url);

            var request = new RestRequest(Method.POST);

            var xml = EncondingUtf8(String.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                    "<main>" +
                                    "<emkt trans=\"40.02\">" +
                                    "<sms>" +
                                    "<codigo>{0}</codigo>" +
                                    "</sms>" +
                                    "</emkt>" +
                                    "</main>", codigoEnvio));


            var postData = String.Format("User={0}&Pass={1}&XML={2}", user, pass, xml);
            request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);

            var response = client.Execute(request).Content;

            var objTelefone = XDocument.Parse(response).Descendants("TELEFONE").FirstOrDefault();
            var telefone = (objTelefone != null) ? objTelefone.Value : String.Empty;

            var objStatus = XDocument.Parse(response).Descendants("STATUS").FirstOrDefault();
            var status = (objStatus != null) ? objStatus.Value.ToUpper() : String.Empty;

            return new StatusEnvioSMS { Telefone = telefone, Status = status };

        }
               
        public static string EncondingUtf8(string texto)
        {
            var enc = new UTF8Encoding(true, true);
            var bytes = enc.GetBytes(texto);
            return enc.GetString(bytes);
        }

        public static string EncurtadorUrl(string url)
        {
            var key = WebConfigurationManager.AppSettings["EncurtadorUrl"];
            var post = "{\"longUrl\": \"" + url + "\"}";
            var shortUrl = url;
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + key);

            try
            {
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentLength = post.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Cache-Control", "no-cache");

                using (var requestStream = request.GetRequestStream())
                {
                    var postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            var json = responseReader.ReadToEnd();
                            shortUrl = Regex.Match(json, @"""id"": ?""(?<id>.+)""").Groups["id"].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // if Google's URL Shortner is down...
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return shortUrl;
        }
    
    }
}
