using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;

namespace Admin.Olympus.Dominio.Interface
{
    public interface IUsuario
    {
        List<Usuario> ListaUsuario(UsuarioViewModel usuarioViewModel);

        List<Usuario> ListaAcessos(UsuarioViewModel usuarioViewModel);

        List<Usuario> ListaUsuarioPorCoordenador(int idUsuarioCoordenador);

        List<Usuario> ListaUsuarioPreCadastro(int? idDistribuidora);

        Usuario ListaUsuarioPreCadastroPorId(int idPreCadastro);

        RetornoPadrao CadastraUsuarioPreCadastro(UsuarioViewModel usuarioViewModel);

        RetornoPadrao ValidaUsuarioPreCadastro(UsuarioViewModel usuarioViewModel);

        Usuario ListaUsuarioPorId(int idUsuario);

        RetornoPadrao AlteraUsuario(Usuario usuario);

        bool AtivaInativaUsuario(int idUsuario, bool btAtivo);
    }
}