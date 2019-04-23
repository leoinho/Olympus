using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.MVC.Util;
using MvcMultilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fullbar.Olympus.MVC.Controllers
{
    public class OlympusController : Controller
    {
        private readonly IPagina _pagina;

        public OlympusController(IPagina pagina)
        {

            _pagina = pagina;
        }

        public ActionResult Index()
        {
            new SiteLanguages().SetLanguage();

            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 5);


            return View();
        }

    }
}
