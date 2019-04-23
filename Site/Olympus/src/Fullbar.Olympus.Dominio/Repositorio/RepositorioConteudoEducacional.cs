using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Repositorio
{
    public class RepositorioConteudoEducacional: IConteudoEducacional
    {
        private EFDbContexto _db = new EFDbContexto();

        public List<ConteudoEducacional> ListaConteudoEducacional()
        {
            return _db.Database.SqlQuery<ConteudoEducacional>("SP_ConteudoEducacionalLista").ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
