using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class RetornoSMS
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Texto { get; set; }
        public string Xml { get; set; }

        public RetornoSMS()
        {
            Id = String.Empty;
            Codigo = String.Empty;
            Texto = String.Empty;
            Xml = String.Empty;

        }
    }
}
