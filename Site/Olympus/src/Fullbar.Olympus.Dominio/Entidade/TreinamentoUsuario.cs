using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoUsuario
    {
        public int idTreinamento { get; set; }
        public int idStatusInscricao { get; set; }
        public string dsNome { get; set; }
        public string dsImagem { get; set; }
        public string dsEndereco { get; set; }
        public string dsNumero { get; set; }
        public string dsBairro { get; set; }
        public string dsCidade { get; set; }
        public string dsUF { get; set; }
        public string DataTreinamento { get; set; }
        public DateTime dtTreinamento { get; set; }
        public DateTime dtTreinamentoFim { get; set; }
        public int idDivisao { get; set; }

        public string dsMes { get; set; }
        public string dsDia { get; set; }
        public string dsMesFim { get; set; }
        public string dsDiaFim { get; set; }

        public bool btConvidado { get; set; }
        
        public bool btMaterial { get; set; }
        public string dsMaterial { get; set; }

        public bool btQuestionarioPre { get; set; }
        public bool btQuestionarioPreRespondido { get; set; }
        public bool btQuestionarioPos { get; set; }
        public bool btQuestionarioPosRespondido { get; set; }
        public bool btCertificadoIndicado { get; set; }

        public List<TreinamentoAgenda> Agenda { get; set; }
   
    }
}
