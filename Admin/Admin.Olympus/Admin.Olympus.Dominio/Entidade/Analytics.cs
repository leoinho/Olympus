
namespace Admin.Olympus.Dominio.Entidade
{
    public class Analytics
    {
        public string idProjeto { get; set; }
        public string dtInicial { get; set; }
        public string dtFinal { get; set; }
        public string dimensoes { get; set; }
        public string metricas { get; set; }
        public string ordernacao { get; set; }
    }

    public class Cidades
    {
        public int Posicao { get; set; }
        public string Cidade { get; set; }
        public string Sessao { get; set; }
    }

    public class Home
    {
        public int Posicao { get; set; }
        public string Pagina { get; set; }
        public int Sessao { get; set; }
    }

    public class Periodo
    {
        public string Tipo { get; set; }
        public int Quantidade { get; set; }
    }
}
