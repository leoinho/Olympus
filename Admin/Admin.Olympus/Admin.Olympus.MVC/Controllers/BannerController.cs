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
    public class BannerController : Controller
    {
        public RepositorioBanner _banner = new RepositorioBanner();

        public ActionResult Index()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            ViewBag.Ordem = DdlOrdem();
            ViewBag.Site = DdlSite();
            ViewBag.Status = DdlStatus();

            var listBanner = _banner.ListaBanner();

            ViewBag.BannersAtivos = listBanner.Where(x => x.idStatus == 1).OrderBy(b => b.inOrdem).ToList();
            ViewBag.BannersInativos = listBanner.Where(x => x.idStatus == 2).ToList();

            //Verifica se há mensagem a ser exibida
            if (TempData["Mensagem"] != null)
            {
                ViewBag.Alerta = TempData["Mensagem"];

                return View((Banner)TempData["Banner"]);
            }

            return View(new Banner());
        }

        [HttpPost]
        public ActionResult Index(Banner banner, HttpPostedFileBase dsImagem)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            #region Validações

            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            if (dsImagem == null)
            {
                msg.Mensagem += Geral.RetornaTexto("BannerModalMensagemArquivoNulo") + " <br /> ";
            }
            else if (dsImagem.ContentType != "image/pjpeg" && dsImagem.ContentType != "image/jpeg" && dsImagem.ContentType != "image/png")
            {
                msg.Mensagem += Geral.RetornaTexto("BannerModalMensagemArquivoIncorreto") + " <br /> ";
            }

            //TAMANHO DA IMAGEM
            if (dsImagem != null)
            {
                var img = System.Drawing.Image.FromStream(dsImagem.InputStream, true, true);
                int width = img.Width;
                int height = img.Height;

                if (width < 791 || height < 335)
                {
                    msg.Mensagem += Geral.RetornaTexto("BannerModalMensagemTamanhoImagem") + " <br /> ";
                }
            }

            if (string.IsNullOrEmpty(banner.dsTitulo))
            {
                msg.Mensagem += Geral.RetornaTexto("BannerModalMensagem") + " <br /> ";
            }

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                TempData["Mensagem"] = msg;
                TempData["Banner"] = banner;

                return RedirectToAction("Index");
            }

            #endregion

            try
            {
                string nomeArquivo = Path.GetFileNameWithoutExtension(dsImagem.FileName);
                string extensao = Path.GetExtension(dsImagem.FileName);
                string dataHora = System.DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "").Replace("\\", "").Replace(".", "");
                string nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                //Salva arquivo
                var path = Path.Combine(Server.MapPath("~/assets/img/banner"), nomeCompleto);
                dsImagem.SaveAs(path);

                banner.idAdministrador = usuario.idAdministrador;
                banner.dsImagem = nomeCompleto;

                //Salva banner
                var retorno = _banner.CadastraBanner(banner);

                if (retorno.id != 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModalMensagem msgErro = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("BannerCadastroErro") };
                    TempData["Mensagem"] = msgErro;
                    TempData["Banner"] = banner;

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModalMensagem msgErro = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("BannerCadastroErro") };
                TempData["Mensagem"] = msgErro;
                TempData["Banner"] = banner;

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult Alterar(int idBanner, int idStatus)
        {
            var usuario = (Administrador)Session["Administrador"];

            string mensagem = string.Empty;

            var retorno = _banner.AlteraBanner(new Banner { idBanner = idBanner, idAdministrador = usuario.idAdministrador, idStatus = idStatus });
            
            return Json(mensagem, JsonRequestBehavior.AllowGet);
        }

        public SelectList DdlSite()
        {
            var listSite = new List<Banner>();
            listSite.Add(new Banner { idIdioma = 0, dsSite = "Todos" });
            listSite.Add(new Banner { idIdioma = 1, dsSite = "OBL" });
            listSite.Add(new Banner { idIdioma = 3, dsSite = "OMS" });
            listSite.Add(new Banner { idIdioma = 2, dsSite = "OLA" });

            return new SelectList(listSite, "idIdioma", "dsSite", 0);
        }
        public SelectList DdlStatus()
        {
            var listStatus = new List<Banner>();

            if ((Session["CultureIdioma"]) == "es")
            {
                listStatus.Add(new Banner { idStatus = 1, dsStatus = "Activo" });
                listStatus.Add(new Banner { idStatus = 2, dsStatus = "Inactivo" });
            }
            else
            {
                listStatus.Add(new Banner { idStatus = 1, dsStatus = "Ativo" });
                listStatus.Add(new Banner { idStatus = 2, dsStatus = "Inativo" });
            }

            

            return new SelectList(listStatus, "idStatus", "dsStatus");
        }
        public SelectList DdlOrdem()
        {
            var listOrdem = new List<Banner>();
            listOrdem.Add(new Banner { inOrdem = 1 });
            listOrdem.Add(new Banner { inOrdem = 2 });
            listOrdem.Add(new Banner { inOrdem = 3 });

            return new SelectList(listOrdem, "inOrdem", "inOrdem", 1);
        }



        [HttpPost]
        public JsonResult Ordenar(string ordem)
        {
            RetornoPadrao retorno = new RetornoPadrao();

            try
            {
                string[] id;
                id = ordem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < id.Length; i++)
                {
                    retorno = _banner.Ordena(Convert.ToInt32(id[i]), (i + 1));
                }

            }
            catch (Exception ex)
            {
                retorno = new RetornoPadrao();
                retorno.id = -1;
                retorno.dsMensagem = ex.Message;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AlteraLink(int idBanner, string dsLink)
        {
            RetornoPadrao retorno = new RetornoPadrao();

            try
            {
                retorno = _banner.AlteraLink(idBanner, dsLink);

            }
            catch (Exception ex)
            {
                retorno = new RetornoPadrao();
                retorno.id = -1;
                retorno.dsMensagem = ex.Message;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Excluir(int idBanner)
        {
            var retorno = _banner.Exclui(idBanner);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        
	}
}