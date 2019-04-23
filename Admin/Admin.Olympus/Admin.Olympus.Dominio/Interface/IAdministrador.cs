using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Interface
{
    public interface IAdministrador
    {
        Administrador Logar(string dsLogin, string dsSenha, string dsIP);

        RetornoPadrao CadastraUsuario(Administrador admin);

        List<Administrador> ListaUsuario();
    }
}
