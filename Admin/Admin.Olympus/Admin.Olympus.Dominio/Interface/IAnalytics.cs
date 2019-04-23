using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Data;

namespace Admin.Olympus.Dominio.Interface
{
    public interface IAnalytics
    {
        string CarregaGraficoPaginaSessao(DateTime dtInicial, DateTime dtFinal);
        string CarregaGraficoVisitante(DataTable dadosAnalytics);
        string CarregaGraficoCidade(DataTable dadosAnalytics);
        List<Cidades> CarregaCidadesAtivas(DataTable dadosAnalytics);
        string CarregaGraficoPagina(DataTable dadosAnalytics);
        List<Home> CarregaPaginasAtivas(DataTable dadosAnalytics);
        string CarregaGraficoBrowser(DataTable dadosAnalytics);
        string CarregaGraficoSistemaOperacional(DataTable dadosAnalytics);
        string CarregaGraficoPeriodoAcesso(DataTable dadosAnalytics);
        string CarregaGrafico(DataTable dadosAnalytics);
    }
}
