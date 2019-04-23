using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fullbar.Olympus.Dominio.Util
{
    public static class Geral
    {
        static Regex RegExEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
        //new Regex(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$");

        // Remove todas as aspas simples de uma string para evitar SQL injection
        public static string TiraAspas(string s, bool bUpper)
        {

            if (bUpper == true)
            {
                return s.Trim().Replace("'", "").ToUpper();
            }
            else
            {
                return s.Trim().Replace("'", "");
            }
        }
        public static string formatErro(string s)
        {
            return s.Trim().Replace("'", "").Replace(" \n", "<br/>").Replace("\r\n", "<br/>");
        }
        public static string formatSaldo(Decimal valor)
        {
            string retorno = "";
            int len = valor.ToString("f2").Length;


            if (len >= 7)
            {
                retorno = valor.ToString("f2");
            }
            else
            {
                retorno = valor.ToString("f2");

                for (int i = len; i < 7; i++)
                {
                    retorno = "0" + retorno;
                }
            }

            return retorno;
        }
        public static string formatOrdem(int valor)
        {
            string retorno = "";
            int len = valor.ToString().Length;


            if (len >= 7)
            {
                retorno = valor.ToString();
            }
            else
            {
                retorno = valor.ToString();

                for (int i = len; i < 7; i++)
                {
                    retorno = "0" + retorno;
                }
            }

            return retorno;
        }
        public static string formatLogin(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            else
            {
                return s.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }
        public static string formatCNPJ(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            else
            {
                return s.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }
        public static string formatCNPJ2(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            else
            {
                return s.Trim().Substring(0, 2) + "." + s.Trim().Substring(2, 3) + "." + s.Trim().Substring(5, 3) + "/" + s.Trim().Substring(8, 4) + "-" + s.Trim().Substring(12, 2);
            }
        }
        public static string formatNome(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            else
            {
                string[] a = s.Trim().Split(' ');

                foreach (var item in a)
                {
                    return item;
                }

                return s.Trim();
            }
        }

       
        public static bool isEmail(string sEmail)
        {
            return RegExEmail.Match(sEmail).Success;
        }
        public static bool isCNPJ(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
        public static bool isCPF(string CPF)
        {
            if (CPF.Trim().Length == 0)
                return false;

            int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string TempCPF;
            string Digito;
            int soma;
            int resto;

            CPF = CPF.Trim();
            CPF = CPF.Replace(".", "").Replace("-", "");

            if (CPF.Length != 11)
                return false;

            if (CPF == "11111111111" || CPF == "22222222222" || CPF == "33333333333" || CPF == "44444444444" || CPF == "55555555555" || CPF == "66666666666" || CPF == "77777777777" || CPF == "88888888888" || CPF == "99999999999")
                return false;

            TempCPF = CPF.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = resto.ToString();
            TempCPF = TempCPF + Digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = Digito + resto.ToString();

            return CPF.EndsWith(Digito);
        }
        public static bool isRG(string par_rg)
        {
            par_rg = par_rg.ToUpper().Replace("-", "").Replace(".", "");

            if (par_rg.Trim().Length < 9)
                return false;

            int rg = Convert.ToInt32(par_rg.Substring(0, 8)), d1 = 0, d2 = 0, d3 = 0, d4 = 0, d5 = 0, d6 = 0, d7 = 0, d8 = 0;
            int md1 = 0, md2 = 0, md3 = 0, md4 = 0, md5 = 0, md6 = 0, md7 = 0, md8 = 0;
            int m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, mt = 0, dv;

            d1 = rg / 10000000;
            m1 = d1 * 9;
            md1 = rg % 10000000;
            d2 = md1 / 1000000;
            m2 = d2 * 8;
            md2 = md1 % 1000000;
            d3 = md2 / 100000;
            m3 = d3 * 7;
            md3 = md2 % 100000;
            d4 = md3 / 10000;
            m4 = d4 * 6;
            md4 = md3 % 10000;
            d5 = md4 / 1000;
            m5 = d5 * 5;
            md5 = md4 % 1000;
            d6 = md5 / 100;
            m6 = d6 * 4;
            md6 = md5 % 100;
            d7 = md6 / 10;
            m7 = d7 * 3;
            md7 = md6 % 10;
            d8 = md7 / 1;
            m8 = d8 * 2;
            md8 = md7 % 1;
            mt = m1 + m2 + m3 + m4 + m5 + m6 + m7 + m8;
            dv = mt % 11;
            string Digito;
            if (dv == 10)
                Digito = "X";
            else
                Digito = dv.ToString();

            return par_rg.EndsWith(Digito);
        }

        public static void showModal(string idModal, string idTitulo, string titulo, string idTexto, string texto, Literal literal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            if (idTitulo.Length > 0)
                sb.Append(" $('#" + idTitulo + "').text('" + titulo + "'); ");

            if (idTexto.Length > 0)
                sb.Append(" $('#" + idTexto + "').html('" + formatErro(texto.Replace(" \n", "<br/>")) + "'); ");

            sb.Append(" $('#" + idModal + "').modal('show'); ");

            sb.Append(@"</script>");

            literal.Text = sb.ToString();
        }
        public static void showModalSite(string idModal, string idTitulo, string titulo, string idTexto, string texto, Literal literal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append("var id = $(this).attr('href');");

            sb.Append("var maskHeight = $(document).height();");
            sb.Append("var maskWidth = $(window).width();");

            sb.Append("$('#mask').css({ 'width': maskWidth, 'height': maskHeight });");

            sb.Append("$('#mask').fadeIn(1000);");
            sb.Append("$('#mask').fadeTo('slow', 0.8);");

            sb.Append("var winW = $(window).width();");

            sb.Append("$('#" + idModal + "').css('top', 150);");
            sb.Append("$('#" + idModal + "').css('left', winW / 2 - $('#" + idModal + "').width() / 2);");
            sb.Append("$('#" + idModal + "').fadeIn(2000);");

            if (idTitulo.Length > 0)
                sb.Append(" $('#" + idTitulo + "').html('" + titulo + "'); ");

            if (idTexto.Length > 0)
                sb.Append(" $('#" + idTexto + "').html('" + formatErro(texto.Replace(" \n", "<br/>")) + "<br/><br/><br/>'); ");

            sb.Append("$('#" + idModal + "').show();");

            sb.Append(@"</script>");

            literal.Text = sb.ToString();
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        // Verifica se duas string são iguais desconsiderando diferenças de maiúsculas e minúsculas, acentuação e espaços.
        public static bool CompareStrings(string str1, string str2)
        {
            bool result = false;
            string s1 = RemoveDiacritics(str1.Replace(" ", "").ToUpper());
            string s2 = RemoveDiacritics(str2.Replace(" ", "").ToUpper());

            s1 = s1.Replace("M", "N");
            s2 = s2.Replace("M", "N");

            if (s1 == s2) result = true;
            return result;
        }

        public static void HabilitaDesabilitaControls(Control Controle, bool habilitado)
        {
            foreach (Control c in Controle.Controls)
            {
                if (c is TextBox)
                {
                    TextBox t = (TextBox)c;
                    t.ReadOnly = !habilitado;
                }
                else if (c.Controls.Count > 0)
                {
                    HabilitaDesabilitaControls(c, habilitado);
                }
            }
            foreach (Control c in Controle.Controls)
            {
                if (c is DropDownList)
                {
                    DropDownList d = (DropDownList)c;
                    d.Enabled = habilitado;
                }
                else if (c.Controls.Count > 0)
                {
                    HabilitaDesabilitaControls(c, !habilitado);
                }
            }
            foreach (Control c in Controle.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton r = (RadioButton)c;
                    r.Enabled = habilitado;
                }
                else if (c.Controls.Count > 0)
                {
                    HabilitaDesabilitaControls(c, !habilitado);
                }
            }
            foreach (Control c in Controle.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox ch = (CheckBox)c;
                    ch.Enabled = habilitado;
                }
                else if (c.Controls.Count > 0)
                {
                    HabilitaDesabilitaControls(c, !habilitado);
                }
            }
            foreach (Control c in Controle.Controls)
            {
                if (c is RadioButtonList)
                {
                    RadioButtonList rbl = (RadioButtonList)c;
                    rbl.Enabled = habilitado;
                }
                else if (c.Controls.Count > 0)
                {
                    HabilitaDesabilitaControls(c, !habilitado);
                }
            }
            foreach (Control c in Controle.Controls)
            {
                if (c is CheckBoxList)
                {
                    CheckBoxList chl = (CheckBoxList)c;
                    chl.Enabled = habilitado;
                }
                else if (c.Controls.Count > 0)
                {
                    HabilitaDesabilitaControls(c, !habilitado);
                }
            }

        }

        public static void cria_cookie(string nomecookie, string valor)
        {
            cria_cookie(nomecookie, valor, 1);
        }
        public static void cria_cookie(string nomecookie, string valor, int minutos)
        {
            //Cria a estancia do obj HttpCookie passando o nome do mesmo

            System.Web.HttpCookie cookie = new System.Web.HttpCookie(nomecookie);

            //Define o valor do cookie

            cookie.Value = valor;

            //Time para expiração (1min)

            DateTime dtNow = DateTime.Now;

            TimeSpan tsMinute = new TimeSpan(0, 0, minutos, 0);

            cookie.Expires = dtNow + tsMinute;

            //Adiciona o cookie

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string recupera_cookie(string nomecookie)
        {

            //Cria o obj cookie e recebe o mesmo pelo obj Request

            System.Web.HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[nomecookie];

            //Imprime o valor do cookie

            //System.Web.HttpContext.Current.Response.Write(cookie.Value.ToString());

            return cookie.Value.ToString();
        }

        public static void LimpaCamposTexto(Control controle)
        {
            foreach (Control item in controle.Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = string.Empty;
                }
                else if (item.Controls.Count > 0)
                {
                    LimpaCamposTexto(item);
                }
            }
        }
    }

}
