using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
using System.Linq;
using Admin.Olympus.Dominio.Util;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioAdministrador
    {
        private EFDbContexto _db = new EFDbContexto();

        public Administrador Logar(string dsLogin, string dsSenha, string dsIP)
        {
            var admin = new Administrador();
            dsSenha = Crypto.Encrypt(dsSenha, Crypto.Key256, 256);

            try
            {
                admin = _db.Database.SqlQuery<Administrador>("SP_AdministradorLogin  @dsLogin, @dsSenha, @dsIP",
                    new SqlParameter("dsLogin", dsLogin),
                    new SqlParameter("dsSenha", dsSenha),
                    new SqlParameter("dsIP", dsIP)).FirstOrDefault();
            }
            catch { admin = null; }

            return admin;
        }

        public RetornoPadrao CadastraUsuario(Administrador admin)
        {
            RetornoPadrao retorno = new RetornoPadrao();
            var dsSenha = Crypto.Encrypt(admin.dsSenha, Crypto.Key256, 256);

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_AdministradorCadastro @idTipo, @idAdministradorCadastro, @dsNome, @dsLogin, @dsEmail, @dsSenha",
                    new SqlParameter("idTipo", admin.idTipo),
                    new SqlParameter("idAdministradorCadastro", admin.idAdministrador),
                    new SqlParameter("dsNome", admin.dsNome),
                    new SqlParameter("dsLogin", admin.dsLogin),
                    new SqlParameter("dsEmail", admin.dsEmail),
                    new SqlParameter("dsSenha", dsSenha)).FirstOrDefault();
            }
            catch
            {
                retorno.dsMensagem = "Erro ao cadastrar usuário.";
            }

            return retorno;
        }

        public List<Administrador> ListaUsuario()
        {
            return _db.Database.SqlQuery<Administrador>("SP_AdministradorLista").ToList();
        }
    }
}
