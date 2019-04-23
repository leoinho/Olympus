using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class Login
    {
        public string Email { get; set; }
        public string Senha { get; set; }

        public Login(string email, string senha)
        {
            this.Email = email;
            this.Senha = senha;
        }
    }
}
