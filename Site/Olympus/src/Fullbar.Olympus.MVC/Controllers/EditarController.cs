using AutoMapper;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.MVC.Models;
using Fullbar.Olympus.MVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fullbar.Olympus.MVC.Controllers
{
    public class EditarController : BaseController
    {
        private readonly IUsuario _usuario;
        private readonly IPagina _pagina;

        public EditarController(IUsuario usuario, IPagina pagina)
        {
            _usuario = usuario;
            _pagina = pagina;
        }

        [Authorize]
        public ActionResult Index()
        {
            var usuarioCookie = new Cookie().cookieUsuario();

            if (usuarioCookie == null) return RedirectToAction("Index", "Home");


            var usuarioModel = _usuario.ListaEditar(usuarioCookie.idusuario);
            var usuarioViewModel = Mapper.Map<UsuarioEditar, UsuarioEditarViewModel>(usuarioModel);

            if (usuarioViewModel.LicencaMedicaEUA == 1)
            {
                var numeroEstado = usuarioViewModel.NumeroEstado.Split('/');

                if (numeroEstado.Count() > 1)
                {

                    usuarioViewModel.NumeroLicencaMedicaEUA = numeroEstado[0].ToString();
                    usuarioViewModel.Estado = numeroEstado[1].ToString();
                }

            }

            if (usuarioViewModel.idNacionalidade == 2)
            {
                usuarioViewModel.NomeEstrangeiro = usuarioCookie.dsNomeCompleto;
                usuarioViewModel.TelefoneEstrangeiro = usuarioCookie.dsTelefone;
            }



            ViewBag.LicencaMedica = usuarioViewModel.LicencaMedicaEUA;

            ViewBag.Sucesso = TempData["mensagem"];
            ViewBag.ErroEditarTitulo = TempData["mensagemTitulo"];

            ViewBag.idNacionalidade = usuarioViewModel.idNacionalidade;
            ViewBag.LicencaMedicaEUA = usuarioViewModel.LicencaMedicaEUA;
            ViewBag.TituloProfissionalOutros = usuarioViewModel.TituloProfissionalOutros;
            ViewBag.TituloProfissional = usuarioViewModel.TituloProfissional;
            ViewBag.idUsuario = usuarioViewModel.idUsuario;

            ViewBag.Email = usuarioViewModel.Email;

            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 3);


            return View(usuarioViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Index(UsuarioEditarViewModel usuarioEditarViewModel)
        {

            var msg = string.Empty;

            if (usuarioEditarViewModel.idNacionalidade == 1)
            {
                msg += usuarioEditarViewModel.ValidaNome();
                msg += usuarioEditarViewModel.ValidaTelefone();
                msg += usuarioEditarViewModel.ValidaCelular();
                msg += usuarioEditarViewModel.ValidaEmail();
                msg += usuarioEditarViewModel.ValidaSenha();
            }

            if (usuarioEditarViewModel.idNacionalidade == 2)
            {
                msg += usuarioEditarViewModel.ValidaNomeEstrangeiro();
                msg += usuarioEditarViewModel.ValidaTelefoneEstrangeiro();
                msg += usuarioEditarViewModel.ValidaEmailEstrangeiro();
                msg += usuarioEditarViewModel.ValidaSenhaEstrangeiro();
            }

            msg += usuarioEditarViewModel.ValidaTituloProfissional();
            msg += usuarioEditarViewModel.ValidaCNPJ();
            msg += usuarioEditarViewModel.ValidaNomeInstituicao();
            msg += usuarioEditarViewModel.ValidaCep();
            msg += usuarioEditarViewModel.ValidaUF();
            msg += usuarioEditarViewModel.ValidaCidade();
            msg += usuarioEditarViewModel.ValidaBairro();
            msg += usuarioEditarViewModel.ValidaEndereco();
            msg += usuarioEditarViewModel.ValidaLicencaMedicaEUA();
            msg += usuarioEditarViewModel.ValidaLicencaMedicaEUAEstado();






            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.ErroEditar = Resource.LoginTextoErro + "<br/><br/>" + msg;
                ViewBag.ErroEditarTitulo = Resource.LoginTtituloErro;

                ViewBag.idNacionalidade = usuarioEditarViewModel.idNacionalidade;
                ViewBag.LicencaMedicaEUA = usuarioEditarViewModel.LicencaMedicaEUA;
                ViewBag.TituloProfissionalOutros = usuarioEditarViewModel.TituloProfissionalOutros;
                ViewBag.TituloProfissional = usuarioEditarViewModel.TituloProfissional;
                ViewBag.idUsuario = usuarioEditarViewModel.idUsuario;
                ViewBag.Email = usuarioEditarViewModel.Email;

                return View(usuarioEditarViewModel);
            }

            if (usuarioEditarViewModel.LicencaMedicaEUA == 1) usuarioEditarViewModel.NumeroEstado = String.Format("{0} /{1}", usuarioEditarViewModel.NumeroLicencaMedicaEUA, usuarioEditarViewModel.Estado);

            if (usuarioEditarViewModel.idNacionalidade == 2)
            {
                usuarioEditarViewModel.Nome = usuarioEditarViewModel.NomeEstrangeiro;
                usuarioEditarViewModel.Email = usuarioEditarViewModel.EmailCadastroEstrangeiro;
                usuarioEditarViewModel.Telefone = usuarioEditarViewModel.TelefoneEstrangeiro;
                usuarioEditarViewModel.Senha = usuarioEditarViewModel.SenhaEstrangeiro;
            }

            var usuarioEditar = Mapper.Map<UsuarioEditarViewModel, UsuarioEditar>(usuarioEditarViewModel);
            var retornoEditar = _usuario.Editar(usuarioEditar);

            if (retornoEditar.id > 0)
            {


                Session.Remove("USUARIO");

                var usuario = _usuario.ListaUsuarioPorId(retornoEditar.id);
                var cookie = new Cookie();
                cookie.CriarCookie(usuario);

                TempData["mensagem"] = Resource.PaginaEidtarCadastroMensagem;
                TempData["mensagemTitulo"] = Resource.LoginTtituloErro;
            }
            else
            {
                TempData["mensagem"] = usuarioEditarViewModel.ValidaMensagemRetorno(retornoEditar.id);
                TempData["mensagemTitulo"] = Resource.LoginTtituloErro;
            }

            return RedirectToAction("Index");

        }

        public JsonResult BuscaCNPJ(string cnpj)
        {
            var retornoCNPJ = _usuario.BuscaCNPJ(cnpj);

            retornoCNPJ.dsMensagem = Resource.CNPJNaoEncontrado;

            return Json(retornoCNPJ, JsonRequestBehavior.AllowGet);
        }
    }
}
