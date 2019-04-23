using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioConteudoEducacional 
    {
        EFDbContexto _db = new EFDbContexto();

        public List<ConteudoEducacional> ListaConteudoEducacionalTipo()
        {
            return _db.Database.SqlQuery<ConteudoEducacional>("SP_ConteudoEducacionalTipoLista").ToList();
        }

        public List<ConteudoEducacional> ListaConteudoEducacional()
        {
            return _db.Database.SqlQuery<ConteudoEducacional>("SP_ConteudoEducacionalLista").ToList();
        }

        public RetornoPadrao CadastraConteudo(ConteudoEducacional conteudoEducacional)
        {
            RetornoPadrao retorno = new RetornoPadrao();

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_ConteudoEducacionalCadastro @idIdioma, @idTipo, @dsTitulo, @dsLink, @dsArquivo, @btAtivo"
                    , new SqlParameter("idIdioma", conteudoEducacional.idIdioma)
                    , new SqlParameter("idTipo", conteudoEducacional.idTipo)
                    , new SqlParameter("dsTitulo", conteudoEducacional.dsTitulo)
                    , (string.IsNullOrEmpty(conteudoEducacional.dsLink)) ? new SqlParameter("dsLink", string.Empty) : new SqlParameter("dsLink", conteudoEducacional.dsLink)
                    , (string.IsNullOrEmpty(conteudoEducacional.dsArquivo)) ? new SqlParameter("dsArquivo", string.Empty) : new SqlParameter("dsArquivo", conteudoEducacional.dsArquivo)
                    , new SqlParameter("btAtivo", conteudoEducacional.btAtivo)
                    ).FirstOrDefault();
            }                                                                                  	
            catch                                                                              	
            {
                return retorno;
            }

            return retorno;
        }

        public RetornoPadrao AlteraConteudo(ConteudoEducacional conteudoEducacional)
        {
            RetornoPadrao retorno = new RetornoPadrao();

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_ConteudoEducacionalAltera @idConteudo, @btAtivo"
                    , new SqlParameter("idConteudo", conteudoEducacional.idConteudo)
                    , new SqlParameter("btAtivo", conteudoEducacional.btAtivo)
                    ).FirstOrDefault();
            }
            catch
            {
                return retorno;
            }

            return retorno;
        }
    }
}