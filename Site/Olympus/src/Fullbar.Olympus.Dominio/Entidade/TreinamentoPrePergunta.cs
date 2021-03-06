﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoPrePergunta
    {
        public int idPergunta { get; set; }
        public int idTipo { get; set; }
        public string dsTipo { get; set; }
        public string dsDescricao { get; set; }
        public DateTime dtCadastro { get; set; }

        public int idTreinamento { get; set; }
        public int idUsuario { get; set; }
        public int idInscricao { get; set; }

        public List<TreinamentoPreAlternativa> Alternativas { get; set; }

    }
}
