using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Repositorio;
using Admin.Olympus.Dominio.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Olympus.MVC.Controllers
{
    public class ConteudoEducacionalController : Controller
    {
        public RepositorioConteudoEducacional _conteudo = new RepositorioConteudoEducacional();
        
        public ActionResult Index()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            ViewBag.Tipo = new SelectList(_conteudo.ListaConteudoEducacionalTipo(), "idTipo", "dsNome");
            ViewBag.Site = DdlSite();
            ViewBag.Status = DdlStatus();

            var listConteudo = _conteudo.ListaConteudoEducacional();

            if (idIdioma != 1)
                listConteudo = listConteudo.Where(x => x.idIdioma == idIdioma).ToList();

            ViewBag.ConteudosAtivos = listConteudo.Where(x => x.btAtivo == true).ToList();
            ViewBag.ConteudosInativos = listConteudo.Where(x => x.btAtivo == false).ToList();

            //Verifica se há mensagem a ser exibida
            if (TempData["Mensagem"] != null)
            {
                ViewBag.Alerta = TempData["Mensagem"];

                return View((ConteudoEducacional)TempData["ConteudoEducacional"]);
            }

            return View(new ConteudoEducacional { btAtivo = true });
        }

        [HttpPost]
        public ActionResult Index(ConteudoEducacional conteudoEducacional, HttpPostedFileBase dsArquivo)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            #region Validações

            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            if (conteudoEducacional.idTipo == 1)
            {
                if (dsArquivo == null)
                    msg.Mensagem += Geral.RetornaTexto("ConteudoModalMensagemArquivoNulo") + " <br /> ";
                else if (dsArquivo.ContentType != "application/pdf")
                    msg.Mensagem += Geral.RetornaTexto("ConteudoModalMensagemArquivoIncorreto") + " <br /> ";

                if (string.IsNullOrEmpty(conteudoEducacional.dsTitulo))
                {
                    msg.Mensagem += Geral.RetornaTexto("ConteudoModalMensagem") + " <br /> ";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(conteudoEducacional.dsTitulo) || string.IsNullOrEmpty(conteudoEducacional.dsLink))
                {
                    msg.Mensagem += Geral.RetornaTexto("ConteudoModalMensagem") + " <br /> ";
                }
            }

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                TempData["Mensagem"] = msg;
                TempData["ConteudoEducacional"] = conteudoEducacional;

                return RedirectToAction("Index");
            }

            #endregion

            if (conteudoEducacional.idTipo == 1)
            {
                try
                {
                    string nomeArquivo = Path.GetFileNameWithoutExtension(dsArquivo.FileName);
                    string extensao = Path.GetExtension(dsArquivo.FileName);
                    string dataHora = System.DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "").Replace("\\", "").Replace(".", "");
                    string nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                    //Salva arquivo
                    var path = Path.Combine(Server.MapPath("~/assets/file/conteudo"), nomeCompleto);
                    dsArquivo.SaveAs(path);

                    conteudoEducacional.dsArquivo = nomeCompleto;
                }
                catch
                {
                    ModalMensagem msgErro = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("ConteudoModalMensagemErro") };
                    TempData["Mensagem"] = msgErro;
                    TempData["ConteudoEducacional"] = conteudoEducacional;

                    return RedirectToAction("Index");
                }
            }

            if (usuario.idTipo != 1)
                conteudoEducacional.idIdioma = usuario.idTipo;

            //Salva conteúdo
            var retorno = _conteudo.CadastraConteudo(conteudoEducacional);

            if (retorno.id != 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModalMensagem msgErro = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("ConteudoModalMensagemErro") };
                TempData["Mensagem"] = msgErro;
                TempData["ConteudoEducacional"] = conteudoEducacional;

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult Alterar(int idConteudo, bool btAtivo)
        {
            var usuario = (Administrador)Session["Administrador"];

            string mensagem = string.Empty;

            var retorno = _conteudo.AlteraConteudo(new ConteudoEducacional { idConteudo = idConteudo, btAtivo = btAtivo });

            return Json(mensagem, JsonRequestBehavior.AllowGet);
        }

        public SelectList DdlSite()
        {
            var listSite = new List<ConteudoEducacional>();
            listSite.Add(new ConteudoEducacional { idIdioma = 0, dsSite = "Todos" });
            listSite.Add(new ConteudoEducacional { idIdioma = 1, dsSite = "OBL" });
            listSite.Add(new ConteudoEducacional { idIdioma = 3, dsSite = "OMS" });
            listSite.Add(new ConteudoEducacional { idIdioma = 2, dsSite = "OLA" });

            return new SelectList(listSite, "idIdioma", "dsSite");
        }
        public SelectList DdlStatus()
        {
            var listStatus = new List<ConteudoEducacional>();
            

            if ((Session["CultureIdioma"]) == "es")
            {
                listStatus.Add(new ConteudoEducacional { btAtivo = true, dsNome = "Activo" });
                listStatus.Add(new ConteudoEducacional { btAtivo = false, dsNome = "Inactivo" });
            }
            else
            {
                listStatus.Add(new ConteudoEducacional { btAtivo = true, dsNome = "Ativo" });
                listStatus.Add(new ConteudoEducacional { btAtivo = false, dsNome = "Inativo" });
            }

            return new SelectList(listStatus, "btAtivo", "dsNome", true);
        }
	}
}