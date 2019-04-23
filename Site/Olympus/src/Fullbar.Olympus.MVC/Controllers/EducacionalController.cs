using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.MVC.Util;
using MvcMultilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Fullbar.Olympus.MVC.Controllers
{
    public class EducacionalController : BaseController
    {
        private readonly IPagina _pagina;
        private readonly IConteudoEducacional _conteudo;

        public EducacionalController(IPagina pagina, IConteudoEducacional conteudo)
        {
            _pagina = pagina;
            _conteudo = conteudo;
        }

        public ActionResult Index()
        {
            new SiteLanguages().SetLanguage();
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);            

            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;

            if (idIdioma == 3) //No site Espanhol é necessário estar logado
            {
                if (usuario == null) return RedirectToAction("Index", "Home");
            }

            _pagina.Log(idUsuario, 4);

            //Lista Conteúdo
            ViewBag.Conteudo = _conteudo.ListaConteudoEducacional().Where(x => x.idIdioma == 0 || x.idIdioma == idIdioma).Where(x => x.btAtivo).ToList();

            return View();
        }

    }
}
