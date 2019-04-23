using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class ConteudoEducacional
    {
        public int idConteudo { get; set; }
        public int idIdioma { get; set; }
        public int idTipo { get; set; }
        public string dsTitulo { get; set; }
        public string dsLink { get; set; }
        public string dsArquivo { get; set; }
        public bool btAtivo { get; set; }
        public DateTime dtOperacao { get; set; }
        
        public string dsNome { get; set; }
        public string dsSite { get; set; }
    }
}
