using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoAlternativaCadastro
    {
        public int idTreinamento { get; set; }
        public int idPergunta { get; set; }
        public string dsDescricao { get; set; }
        public bool btCorreta { get; set; }

    }
}
