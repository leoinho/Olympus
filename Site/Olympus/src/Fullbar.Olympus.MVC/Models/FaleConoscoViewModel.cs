using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fullbar.Olympus.MVC.Models
{
    public class FaleConoscoViewModel
    {
        public int idUsuario { get; set; }
        public string Assunto { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Mensagem { get; set; }
        public string CPF { get; set; }
        public int Nacionalidade { get; set; }
        public string Pais { get; set; }
        public string Contato { get; set; }

        public string ValidaCPF()
        {

            if (string.IsNullOrEmpty(this.CPF) && this.Nacionalidade == 1)
            {
                return "CPF \n";
            }

            return string.Empty;
        }


        public string ValidaNome()
        {

            if (string.IsNullOrEmpty(this.Nome) || contemNumeros(this.Nome))
            {
                return Resource.PaginaContatoNomeCompleto + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaTelefone()
        {
            var _fone = this.Telefone;

            if (_fone != null)
                _fone = _fone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (string.IsNullOrEmpty(this.Telefone) || _fone.Length < 10 || _fone.Substring(0, 1) == "0")
            {
                return Resource.PaginaContatoTelefone + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaEmail()
        {

            if (String.IsNullOrEmpty(this.Email))
            {
                return Fullbar.Olympus.MVC.Resource.EmailCadastroObrigatorio + "<br/>";
            }
            else
            {
                bool isEmail = Regex.IsMatch(this.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                {
                    return Fullbar.Olympus.MVC.Resource.EmailInvalido + "<br/>";
                }
            }
            return string.Empty;
        }

        public string ValidaAssunto()
        {

            if (string.IsNullOrEmpty(this.Assunto))
            {
                return Resource.PaginaContatoAssunto + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaMensagem()
        {

            if (string.IsNullOrEmpty(this.Mensagem))
            {
                return Resource.PaginaContatoMensagem + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaPais()
        {

            if (string.IsNullOrEmpty(this.Pais))
            {
                return Resource.PaginaContatoContato + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaContato()
        {

            if (string.IsNullOrEmpty(this.Contato))
            {
                return Resource.PaginaContatoContato + "<br/>";
            }
            return string.Empty;
        }


        public bool contemNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }

    }
}