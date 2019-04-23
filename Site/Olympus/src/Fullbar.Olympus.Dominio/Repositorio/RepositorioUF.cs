using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Repositorio
{
   public class RepositorioUF :IUF
    {
       private EFDbContexto _db = new EFDbContexto();

        public List<UF> ListaEstado()
        {
            return _db.Database.SqlQuery<UF>("SP_EstadoLista").ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
