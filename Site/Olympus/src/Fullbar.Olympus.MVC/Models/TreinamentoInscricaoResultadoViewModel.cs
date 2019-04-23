using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fullbar.Olympus.MVC.Models
{
    public class TreinamentoInscricaoResultadoViewModel
    {
        //public int idConvite { get; set; }
        //public string dsCPF { get; set; }
        //public string dsNome { get; set; }
        //public string dsEmail { get; set; }
        //public string dsStatus { get; set; }
        //public int idStatus { get; set; }

        public List<TreinamentoInscricaoResultadoConfirmado> Confirmado { get; set; }
        public List<TreinamentoResultadoListaEspera> ListaEspera { get; set; }
    }
}