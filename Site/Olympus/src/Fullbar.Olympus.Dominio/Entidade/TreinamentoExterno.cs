using System;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class TreinamentoExterno
    {
        public int idLog { get; set; }
        public int idUsuario { get; set; }
        public int idTreinamento { get; set; }
        public int idIdioma { get; set; }
        public DateTime dtCadastro { get; set; }
        public bool btAtivo { get; set; }
    }
}
