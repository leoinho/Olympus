using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
//using Admin.Olympus.Dominio.Interface;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioDashboard
    {
        private EFDbContexto _db = new EFDbContexto();

        public List<Dashboard> Resumo(int idIdioma)
        {
            return _db.Database.SqlQuery<Dashboard>("SP_RelatorioDashboardResumo @idIdioma"
                , new SqlParameter("idIdioma", idIdioma))
                .ToList();
        }

        public List<Dashboard> RelatorioCadastroSite()
        {
            return _db.Database.SqlQuery<Dashboard>("SP_RelatorioCadastroSite").ToList();
        }
        public List<Dashboard> RelatorioInscricaoSite()
        {
            return _db.Database.SqlQuery<Dashboard>("SP_RelatorioInscricaoSite").ToList();
        }

        public List<Treinamento> GraficoTreinamentoInscricao(int inMes)
        {
            return _db.Database.SqlQuery<Treinamento>("SP_GraficoTreinamentoInscricao @inMes"
                , new SqlParameter("inMes", inMes)).ToList();
        }

        public List<Dashboard> GraficoTreinamentoExterno(int idIdioma)
        {
            return _db.Database.SqlQuery<Dashboard>("SP_GraficoTreinamentoExterno @idIdioma"
                , new SqlParameter("idIdioma", idIdioma))
                .ToList();
        }

        public List<Dashboard> GraficoAcessosDiarios(int idIdioma)
        {
            return _db.Database.SqlQuery<Dashboard>("SP_GraficoAcessoDia @idIdioma"
                , new SqlParameter("idIdioma", idIdioma))
                .ToList();
        }
    }
}
