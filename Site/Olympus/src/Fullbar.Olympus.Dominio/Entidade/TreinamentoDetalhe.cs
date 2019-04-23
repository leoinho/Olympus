using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoDetalhe
    {
        public int idTreinamento { get; set; }
        public int idStatus { get; set; }
        public int idAdministrador { get; set; }
        public int idDivisao { get; set; }
        public int idCategoria { get; set; }
        public string dsNome { get; set; }
        public string dsCodigo { get; set; }
        public DateTime dtTreinamento { get; set; }
        public int inQuantidadeTotalVagas { get; set; }
        public int inQuantidadeVagas { get; set; }
        public int inQuantidadeVagasPorCNPJ { get; set; }
        public DateTime dtInscricaoInicio { get; set; }
        public DateTime dtInscricaoFim { get; set; }

        public DateTime dtTreinamentoInicio { get; set; }
        public DateTime dtTreinamentoFim { get; set; }

        public string dsHoraInicio { get; set; }
        public string dsHoraTermino { get; set; }

        public string dsImagem { get; set; }
        public string dsMaterial { get; set; }
        public string dsDescricao { get; set; }
        public DateTime dtCadastro { get; set; }
        public string dsLocalTreinamento { get; set; }
        public string dsPais { get; set; }
        public string dsCEP { get; set; }
        public string  dsUF { get; set; }
        public string dsEndereco { get; set; }
        public string dsNumero { get; set; }
        public string dsBairro { get; set; }
        public string dsCidade { get; set; }
        public string dsLatitude { get; set; }
        public string dsLongitude { get; set; }
        public string dsUrl { get; set; }
        public bool btBrasil { get; set; }


        public string dsDia { get; set; }
        public string dsMesPorExtenso { get; set; }
        public string mesFinalPorExtenso { get; set; }        
        public string Divisao { get; set; }
        public int QuantidadeVagasEmAberto { get; set; }

        public List<TreinamentoPalestrante> Palestrantes { get; set; }
        public List<TreinamentoAgenda> Agendas { get; set; }

        public List<string> dsListaDescricao { get; set; }

        public string dsPesquisa { get; set; }
        public int inUsuarioStatus { get; set; }

        public int IdUsuarioInscricao { get; set; }
        public int idStatusInscricao { get; set; }        
    }
}
