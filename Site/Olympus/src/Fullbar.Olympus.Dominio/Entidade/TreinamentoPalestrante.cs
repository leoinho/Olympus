using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoPalestrante
    {
        public int idPalestrante { get; set; }
        public int idTreinamento { get; set; }
        public string dsNome { get; set; }
        public string dsPerfil { get; set; }
        public string dsImagem { get; set; }
    }
}
