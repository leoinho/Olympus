using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fullbar.Olympus.MVC.Models
{
    public class LoginViewModel
    {
       
        public string Email { get; set; }
        public string Senha { get; set; }


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

        public string ValidaSenha()
        {
            if (string.IsNullOrEmpty(this.Senha))
            {
                return Fullbar.Olympus.MVC.Resource.SenhaCadastroObrigatorio + "<br/>";
            }
            return string.Empty;
        }


    }
}
