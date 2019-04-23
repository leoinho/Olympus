using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;


namespace Fullbar.Olympus.MVC.Util
{
    public class Cookie
    {
        public void CriarCookie(Usuario usuario)
        {
            //HttpCookie cookie = new HttpCookie("SITE");
            //var usuarioJson = new JavaScriptSerializer().Serialize(usuario);

            //cookie.Values.Add("USUARIO", usuarioJson);

            ////colocando o cookie para expirar
            //HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");
            //cookie.Expires = DateTime.Now.AddDays(1);
            //HttpContext.Current.Response.AppendCookie(cookie);

            HttpContext.Current.Session["USUARIO"] = usuario;

        }

        public void CriarCookieMenuTreinamento(string categoria)
        {
            HttpContext.Current.Session["CATEGORIA"] = categoria;
        }

        public void CriarCookieIdTreinamento(string id)
        {
            HttpContext.Current.Session["TREINAMENTO"] = id;
        }

        public string ReadCookieMenuTreinamento()
        {
            try
            {
                return HttpContext.Current.Session["CATEGORIA"] as string;
            }
            catch
            {
                return null;
            }
        }

        public string ReadCookieTreinamento()
        {
            try
            {
                //return HttpContext.Current.Request.Cookies["TREINAMENTO"];
                return HttpContext.Current.Session["TREINAMENTO"] as string;
            }
            catch
            {
                return null;
            }
        }

        public Usuario ReadCookie()
        {
            try
            {

                return HttpContext.Current.Session["Usuario"] as Usuario;
            }
            catch
            {
                return null;
            }
        }

        public Usuario cookieUsuario()
        {

            //HttpCookie cookieRead = ReadCookie();

            var cookieRead = ReadCookie();
            //var usuario = new Usuario();

            if (cookieRead != null)
            {
                //var usuarioJson = cookieRead["USUARIO"].ToString();
                //usuario = new JavaScriptSerializer().Deserialize<Usuario>(usuarioJson);

                return cookieRead;
            }

            return null;
        }

        public string cookieTreinamento()
        {

            //HttpCookie cookieRead = ReadCookieTreinamento();
            var cookieRead = ReadCookieTreinamento();

            if (cookieRead != null)
            {
                //var idTreinamento = cookieRead["idTreinamento"].ToString();
                var idTreinamento = cookieRead.ToString();
                return idTreinamento;
            }

            return string.Empty;
        }

        public string cookieMenuTreinamento()
        {

            var cookieRead = ReadCookieMenuTreinamento();

            if (cookieRead != null)
            {
                var menuTreinamento = cookieRead.ToString();
                return menuTreinamento;
            }

            return null;
        }

        public void LimpaCookieTreinamento()
        {
            HttpContext.Current.Session.Remove("TREINAMENTO");
        }

        public void CriarCookieTreinamentoConvite(List<TreinamentoConvite> convites)
        {
            HttpContext.Current.Session["TREINAMENTOCONVITE"] = convites;
        }

        public List<TreinamentoConvite> ReadCookieTreinamentoConvite()
        {
            try
            {
                return HttpContext.Current.Session["TREINAMENTOCONVITE"] as List<TreinamentoConvite>;
            }
            catch
            {
                return null;
            }
        }

        public List<TreinamentoConvite> cookieTreinamentoConvite()
        {
            var cookieRead = ReadCookieTreinamentoConvite();

            if (cookieRead != null)
            {
                var listaTreinamentoConvite = cookieRead;
                return listaTreinamentoConvite;
            }

            return null;
        }

        public void LimpaCookieListaTreinamento()
        {
            HttpContext.Current.Session.Remove("TREINAMENTOCONVITE");
        }
    }
}
