using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoConvite
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Celular { get; set; }

        public string dsNome { get; set; }
        public string dsEmail { get; set; }
        public string dsCPF { get; set; }
        public string dsCelular { get; set; }
        public int idStatus { get; set; }
        public int idUsuarioConvidado { get; set; }
        public int idTreinamento { get; set; }
        public string dsTreinamento { get; set; }
    }
}
