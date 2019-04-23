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
    public class RepositorioCategoria : ICategoria
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public List<Categoria> Lista(int idDivisao)
        {
            return _db.Database.SqlQuery<Categoria>("SP_CategoriaLista @idDivisao ", (idDivisao == 0) ? new SqlParameter("idDivisao", DBNull.Value) : new SqlParameter("idDivisao", idDivisao)).ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }

    public static class RepositorioCategoriaHome
    {
        private static readonly EFDbContexto _db = new EFDbContexto();

        public static List<Categoria> Lista(int idDivisao)
        {
            return _db.Database.SqlQuery<Categoria>("SP_CategoriaLista @idDivisao ", (idDivisao == 0) ? new SqlParameter("idDivisao", DBNull.Value) : new SqlParameter("idDivisao", idDivisao)).ToList();
        }

    }
}
