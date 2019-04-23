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
    public class RepositorioBanner : IBanner
    {
        private EFDbContexto _db = new EFDbContexto();

        public List<Banner> ListaBanner()
        {
            return _db.Database.SqlQuery<Banner>("SP_BannerLista @idStatus, @idIdioma",
                new SqlParameter("idStatus", 1),
                new SqlParameter("idIdioma", DBNull.Value)).ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
