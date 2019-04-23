using Admin.Olympus.Dominio.Repositorio;

namespace Admin.Olympus.LiberarTreinamento
{
    class Program
    {
        static void Main(string[] args)
        {
            RepositorioTreinamento _treinamento = new RepositorioTreinamento();
            _treinamento.LiberarTreinamento();
        }
    }
}
