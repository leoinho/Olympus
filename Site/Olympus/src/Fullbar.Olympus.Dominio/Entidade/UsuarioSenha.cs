using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
   public class UsuarioSenha
    {
        public int idUsuario { get; set; }
        public string dsSenha { get; set; }
        public string dsNomeCompleto { get; set; }
        public string dsEmail { get; set; }
    }
}
