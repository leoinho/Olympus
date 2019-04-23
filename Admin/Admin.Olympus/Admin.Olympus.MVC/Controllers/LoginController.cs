using Admin.Olympus.Dominio.Entidade;
//using Admin.Olympus.Dominio.Interface;
using Admin.Olympus.Dominio.Repositorio;
using Admin.Olympus.Dominio.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Admin.Olympus.MVC.Controllers
{
    public class LoginController : Controller
    {
        //private readonly IAdministrador _admin;

        //public LoginController(IAdministrador admin)
        //{
        //    _admin = admin;
        //}

        public RepositorioAdministrador _admin = new RepositorioAdministrador();

        public ActionResult Index()
        {
            //Verifica mensagem
            if (TempData["Mensagem"] != null)
                ViewBag.Alerta = TempData["Mensagem"];
            
            return View();
        }

        public ActionResult Login(string login, string senha)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("PaginaLoginModalTitulo"), Mensagem = Geral.RetornaTexto("PaginaLoginModalMensagemValidacao") };
                TempData.Add("Mensagem", msg);

                return RedirectToAction("Index");
            }
            else
            {
                var resultado = _admin.Logar(login, senha, Request.UserHostAddress);

                if (resultado != null)
                {
                    Session.Add("Administrador", resultado);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("PaginaLoginModalTitulo"), Mensagem = Geral.RetornaTexto("PaginaLoginModalMensagemErro") };
                    TempData.Add("Mensagem", msg);
                    
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }        
	}
}