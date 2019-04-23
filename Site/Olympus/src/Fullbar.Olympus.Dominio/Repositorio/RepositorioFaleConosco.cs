using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Fullbar.Olympus.Dominio.Repositorio
{
    public class RepositorioFaleConosco : IFaleConosco
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public RetornoPadrao Cadastrar(FaleConosco dados)
        {
            var retorno = new RetornoPadrao();
            var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);

            try
            {

                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_FaleConoscoCadastro @idUsuario, @dsAssunto, @dsNome ,@dsEmail, @dsTelefone,@dsMensagem, @dsContato",

                                                                                            new SqlParameter("idUsuario", dados.idUsuario)
                                                                                          , new SqlParameter("dsAssunto", dados.Assunto)
                                                                                          , new SqlParameter("dsNome", dados.Nome)
                                                                                          , new SqlParameter("dsEmail", dados.Email)
                                                                                          , new SqlParameter("dsTelefone", dados.Telefone)
                                                                                          , new SqlParameter("dsMensagem", dados.Mensagem)
                                                                                          , (!string.IsNullOrEmpty(dados.Contato)) ? new SqlParameter("dsContato", dados.Contato) : new SqlParameter("dsContato", string.Empty)).FirstOrDefault();


                if (retorno.id > 0)
                {
                    var isEmail = new Util.Email().EnviarFaleConosco(dados, idioma);

                    if (!isEmail)
                    {
                        retorno.dsMensagem = "";
                        retorno.id = -2;
                    }
                }

            }
            catch (Exception ex)
            {
                return new RetornoPadrao() { id = -1, dsMensagem = "" };
            }

            return retorno;
        }

        public List<Pais> RetornaPais()
        {
            var idIdioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);

            return _db.Database.SqlQuery<Pais>("SP_PaisLista @idIdioma", new SqlParameter("idIdioma", idIdioma)).ToList();

        }
        public void Dispose()
        {
            _db.Dispose();
        }



    }
}
