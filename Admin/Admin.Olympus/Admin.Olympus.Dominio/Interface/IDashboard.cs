using Admin.Olympus.Dominio.Entidade;
using System.Collections.Generic;

namespace Admin.Olympus.Dominio.Interface
{
    public interface IDashboard
    {
        List<Dashboard> Resumo(int idIdioma);

        List<Dashboard> RelatorioCadastroSite();
        List<Dashboard> RelatorioInscricaoSite();

        List<Treinamento> GraficoTreinamentoInscricao(int idStatus);

        List<Dashboard> GraficoTreinamentoExterno(int idIdioma);

        List<Dashboard> GraficoAcessosDiarios(int idIdioma);
    }
}
