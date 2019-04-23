using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioFaleConosco 
    {
        EFDbContexto _db = new EFDbContexto();

        public List<FaleConosco> ListaFaleConosco(FaleConosco faleConosco)
        {
            string dsContato2 = string.Empty;
            if (faleConosco.dsContato == "Brasil")
                dsContato2 = "Brazil";
            else if (faleConosco.dsContato == "México")
                dsContato2 = "Mexico";
            else
                dsContato2 = "Latin America";

            return _db.Database.SqlQuery<FaleConosco>("SP_FaleConoscoLista @dsContato, @dsContato2, @dsNomeCompleto, @dsInstituicao",
                new SqlParameter("dsContato", faleConosco.dsContato),
                new SqlParameter("dsContato2", dsContato2),
                (string.IsNullOrEmpty(faleConosco.dsNomeCompleto)) ? new SqlParameter("dsNomeCompleto", DBNull.Value) : new SqlParameter("dsNomeCompleto", faleConosco.dsNomeCompleto),
                (string.IsNullOrEmpty(faleConosco.dsInstituicao)) ? new SqlParameter("dsInstituicao", DBNull.Value) : new SqlParameter("dsInstituicao", faleConosco.dsInstituicao)
                ).ToList();
        }
    }
}