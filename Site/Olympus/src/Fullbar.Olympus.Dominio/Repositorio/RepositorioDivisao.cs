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
    public class RepositorioDivisao : IDivisao
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public List<Divisao> Lista()
        {
            var listaDivisao = new List<Divisao>();

            var retorno = _db.Database.SqlQuery<Divisao>("SP_DivisaoLista").ToList();

            foreach (var item in retorno)
            {
                var listaCategoria = _db.Database.SqlQuery<Categoria>("SP_CategoriaLista @idDivisao ", new SqlParameter("idDivisao",item.idDivisao)).ToList();

                var divisao = new Divisao() { 
                
                     idDivisao = item.idDivisao
                    ,dsNome = item.dsNome
                    ,Categorias = listaCategoria
                
                };
            }

            return listaDivisao;
        }



        public void Dispose()
        {
            _db.Dispose();
        }
    }

    public static class RepositorioDivisaoTreinamento
    {
        private static readonly EFDbContexto _db = new EFDbContexto();

        public static List<Divisao> Lista()
        {
            var listaDivisao = new List<Divisao>();

            var retorno = _db.Database.SqlQuery<Divisao>("SP_DivisaoLista").ToList();

            foreach (var item in retorno)
            {
                var listaCategoria = _db.Database.SqlQuery<Categoria>("SP_CategoriaLista @idDivisao ", new SqlParameter("idDivisao",item.idDivisao)).ToList();

                var divisao = new Divisao() { 
                
                     idDivisao = item.idDivisao
                    ,dsNome = item.dsNome
                    ,Categorias = listaCategoria
                
                };
            }

            return listaDivisao;
        }
    }
}
