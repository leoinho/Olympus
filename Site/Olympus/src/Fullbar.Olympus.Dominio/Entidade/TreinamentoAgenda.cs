using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
   public class TreinamentoAgenda
    {
        public int idAgenda { get; set; }
        public int idTreinamento { get; set; }
        public int inDia { get; set; }
        public string dsHorario { get; set; }
        public string dsAtividade { get; set; }
        public string dsPalestrante { get; set; }

    }
}
