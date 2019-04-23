using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Treinamento
    {
        public int idTreinamento { get; set; }
        public int idAdministrador { get; set; }
        public int idStatus { get; set; }
        public string dsStatus { get; set; }

        public string dsCodigo { get; set; }
        public string dsCodigoTreinamento { get; set; }
        public int inQtdInscrito { get; set; }
        public int inQtdListaEspera { get; set; }
        public int inQtdCancelado { get; set; }
        public int inQtdPresenca { get; set; }

        public int idDivisao { get; set; }
        public string dsDivisao { get; set; }
        public int idCategoria { get; set; }
        public string dsCategoria { get; set; }
        public string dsNome { get; set; }
        public string dsNomeComData { get; set; }
        public int idIdioma { get; set; }
        public DateTime dtTreinamento { get; set; }
        public DateTime dtTreinamentoFim { get; set; }
        public string dsNomeES { get; set; }

        public bool btSiteOBL { get; set; }
        public bool btSiteOLA { get; set; }
        public bool btSiteOMS { get; set; }

        public string dsData { get; set; }
        public string dsInicio { get; set; }
        public string dsTermino { get; set; }
        public string dsInscInicio { get; set; }
        public string dsInscFim { get; set; }
        public string dsHoraInicio { get; set; }
        public string dsHoraTermino { get; set; }
        public string dsUrlInscricao { get; set; }
        public string dsUrl { get; set; }
        public bool btBrasil { get; set; }

        public bool btPesquisa { get; set; }
        public bool btPesquisaPre { get; set; }
        public bool btPesquisaPos { get; set; }
        public bool btPesquisaObg { get; set; }

        //ARQUIVOS
        public string dsImagem { get; set; }
        public string dsMaterial { get; set; }
        public string dsPesquisa { get; set; }

        public int? inQuantidadeVagas { get; set; }
        public int? inQuantidadeVagasPorCNPJ { get; set; }

        public DateTime? dtInscricaoInicio { get; set; }
		public DateTime? dtInscricaoFim { get; set; }
		public string dsDescricao { get; set; }
        public DateTime dtCadastro { get; set; }

        //LOCAL DO TREINAMENTO
        public string dsLocal { get; set; }
        public string dsLocalTreinamento { get; set; }
        public string dsEndereco { get; set; }
        public string dsNumero { get; set; }
        public string dsBairro { get; set; }
        public string dsCEP { get; set; }
        public string dsCidade { get; set; }
        public string dsUF { get; set; }
        public string dsPais { get; set; }
        public string dsLatitude { get; set; }
        public string dsLongitude { get; set; }

        //CONTROLA ERRO AO SALVAR ARQUIVOS
        public bool btErro { get; set; }


        //PALESTRANTE
        public string dsPalestrante { get; set; }
        public string dsPerfil { get; set; }
        public string dsFoto { get; set; }

        //AGENDA
        public int idAgenda { get; set; }
        public string dsDtTreinamento { get; set; }
        public string dsHorario { get; set; }
        public string dsAtividade { get; set; }
        public string dsIdAgenda { get; set; }

    }
}
