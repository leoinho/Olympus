using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class Divisao
    {
        public int idDivisao { get; set; }
        public string dsNome { get; set; }

        public List<Categoria> Categorias { get; set; }
    }
}
