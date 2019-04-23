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
    public class CadastroController : BaseController
    {
        private readonly IUsuario _usuario;
        private readonly IPagina _pagina;

        public CadastroController(IUsuario usuario, IPagina pagina)
        {
            _usuario = usuario;
            _pagina = pagina;
        }



        public ActionResult Index()
        {
            var usuario = new Cookie().cookieUsuario();

            if (usuario != null) return RedirectToAction("Index", "Home");

            new SiteLanguages().SetLanguage();

            ViewBag.Titulo = Fullbar.Olympus.MVC.Resource.Titulo;
            //ViewBag.Nacionalidade = 1;

            ViewBag.Pais = new SelectList(_usuario.ListaPais().Where(x => x.dsNome != "BRAZIL"), "idPais", "dsNome");

            _pagina.Log(0, 2);

            return View(new UsuarioCadastroViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UsuarioCadastroViewModel UsuarioViewModel)
        {
            
            var msg = string.Empty;
            Session.Add("ErroCadastro", "");            

            if (UsuarioViewModel.nacionalidade == 1) msg += UsuarioViewModel.ValidaCPF();

            if (UsuarioViewModel.nacionalidade == 1)
            {
                msg += UsuarioViewModel.ValidaNome();
                msg += UsuarioViewModel.ValidaTelefone();
                msg += UsuarioViewModel.ValidaCelular();
                msg += UsuarioViewModel.ValidaEmail();
                msg += UsuarioViewModel.ValidaSenha();
            }

            if (UsuarioViewModel.nacionalidade == 2)
            {
                msg += UsuarioViewModel.ValidaPais();
                msg += UsuarioViewModel.ValidaNomeEstrangeiro();
                msg += UsuarioViewModel.ValidaTelefoneEstrangeiro();
                msg += UsuarioViewModel.ValidaEmailEstrangeiro();
                msg += UsuarioViewModel.ValidaSenhaEstrangeiro();
            }


            msg += UsuarioViewModel.ValidaTituloProfissional();

            if (UsuarioViewModel.nacionalidade == 1)
            {
                msg += UsuarioViewModel.ValidaCNPJ();
            }

            msg += UsuarioViewModel.ValidaNomeInstituicao();
            msg += UsuarioViewModel.ValidaCep();
            msg += UsuarioViewModel.ValidaUF();
            msg += UsuarioViewModel.ValidaCidade();
            msg += UsuarioViewModel.ValidaBairro();
            msg += UsuarioViewModel.ValidaEndereco();
            msg += UsuarioViewModel.ValidaNumero();

            msg += UsuarioViewModel.ValidaLicencaMedicaEUA();
            msg += UsuarioViewModel.ValidaLicencaMedicaEUAEstado();
            msg += UsuarioViewModel.ValidaInformacaoVerdadeira();
            //msg += UsuarioViewModel.ValidaRegulamento();


            ViewBag.Nacionalidade = UsuarioViewModel.nacionalidade;
            ViewBag.LicencaMedicaEUA = UsuarioViewModel.LicencaMedicaEUA;
            ViewBag.InformacaoVerdadeira = (UsuarioViewModel.InformacaoVerdadeira) ? 1 : 0;
            ViewBag.Pais = new SelectList(_usuario.ListaPais().Where(x => x.dsNome != "BRAZIL"), "idPais", "dsNome");


            var idsErro = Session["ErroCadastro"].ToString();
            if (idsErro != "")
                ViewBag.IDsErros = Session["ErroCadastro"].ToString();
            
            Session.Remove("ErroCadastro");

            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.ErroCadastro = Resource.LoginTextoErro + "<br/><br/>" + msg;
                ViewBag.ErroCadastroTitulo = Resource.LoginTtituloErro;
                
                return View("Index", UsuarioViewModel);
            }else if(!UsuarioViewModel.Regulamento){
                ViewBag.ErroCadastro = Resource.CadastroAceitarRegulamento;
                ViewBag.ErroCadastroTitulo = Resource.LoginTtituloErro;

                return View("Index", UsuarioViewModel);
            }


            if (UsuarioViewModel.LicencaMedicaEUA == 1) UsuarioViewModel.NumeroEstado = String.Format("{0} /{1}", UsuarioViewModel.NumeroLicencaMedicaEUA, UsuarioViewModel.Estado);

            if (UsuarioViewModel.nacionalidade == 2)
            {
                UsuarioViewModel.Nome = UsuarioViewModel.NomeEstrangeiro;
                UsuarioViewModel.EmailCadastro = UsuarioViewModel.EmailCadastroEstrangeiro;
                UsuarioViewModel.Telefone = UsuarioViewModel.TelefoneEstrangeiro;
                UsuarioViewModel.Senha = UsuarioViewModel.SenhaEstrangeiro;
            }

            var usuarioCadastro = Mapper.Map<UsuarioCadastroViewModel, UsuarioCadastro>(UsuarioViewModel);
            var usuarioCadastroRetorno = _usuario.Cadastro(usuarioCadastro);

            msg += UsuarioViewModel.ValidaMensagemRetorno(usuarioCadastroRetorno.id);


            if (usuarioCadastroRetorno.id > 0) { 
                //Envia E-mail com login e Senha:
                var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
                Fullbar.Olympus.Dominio.Util.Email _email = new Fullbar.Olympus.Dominio.Util.Email();
                _email.EnviarEmailConfirmacaoCadastro(usuarioCadastro.EmailCadastro, usuarioCadastro.Senha, idioma);
            }


            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.ErroCadastro = Resource.LoginTextoErro + "<br/><br/>" + msg;
                ViewBag.ErroCadastroTitulo = Resource.LoginTtituloErro;                    

                return View("Index", UsuarioViewModel);
            }
            else
            {
                // Verificar se usuario vai ser redirecionado logado

                var usuarioLogin = _usuario.ListaUsuarioPorId(usuarioCadastroRetorno.id);

                FormsAuthentication.SetAuthCookie("loginUsuario", false);

                var cookie = new Cookie();
                cookie.CriarCookie(usuarioLogin);

                var treinamento = cookie.cookieTreinamento();

                if (!string.IsNullOrEmpty(treinamento)) return RedirectToAction("Inscricao", "Treinamento");

            }

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Cadastrar(UsuarioCadastroViewModel UsuarioViewModel)
        {

            var retorno = new RetornoPadrao();
            retorno.btSucesso = true;
            var msg = string.Empty;
            Session.Add("ErroCadastro", "");

            if (UsuarioViewModel.nacionalidade == 1) msg += UsuarioViewModel.ValidaCPF();

            if (UsuarioViewModel.nacionalidade == 1)
            {
                msg += UsuarioViewModel.ValidaNome();
                msg += UsuarioViewModel.ValidaTelefone();
                msg += UsuarioViewModel.ValidaCelular();
                msg += UsuarioViewModel.ValidaEmail();
                msg += UsuarioViewModel.ValidaSenha();
            }

            if (UsuarioViewModel.nacionalidade == 2)
            {
                msg += UsuarioViewModel.ValidaPais();
                msg += UsuarioViewModel.ValidaNomeEstrangeiro();
                msg += UsuarioViewModel.ValidaTelefoneEstrangeiro();
                msg += UsuarioViewModel.ValidaEmailEstrangeiro();
                msg += UsuarioViewModel.ValidaSenhaEstrangeiro();
            }


            msg += UsuarioViewModel.ValidaTituloProfissional();

            if (UsuarioViewModel.nacionalidade == 1)
            {
                msg += UsuarioViewModel.ValidaCNPJ();
            }

            msg += UsuarioViewModel.ValidaNomeInstituicao();
            msg += UsuarioViewModel.ValidaCep();
            msg += UsuarioViewModel.ValidaUF();
            msg += UsuarioViewModel.ValidaCidade();
            msg += UsuarioViewModel.ValidaBairro();
            msg += UsuarioViewModel.ValidaEndereco();
            msg += UsuarioViewModel.ValidaNumero();

            msg += UsuarioViewModel.ValidaLicencaMedicaEUA();
            msg += UsuarioViewModel.ValidaLicencaMedicaEUAEstado();
            msg += UsuarioViewModel.ValidaInformacaoVerdadeira();
            //msg += UsuarioViewModel.ValidaRegulamento();


            ViewBag.Nacionalidade = UsuarioViewModel.nacionalidade;
            ViewBag.LicencaMedicaEUA = UsuarioViewModel.LicencaMedicaEUA;
            ViewBag.InformacaoVerdadeira = (UsuarioViewModel.InformacaoVerdadeira) ? 1 : 0;
            ViewBag.Pais = new SelectList(_usuario.ListaPais().Where(x => x.dsNome != "BRAZIL"), "idPais", "dsNome");


            var idsErro = Session["ErroCadastro"].ToString();
            if (idsErro != "")
            {
                retorno.btSucesso = false;
                retorno.dsMensagem = Session["ErroCadastro"].ToString();
            }

            Session.Remove("ErroCadastro");

            if (!string.IsNullOrEmpty(msg))
            {
                retorno.btSucesso = false;
                retorno.dsTexto = Resource.LoginTextoErro + "<br/><br/>" + msg;
                retorno.dsTitulo = Resource.LoginTtituloErro;

                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            else if (!UsuarioViewModel.Regulamento)
            {
                retorno.btSucesso = false;
                retorno.dsTexto = Resource.CadastroAceitarRegulamento;
                retorno.dsTitulo = Resource.LoginTtituloErro;

                return Json(retorno, JsonRequestBehavior.AllowGet);
            }


            if (UsuarioViewModel.LicencaMedicaEUA == 1) UsuarioViewModel.NumeroEstado = String.Format("{0} /{1}", UsuarioViewModel.NumeroLicencaMedicaEUA, UsuarioViewModel.Estado);

            if (UsuarioViewModel.nacionalidade == 2)
            {
                UsuarioViewModel.Nome = UsuarioViewModel.NomeEstrangeiro;
                UsuarioViewModel.EmailCadastro = UsuarioViewModel.EmailCadastroEstrangeiro;
                UsuarioViewModel.Telefone = UsuarioViewModel.TelefoneEstrangeiro;
                UsuarioViewModel.Senha = UsuarioViewModel.SenhaEstrangeiro;
            }

            var usuarioCadastro = Mapper.Map<UsuarioCadastroViewModel, UsuarioCadastro>(UsuarioViewModel);
            var usuarioCadastroRetorno = _usuario.Cadastro(usuarioCadastro);

            msg += UsuarioViewModel.ValidaMensagemRetorno(usuarioCadastroRetorno.id);


            if (usuarioCadastroRetorno.id > 0)
            {
                retorno.dsTitulo = Fullbar.Olympus.MVC.Resource.PaginaCadastro;
                retorno.dsMensagem = Fullbar.Olympus.MVC.Resource.MensagemRetornoCadastro;

                //Envia E-mail com login e Senha:
                var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
                Fullbar.Olympus.Dominio.Util.Email _email = new Fullbar.Olympus.Dominio.Util.Email();
                _email.EnviarEmailConfirmacaoCadastro(usuarioCadastro.EmailCadastro, usuarioCadastro.Senha, idioma);
            }


            if (!string.IsNullOrEmpty(msg))
            {
                retorno.btSucesso = false;
                retorno.dsTexto = Resource.LoginTextoErro + "<br/><br/>" + msg;
                retorno.dsTitulo = Resource.LoginTtituloErro;

                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Verificar se usuario vai ser redirecionado logado

                var usuarioLogin = _usuario.ListaUsuarioPorId(usuarioCadastroRetorno.id);

                FormsAuthentication.SetAuthCookie("loginUsuario", false);

                var cookie = new Cookie();
                cookie.CriarCookie(usuarioLogin);

                var treinamento = cookie.cookieTreinamento();

                if (!string.IsNullOrEmpty(treinamento))
                {
                    retorno.treinamento = true;
                    return Json(retorno, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(retorno, JsonRequestBehavior.AllowGet);

        }

        public JsonResult BuscaCNPJ(string cnpj)
        {
            var retornoCNPJ = _usuario.BuscaCNPJ(cnpj);

            retornoCNPJ.dsMensagem = Resource.CNPJNaoEncontrado;
            retornoCNPJ.dsTitulo = Fullbar.Olympus.MVC.Resource.LoginTtituloErro;

            return Json(retornoCNPJ, JsonRequestBehavior.AllowGet);
        }


        

    }
}

