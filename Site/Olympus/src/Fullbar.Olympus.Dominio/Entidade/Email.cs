using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class Email
    {
        public int idAgendamento { get; set; }
        public int idLayout { get; set; }
        public string dsAssunto { get; set; }

        public string dsArquivoHTML { get; set; }
        public string dsNomeCompleto { get; set; }
        public string dsCPF { get; set; }
        public string dsCNPJ { get; set; }
        public string dsTelefone { get; set; }
        public int idTreinamento { get; set; }
        public string dsNomeTreinamento { get; set; }
        public DateTime dtTreinamento { get; set; }
        public string dsEndereco { get; set; }
        public string dsNumero { get; set; }
        public string dsBairro { get; set; }
        public string dsCidade { get; set; }
        public string dsLocalTreinamento { get; set; }

        public string dsUF { get; set; }
        public string dsCEP { get; set; }
        public string dsPais { get; set; }

        public string dsEmail { get; set; }

        public string dsTipo { get; set; }
        public int idIdioma { get; set; }

        public string dsDataTreinamento { get; set; }

        public bool btCadastrado { get; set; }
        public bool btTemConvidado { get; set; }

        public string dsLatitude { get; set; }
        public string dsLongitude { get; set; }

        //Convidados
        public string dsNomeConvidante { get; set; }
        public int idInscricaoUsuarioConvidante { get; set; }
        public int idUsuario { get; set; }
        public int idStatus { get; set; }
    }
}
