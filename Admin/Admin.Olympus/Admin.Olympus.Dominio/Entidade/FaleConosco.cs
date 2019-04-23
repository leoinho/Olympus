using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class FaleConosco
    {
        public int idFaleConosco { get; set; }
        public int idUsuario { get; set; }
        public string dsAssunto { get; set; }
        public string dsNomeCompleto { get; set; }
        public string dsEmail { get; set; }
        public string dsTelefone { get; set; }
        public string dsMensagem { get; set; }
        public string dsContato { get; set; }
        public DateTime dtCadastro { get; set; }

        public string dsInstituicao { get; set; }
        public string dsCPF { get; set; }
    }
}
