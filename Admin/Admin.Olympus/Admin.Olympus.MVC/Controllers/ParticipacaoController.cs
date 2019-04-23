using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Repositorio;
using Admin.Olympus.Dominio.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

namespace Admin.Olympus.MVC.Controllers
{
    public class ParticipacaoController : Controller
    {
        RepositorioTreinamento _treinamento = new RepositorioTreinamento();

        public ActionResult Inscricoes()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Status = ddlStatusInscricao();

            if (idIdioma == 1)
            {
                //OBL
                if (TempData["Alerta"] != null)
                    ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];

                Participacao participacao = new Participacao { inTipo = 1 };

                if (Session["InscricoesFiltrado"] != null)
                {
                    participacao = (Participacao)Session["InscricoesFiltrado"];
                    ViewBag.Inscricao = _treinamento.ListaInscricoes(participacao);
                }

                return View(participacao);
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        public ActionResult Inscricoes(Participacao participacao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Status = ddlStatusInscricao();

            if (idIdioma == 1)
            {
                //OBL
                ViewBag.Inscricao = _treinamento.ListaInscricoes(participacao);

                //Cria sessão para recarregar as inscrições após executar alguma ação
                Session.Add("InscricoesFiltrado", participacao);

                return View(participacao);
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult InscricoesExcel(Participacao participacao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var admin = (Administrador)Session["Administrador"];

            var inscricoes = _treinamento.ListaInscricoes(participacao);

            // Create file
            DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time"));
            string filename = "Relatorio_Inscricoes_" + now.ToString("ddMMyyyyHHmm") + ".xlsx";

            // Criar excel
            MemoryStream stream = new MemoryStream();
            ExcelStructure excel = new ExcelStructure("Inscricoes", ref stream);

            excel.setColumnWidth(1, 25);
            excel.setColumnWidth(2, 80);
            excel.setColumnWidth(3, 25);
            excel.setColumnWidth(4, 25);
            excel.setColumnWidth(5, 20);
            excel.setColumnWidth(6, 20);
            excel.setColumnWidth(7, 20);
            excel.setColumnWidth(8, 30);
            excel.setColumnWidth(9, 25);
            excel.setColumnWidth(10, 65);
            excel.setColumnWidth(11, 30);
            excel.setColumnWidth(12, 25);
            excel.setColumnWidth(13, 30);
            excel.setColumnWidth(14, 25);
            excel.setColumnWidth(15, 35);
            excel.setColumnWidth(16, 35);

            excel.createSheetData();

            IList<string> headers = new List<string>();

            headers = new List<string>();

            headers.Add("CÓDIGO DO EVENTO");
            headers.Add("NOME TREINAMENTO");
            headers.Add("DATA TREINAMENTO");
            headers.Add("NOME");
            headers.Add("CPF | PASSAPORTE");
            headers.Add("TELEFONE");
            headers.Add("CELULAR");
            headers.Add("E-MAIL");
            headers.Add("CNPJ");
            headers.Add("INSTITUIÇÃO");
            headers.Add("NOVA EMPRESA");
            headers.Add("STATUS INSCRIÇÃO");
            headers.Add("HOSPEDAGEM/PASSAGEM");
            headers.Add("DATA DE INSCRIÇÃO");
            headers.Add("DATA DE ENVIO DE E-MAIL");
            headers.Add("DATA DE ENVIO DE SMS");

            excel.CreateColumnHeader(1, headers);

            IList<string> valores = new List<string>();
            IList<int> tipos = new List<int>();

            uint row = 2;

            foreach (var m in inscricoes)
            {
                valores = new List<string>();
                tipos = new List<int>();

                valores.Add(m.dsCodigo);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTreinamento);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dtTreinamento.ToShortDateString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNomeCompleto);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCPF);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTelefone);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCelular);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsEmail);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCNPJ);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsInstituicao);
                tipos.Add(ExcelStructure.TYPE_STRING);

                if(m.btNovaEmpresa == true)
                    valores.Add("SIM");
                else
                    valores.Add("NÃO");                
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsStatusInscricao);
                tipos.Add(ExcelStructure.TYPE_STRING);
                
                valores.Add(m.dsHospedagem);
                tipos.Add(ExcelStructure.TYPE_STRING);               
                
                valores.Add(m.dtCadastro.ToString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dtEmail == null ? "Não Enviado" : m.dtEmail.ToString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dtSMS == null ? "Não Enviado" : m.dtSMS.ToString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                excel.CreateRowWithContent(row, valores, tipos);
                row++;
            }

            excel.save();
            excel.close();

            stream.Position = 0;

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = filename
            };
        }

        public ActionResult ListaEspera()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL   

                if (TempData["Alerta"] != null)
                    ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];

                Participacao participacao = new Participacao { inTipo = 2 };

                if (Session["ListaEsperaFiltrado"] != null)
                {
                    participacao = (Participacao)Session["ListaEsperaFiltrado"];
                    ViewBag.ListaEspera = _treinamento.ListaInscricoes((Participacao)Session["ListaEsperaFiltrado"]);
                }

                return View(participacao);
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ListaEspera(Participacao participacao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
                ViewBag.ListaEspera = _treinamento.ListaInscricoes(participacao);

                //Cria sessão para recarregar as inscrições após executar alguma ação
                Session.Add("ListaEsperaFiltrado", participacao);

                return View(participacao);
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ListaEsperaExcel(Participacao participacao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var admin = (Administrador)Session["Administrador"];

            var inscricoes = _treinamento.ListaInscricoes(participacao);

            // Create file
            DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time"));
            string filename = "Relatorio_ListaEspera_" + now.ToString("ddMMyyyyHHmm") + ".xlsx";

            // Criar excel
            MemoryStream stream = new MemoryStream();
            ExcelStructure excel = new ExcelStructure("ListaEspera", ref stream);

            excel.setColumnWidth(1, 25);
            excel.setColumnWidth(2, 80);
            excel.setColumnWidth(3, 25);
            excel.setColumnWidth(4, 25);
            excel.setColumnWidth(5, 20);
            excel.setColumnWidth(6, 20);
            excel.setColumnWidth(7, 20);
            excel.setColumnWidth(8, 30);
            excel.setColumnWidth(9, 20);
            excel.setColumnWidth(10, 25);
            excel.setColumnWidth(11, 40);
            excel.setColumnWidth(12, 25);
            excel.setColumnWidth(13, 30);
            excel.setColumnWidth(14, 25);

            excel.createSheetData();

            IList<string> headers = new List<string>();

            headers = new List<string>();

            headers.Add("CÓDIGO DO EVENTO");
            headers.Add("NOME TREINAMENTO");
            headers.Add("DATA TREINAMENTO");
            headers.Add("NOME");
            headers.Add("CPF | PASSAPORTE");
            headers.Add("TELEFONE");
            headers.Add("CELULAR");
            headers.Add("E-MAIL");
            headers.Add("ESTADO");
            headers.Add("CNPJ");
            headers.Add("INSTITUIÇÃO");
            headers.Add("STATUS INSCRIÇÃO");
            headers.Add("HOSPEDAGEM/PASSAGEM");
            headers.Add("DATA DE INSCRIÇÃO");

            excel.CreateColumnHeader(1, headers);

            IList<string> valores = new List<string>();
            IList<int> tipos = new List<int>();

            uint row = 2;

            foreach (var m in inscricoes)
            {
                valores = new List<string>();
                tipos = new List<int>();

                valores.Add(m.dsCodigo);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTreinamento);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dtTreinamento.ToShortDateString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNomeCompleto);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCPF);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTelefone);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCelular);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsEmail);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsEstado);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCNPJ);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNomeFantasia);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsStatusInscricao);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsHospedagem);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dtCadastro.ToShortDateString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                excel.CreateRowWithContent(row, valores, tipos);
                row++;
            }

            excel.save();
            excel.close();

            stream.Position = 0;

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = filename
            };
        }

        public ActionResult ListaPresenca()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL   
                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).ToList(), "idTreinamento", "dsNomeComData");
               
                if (TempData["Alerta"] != null)
                    ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];
                
                Participacao participacao = new Participacao { inTipo = 3 };

                if (Session["ListaPresencaFiltrado"] != null)
                {
                    participacao = (Participacao)Session["ListaPresencaFiltrado"];
                    ViewBag.Presenca = _treinamento.ListaInscricoes((Participacao)Session["ListaPresencaFiltrado"]).Where(x => x.idStatusInscricao == 2).ToList();
                }

                return View(participacao);
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Home");
            }            
        }
        [HttpPost]
        public ActionResult ListaPresenca(Participacao participacao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL                
                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).ToList(), "idTreinamento", "dsNomeComData");
                
                if (TempData["Alerta"] != null)
                    ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];

                ViewBag.Presenca = _treinamento.ListaInscricoes(participacao).Where(x => x.idStatusInscricao == 2).ToList();

                //Cria sessão para recarregar as inscrições após executar alguma ação
                Session.Add("ListaPresencaFiltrado", participacao);
                
                return View(participacao);
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult ConfirmarPresenca(Participacao participacao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL                
                participacao.idAdministrador = usuario.idAdministrador;
                var retorno = _treinamento.ConfirmarPresenca(participacao);

                if (retorno != null && retorno.id > 0)
                {
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoInscricaoOK") };
                    TempData["Alerta"] = msg;

                    //Envia E-mail do Certificado:
                    var idioma = retorno.idReferencia;
                    if (idioma == 0)
                        idioma = 1;

                    Admin.Olympus.Dominio.Util.Email _email = new Admin.Olympus.Dominio.Util.Email();
                    _email.EnviarEmailCertificado(idioma, participacao.dsEmail);


                    return RedirectToAction("ListaPresenca");
                }
                
                if (retorno == null || retorno.id == 0)
                {
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoInscricaoErro") };
                    TempData["Alerta"] = msg;
                    return RedirectToAction("ListaPresenca");
                }

                return RedirectToAction("ListaPresenca");
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Home");
            }
        }
        //[HttpPost]
        //public ActionResult GerarListaPresenca(Participacao participacao)
        //{
        //    if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
        //    var admin = (Administrador)Session["Administrador"];

        //    var presenca = _treinamento.ListaInscricoes(participacao);

        //    // Create file
        //    string filename = "ListaPresenca_" + System.DateTime.Now.ToString("ddMMyyyyHHmm") + ".xlsx";

        //    // Criar excel
        //    MemoryStream stream = new MemoryStream();
        //    ExcelStructure excel = new ExcelStructure("ListaPresenca", ref stream);

        //    excel.setColumnWidth(1, 30);
        //    excel.setColumnWidth(2, 80);
        //    excel.setColumnWidth(3, 30);
        //    excel.setColumnWidth(4, 90);

        //    excel.createSheetData();

        //    IList<string> headers = new List<string>();

        //    IList<string> valores = new List<string>();
        //    IList<int> tipos = new List<int>();

        //    valores = new List<string>();
        //    tipos = new List<int>();

        //    headers = new List<string>();

        //    headers.Add(presenca.FirstOrDefault().dsTreinamento);
        //    excel.CreateColumnHeader(1, headers);

        //    excel.CreateHeader(2, " ", 28, 0);

        //    valores = new List<string>();
        //    tipos = new List<int>();

        //    valores.Add("Data: " + presenca.FirstOrDefault().dtTreinamento.ToShortDateString());
        //    tipos.Add(ExcelStructure.TYPE_STRING);

        //    valores.Add("Início: " + presenca.FirstOrDefault().dsHoraInicio);
        //    tipos.Add(ExcelStructure.TYPE_STRING);

        //    valores.Add("Término: " + presenca.FirstOrDefault().dsHoraFim);
        //    tipos.Add(ExcelStructure.TYPE_STRING);

        //    excel.CreateRowWithContent(2, valores, tipos);

        //    excel.CreateHeader(3, " ", 28, 0);

        //    valores = new List<string>();
        //    tipos = new List<int>();

        //    valores.Add("Palestrante: " + presenca.FirstOrDefault().dsPalestrante);
        //    tipos.Add(ExcelStructure.TYPE_STRING);

        //    valores.Add("Local: " + presenca.FirstOrDefault().dsLocal);
        //    tipos.Add(ExcelStructure.TYPE_STRING);

        //    excel.CreateRowWithContent(3, valores, tipos);

        //    excel.CreateHeader(4, " ", 28, 0);
        //    excel.CreateHeader(5, "Relatório de Cadastros - " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"), 20, 3);
        //    excel.CreateHeader(6, " ", 22, 3);

        //    headers = new List<string>();

        //    headers.Add("Código do treinamento");
        //    headers.Add("Título do Treinamento");
        //    excel.CreateColumnHeader(7, headers);

        //    valores = new List<string>();
        //    tipos = new List<int>();

        //    valores.Add(presenca.FirstOrDefault().dsCodigo);
        //    tipos.Add(ExcelStructure.TYPE_STRING);

        //    valores.Add(presenca.FirstOrDefault().dsTreinamento);
        //    tipos.Add(ExcelStructure.TYPE_STRING);

        //    excel.CreateRowWithContent(8, valores, tipos);
        //    excel.CreateHeader(9, " ");

        //    excel.CreateHeader(10, "Descrição do Treinamento", 14, 3);
        //    excel.CreateHeader(11, " ", 11, 3);

        //    valores = new List<string>();
        //    tipos = new List<int>();
        //    valores.Add(presenca.FirstOrDefault().dsDescricao);
        //    tipos.Add(ExcelStructure.TYPE_STRING);
        //    excel.CreateRowWithContent(11, valores, tipos);

        //    excel.CreateHeader(12, " ");

        //    headers = new List<string>();

        //    headers.Add("Nome Completo");
        //    headers.Add("Instituição");
        //    headers.Add("E-mail");
        //    headers.Add("Assinatura");

        //    excel.CreateColumnHeader(13, headers);

        //    valores = new List<string>();
        //    tipos = new List<int>();

        //    uint row = 14;

        //    foreach (var m in presenca)
        //    {
        //        valores = new List<string>();
        //        tipos = new List<int>();

        //        valores.Add(m.dsNomeCompleto);
        //        tipos.Add(ExcelStructure.TYPE_STRING);

        //        valores.Add(m.dsNomeFantasia);
        //        tipos.Add(ExcelStructure.TYPE_STRING);

        //        valores.Add(m.dsEmail);
        //        tipos.Add(ExcelStructure.TYPE_STRING);

        //        valores.Add("");
        //        tipos.Add(ExcelStructure.TYPE_STRING);

        //        excel.CreateRowWithContent(row, valores, tipos);
        //        row++;
        //    }

        //    excel.insertImageOnPositionFooter(AppDomain.CurrentDomain.BaseDirectory + "assets\\img\\footer.png", Convert.ToInt16(row) + 2, 30, 1000, 1000);

        //    excel.save();
        //    excel.close();

        //    stream.Position = 0;

        //    return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //    {
        //        FileDownloadName = filename
        //    };
        //}
        [HttpPost]
        public ActionResult GerarListaPresenca(Participacao participacao)
        {
            try
            {
                if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }

                LocalReport relatorio = new LocalReport();
                var presenca = _treinamento.ListaInscricoes(participacao);

                relatorio.ReportPath = Server.MapPath("~/Reports/RelatorioPresenca.rdlc");
                //relatorio.ReportEmbeddedResource = "Admin.Olympus.MCV.Reports.RelatorioPresenca.rdlc";
                relatorio.DataSources.Add(new ReportDataSource("Presenca", presenca));

                string reportType = "Excel";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                  "<DeviceInfo>" +
                  " <OutputFormat>xls</OutputFormat>" +
                  " <PageWidth>9in</PageWidth>" +
                  " <PageHeight>11in</PageHeight>" +
                  " <MarginTop>0.7in</MarginTop>" +
                  " <MarginLeft>2in</MarginLeft>" +
                  " <MarginRight>2in</MarginRight>" +
                  " <MarginBottom>0.7in</MarginBottom>" +
                  "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] bytes;

                bytes = relatorio.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                Response.AddHeader("content-disposition", "attachment; filename=RelatorioPresenca_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + fileNameExtension);
                return File(bytes, mimeType);
            }
            catch (Exception ex)
            {
                return Content(ex.Message + " - " + ex.InnerException.Message + " - " + ex.InnerException.InnerException.Message);
            }
        }

        [HttpPost]
        public ActionResult AlterarStatusInscricao(Participacao participacao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            var status = _treinamento.ListaTreinamentoInscricaoStatusUsuario(participacao.idUsuario, participacao.idTreinamento);

            participacao.idAdministrador = usuario.idAdministrador;
            var retorno = _treinamento.AlteraStatusInscricao(participacao);

            if (retorno != null && retorno.id > 0)
            {

                if (participacao.idStatusInscricaoAltera == 5 && status == 2)
                {
                    try { _treinamento.ConfirmaProximoListaEspera(participacao.idInscricao, usuario.idAdministrador); }
                    catch (Exception ex) { var erro = ex.Message; }
                }

                var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoInscricaoOK") };
                TempData["Alerta"] = msg;
                return RedirectToAction(participacao.dsAction);
            }
            else if (retorno == null || retorno.id == 0)
            {
                var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoInscricaoErro") };
                TempData["Alerta"] = msg;
                return RedirectToAction(participacao.dsAction);
            }
            else if (retorno != null && retorno.id == -1)
            {
                var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoInscricaoJaEfetuada") };
                TempData["Alerta"] = msg;
                return RedirectToAction(participacao.dsAction);
            }

            return RedirectToAction(participacao.dsAction);
        }



        public ActionResult PesquisaUsuario()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;


            int idUsuario = 0;
            int idPesquisa = 0;
            int idInscricao = 0;
            int idTreinamento = 0;

            try
            {
                idUsuario = Convert.ToInt32(Request.QueryString["idUsuario"]);
                idPesquisa = Convert.ToInt32(Request.QueryString["idPesquisa"]);
                idInscricao = Convert.ToInt32(Request.QueryString["idInscricao"]);
                idTreinamento = Convert.ToInt32(Request.QueryString["idTreinamento"]);
            }
            catch{}

            var filtroInscricao = new Participacao { inTipo = 1 };
            var inscricao = _treinamento.ListaInscricoes(filtroInscricao).Where(i => i.idInscricao == idInscricao).FirstOrDefault();
            ViewBag.Inscricao = inscricao;
            var lista = _treinamento.ListaPesquisaRespostasUsuario(idPesquisa, idUsuario, idInscricao);
            ViewBag.StatusInscricao = _treinamento.ListaTreinamentoInscricaoStatusUsuario(idUsuario, idTreinamento);

            return View(lista);
           
        }


        [HttpPost]
        public JsonResult AlteraStatusInscricao(int idStatus, int idInscricao)
        {
            RetornoPadrao retorno = new RetornoPadrao();

            if (Session["Administrador"] == null) {
                retorno.id = -1;
                return Json(retorno, JsonRequestBehavior.AllowGet);            
            }

            try
            {

                var usuario = (Administrador)Session["Administrador"];

                var participacao = new Participacao();
                participacao.idStatusInscricaoAltera = idStatus;
                participacao.idAdministrador = usuario.idAdministrador;
                participacao.idInscricao = idInscricao;

                retorno = _treinamento.AlteraStatusInscricaoComListaEspera(participacao);
            }
            catch(Exception ex)
            {
                retorno.id = -1;
                retorno.dsMensagem = ex.Message;
            }
            
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        public SelectList ddlStatusInscricao()
        {
            var listStatus = new List<Participacao>();
            listStatus.Add(new Participacao { idStatusInscricao = 0, dsStatusInscricao = "Todos" });
            listStatus.Add(new Participacao { idStatusInscricao = 1, dsStatusInscricao = "Pendente" });
            listStatus.Add(new Participacao { idStatusInscricao = 2, dsStatusInscricao = "Confirmada" });
            listStatus.Add(new Participacao { idStatusInscricao = 3, dsStatusInscricao = "Lista de Espera - Vagas" });
            listStatus.Add(new Participacao { idStatusInscricao = 4, dsStatusInscricao = "Lista de Espera - Vagas por CNPJ" });
            listStatus.Add(new Participacao { idStatusInscricao = 5, dsStatusInscricao = "Cancelada" });
            listStatus.Add(new Participacao { idStatusInscricao = 6, dsStatusInscricao = "Cancelada. Treinamento já efetuado nos ultimos 12 meses." });
            listStatus.Add(new Participacao { idStatusInscricao = 7, dsStatusInscricao = "Ausencia" });
            listStatus.Add(new Participacao { idStatusInscricao = 8, dsStatusInscricao = "Presença Confirmada" });

            return new SelectList(listStatus, "idStatusInscricao", "dsStatusInscricao", 0);
        }
	}
}