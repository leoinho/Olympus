using System;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Banner
    {
        public int idBanner { get; set; }
        public int idStatus { get; set; }
        public int idAdministrador { get; set; }
        public int idIdioma { get; set; }
        public string dsTitulo { get; set; }
        public string dsLink { get; set; }
        public string dsImagem { get; set; }
        public int inOrdem { get; set; }
        public DateTime dtOperacao { get; set; }
        public DateTime dtCadastro { get; set; }
        public bool btAtivo { get; set; }

        //DROP
        public string dsStatus { get; set; }
        public string dsSite { get; set; }
    }
}
