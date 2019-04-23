using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Fullbar.Olympus.Dominio.Repositorio
{
    public class RepositorioPagina : IPagina
    {

        private readonly EFDbContexto _db = new EFDbContexto();

        public RetornoPadrao Log(int idUsuario, int idPagina)
        {
            try
            {
                var idIdioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);

                return _db.Database.SqlQuery<RetornoPadrao>("SP_AcessoCadastro @idUsuario , @idPagina , @idIdioma", new SqlParameter("idUsuario", idUsuario), new SqlParameter("idPagina", idPagina), new SqlParameter("idIdioma", idIdioma)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new RetornoPadrao { id = -1, dsMensagem = "" };
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
