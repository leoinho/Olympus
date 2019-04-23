using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Fullbar.Olympus.Dominio.Util
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

        public bool EnviarSenha(string Email, string senha, int idIdioma , string nome)
        {
            bool retorno = false;

            var htmlEsqueciSenha = string.Empty;
            var assuntoEsqueciSenha = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();

            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();

            switch (idIdioma)
            {
                case 1:
                    htmlEsqueciSenha = WebConfigurationManager.AppSettings["htmlEsqueciSenha"].ToString();
                    assuntoEsqueciSenha = WebConfigurationManager.AppSettings["assuntoEsqueciSenha"].ToString();
                    break;

                case 2:
                    htmlEsqueciSenha = WebConfigurationManager.AppSettings["htmlEsqueciSenhaEn"].ToString();
                    assuntoEsqueciSenha = WebConfigurationManager.AppSettings["assuntoEsqueciSenhaEn"].ToString();
                    break;

                case 3:
                    htmlEsqueciSenha = WebConfigurationManager.AppSettings["htmlEsqueciSenhaEs"].ToString();
                    assuntoEsqueciSenha = WebConfigurationManager.AppSettings["assuntoEsqueciSenhaEs"].ToString();
                    break;
            }

            var Senha = Crypto.Decrypt(senha, Crypto.Key256, 256);

            htmlEsqueciSenha = System.IO.File.ReadAllText(htmlEsqueciSenha);

            htmlEsqueciSenha = htmlEsqueciSenha.Replace("#Email#", Email).Replace("#SENHA#", Senha).Replace("#NOME#",nome).ToString();


            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, Email, assuntoEsqueciSenha, htmlEsqueciSenha);
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

        public bool EnviarEmailConfirmacaoInscricao(Entidade.Email email, List<Entidade.Email> listaConvidados)
        {
            bool retorno = false;

            var html = string.Empty;
            var assunto = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();
            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();
            var indicadosTitulo = string.Empty;
            var indicadosConfirmado = string.Empty;
            var indicadosListaEspera = string.Empty;

            var indicadosTextoLista = string.Empty;

            switch (email.idIdioma)
            {
                case 1:
                    html = WebConfigurationManager.AppSettings["htmlConfirmaInscricaoPtBr"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoConfirmaInscricaoPtBr"].ToString();
                    indicadosTitulo = "Indicados";
                    indicadosConfirmado = "Confirmado";
                    indicadosListaEspera = "Em espera";
                    indicadosTextoLista = "Outras pessoas que indiquei";
                    break;

                case 2:
                    html = WebConfigurationManager.AppSettings["htmlConfirmaInscricaoEn"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoConfirmaInscricaoEn"].ToString();
                    indicadosTitulo = "Nominations";
                    indicadosConfirmado = "Registered";
                    indicadosListaEspera = "Pending";
                    indicadosTextoLista = "Other persons I've subscribed";
                    break;

                case 3:
                    html = WebConfigurationManager.AppSettings["htmlConfirmaInscricaoEs"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoConfirmaInscricaoEs"].ToString();
                    indicadosTitulo = "Indicados";
                    indicadosConfirmado = "Confirmado";
                    indicadosListaEspera = "En espera";
                    indicadosTextoLista = "Otras personas que inscribí";
                    break;
                default:
                    break;
            }
            
            html = System.IO.File.ReadAllText(html);

            html = html.Replace("#NOME#", email.dsNomeCompleto).
                Replace("#NOMETREINAMENTO#", email.dsNomeTreinamento).
                Replace("#CPF#", email.dsCPF).
                Replace("#CNPJ#", email.dsCNPJ).
                Replace("#DATATREINAMENTO#", email.dtTreinamento.ToString("dd/MM/yy")).
                Replace("#HORATREINAMENTO#", email.dtTreinamento.ToString("HH:mm")).
                Replace("#LOCAL#", email.dsLocalTreinamento).
                Replace("#ENDERECO#", email.dsEndereco).
                Replace("#NUMERO#", email.dsNumero).
                Replace("#BAIRRO#", email.dsBairro).
                Replace("#CIDADE#", email.dsCidade).
                Replace("#UF#", email.dsUF).ToString().
                Replace("#LONGITUDE#", email.dsLongitude).ToString().
                Replace("#LATITUDE#", email.dsLatitude).ToString();


            //Verifica se tem INDICADOS
            if (listaConvidados != null && listaConvidados.Count > 0)
            {
                //MONTA HTML
                StringBuilder strIndicado = new StringBuilder();         

                 strIndicado.Append("<hr />");
                 strIndicado.Append("       <br />");
                 strIndicado.Append("       <p>");
                 strIndicado.Append("       	<strong>" + indicadosTextoLista + "</strong>");
                 strIndicado.Append("       </p>");
                 strIndicado.Append("       <p>");
                 strIndicado.Append("       	<table width=\"90%\">");


                foreach (var item in listaConvidados)
                {
                    string dsCor = "#5FAEDD";
                    string dsStatus = indicadosConfirmado;

                    if (item.idStatus == 3 || item.idStatus == 4)
                    {
                        dsCor = "#E8732F";
                        dsStatus = indicadosListaEspera;
                    }

                    strIndicado.Append("<tr>");
                    strIndicado.Append("    <td>");
                    strIndicado.Append(item.dsNomeCompleto);
                    strIndicado.Append("    </td>");
                    strIndicado.Append("    <td>");
                    strIndicado.Append("        <font color=" + dsCor + ">" + dsStatus + "</font>");
                    strIndicado.Append("    </td>");
                    strIndicado.Append("</tr>");
                }


                strIndicado.Append("       	</table>");
                strIndicado.Append("       </p>");
                strIndicado.Append("       <br />");

                html = html.Replace("#INDICADOS#", strIndicado.ToString()).Replace("#mostraIndicados#", "block"); ;
            }
            else
            {
                html = html.Replace("#INDICADOS#", string.Empty);
            }
            

            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, email.dsEmail, assunto, html);
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

        public bool EnviarEmailConfirmacaoInscricaoConvidado(Entidade.Email email)
        {
            bool retorno = false;

            var html = string.Empty;
            var assunto = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();
            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();

            switch (email.idIdioma)
            {
                case 1:
                    html = WebConfigurationManager.AppSettings["htmlConfirmaInscricaoConvidadoPtBr"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoConfirmaInscricaoConvidadoPtBr"].ToString();
                    break;

                case 2:
                    html = WebConfigurationManager.AppSettings["htmlConfirmaInscricaoConvidadoEn"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoConfirmaInscricaoConvidadoEn"].ToString();
                    break;

                case 3:
                    html = WebConfigurationManager.AppSettings["htmlConfirmaInscricaoConvidadoEs"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoConfirmaInscricaoConvidadoEs"].ToString();
                    break;
                default:
                    break;
            }
            
            html = System.IO.File.ReadAllText(html);

            html = html.Replace("#NOME#", email.dsNomeCompleto).
                Replace("#NOMECONVIDANTE#", email.dsNomeConvidante).
                Replace("#NOMETREINAMENTO#", email.dsNomeTreinamento).
                Replace("#CPF#", email.dsCPF).
                Replace("#CNPJ#", email.dsCNPJ).
                Replace("#DATATREINAMENTO#", email.dtTreinamento.ToString("dd/MM/yy")).
                Replace("#HORATREINAMENTO#", email.dtTreinamento.ToString("HH:mm")).
                Replace("#LOCAL#", email.dsLocalTreinamento).
                Replace("#ENDERECO#", email.dsEndereco).
                Replace("#NUMERO#", email.dsNumero).
                Replace("#BAIRRO#", email.dsBairro).
                Replace("#CIDADE#", email.dsCidade).
                Replace("#UF#", email.dsUF).ToString().
                Replace("#LONGITUDE#", email.dsLongitude).ToString().
                Replace("#LATITUDE#", email.dsLatitude).ToString();

            
            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, email.dsEmail, assunto, html);
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

        public bool EnviarEmailListaEspera(Entidade.Email email)
        {
            bool retorno = false;

            var html = string.Empty;
            var assunto = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();
            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();


            switch (email.idIdioma)
            {
                case 1:
                    html = WebConfigurationManager.AppSettings["htmlListaEsperaPtBr"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoListaEsperaPtBr"].ToString();
                    break;

                case 2:
                    html = WebConfigurationManager.AppSettings["htmlListaEsperaEn"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoListaEsperaEn"].ToString();
                    break;

                case 3:
                    html = WebConfigurationManager.AppSettings["htmlListaEsperaEs"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoListaEsperaEs"].ToString();
                    break;
                default:
                    break;
            }

            html = System.IO.File.ReadAllText(html);

            html = html.Replace("#NOME#", email.dsNomeCompleto).
                Replace("#CPF#", email.dsCPF).
                Replace("#CNPJ#", email.dsCNPJ).ToString();

            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, email.dsEmail, assunto, html);
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

        public bool EnviarEmailCancelamento(Entidade.Email email)
        {
            bool retorno = false;

            var html = string.Empty;
            var assunto = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();
            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();


            switch (email.idIdioma)
            {
                case 1:
                    html = WebConfigurationManager.AppSettings["htmlCancelamentoPtBr"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoCancelamentoPtBr"].ToString();
                    break;

                case 2:
                    html = WebConfigurationManager.AppSettings["htmlCancelamentoEn"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoCancelamentoEn"].ToString();
                    break;

                case 3:
                    html = WebConfigurationManager.AppSettings["htmlCancelamentoEs"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoCancelamentoEs"].ToString();
                    break;
                default:
                    break;
            }

            html = System.IO.File.ReadAllText(html);

            html = html.
                Replace("#NOME#", email.dsNomeCompleto).
                Replace("#TREINAMENTO#", email.dsNomeTreinamento).
                Replace("#DATA#", email.dsDataTreinamento).ToString().
                Replace("#CIDADE#", email.dsCidade).ToString();

            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, email.dsEmail, assunto, html);
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


        public bool EnviarEmailTreinamentoCancelado(Entidade.Email email)
        {
            bool retorno = false;

            var html = string.Empty;
            var assunto = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();
            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();


            switch (email.idIdioma)
            {
                case 1:
                    html = WebConfigurationManager.AppSettings["htmlTreinamentoCanceladoPtBr"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoTreinamentoCanceladoPtBr"].ToString();
                    break;

                case 2:
                    html = WebConfigurationManager.AppSettings["htmlTreinamentoCanceladoEn"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoTreinamentoCanceladoEn"].ToString();
                    break;

                case 3:
                    html = WebConfigurationManager.AppSettings["htmlTreinamentoCanceladoEs"].ToString();
                    assunto = WebConfigurationManager.AppSettings["assuntoTreinamentoCanceladoEs"].ToString();
                    break;
                default:
                    break;
            }

            html = System.IO.File.ReadAllText(html);

            html = html.
                Replace("#NOME#", email.dsNomeCompleto).
                Replace("#NOMETREINAMENTO#", email.dsNomeTreinamento).
                Replace("#DATATREINAMENTO#", email.dsDataTreinamento).ToString();

            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, email.dsEmail, assunto, html);
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


        public bool EnviarFaleConosco(FaleConosco dados, int idIdioma)
        {
            bool retorno = false;

            var htmlFaleConosco = string.Empty;
            var assuntoFaleConosco = string.Empty;

            switch (idIdioma)
            {
                case 1:
                    htmlFaleConosco = WebConfigurationManager.AppSettings["htmlFaleConosco"].ToString();
                    assuntoFaleConosco = WebConfigurationManager.AppSettings["assuntoFaleConosco"].ToString();
                    break;

                case 2:
                    htmlFaleConosco = WebConfigurationManager.AppSettings["htmlFaleConoscoEn"].ToString();
                    assuntoFaleConosco = WebConfigurationManager.AppSettings["assuntoFaleConoscoEn"].ToString();
                    break;

                case 3:
                    htmlFaleConosco = WebConfigurationManager.AppSettings["htmlFaleConoscoEs"].ToString();
                    assuntoFaleConosco = WebConfigurationManager.AppSettings["assuntoFaleConoscoEs"].ToString();
                    break;
            }

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();
            var hostDestiantario = WebConfigurationManager.AppSettings["hostDestinatario"].ToString();

            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();


            htmlFaleConosco = System.IO.File.ReadAllText(htmlFaleConosco);

            htmlFaleConosco = htmlFaleConosco.Replace("#CPF#", dados.CPF).Replace("#NOME#", dados.Nome).Replace("#EMAIL#", dados.Email).Replace("#TELEFONE#", dados.Telefone).Replace("#MENSAGEM#", dados.Mensagem).Replace("#ASSUNTO#", dados.Assunto).Replace("#PAIS#", dados.Pais).ToString();


            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, hostDestiantario, assuntoFaleConosco, htmlFaleConosco);
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
        
        public bool EnviarEmailConfirmacaoCadastro(string login, string senha, int idIdioma)
        {
            bool retorno = false;

            var htmlConfirmaCadastro = string.Empty;
            var assuntoConfirmaCadastro = string.Empty;

            var smtpUsuario = WebConfigurationManager.AppSettings["smtpUsuario"].ToString();
            var smtpSenha = WebConfigurationManager.AppSettings["smtpSenha"].ToString();
            var hostRemetente = WebConfigurationManager.AppSettings["hostRemetente"].ToString();

            var hostSMTP = WebConfigurationManager.AppSettings["hostSMTP"].ToString();
            var hostPORT = WebConfigurationManager.AppSettings["hostPORT"].ToString();

            switch (idIdioma)
            {
                case 1:
                    htmlConfirmaCadastro = WebConfigurationManager.AppSettings["htmlConfirmaCadastroPtBr"].ToString();
                    assuntoConfirmaCadastro = WebConfigurationManager.AppSettings["assuntoConfirmaCadastroPtBr"].ToString();
                    break;

                case 2:
                    htmlConfirmaCadastro = WebConfigurationManager.AppSettings["htmlConfirmaCadastroEn"].ToString();
                    assuntoConfirmaCadastro = WebConfigurationManager.AppSettings["assuntoConfirmaCadastroEn"].ToString();
                    break;

                case 3:
                    htmlConfirmaCadastro = WebConfigurationManager.AppSettings["htmlConfirmaCadastroEs"].ToString();
                    assuntoConfirmaCadastro = WebConfigurationManager.AppSettings["assuntoConfirmaCadastroEs"].ToString();
                    break;
            }
            
            htmlConfirmaCadastro = System.IO.File.ReadAllText(htmlConfirmaCadastro);

            htmlConfirmaCadastro = htmlConfirmaCadastro.Replace("#LOGIN#", login).Replace("#SENHA#", senha).ToString();


            using (SmtpClient client = new SmtpClient(hostSMTP, Convert.ToInt16(hostPORT)))
            {

                client.Credentials = new NetworkCredential(smtpUsuario, smtpSenha);
                client.EnableSsl = true;

                try
                {
                    MailMessage msg = new MailMessage(hostRemetente, login, assuntoConfirmaCadastro, htmlConfirmaCadastro);
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
