using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fullbar.Olympus.MVC.Models
{
    public class TreinamentoHomeViewModel
    {
        public int idTreinamento { get; set; }
        public int idIdioma { get; set; }
        public int idCategoria { get; set; }
        public string dsDivisao { get; set; }
        public string dsCategoria { get; set; }
        public string dsNome { get; set; }
        public DateTime dtTreinamento { get; set; }
        public string dsImagem { get; set; }
        public int inQuantidadeVagas { get; set; }
        public int inQuantidadeInscricoes { get; set; }
        public int idStatus { get; set; }
        public string dsStatus { get; set; }
        public string dsDataPorExtenso { get; set; }
        public string dsPais { get; set; }
        public string dsCidade { get; set; }
        public int inUsuarioStatus { get; set; }

    }
}