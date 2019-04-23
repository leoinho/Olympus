using MvcMultilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fullbar.Olympus.MVC.Controllers
{
    public class BolsasEstudoController : Controller
    {
        //
        // GET: /BolsasEstudo/

        public ActionResult Index()
        {
            new SiteLanguages().SetLanguage();

            return View();
        }

    }
}
