using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class UsuarioViewModel
    {
        public int idPerfil { get; set; }
        public int idCargo { get; set; }
        public int idDistribuidora { get; set; }
        public int idUsuario { get; set; }
        public int idUsuarioVendedor { get; set; }
        public string dsCPFCoord { get; set; }
        public string dsCPFSuporte { get; set; }
        public string dsNome { get; set; }
        public string dsEmail { get; set; }
        public string dsCPF { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }

        public Usuario Usuario { get; set; }
        public List<Usuario> ListaUsuario { get; set; }
        public List<Usuario> Acessos { get; set; }
    }
}
