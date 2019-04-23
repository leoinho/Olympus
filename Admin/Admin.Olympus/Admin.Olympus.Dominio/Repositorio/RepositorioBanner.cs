using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioBanner 
    {
        EFDbContexto _db = new EFDbContexto();

        public List<Banner> ListaBanner()
        {
            return _db.Database.SqlQuery<Banner>("SP_BannerLista").ToList();
        }

        public RetornoPadrao CadastraBanner(Banner banner)
        {
            RetornoPadrao retorno = new RetornoPadrao();

            banner.inOrdem = 1;

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_BannerCadastro @idAdministrador, @idIdioma, @idStatus, @dsTitulo, @dsImagem, @dsLink, @inOrdem"
                    , new SqlParameter("idAdministrador", banner.idAdministrador)
                    , new SqlParameter("idIdioma", banner.idIdioma)
                    , new SqlParameter("idStatus", banner.idStatus)
                    , new SqlParameter("dsTitulo", banner.dsTitulo)
                    , new SqlParameter("dsImagem", banner.dsImagem)
                    , (string.IsNullOrEmpty(banner.dsLink)) ? new SqlParameter("dsLink", string.Empty) : new SqlParameter("dsLink", banner.dsLink)
                    , new SqlParameter("inOrdem", banner.inOrdem)
                    ).FirstOrDefault();
            }
            catch 
            {
                return retorno;
            }

            return retorno;
        }

        public RetornoPadrao AlteraBanner(Banner banner)
        {
            RetornoPadrao retorno = new RetornoPadrao();

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_BannerAlteraStatus @idBanner, @idAdministrador, @idStatus"
                    , new SqlParameter("idBanner", banner.idBanner)
                    , new SqlParameter("idAdministrador", banner.idAdministrador)
                    , new SqlParameter("idStatus", banner.idStatus)
                    ).FirstOrDefault();
            }
            catch
            {
                return retorno;
            }

            return retorno;
        }


        public RetornoPadrao Ordena(int idBanner, int inOrdem)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_BannerOrdena @idBanner, @inOrdem",
                                        new SqlParameter("idBanner", idBanner),
                                        new SqlParameter("inOrdem", inOrdem)).FirstOrDefault();
            }
            catch (Exception ex)
            {

                return new RetornoPadrao { id = -1, dsMensagem = ex.Message };
            }
        }


        public RetornoPadrao AlteraLink(int idBanner, string dsLink)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_BannerAlteraLink @idBanner, @dsLink",
                                        new SqlParameter("idBanner", idBanner),
                                        new SqlParameter("dsLink", dsLink)).FirstOrDefault();
            }
            catch (Exception ex)
            {

                return new RetornoPadrao { id = -1, dsMensagem = ex.Message };
            }
        }


        public RetornoPadrao Exclui(int idBanner)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_BannerExclui @idBanner",
                                        new SqlParameter("idBanner", idBanner)).FirstOrDefault();
            }
            catch (Exception ex)
            {

                return new RetornoPadrao { id = -1, dsMensagem = ex.Message };
            }
        }    

    }
}