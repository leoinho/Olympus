using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
   public class SMS
    {
        public string dsNomeCompleto { get; set; }
        public string dsTelefone { get; set; }
        public int idAgendamento { get; set; }
        public string dsTipo { get; set; }

        public int idLog { get; set; }
        public string dsCodigo { get; set; }
   }
}
