using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Olympus.MVC.Models
{
    public class TreinamentoViewModel
    {
        public int idTreinamento { get; set; }

        public Treinamento Treinamento { get; set; }
        public HttpPostedFileBase Foto { get; set; }
    }
}