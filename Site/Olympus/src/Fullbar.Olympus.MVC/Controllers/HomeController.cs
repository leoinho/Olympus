using AutoMapper;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.MVC.Models;
using Fullbar.Olympus.MVC.Util;
using MvcMultilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace Fullbar.Olympus.MVC.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IBanner _banner;
        private readonly ILogin _login;
        private readonly IUsuario _usuario;
        private readonly IUF _uf;
        private readonly ITreinamento _treinamento;
        private readonly IPagina _pagina;

        public HomeController(ILogin login, IUsuario usuario, IUF uf, ITreinamento treinamento, IPagina pagina, IBanner banner)
        {
            _login = login;
            _usuario = usuario;
            _uf = uf;
            _treinamento = treinamento;
            _pagina = pagina;
            _banner = banner;
        }

        public ActionResult Index()
        {
            new SiteLanguages().SetLanguage();
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);

            var login = new Login(null, null);
            var loginViewModel = Mapper.Map<Login, LoginViewModel>(login);

            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 1);

            var treinamento = _treinamento.ListaHome(null, null, idIdioma, "Todos", idUsuario);
          
            
            var treinamentoViewModel = Mapper.Map<List<TreinamentoHome>, List<TreinamentoHomeViewModel>>(treinamento);

            var listPais = _treinamento.ListaTreinamentoPais(idIdioma);
            listPais.Insert(0, new TreinamentoHome { dsPais = BuscaTextoSelecao() });
            ViewBag.Pais = new SelectList(listPais, "dsPais", "dsPais");

            //BANNER
            var listBanner = _banner.ListaBanner().Where(x => x.idIdioma == 0 || x.idIdioma == idIdioma).ToList();
            ViewBag.Banner = listBanner;

            ViewBag.ErroLogin = TempData["loginMsg"];
            ViewBag.erroTitulo = TempData["loginMsgTitulo"];

            ViewBag.Email = TempData["Email"];
            ViewBag.Senha = TempData["Senha"];

            return View(treinamentoViewModel);
        }

        [HttpPost]
        public ActionResult Index(TreinamentoHomeViewModel treinamentoHome)
        {
            new SiteLanguages().SetLanguage();
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);

            var login = new Login(null, null);
            var loginViewModel = Mapper.Map<Login, LoginViewModel>(login);

            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 1);

            List<TreinamentoHome> treinamento = new List<TreinamentoHome>();
            List<TreinamentoHomeViewModel> treinamentoViewModel = new List<TreinamentoHomeViewModel>();
            if (treinamentoHome.dsPais == BuscaTextoSelecao())
            {
                treinamento = _treinamento.ListaHome(null, null, idIdioma, "Todos", idUsuario);

            }
            else
            {
                treinamento = _treinamento.ListaHome(null, null, null, treinamentoHome.dsPais, idUsuario);

            }

            treinamentoViewModel = Mapper.Map<List<TreinamentoHome>, List<TreinamentoHomeViewModel>>(treinamento);
            var listPais = _treinamento.ListaTreinamentoPais(idIdioma);
            listPais.Insert(0, new TreinamentoHome { dsPais = BuscaTextoSelecao() });
            ViewBag.Pais = new SelectList(listPais, "dsPais", "dsPais");

            //BANNER
            var listBanner = _banner.ListaBanner().Where(x => x.idIdioma == 0 || x.idIdioma == idIdioma).ToList();
            ViewBag.Banner = listBanner;

            ViewBag.ErroLogin = TempData["loginMsg"];
            ViewBag.erroTitulo = TempData["loginMsgTitulo"];

            ViewBag.Email = TempData["Email"];
            ViewBag.Senha = TempData["Senha"];

            return View(treinamentoViewModel);
        }

        public string BuscaTextoSelecao()
        {
            string texto = "Seleccione el país";
            if (Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]) == 1)
            {
                texto = "Selecione o país ";
            }
            else if (Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]) == 2)
            {
                texto = "Choose the country";
            }
            return texto;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginViewModel loginViewModel)
        {
            var msg = string.Empty;

            msg += loginViewModel.ValidaEmail();
            msg += loginViewModel.ValidaSenha();

            ViewBag.Email = loginViewModel.Email;
            ViewBag.Senha = loginViewModel.Senha;

            if (!String.IsNullOrEmpty(msg))
            {
                TempData["loginMsg"] = Resource.LoginTextoErro + "<br/><br/>" + msg;

                TempData["Email"] = loginViewModel.Email;
                TempData["Senha"] = loginViewModel.Senha;
                TempData["loginMsgTitulo"] = Resource.LoginTtituloErro;

                return RedirectToAction("Index");
            }

            var login = Mapper.Map<LoginViewModel, Login>(loginViewModel);
            var loginRetorno = _login.Acessar(new Login(login.Email, login.Senha));

            if (loginRetorno == null)
            {
                TempData["loginMsg"] = Resource.LoginTextoErro + "<br/><br/>" + Resource.loginValida;
                TempData["loginMsgTitulo"] = Resource.LoginTtituloErro;

                TempData["Email"] = loginViewModel.Email;
                TempData["Senha"] = loginViewModel.Senha;

                return RedirectToAction("Index");


            }
            else
            {
                FormsAuthentication.SetAuthCookie("loginUsuario", false);

                var cookie = new Cookie();

                cookie.CriarCookie(loginRetorno);

                var treinamento = cookie.cookieTreinamento();
                if (!string.IsNullOrEmpty(treinamento)) return RedirectToAction("Inscricao", "Treinamento");

                return RedirectToAction("Index", "Home");
            }


            return RedirectToAction("Index");

        }



        public ActionResult Error()
        {
            new SiteLanguages().SetLanguage();
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);

            return View();
        }

        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Response.Cookies.Remove("SITE");
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));


            HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            HttpCookie cookieSite = HttpContext.Request.Cookies["SITE"];

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            if (cookieSite != null)
            {
                cookieSite.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookieSite);
            }

            Session.Remove("TREINAMENTO");
            Session.Remove("USUARIO");
            Session.Remove("CATEGORIA");

            return RedirectToAction("Index", "Home");
        }

        public JsonResult Calendario()
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            var Idioma = WebConfigurationManager.AppSettings["Idioma"];

            var listaCalendario = (usuario != null) ? _treinamento.TreinamentoCalendario(DateTime.Now.Month, DateTime.Now.Year, usuario.idusuario) : _treinamento.TreinamentoCalendario(DateTime.Now.Month, DateTime.Now.Year, 0);
            var calendario = new List<TreinamentoCalendario>();


            foreach (var item in listaCalendario)
            {

                var treinamentoCalendario = new TreinamentoCalendario();

                treinamentoCalendario.Idioma = Idioma;
                treinamentoCalendario.dsNome = item.dsNome;
                treinamentoCalendario.dtTreinamento = item.dtTreinamento;
                treinamentoCalendario.idTreinamento = item.idTreinamento;
                treinamentoCalendario.Mes = item.dtTreinamento.Month;
                treinamentoCalendario.Ano = item.dtTreinamento.Year;
                treinamentoCalendario.DataTreinamento = item.dtTreinamento.ToString("dd/MM/yyyy");
                treinamentoCalendario.idStatus = (item.idStatusInscricao == 0) ? item.idStatusTreinamento : item.idStatusInscricao;
                treinamentoCalendario.dsStatusInscricao = item.dsStatusInscricao;
                treinamentoCalendario.dsUrl = string.Format("/{0}/{1}/{2}", Resource.PaginaTreinamentoController, Resource.PaginaTreinamentoDetalhe, item.idTreinamento);

                calendario.Add(treinamentoCalendario);
            }


            if (calendario.Count == 0)
            {
                var treinamentoCalendario = new TreinamentoCalendario();
                treinamentoCalendario.idTreinamento = -1;
                treinamentoCalendario.Idioma = Idioma;
                calendario.Add(treinamentoCalendario);
            }



            return Json(calendario, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EnviarSenha(string Login)
        {
            var retorno = new RetornoPadrao();

            if (string.IsNullOrEmpty(Login.Replace(".", "").Replace("-", "")))
            {
                retorno.id = -1;
                retorno.dsMensagem = Resource.EnviaSenhaValidaCampo;
                retorno.dsTitulo = Resource.LoginTtituloErro;

                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

            retorno = _usuario.EnviarSenha(Login);

            if (retorno.id > 0)
            {
                retorno.dsMensagem = Resource.EnviarSenhaMensgaemSucesso.Replace("[EMAIL]", retorno.dsTexto);
                retorno.dsTitulo = Resource.LoginTtituloErro;
            }


            if (retorno.id == -2 || retorno.id == -1)
            {
                retorno.dsMensagem = Resource.EnviarSenhaMensagemErroEnviarEmail;
                retorno.dsTitulo = Resource.LoginTtituloErro;
            }


            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

    }
}
