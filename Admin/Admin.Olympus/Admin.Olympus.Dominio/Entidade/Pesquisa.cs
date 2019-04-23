using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Pesquisa
    {
        public int idPesquisa { get; set; }

        public int idTreinamento { get; set; }
        public string dsNome { get; set; }
        public string dsCodigoTreinamento { get; set; }

        public string dsCodigo { get; set; }
        public string dsDtTreinamento { get; set; }
        public DateTime dtTreinamento { get; set; }
        public string dsNomeCompleto { get; set; }
        public string dsCPF { get; set; }
        public DateTime dtResposta { get; set; }
        
        public int idTipo { get; set; }
        public int idTipoPergunta { get; set; }

        public int idPergunta { get; set; }
        public string dsPergunta { get; set; }
        public string dsDescricao { get; set; }

        public string dsRespostaCorreta { get; set; }

        public int idAlternativa { get; set; }
        public string dsAlternativa { get; set; }

        public int idAlternativaA { get; set; }
        public int idAlternativaB { get; set; }
        public int idAlternativaC { get; set; }
        public int idAlternativaD { get; set; }
        public int idAlternativaE { get; set; }

        public string dsAlternativaA { get; set; }
        public string dsAlternativaB { get; set; }
        public string dsAlternativaC { get; set; }
        public string dsAlternativaD { get; set; }
        public string dsAlternativaE { get; set; }

        public bool btCorretaA { get; set; }
        public bool btCorretaB { get; set; }
        public bool btCorretaC { get; set; }
        public bool btCorretaD { get; set; }
        public bool btCorretaE { get; set; }

        public List<Alternativa> Alternativas { get; set; }

        public bool btExcel { get; set; }
        public bool btAtivo { get; set; }
    }
}
