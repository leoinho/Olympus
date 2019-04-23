using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fullbar.Olympus.Dominio.Util;
using System.Data.SqlClient;

namespace Fullbar.Olympus.Dominio.Repositorio
{

    public class RepositorioLogin : ILogin
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public Usuario Acessar(Login login)
        {
            var senha = Crypto.Encrypt(login.Senha, Util.Crypto.Key256, 256);
            var ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            var retornoUsuario = _db.Database.SqlQuery<Usuario>("SP_UsuarioLogin @dsEmail, @dsSenha, @dsIP", new SqlParameter("dsEmail", login.Email), new SqlParameter("dsSenha", senha), new SqlParameter("dsIP", ip)).FirstOrDefault();

            if (retornoUsuario != null)
            {
                var retornoCNPJ = _db.Database.SqlQuery<RetornoCNPJ>("SP_EmpresaListaPorCNPJ @dsCNPJ", new SqlParameter("dsCNPJ", retornoUsuario.dsCNPJ.Replace(".", "").Replace("/", "").Replace("-", ""))).FirstOrDefault();

                var usuarioLogin = new Usuario
                {

                    btLicencaEUA = retornoUsuario.btLicencaEUA
                    ,
                    dsCNPJ = retornoUsuario.dsCNPJ
                    ,
                    dsCPF = retornoUsuario.dsCPF
                    ,
                    dsEmail = retornoUsuario.dsEmail
                    ,
                    dsEspecialidade = retornoUsuario.dsEspecialidade
                    ,
                    dsNacionalidade = retornoUsuario.dsNacionalidade
                    ,
                    dsNomeCompleto = retornoUsuario.dsNomeCompleto
                    ,
                    dsNumConselho = retornoUsuario.dsNumConselho
                    ,
                    dsNumEstado = retornoUsuario.dsNumEstado
                    ,
                    dsTelefone = retornoUsuario.dsTelefone
                    ,
                    dsCelular = retornoUsuario.dsCelular
                    ,
                    dsTituloProfissional = retornoUsuario.dsTituloProfissional
                    ,
                    dsTituloProfissionalOutros = retornoUsuario.dsTituloProfissionalOutros
                    ,
                    idNacionalidade = retornoUsuario.idNacionalidade
                    ,
                    idusuario = retornoUsuario.idusuario
                    ,
                    Passaporte = retornoUsuario.Passaporte
                    ,
                    RetornoCNPJ = retornoCNPJ
                    ,
                    dsSenha = retornoUsuario.dsSenha 

                };

                return usuarioLogin;
            }

            return null;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
