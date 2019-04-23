using System;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Administrador
    {
        public int idAdministrador { get; set; }
        public int idTipo { get; set; }
        public string dsTipo { get; set; }
        public int idPerfil { get; set; }
        public string dsPerfil { get; set; }
        public int idDistribuidora { get; set; }
        public string dsDistribuidora { get; set; }
        public string dsNome { get; set; }
        public string dsLogin { get; set; }
        public string dsEmail { get; set; }
        public string dsSenha { get; set; }
        public DateTime dtCadastro { get; set; }
        public bool btAtivo { get; set; }
    }
}
