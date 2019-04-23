using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Interface
{
    public interface IUsuario : IDisposable
    {
        RetornoPadrao Cadastro(UsuarioCadastro usuario);
        List<Pais> ListaPais();
        RetornoCNPJ BuscaCNPJ(string cnpj);
        Usuario ListaUsuarioPorId(int id);
        RetornoPadrao Editar(UsuarioEditar usuario);
        RetornoPadrao EditaNoTreinamento(UsuarioEditar usuario);
        UsuarioEditar ListaEditar(int idUsuario);
        RetornoPadrao EnviarSenha(string cpf);
    }
}
