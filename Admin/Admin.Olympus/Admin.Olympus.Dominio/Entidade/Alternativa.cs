using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Alternativa
    {
        public int idAlternativa { get; set; }
        public int idPergunta { get; set; }
        public string dsDescricao { get; set; }
        public bool btCorreta { get; set; }
        public DateTime dtCadastro { get; set; }
        public bool btExcluir { get; set; }


        public bool btCorretaUsuario { get; set; }
        public bool btResposta { get; set; }
    }
}
