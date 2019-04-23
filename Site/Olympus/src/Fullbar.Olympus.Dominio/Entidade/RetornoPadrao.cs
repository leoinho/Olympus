using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
   public class RetornoPadrao
    {
        public int id { get; set; }
        public int idReferencia { get; set; }
        public string dsMensagem { get; set; }
        public string dsTitulo { get; set; }
        public int idStatus { get; set; }
        public string dsTexto { get; set; }
        public string dsPesquisa { get; set; }
        public bool btSucesso { get; set; }
        public bool treinamento { get; set; }
   }
}
