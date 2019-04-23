using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoInscricao
    {
        public int idUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int idTreinamento { get; set; }
        public bool hospedagemPassagem { get; set; }
        

        public List<TreinamentoConvite> Convites { get; set; }

    }

}
