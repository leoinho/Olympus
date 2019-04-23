using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoCalendario
    {
        public int idTreinamento { get; set; }
        public string dsNome { get; set; }
        public DateTime dtTreinamento { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Idioma { get; set; }
        public string DataTreinamento { get; set; }
        public int idStatus { get; set; }
        public int idStatusTreinamento { get; set; }
        public string dsStatusTreinamento { get; set; }
        public int idStatusInscricao { get; set; }
        public string dsStatusInscricao { get; set; }
        public string dsUrl { get; set; }

    
    }
}
