using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using Admin.Olympus.Dominio.Entidade;

namespace Admin.Olympus.MVC.Controllers
{
    public class HomeController : Controller
    {
        //INICIA SESSÕES COM IDIOMA ESCOLHIDO OU PEGA DA WEB CONFIG O IDIOMA INICIAL
        public ActionResult Index()
        {
            //Idioma Resource
            Session.Add("ArquivoResource", WebConfigurationManager.AppSettings["ArquivoResource"]);

            //Culture Idioma
            Session.Add("CultureIdioma", WebConfigurationManager.AppSettings["CultureIdioma"]);

            //idIdioma
            //Session.Add("idIdioma", WebConfigurationManager.AppSettings["idIdioma"]);

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult Index(Idioma idioma)
        {
            if (idioma.idIdiomaSite == 1)
            {
                //Idioma Resource
                Session.Add("ArquivoResource", WebConfigurationManager.AppSettings["ArquivoResource"]);

                //Culture Idioma
                Session.Add("CultureIdioma", WebConfigurationManager.AppSettings["CultureIdioma"]);

                //idIdioma
                //Session.Add("idIdioma", idioma.idIdiomaSite);
            }
            else
            {
                //Idioma Resource
                Session.Add("ArquivoResource", "ResourceES");

                //Culture Idioma
                Session.Add("CultureIdioma", "es");

                //idIdioma
                //Session.Add("idIdioma", idioma.idIdiomaSite);
            }


            if (Session["Administrador"] == null)
                return RedirectToAction("Index", "Login");
            else
                return RedirectToAction("Index", "Dashboard");
        }
	}
}