using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Participacao
    {
        public int idInscricao { get; set; }
        public int idTreinamento { get; set; }
        public int idUsuario { get; set; }

        public string dsNomeCompleto { get; set; }
        public string dsCPF { get; set; }
        public string dsTelefone { get; set; }
        public string dsCelular { get; set; }
        public string dsEmail { get; set; }

        public string dsInstituicao { get; set; }
        public string dsNomeFantasia { get; set; }
        public string dsCNPJ { get; set; }

        public string dsCodigo { get; set; }
        public string dsTreinamento { get; set; }
        public string dsDtTreinamento { get; set; }
        public DateTime dtTreinamento { get; set; }
        public string dsDescricao { get; set; }
        public string dsHoraInicio { get; set; }
        public string dsHoraFim { get; set; }

        public string dsPalestrante { get; set; }
        public string dsLocal { get; set; }

        public DateTime? dtEmail { get; set; }
        public DateTime? dtSMS { get; set; }

        public int idAdministrador { get; set; }
        public string dsEstado { get; set; }
        public DateTime dtCadastro { get; set; }

        public int idStatusInscricao { get; set; }
        public string dsStatusInscricao { get; set; }
        public int idStatusInscricaoAltera { get; set; }
        

        public string dsAction { get; set; }

        public string dsCertificado { get; set; }

        public bool btNovaEmpresa { get; set; }
        public int idPesquisa { get; set; }
        public string dsCurriculo { get; set; }

        public string dsPassagem { get; set; }
        public string dsHospedagem { get; set; }

        //CONTROLA STATUS DE INSCRIÇÕES
        public int inTipo { get; set; }
    }
}
