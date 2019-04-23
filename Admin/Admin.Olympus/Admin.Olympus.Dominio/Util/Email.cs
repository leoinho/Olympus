using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Admin.Olympus.Dominio.Util
{
    public class Email
    {
        private string destinatario;

        public string Destinatario
        {
            get { return destinatario; }
            set { destinatario = value; }
        }

        private bool erro;
        private string mensagem;

        public bool Erro
        {
            get { return erro; }
            set { erro = value; }
        }

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        public Email()
        {

        }
        public Email(string destinatario)
        {
            this.Destinatario = destinatario;
        }


        public bool EnviarEmailCertificado(int idIdioma, string email)
        {
            bool retorno = false;

            var htmlEmailCertificado = string.Empty;
            var assuntoEmailCertificado = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();

            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();

            switch (idIdioma)
            {
                case 1:
                    htmlEmailCertificado = WebConfigurationManager.AppSettings["htmlCertificadoPtBr"].ToString();
                    assuntoEmailCertificado = WebConfigurationManager.AppSettings["assuntoCertificadoPtBr"].ToString();
                    break;

                case 2:
                    htmlEmailCertificado = WebConfigurationManager.AppSettings["htmlCertificadoEn"].ToString();
                    assuntoEmailCertificado = WebConfigurationManager.AppSettings["assuntoCertificadoEn"].ToString();
                    break;

                case 3:
                    htmlEmailCertificado = WebConfigurationManager.AppSettings["htmlCertificadoEs"].ToString();
                    assuntoEmailCertificado = WebConfigurationManager.AppSettings["assuntoCertificadoEs"].ToString();
                    break;
            }

            htmlEmailCertificado = System.IO.File.ReadAllText(htmlEmailCertificado);


            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, email, assuntoEmailCertificado, htmlEmailCertificado);
                    msg.IsBodyHtml = true;

                    client.Send(msg);
                    retorno = true;

                }
                catch (Exception ex)
                {
                }
            }
            return retorno;
        }


    }
}
