using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
   public  class TreinamentoInscricaoResultadoConfirmado
    {
        public int idConvite { get; set; }
        public string dsCPF { get; set; }
        public string dsNome { get; set; }
        public string dsEmail { get; set; }
        public string dsStatus { get; set; }
        public int idStatus { get; set; }
    }
}
