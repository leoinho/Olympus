using Fullbar.Olympus.Dominio.Repositorio;
using Fullbar.Olympus.MVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;

namespace Fullbar.Olympus.MVC.Helpers
{
    public static class Home
    {
        public static MvcHtmlString BoxLogin(this HtmlHelper html)
        {
            var usuario = new Cookie().cookieUsuario();
            var box = new StringBuilder();

            if (usuario == null)
            {

                box.Append("<div class='usuario-deslogado'>");
                box.Append("<input type='text' name='Email' id='' class='form-control inp-contato' placeholder='Login' value=''/>");
                box.Append(string.Format("<input type='password' name='senha' id='senha' class='form-control inp-contato' placeholder='{0}' value='' />", Resource.Senha));
                box.Append(string.Format("<div class='pad-top-20'> <input type='submit' value='{0}'  role='button' class='btn btn-lg btn-primary btn-entrar' id='entrar' /></div>", Resource.btLogar));
                box.Append(string.Format("<p class='pad-top-20'><a href='javascript:void(0)' id='aEsqueciSenha'>{0}</a></p>", Resource.HomeEsqueciSenha));
                box.Append(string.Format("<p><a href='{0}'>{1}</a></p>", Resource.PaginaCadastro, Resource.HomeAindaNaoSouCadastrado));
                box.Append("</div>");

            }
            else
            {

                box.Append("<div class='usuario-logado'>");
                box.Append("<div class='col-md-12'>");
                box.Append(string.Format("<h4>{0}</h4>", usuario.dsNomeCompleto));
                box.Append("</div>");
                box.Append("<div class='col-md-12 pad-bot-20'>");
                box.Append("<div class='col-md-5 zera-pad'>");
                box.Append(string.Format("<p><a href='/{0}'>{1}</a></p>", Resource.PaginaEditar, Resource.labelMeuPerfil));
                box.Append(string.Format("<p><a href='/Home/Sair'>{0}</a></p>", Resource.btSair));
                box.Append("</div>");

                box.Append("<div class='col-md-7 zera-pad'>");
                box.Append(string.Format("<p><a href='/{0}/{1}'>{2}</a></p>", Resource.PaginaTreinamentoController, Resource.PaginaTreinamentoActionMeusTreinamentos, Resource.HomeMeusTreinamentos));
                box.Append("</div>");

                box.Append("</div>");
                box.Append("</div>");



            }

            return MvcHtmlString.Create(box.ToString());
        }

        public static MvcHtmlString MenuTreinamento(this HtmlHelper html)
        {

            var cookieListaCategoria = new Cookie();
            var strCategoria = new StringBuilder();

            if (cookieListaCategoria.cookieMenuTreinamento() == null)
            {
                var listaCategoria = RepositorioDivisaoTreinamento.Lista();


                foreach (var item in listaCategoria)
                {
                    //strCategoria.Append("<div class='col-sm-6 text-center'>");
                    //strCategoria.Append(string.Format("<a href='{0}/{1}/{2}'>{3}</a>", Resource.PaginaTreinamento, Resource.PaginaTreinamentoActionCategoria, item.idCategoria, item.dsNome));
                    //strCategoria.Append("</div>");
                }

                cookieListaCategoria.CriarCookieMenuTreinamento(strCategoria.ToString());
            }
            else
            {
                return MvcHtmlString.Create(cookieListaCategoria.cookieMenuTreinamento().ToString());
            }

            return MvcHtmlString.Create(strCategoria.ToString());

        }
    }
}



