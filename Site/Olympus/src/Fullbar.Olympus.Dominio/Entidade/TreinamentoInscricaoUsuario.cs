using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoInscricaoUsuario
    {
        public string Nome { get; set; }
        public string Treinamento { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string Data { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }

        //CERTIFICADO
        public DateTime DataTreinamento { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }

    }
}
