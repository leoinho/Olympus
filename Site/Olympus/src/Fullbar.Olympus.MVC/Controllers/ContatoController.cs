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

namespace Fullbar.Olympus.MVC.Controllers
{
    public class ContatoController : Controller
    {

        private readonly IFaleConosco _faleConosco;
        private readonly IPagina _pagina;

        public ContatoController(IFaleConosco faleConosco, IPagina pagina)
        {
            _faleConosco = faleConosco;
            _pagina = pagina;
        }

        public ActionResult Index()
        {
            new SiteLanguages().SetLanguage();

            var cookie = new Cookie();
            var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
            var usuario = cookie.cookieUsuario();

            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 6);

            ViewBag.logado = (usuario != null) ? 1 : 0;
            ViewBag.CPF = (usuario != null) ? usuario.dsCPF : string.Empty;
            ViewBag.nome = (usuario != null) ? usuario.dsNomeCompleto : string.Empty;
            ViewBag.telefone = (usuario != null) ? usuario.dsTelefone : string.Empty;
            ViewBag.email = (usuario != null) ? usuario.dsEmail : string.Empty;

            var contato = new List<FaleConoscoViewModel>();

            if (idioma == 1 || idioma == 3)
                contato = new List<FaleConoscoViewModel>() { 
                    new FaleConoscoViewModel { Contato = "Brasil" }, 
                    new FaleConoscoViewModel { Contato = "México" }, 
                    new FaleConoscoViewModel { Contato = "América Latina" } 
                };
            else if (idioma == 2)
                contato = new List<FaleConoscoViewModel>() { 
                    new FaleConoscoViewModel { Contato = "Brazil" }, 
                    new FaleConoscoViewModel { Contato = "Latin America" }, 
                    new FaleConoscoViewModel { Contato = "Mexico" } 
                };

            if (usuario != null)
            {
                ViewBag.Nacionalidade = usuario.idNacionalidade;
            }
            else
            {
                ViewBag.Nacionalidade = (idioma == 1) ? 1 : 2;
            }

            var assunto = new List<Assunto>() { new Assunto() { dsAssunto = Resource.PaginaContatoAssunto1 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto2 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto3 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto4 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto5 },
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto6 }};

            ViewBag.Assunto = new SelectList(assunto, "dsAssunto", "dsAssunto");
            ViewBag.Contato = new SelectList(contato, "Contato", "Contato");

            ViewBag.MensagemRetorno = TempData["msgRetorno"];
            ViewBag.ErroCadastroTitulo = TempData["msgRetornoTitulo"];


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(FaleConoscoViewModel faleConoscoViewModel)
        {
            new SiteLanguages().SetLanguage();

            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();


            var msg = string.Empty;

            //msg += faleConoscoViewModel.ValidaCPF();
            msg += faleConoscoViewModel.ValidaNome();
            msg += faleConoscoViewModel.ValidaTelefone();
            msg += faleConoscoViewModel.ValidaEmail();
            msg += faleConoscoViewModel.ValidaContato();
            msg += faleConoscoViewModel.ValidaAssunto();
            msg += faleConoscoViewModel.ValidaMensagem();

            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.ErroCadastroTitulo = Resource.ContatoTituloErro;
                CarregarCampos(faleConoscoViewModel, Resource.PaginaContatoTextoErro + "<br/><br/>" + msg);

                return View("Index");
            }

            var faleConoscoModel = Mapper.Map<FaleConoscoViewModel, FaleConosco>(faleConoscoViewModel);
            faleConoscoModel.CPF = string.Empty;
            faleConoscoModel.idUsuario = (usuario != null) ? usuario.idusuario : 0;

            var retorno = _faleConosco.Cadastrar(faleConoscoModel);

            if (retorno.id > 0)
            {

                TempData["msgRetorno"] = Resource.PaginaContatoTextoSucesso;
                TempData["msgRetornoTitulo"] = string.Empty;

                return RedirectToAction("Index");
            }
            else
            {
                var mensagem = (retorno.id == -2) ? Resource.PaginaContatoTextoErro + "<br/><br/>" + Resource.MensagemEnvioContato : Resource.PaginaContatoTextoErro + "<br/><br/>" + Resource.MensagemCadastroContato;

                CarregarCampos(faleConoscoViewModel, mensagem);
                ViewBag.ErroCadastroTitulo = Resource.ContatoTituloErro;

                return View("Index");
            }

        }

        public void CarregarCampos(FaleConoscoViewModel faleConoscoViewModel, string msgRetorno)
        {
            var cookie = new Cookie();
            var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
            var usuario = cookie.cookieUsuario();

            if (usuario != null)
            {
                ViewBag.Nacionalidade = usuario.idNacionalidade;
            }
            else
            {
                ViewBag.Nacionalidade = (idioma == 1) ? 1 : 2;
            }

            var assunto = new List<Assunto>() { new Assunto() { dsAssunto = Resource.PaginaContatoAssunto1 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto2 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto3 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto4 }, 
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto5 },
                                                new Assunto() { dsAssunto = Resource.PaginaContatoAssunto6 }};

            var contato = new List<FaleConoscoViewModel>();

            if (idioma == 1 || idioma == 3)
                contato = new List<FaleConoscoViewModel>() { 
                    new FaleConoscoViewModel { Contato = "Brasil" }, 
                    new FaleConoscoViewModel { Contato = "México" }, 
                    new FaleConoscoViewModel { Contato = "América Latina" } 
                };
            else if (idioma == 2)
                contato = new List<FaleConoscoViewModel>() { 
                    new FaleConoscoViewModel { Contato = "Brazil" }, 
                    new FaleConoscoViewModel { Contato = "Latin America" }, 
                    new FaleConoscoViewModel { Contato = "Mexico" } 
                };


            ViewBag.Assunto = new SelectList(assunto, "dsAssunto", "dsAssunto", faleConoscoViewModel.Assunto);
            ViewBag.Contato = new SelectList(contato, "Contato", "Contato", faleConoscoViewModel.Contato);

            ViewBag.logado = (usuario != null) ? 1 : 0;

            ViewBag.CPF = faleConoscoViewModel.CPF;
            ViewBag.nome = faleConoscoViewModel.Nome;
            ViewBag.telefone = faleConoscoViewModel.Telefone;
            ViewBag.email = faleConoscoViewModel.Email;
            ViewBag.mensagem = faleConoscoViewModel.Mensagem;
            ViewBag.MensagemRetorno = msgRetorno;
        }


    }
}
