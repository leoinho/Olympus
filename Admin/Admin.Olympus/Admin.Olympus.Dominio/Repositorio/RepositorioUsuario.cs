using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
//using Admin.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioUsuario 
    {
        EFDbContexto _db = new EFDbContexto();

        public List<Pais> ListaPais()
        {
            return _db.Database.SqlQuery<Pais>("SP_PaisLista @idIdioma", new SqlParameter("idIdioma", 2)).ToList();
        }

        public List<Usuario> RelatorioUsuario(Usuario usuario)
        {
            List<Usuario> listUsuario = new List<Usuario>();

            try
            {
                return listUsuario = _db.Database.SqlQuery<Usuario>("SP_RelatorioCadastro @idIdioma, @idUsuario, @idNacionalidade, @idPais, @dsCPF, @dsNome, @dsCNPJ, @dsCodigoTreinamento"
                    , new SqlParameter("idIdioma", usuario.idIdioma)
                    , new SqlParameter("idUsuario", DBNull.Value)
                    , new SqlParameter("idNacionalidade", DBNull.Value)
                    , (usuario.idPais == 0) ? new SqlParameter("idPais", DBNull.Value) : new SqlParameter("idPais", usuario.idPais)
                    , (string.IsNullOrEmpty(usuario.dsCPF)) ? new SqlParameter("dsCPF", DBNull.Value) : new SqlParameter("dsCPF", usuario.dsCPF.Replace(".", "").Replace("-", ""))
                    , (string.IsNullOrEmpty(usuario.dsNomeCompleto)) ? new SqlParameter("dsNome", DBNull.Value) : new SqlParameter("dsNome", usuario.dsNomeCompleto)
                    , (string.IsNullOrEmpty(usuario.dsCNPJ)) ? new SqlParameter("dsCNPJ", DBNull.Value) : new SqlParameter("dsCNPJ", usuario.dsCNPJ.Replace(".", "").Replace("/", "").Replace("-", ""))
                    , (string.IsNullOrEmpty(usuario.dsCodigoTreinamento)) ? new SqlParameter("dsCodigoTreinamento", DBNull.Value) : new SqlParameter("dsCodigoTreinamento", usuario.dsCodigoTreinamento)
                    ).ToList();
            }
            catch (Exception ex) { var erro = ex.Message; }

            return listUsuario = null;
        }
    }
}
