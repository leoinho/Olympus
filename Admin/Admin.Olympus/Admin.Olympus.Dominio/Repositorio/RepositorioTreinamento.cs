using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioTreinamento 
    {
        EFDbContexto _db = new EFDbContexto();

        public List<Treinamento> ListaDivisao()
        {
            return _db.Database.SqlQuery<Treinamento>("SP_DivisaoLista").ToList();
        }
        public List<Treinamento> ListaCategoria()
        {
            return _db.Database.SqlQuery<Treinamento>("SP_CategoriaLista").ToList();
        }
        public List<Treinamento> ListaEstado()
        {
            return _db.Database.SqlQuery<Treinamento>("SP_EstadoLista").ToList();
        }
        public List<Treinamento> ListaTreinamentoStatus()
        {
            return _db.Database.SqlQuery<Treinamento>("SP_TreinamentoStatusLista").ToList();
        }
        public List<Pais> ListaPais(int idIdioma)
        {
            return _db.Database.SqlQuery<Pais>("SP_PaisLista @idIdioma", new SqlParameter("idIdioma", idIdioma)).ToList();
        }

        public List<Treinamento> ListaTreinamento(Treinamento treinamento)
        {
            DateTime dtData = new DateTime();

            if (!string.IsNullOrEmpty(treinamento.dsData))
            {
                try { dtData = Convert.ToDateTime(treinamento.dsData); }
                catch { dtData = System.DateTime.MinValue; }
            }

            List<Treinamento> listTreinamento = new List<Treinamento>();

            try
            {
                listTreinamento =  _db.Database.SqlQuery<Treinamento>("SP_TreinamentoLista @idDivisao, @idCategoria, @idStatus, @idIdioma, @dsNome, @dtTreinamento"
                    , (treinamento.idDivisao == 0) ? new SqlParameter("idDivisao", DBNull.Value) : new SqlParameter("idDivisao", treinamento.idDivisao)
                    , (treinamento.idCategoria == 0) ? new SqlParameter("idCategoria", DBNull.Value) : new SqlParameter("idCategoria", treinamento.idCategoria)
                    , new SqlParameter("idStatus", DBNull.Value)
                    , (treinamento.idIdioma == 0) ? new SqlParameter("idIdioma", DBNull.Value) : new SqlParameter("idIdioma", treinamento.idIdioma)
                    , (string.IsNullOrEmpty(treinamento.dsNome)) ? new SqlParameter("dsNome", DBNull.Value) : new SqlParameter("dsNome", treinamento.dsNome)
                    , (dtData == System.DateTime.MinValue) ? new SqlParameter("dtTreinamento", DBNull.Value) : new SqlParameter("dtTreinamento", dtData)
                    ).ToList();

                foreach (var item in listTreinamento)
                {
                    item.dsNomeComData = item.dsNome + " - " + item.dtTreinamento.ToString("dd/MM/yyyy");
                }
            }
            catch { return listTreinamento; }

            return listTreinamento;
        }
        public Treinamento ListaTreinamentoPorId(int idTreinamento)
        {
            Treinamento treinamento = new Treinamento();

            treinamento = _db.Database.SqlQuery<Treinamento>("SP_TreinamentoListaPorID @idTreinamento"
                    , new SqlParameter("idTreinamento", idTreinamento)
                    ).FirstOrDefault();

            treinamento.dsUrlInscricao = treinamento.dsUrl;
            treinamento.dsCodigoTreinamento = treinamento.dsCodigo;

            return treinamento;
        }

        public List<Treinamento> ListaPalestrante(Treinamento treinamento)
        {
            List<Treinamento> listPalestrante = new List<Treinamento>();

            try
            {
                listPalestrante = _db.Database.SqlQuery<Treinamento>("SP_TreinamentoPalestranteLista @idTreinamento"
                    , new SqlParameter("idTreinamento", treinamento.idTreinamento)
                    ).ToList();

                foreach (var item in listPalestrante)
                {
                    item.dsPalestrante = item.dsNome;
                }
            }
            catch { return listPalestrante; }

            return listPalestrante;
        }

        public List<Treinamento> ListaAgenda(Treinamento treinamento)
        {
            List<Treinamento> listAgenda = new List<Treinamento>();

            try
            {
                listAgenda = _db.Database.SqlQuery<Treinamento>("SP_TreinamentoAgendaLista @idTreinamento"
                    , new SqlParameter("idTreinamento", treinamento.idTreinamento)
                    ).ToList();
            }
            catch { return listAgenda; }

            return listAgenda;
        }
        public RetornoPadrao ExcluiAgenda(Treinamento treinamento)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoAgendaExclui @idTreinamento, @idAgenda"
                    , new SqlParameter("idTreinamento", treinamento.idTreinamento)
                    , new SqlParameter("idAgenda", treinamento.idAgenda)
                    ).FirstOrDefault();
            }
            catch { return new RetornoPadrao(); }
        }

        public List<Treinamento> ListaData(int idTreinamento)
        {
            List<Treinamento> list = new List<Treinamento>();

            try
            {
                list = _db.Database.SqlQuery<Treinamento>("SP_TreinamentoDataListaPorTreinamentoID @idTreinamento"
                    , new SqlParameter("idTreinamento", idTreinamento)
                    ).ToList();
            }
            catch { return list; }

            return list;
        }

        public RetornoPadrao CadastraTreinamento(Treinamento treinamento)
        {
            var dtTreinamento = Convert.ToDateTime(treinamento.dsInicio);
            var dtTreinamentoFim = Convert.ToDateTime(treinamento.dsTermino);
            var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
            var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

            if (treinamento.idIdioma == 1)
                treinamento.btSiteOBL = true;
            else if (treinamento.idIdioma == 2)
                treinamento.btSiteOLA = true;
            else if (treinamento.idIdioma == 3)
                treinamento.btSiteOMS = true;

            if (dtTreinamento == dtTreinamentoFim)
            {
                //SETA HORA INCIAL DO TREINAMENTO
                dtTreinamento = dtTreinamento.AddHours(Convert.ToInt32(treinamento.dsHoraInicio.Substring(0, 2))).AddMinutes(Convert.ToInt32(treinamento.dsHoraInicio.Substring(3, 2)));

                //TREINAMENTO EM APENAS UM DIA
                try
                {
                    return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoCadastro @idAdministrador, @idIdioma, @idDivisao, @idCategoria, @dsNome, @dsCodigo, @dtTreinamento, @dsHoraInicio, @dsHoraFim, @inQuantidadeVagas, @inQuantidadeVagasPorCNPJ, @dtInscricaoInicio, @dtInscricaoFim, @dsDescricao, @dsUrlInscricao, @btBrasil, @dsImagem, @dsMaterial, @dsPesquisa, @dsLocalTreinamento, @dsPais, @dsCEP, @dsUF, @dsCidade, @dsEndereco, @dsNumero, @dsBairro, @dsLatitude, @dsLongitude, @idStatus, @btSiteOBL, @btSiteOMS, @btSiteOLA"
                        , new SqlParameter("idAdministrador", treinamento.idAdministrador)
                        , new SqlParameter("idIdioma", treinamento.idIdioma)
                        , new SqlParameter("idDivisao", treinamento.idDivisao)
                        , new SqlParameter("idCategoria", treinamento.idCategoria)
                        , new SqlParameter("dsNome", treinamento.dsNome)
                        , new SqlParameter("dsCodigo", treinamento.dsCodigoTreinamento)
                        , new SqlParameter("dtTreinamento", dtTreinamento)
                        , new SqlParameter("dsHoraInicio", treinamento.dsHoraInicio)
                        , new SqlParameter("dsHoraFim", treinamento.dsHoraTermino)
                        , new SqlParameter("inQuantidadeVagas", treinamento.inQuantidadeVagas)
                        , (treinamento.inQuantidadeVagasPorCNPJ != null) ? new SqlParameter("inQuantidadeVagasPorCNPJ", treinamento.inQuantidadeVagasPorCNPJ) : new SqlParameter("inQuantidadeVagasPorCNPJ", 0)
                        , new SqlParameter("dtInscricaoInicio", dtInscInicio)
                        , new SqlParameter("dtInscricaoFim", dtInscFim)
                        , new SqlParameter("dsDescricao", treinamento.dsDescricao)
                        , (string.IsNullOrEmpty(treinamento.dsUrlInscricao)) ? new SqlParameter("dsUrlInscricao", string.Empty) : new SqlParameter("dsUrlInscricao", treinamento.dsUrlInscricao)
                        , new SqlParameter("btBrasil", treinamento.btBrasil)
                        , new SqlParameter("dsImagem", treinamento.dsImagem)
                        , (string.IsNullOrEmpty(treinamento.dsMaterial)) ? new SqlParameter("dsMaterial", string.Empty) : new SqlParameter("dsMaterial", treinamento.dsMaterial)
                        , (string.IsNullOrEmpty(treinamento.dsPesquisa)) ? new SqlParameter("dsPesquisa", string.Empty) : new SqlParameter("dsPesquisa", treinamento.dsPesquisa)
                        , new SqlParameter("dsLocalTreinamento", treinamento.dsLocal)
                        , new SqlParameter("dsPais", treinamento.dsPais)
                        , new SqlParameter("dsCEP", treinamento.dsCEP)
                        , new SqlParameter("dsUF", string.IsNullOrEmpty(treinamento.dsUF) ? "" : treinamento.dsUF)
                        , new SqlParameter("dsCidade", treinamento.dsCidade)
                        , new SqlParameter("dsEndereco", treinamento.dsEndereco)
                        , new SqlParameter("dsNumero", treinamento.dsNumero)
                        , new SqlParameter("dsBairro", treinamento.dsBairro)
                        , new SqlParameter("dsLatitude", treinamento.dsLatitude)
                        , new SqlParameter("dsLongitude", treinamento.dsLongitude)
                        , new SqlParameter("idStatus", treinamento.idStatus)
                        , new SqlParameter("btSiteOBL", treinamento.btSiteOBL)
                        , new SqlParameter("btSiteOMS", treinamento.btSiteOMS)
                        , new SqlParameter("btSiteOLA", treinamento.btSiteOLA)
                        ).FirstOrDefault();
                }
                catch { return new RetornoPadrao(); }
            }
            else
            {
                //TREINAMENTO EM MAIS DE UM DIA

                //SETA HORA INCIAL DO TREINAMENTO
                dtTreinamento = dtTreinamento.AddHours(Convert.ToInt32(treinamento.dsHoraInicio.Substring(0, 2))).AddMinutes(Convert.ToInt32(treinamento.dsHoraInicio.Substring(3, 2)));

                try
                {
                    var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoCadastro @idAdministrador, @idIdioma, @idDivisao, @idCategoria, @dsNome, @dsCodigo, @dtTreinamento, @dsHoraInicio, @dsHoraFim, @inQuantidadeVagas, @inQuantidadeVagasPorCNPJ, @dtInscricaoInicio, @dtInscricaoFim, @dsDescricao, @dsUrlInscricao, @btBrasil, @dsImagem, @dsMaterial, @dsPesquisa, @dsLocalTreinamento, @dsPais, @dsCEP, @dsUF, @dsCidade, @dsEndereco, @dsNumero, @dsBairro, @dsLatitude, @dsLongitude, @idStatus, @btSiteOBL, @btSiteOMS, @btSiteOLA"
                        , new SqlParameter("idAdministrador", treinamento.idAdministrador)
                        , new SqlParameter("idIdioma", treinamento.idIdioma)
                        , new SqlParameter("idDivisao", treinamento.idDivisao)
                        , new SqlParameter("idCategoria", treinamento.idCategoria)
                        , new SqlParameter("dsNome", treinamento.dsNome)
                        , new SqlParameter("dsCodigo", treinamento.dsCodigoTreinamento)
                        , new SqlParameter("dtTreinamento", dtTreinamento)
                        , new SqlParameter("dsHoraInicio", treinamento.dsHoraInicio)
                        , new SqlParameter("dsHoraFim", treinamento.dsHoraTermino)
                        , new SqlParameter("inQuantidadeVagas", treinamento.inQuantidadeVagas)
                        , (treinamento.inQuantidadeVagasPorCNPJ != null) ? new SqlParameter("inQuantidadeVagasPorCNPJ", treinamento.inQuantidadeVagasPorCNPJ) : new SqlParameter("inQuantidadeVagasPorCNPJ", 0)
                        , new SqlParameter("dtInscricaoInicio", dtInscInicio)
                        , new SqlParameter("dtInscricaoFim", dtInscFim)
                        , new SqlParameter("dsDescricao", treinamento.dsDescricao)
                        , (string.IsNullOrEmpty(treinamento.dsUrlInscricao)) ? new SqlParameter("dsUrlInscricao", string.Empty) : new SqlParameter("dsUrlInscricao", treinamento.dsUrlInscricao)
                        , new SqlParameter("btBrasil", treinamento.btBrasil)
                        , new SqlParameter("dsImagem", treinamento.dsImagem)
                        , (string.IsNullOrEmpty(treinamento.dsMaterial)) ? new SqlParameter("dsMaterial", string.Empty) : new SqlParameter("dsMaterial", treinamento.dsMaterial)
                        , (string.IsNullOrEmpty(treinamento.dsPesquisa)) ? new SqlParameter("dsPesquisa", string.Empty) : new SqlParameter("dsPesquisa", treinamento.dsPesquisa)
                        , new SqlParameter("dsLocalTreinamento", treinamento.dsLocal)
                        , new SqlParameter("dsPais", treinamento.dsPais)
                        , new SqlParameter("dsCEP", treinamento.dsCEP)
                        , new SqlParameter("dsUF", string.IsNullOrEmpty(treinamento.dsUF) ? "" : treinamento.dsUF)
                        , new SqlParameter("dsCidade", treinamento.dsCidade)
                        , new SqlParameter("dsEndereco", treinamento.dsEndereco)
                        , new SqlParameter("dsNumero", treinamento.dsNumero)
                        , new SqlParameter("dsBairro", treinamento.dsBairro)
                        , new SqlParameter("dsLatitude", treinamento.dsLatitude)
                        , new SqlParameter("dsLongitude", treinamento.dsLongitude)
                        , new SqlParameter("idStatus", treinamento.idStatus)
                        , new SqlParameter("btSiteOBL", treinamento.btSiteOBL)
                        , new SqlParameter("btSiteOMS", treinamento.btSiteOMS)
                        , new SqlParameter("btSiteOLA", treinamento.btSiteOLA)
                        ).FirstOrDefault();

                    
                    if (retorno != null && retorno.idTreinamento != 0)
                    {
                        //VERIFICAR PERIODO
                        var dtDiferenca = dtTreinamentoFim.Date - dtTreinamento.Date;

                        for (int i = 0; i < dtDiferenca.Days; i++)
                        {
                            dtTreinamento = dtTreinamento.AddDays(1);

                            var retornoData = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoDataCadastro @idTreinamento, @dtTreinamento"
                                , new SqlParameter("idTreinamento", retorno.idTreinamento)
                                , new SqlParameter("dtTreinamento", dtTreinamento)
                                ).FirstOrDefault();
                        }

                        return retorno;
                    }

                    return new RetornoPadrao();
                }
                catch { return new RetornoPadrao(); }
            }
        }

        public RetornoPadrao CadastraTreinamentoInternacional(Treinamento treinamento)
        {
            var dtTreinamento = Convert.ToDateTime(treinamento.dsInicio);
            var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
            var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

            if (treinamento.idDivisao == 4)
                treinamento.idCategoria = 0;
            
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoCadastro @idAdministrador, @idIdioma, @idDivisao, @idCategoria, @dsNome, @dsCodigo, @dtTreinamento, @dsHoraInicio, @dsHoraFim, @inQuantidadeVagas, @inQuantidadeVagasPorCNPJ, @dtInscricaoInicio, @dtInscricaoFim, @dsDescricao, @dsUrlInscricao, @btBrasil, @dsImagem, @dsMaterial, @dsPesquisa, @dsLocalTreinamento, @dsPais, @dsCEP, @dsUF, @dsCidade, @dsEndereco, @dsNumero, @dsBairro, @dsLatitude, @dsLongitude, @idStatus, @btSiteOBL, @btSiteOMS, @btSiteOLA"
                    , new SqlParameter("idAdministrador", treinamento.idAdministrador)
                    , new SqlParameter("idIdioma", treinamento.idIdioma)
                    , new SqlParameter("idDivisao", treinamento.idDivisao)
                    , new SqlParameter("idCategoria", treinamento.idCategoria)
                    , new SqlParameter("dsNome", treinamento.dsNome)
                    , new SqlParameter("dsCodigo", treinamento.dsCodigoTreinamento)
                    , new SqlParameter("dtTreinamento", dtTreinamento)
                    , new SqlParameter("dsHoraInicio", treinamento.dsHoraInicio)
                    , new SqlParameter("dsHoraFim", treinamento.dsHoraTermino)
                    , new SqlParameter("inQuantidadeVagas", treinamento.inQuantidadeVagas)
                    , new SqlParameter("inQuantidadeVagasPorCNPJ", treinamento.inQuantidadeVagasPorCNPJ)
                    , new SqlParameter("dtInscricaoInicio", dtInscInicio)
                    , new SqlParameter("dtInscricaoFim", dtInscFim)
                    , new SqlParameter("dsDescricao", treinamento.dsDescricao)
                    , new SqlParameter("dsUrlInscricao", treinamento.dsUrlInscricao)
                    , new SqlParameter("btBrasil", treinamento.btBrasil)
                    , new SqlParameter("dsImagem", treinamento.dsImagem)
                    , new SqlParameter("dsMaterial", string.Empty)
                    , new SqlParameter("dsPesquisa", string.Empty)
                    , new SqlParameter("dsLocalTreinamento", treinamento.dsLocal)
                    , new SqlParameter("dsPais", treinamento.dsPais)
                    , new SqlParameter("dsCEP", treinamento.dsCEP)
                    , new SqlParameter("dsUF", string.IsNullOrEmpty(treinamento.dsUF) ? "" : treinamento.dsUF)
                    , new SqlParameter("dsCidade", treinamento.dsCidade)
                    , new SqlParameter("dsEndereco", treinamento.dsEndereco)
                    , new SqlParameter("dsNumero", treinamento.dsNumero)
                    , new SqlParameter("dsBairro", treinamento.dsBairro)
                    , new SqlParameter("dsLatitude", treinamento.dsLatitude)
                    , new SqlParameter("dsLongitude", treinamento.dsLongitude)
                    , new SqlParameter("idStatus", treinamento.idStatus)
                    , new SqlParameter("btSiteOBL", treinamento.btSiteOBL)
                    , new SqlParameter("btSiteOMS", treinamento.btSiteOMS)
                    , new SqlParameter("btSiteOLA", treinamento.btSiteOLA)
                    ).FirstOrDefault();
            }
            catch { return new RetornoPadrao(); }
        }

        public RetornoPadrao CadastraPalestrante(List<Treinamento> listPalestrante, string dsTipo)
        {
            if (dsTipo == "Editar")
            {
                _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPalestranteInativa @idTreinamento"
                    , new SqlParameter("idTreinamento", listPalestrante.FirstOrDefault().idTreinamento)
                    ).FirstOrDefault();
            }

            try
            {
                var retorno = new RetornoPadrao();

                foreach (var item in listPalestrante)
                {
                    retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPalestranteCadastro @idTreinamento, @dsNome, @dsPerfil, @dsImagem"
                        , new SqlParameter("idTreinamento", item.idTreinamento)
                        , new SqlParameter("dsNome", item.dsPalestrante)
                        , new SqlParameter("dsPerfil", item.dsPerfil)
                        , new SqlParameter("dsImagem", item.dsImagem)
                        ).FirstOrDefault();
                }

                return retorno;
            }
            catch { return new RetornoPadrao(); }
        }

        public RetornoPadrao CadastraAgenda(List<Treinamento> listAgenda, string dsTipo)
        {
            if (dsTipo == "Editar")
            {
                _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoAgendaInativa @idTreinamento"
                    , new SqlParameter("idTreinamento", listAgenda.FirstOrDefault().idTreinamento)
                    ).FirstOrDefault();
            }

            try
            {
                var retorno = new RetornoPadrao();

                foreach (var item in listAgenda.OrderBy(x => x.dsHorario).OrderBy(x => x.dtTreinamento))
                {
                    retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoAgendaCadastro @idTreinamento, @inDia, @dsHorario, @dsAtividade, @dsPalestrante"
                        , new SqlParameter("idTreinamento", item.idTreinamento)
                        , new SqlParameter("inDia", item.dtTreinamento.Day)
                        , new SqlParameter("dsHorario", item.dsHorario)
                        , new SqlParameter("dsAtividade", item.dsAtividade)
                        , new SqlParameter("dsPalestrante", string.Empty)
                        ).FirstOrDefault();
                }

                return retorno;
            }
            catch { return new RetornoPadrao(); }
        }

        public RetornoPadrao AlteraStatusTreinamento(Treinamento treinamento)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoAlteraStatus @idTreinamento, @idStatus"
                    , new SqlParameter("idTreinamento", treinamento.idTreinamento)
                    , new SqlParameter("idStatus", treinamento.idStatus)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }

        public RetornoPadrao AlteraTreinamento(Treinamento treinamento)
        {
            var dtTreinamento = Convert.ToDateTime(treinamento.dsInicio);
            var dtTreinamentoFim = Convert.ToDateTime(treinamento.dsTermino);
            var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
            var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

            if (treinamento.idIdioma == 1)
                treinamento.btSiteOBL = true;
            else if (treinamento.idIdioma == 2)
                treinamento.btSiteOLA = true;
            else if (treinamento.idIdioma == 3)
                treinamento.btSiteOMS = true;

            if (dtTreinamento == dtTreinamentoFim)
            {
                //SETA HORA INCIAL DO TREINAMENTO
                dtTreinamento = dtTreinamento.AddHours(Convert.ToInt32(treinamento.dsHoraInicio.Substring(0, 2))).AddMinutes(Convert.ToInt32(treinamento.dsHoraInicio.Substring(3, 2)));

                //TREINAMENTO EM APENAS UM DIA
                try
                {
                    return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoAltera @idTreinamento, @idDivisao, @idCategoria, @dsNome, @dsCodigo, @dtTreinamento, @inQuantidadeVagas, @inQuantidadeVagasPorCNPJ, @dtInscricaoInicio, @dtInscricaoFim, @dsDescricao, @dsUrlInscricao, @btBrasil, @dsHoraInicio, @dsHoraFim, @dsLocalTreinamento, @dsPais, @dsCEP, @dsUF, @dsCidade, @dsEndereco, @dsNumero, @dsBairro, @dsLatitude, @dsLongitude, @btSiteOBL, @btSiteOMS, @btSiteOLA"
                        , new SqlParameter("idTreinamento", treinamento.idTreinamento)
                        , new SqlParameter("idDivisao", treinamento.idDivisao)
                        , new SqlParameter("idCategoria", treinamento.idCategoria)
                        , new SqlParameter("dsNome", treinamento.dsNome)
                        , new SqlParameter("dsCodigo", treinamento.dsCodigoTreinamento)
                        , new SqlParameter("dtTreinamento", dtTreinamento)
                        , new SqlParameter("inQuantidadeVagas", treinamento.inQuantidadeVagas)
                        , (treinamento.inQuantidadeVagasPorCNPJ != null) ? new SqlParameter("inQuantidadeVagasPorCNPJ", treinamento.inQuantidadeVagasPorCNPJ) : new SqlParameter("inQuantidadeVagasPorCNPJ", 0)
                        , new SqlParameter("dtInscricaoInicio", dtInscInicio)
                        , new SqlParameter("dtInscricaoFim", dtInscFim)
                        , new SqlParameter("dsDescricao", treinamento.dsDescricao)
                        , (string.IsNullOrEmpty(treinamento.dsUrlInscricao)) ? new SqlParameter("dsUrlInscricao", string.Empty) : new SqlParameter("dsUrlInscricao", treinamento.dsUrlInscricao)
                        , new SqlParameter("btBrasil", treinamento.btBrasil)
                        , new SqlParameter("dsHoraInicio", treinamento.dsHoraInicio)
                        , new SqlParameter("dsHoraFim", treinamento.dsHoraTermino)
                        , new SqlParameter("dsLocalTreinamento", treinamento.dsLocal)
                        , new SqlParameter("dsPais", treinamento.dsPais)
                        , new SqlParameter("dsCEP", treinamento.dsCEP)
                        , new SqlParameter("dsUF", string.IsNullOrEmpty(treinamento.dsUF) ? "" : treinamento.dsUF)
                        , new SqlParameter("dsCidade", treinamento.dsCidade)
                        , new SqlParameter("dsEndereco", treinamento.dsEndereco)
                        , new SqlParameter("dsNumero", treinamento.dsNumero)
                        , new SqlParameter("dsBairro", treinamento.dsBairro)
                        , new SqlParameter("dsLatitude", treinamento.dsLatitude)
                        , new SqlParameter("dsLongitude", treinamento.dsLongitude)
                        , new SqlParameter("btSiteOBL", treinamento.btSiteOBL)
                        , new SqlParameter("btSiteOMS", treinamento.btSiteOMS)
                        , new SqlParameter("btSiteOLA", treinamento.btSiteOLA)
                        ).FirstOrDefault();
                }
                catch { return new RetornoPadrao(); }
            }
            else
            {
                //TREINAMENTO EM MAIS DE UM DIA
                try
                {
                    var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoAltera @idTreinamento, @idDivisao, @idCategoria, @dsNome, @dsCodigo, @dtTreinamento, @inQuantidadeVagas, @inQuantidadeVagasPorCNPJ, @dtInscricaoInicio, @dtInscricaoFim, @dsDescricao, @dsUrlInscricao, @btBrasil, @dsHoraInicio, @dsHoraFim, @dsLocalTreinamento, @dsPais, @dsCEP, @dsUF, @dsCidade, @dsEndereco, @dsNumero, @dsBairro, @dsLatitude, @dsLongitude, @btSiteOBL, @btSiteOMS, @btSiteOLA"
                        , new SqlParameter("idTreinamento", treinamento.idTreinamento)
                        , new SqlParameter("idDivisao", treinamento.idDivisao)
                        , new SqlParameter("idCategoria", treinamento.idCategoria)
                        , new SqlParameter("dsNome", treinamento.dsNome)
                        , new SqlParameter("dsCodigo", treinamento.dsCodigoTreinamento)
                        , new SqlParameter("dtTreinamento", dtTreinamento)
                        , new SqlParameter("inQuantidadeVagas", treinamento.inQuantidadeVagas)
                        , (treinamento.inQuantidadeVagasPorCNPJ != null) ? new SqlParameter("inQuantidadeVagasPorCNPJ", treinamento.inQuantidadeVagasPorCNPJ) : new SqlParameter("inQuantidadeVagasPorCNPJ", 0)
                        , new SqlParameter("dtInscricaoInicio", dtInscInicio)
                        , new SqlParameter("dtInscricaoFim", dtInscFim)
                        , new SqlParameter("dsDescricao", treinamento.dsDescricao)
                        , (string.IsNullOrEmpty(treinamento.dsUrlInscricao)) ? new SqlParameter("dsUrlInscricao", string.Empty) : new SqlParameter("dsUrlInscricao", treinamento.dsUrlInscricao)
                        , new SqlParameter("btBrasil", treinamento.btBrasil)
                        , new SqlParameter("dsHoraInicio", treinamento.dsHoraInicio)
                        , new SqlParameter("dsHoraFim", treinamento.dsHoraTermino)
                        , new SqlParameter("dsLocalTreinamento", treinamento.dsLocal)
                        , new SqlParameter("dsPais", treinamento.dsPais)
                        , new SqlParameter("dsCEP", treinamento.dsCEP)
                        , new SqlParameter("dsUF", string.IsNullOrEmpty(treinamento.dsUF) ? "" : treinamento.dsUF)
                        , new SqlParameter("dsCidade", treinamento.dsCidade)
                        , new SqlParameter("dsEndereco", treinamento.dsEndereco)
                        , new SqlParameter("dsNumero", treinamento.dsNumero)
                        , new SqlParameter("dsBairro", treinamento.dsBairro)
                        , new SqlParameter("dsLatitude", treinamento.dsLatitude)
                        , new SqlParameter("dsLongitude", treinamento.dsLongitude)
                        , new SqlParameter("btSiteOBL", treinamento.btSiteOBL)
                        , new SqlParameter("btSiteOMS", treinamento.btSiteOMS)
                        , new SqlParameter("btSiteOLA", treinamento.btSiteOLA)
                        ).FirstOrDefault();


                    if (retorno != null && retorno.idTreinamento != 0)
                    {
                        //VERIFICAR PERIODO
                        var dtDiferenca = dtTreinamentoFim.Date - dtTreinamento.Date;

                        for (int i = 0; i < dtDiferenca.Days; i++)
                        {
                            dtTreinamento = dtTreinamento.AddDays(1);

                            var retornoData = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoDataCadastro @idTreinamento, @dtTreinamento"
                                , new SqlParameter("idTreinamento", retorno.idTreinamento)
                                , new SqlParameter("dtTreinamento", dtTreinamento)
                                ).FirstOrDefault();
                        }

                        return retorno;
                    }

                    return new RetornoPadrao();
                }
                catch { return new RetornoPadrao(); }
            }
        }

        public RetornoPadrao AlteraTreinamentoInternacional(Treinamento treinamento)
        {
            var dtTreinamento = Convert.ToDateTime(treinamento.dsInicio);
            var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
            var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

            treinamento.btBrasil = false;
            treinamento.inQuantidadeVagasPorCNPJ = 0;

            if (treinamento.idDivisao == 4)
                treinamento.idCategoria = 0;

            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoAltera @idTreinamento, @idDivisao, @idCategoria, @dsNome, @dsCodigo, @dtTreinamento, @inQuantidadeVagas, @inQuantidadeVagasPorCNPJ, @dtInscricaoInicio, @dtInscricaoFim, @dsDescricao, @dsUrlInscricao, @btBrasil, @dsHoraInicio, @dsHoraFim, @dsLocalTreinamento, @dsPais, @dsCEP, @dsUF, @dsCidade, @dsEndereco, @dsNumero, @dsBairro, @dsLatitude, @dsLongitude, @btSiteOBL, @btSiteOMS, @btSiteOLA"
                    , new SqlParameter("idTreinamento", treinamento.idTreinamento)
                    , new SqlParameter("idDivisao", treinamento.idDivisao)
                    , new SqlParameter("idCategoria", treinamento.idCategoria)
                    , new SqlParameter("dsNome", treinamento.dsNome)
                    , new SqlParameter("dsCodigo", treinamento.dsCodigoTreinamento)
                    , new SqlParameter("dtTreinamento", dtTreinamento)
                    , new SqlParameter("inQuantidadeVagas", treinamento.inQuantidadeVagas)
                    , new SqlParameter("inQuantidadeVagasPorCNPJ", treinamento.inQuantidadeVagasPorCNPJ)
                    , new SqlParameter("dtInscricaoInicio", dtInscInicio)
                    , new SqlParameter("dtInscricaoFim", dtInscFim)
                    , new SqlParameter("dsDescricao", treinamento.dsDescricao)
                    , new SqlParameter("dsUrlInscricao", treinamento.dsUrlInscricao)
                    , new SqlParameter("btBrasil", treinamento.btBrasil)
                    , new SqlParameter("dsHoraInicio", treinamento.dsHoraInicio)
                    , new SqlParameter("dsHoraFim", treinamento.dsHoraTermino)
                    , new SqlParameter("dsLocalTreinamento", treinamento.dsLocal)
                    , new SqlParameter("dsPais", treinamento.dsPais)
                    , new SqlParameter("dsCEP", treinamento.dsCEP)
                    , new SqlParameter("dsUF", string.IsNullOrEmpty(treinamento.dsUF) ? "" : treinamento.dsUF)
                    , new SqlParameter("dsCidade", treinamento.dsCidade)
                    , new SqlParameter("dsEndereco", treinamento.dsEndereco)
                    , new SqlParameter("dsNumero", treinamento.dsNumero)
                    , new SqlParameter("dsBairro", treinamento.dsBairro)
                    , new SqlParameter("dsLatitude", treinamento.dsLatitude)
                    , new SqlParameter("dsLongitude", treinamento.dsLongitude)
                    , new SqlParameter("btSiteOBL", treinamento.btSiteOBL)
                    , new SqlParameter("btSiteOMS", treinamento.btSiteOMS)
                    , new SqlParameter("btSiteOLA", treinamento.btSiteOLA)
                    ).FirstOrDefault();
            }
            catch { return new RetornoPadrao(); }
        }

        public List<Participacao> ListaInscricoes(Participacao participacao)
        {
            DateTime dtTreinamento = new DateTime();

            if (!string.IsNullOrEmpty(participacao.dsDtTreinamento))
            {
                try { dtTreinamento = Convert.ToDateTime(participacao.dsDtTreinamento); }
                catch { dtTreinamento = System.DateTime.MinValue; }
            }

            List<Participacao> listInscricoes = new List<Participacao>();

            try
            {
                listInscricoes = _db.Database.SqlQuery<Participacao>("SP_TreinamentoListaInscricao @inTipo, @dsNomeCompleto, @dsInstituicao, @dsCPF, @dtTreinamento, @dsCodigo, @idTreinamento, @idStatus"
                    , new SqlParameter("inTipo", participacao.inTipo)
                    , (string.IsNullOrEmpty(participacao.dsNomeCompleto)) ? new SqlParameter("dsNomeCompleto", DBNull.Value) : new SqlParameter("dsNomeCompleto", participacao.dsNomeCompleto)
                    , (string.IsNullOrEmpty(participacao.dsInstituicao)) ? new SqlParameter("dsInstituicao", DBNull.Value) : new SqlParameter("dsInstituicao", participacao.dsInstituicao)
                    , (string.IsNullOrEmpty(participacao.dsCPF)) ? new SqlParameter("dsCPF", DBNull.Value) : new SqlParameter("dsCPF", participacao.dsCPF.Replace(".", "").Replace("-", ""))
                    , (dtTreinamento == System.DateTime.MinValue) ? new SqlParameter("dtTreinamento", DBNull.Value) : new SqlParameter("dtTreinamento", dtTreinamento)
                    , (string.IsNullOrEmpty(participacao.dsCodigo)) ? new SqlParameter("dsCodigo", DBNull.Value) : new SqlParameter("dsCodigo", participacao.dsCodigo)
                    , (participacao.idTreinamento == 0) ? new SqlParameter("idTreinamento", DBNull.Value) : new SqlParameter("idTreinamento", participacao.idTreinamento)
                    , (participacao.idStatusInscricao == 0) ? new SqlParameter("idStatus", DBNull.Value) : new SqlParameter("idStatus", participacao.idStatusInscricao)
                    ).ToList();
            }
            catch { return listInscricoes; }

            return listInscricoes;
        }

        public RetornoPadrao AlteraStatusInscricao(Participacao participacao)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoInscricaoAlteraStatus @idInscricao, @idStatus, @idAdministrador"
                    , new SqlParameter("idInscricao", participacao.idInscricao)
                    , new SqlParameter("idStatus", participacao.idStatusInscricaoAltera)
                    , new SqlParameter("idAdministrador", participacao.idAdministrador)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }

        public RetornoPadrao AlteraStatusInscricaoComListaEspera(Participacao participacao)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoInscricaoAlteraStatusComListaEspera @idInscricao, @idStatus, @idAdministrador"
                    , new SqlParameter("idInscricao", participacao.idInscricao)
                    , new SqlParameter("idStatus", participacao.idStatusInscricaoAltera)
                    , new SqlParameter("idAdministrador", participacao.idAdministrador)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }

        

        public RetornoPadrao ConfirmarPresenca(Participacao participacao)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoInscricaoConfirmaPresenca @idInscricao, @idStatusAtual, @idAdministrador"
                    , new SqlParameter("idInscricao", participacao.idInscricao)
                    , new SqlParameter("idStatusAtual", participacao.idStatusInscricao)
                    , new SqlParameter("idAdministrador", participacao.idAdministrador)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }

        public RetornoPadrao CadastraCertificado(Participacao participacao)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoCertificadoCadastro @idTreinamento, @idInscricao, @idUsuario, @dsNome"
                    , new SqlParameter("idTreinamento", participacao.idTreinamento)
                    , new SqlParameter("idInscricao", participacao.idInscricao)
                    , new SqlParameter("idUsuario", participacao.idAdministrador)
                    , new SqlParameter("dsNome", participacao.dsCertificado)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }


        public RetornoPadrao ConfirmaProximoListaEspera(int idInscricao, int idAdministrador)
        {
            var retorno = new RetornoPadrao();

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoInscricaoConfirmaListaEspera @idInscricao , @idAdministrador",
                                                                                                           new SqlParameter("idInscricao", idInscricao)
                                                                                                         , new SqlParameter("idAdministrador", idAdministrador)).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return new RetornoPadrao() { id = -1, dsMensagem = ex.Message };
            }

            return retorno;
        }


        public int ListaTreinamentoInscricaoStatusUsuario(int idUsuario, int idTreinamento)
        {
            return _db.Database.SqlQuery<int>("SP_TreinamentoListaInscricaoStatusUsuario @idUsuario ,@idTreinamento",
                                                        new SqlParameter("idUsuario", idUsuario)
                                                        , new SqlParameter("idTreinamento", idTreinamento)).FirstOrDefault();
        }


        //ROTINA PARA LIBERAR TREINAMENTOS PARA INSCRIÇÃO
        public void LiberarTreinamento()
        {
            _db.Database.SqlQuery<bool>("SP_RotinaLiberaTreinamento").FirstOrDefault();
        }


        public RetornoPadrao ExcluiTreinamento(int idTreinamento)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoExclui @idTreinamento"
                    , new SqlParameter("idTreinamento", idTreinamento)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }


        public RetornoPadrao CancelaTreinamento(int idTreinamento)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoCancela @idTreinamento"
                    , new SqlParameter("idTreinamento", idTreinamento)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }


        public List<Pesquisa> ListaPesquisaRespostasUsuario(int idPesquisa, int idUsuario, int idInscricao)
        {
            var listPerguntas = _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPrePerguntaLista @idPesquisa"
                , new SqlParameter("idPesquisa", idPesquisa)
                ).ToList();


            foreach (var pergunta in listPerguntas)
            {
                var listAlternativas = _db.Database.SqlQuery<Alternativa>("SP_TreinamentoPreRespostaLista @idPergunta, @idUsuario, @idInscricao"
                    , new SqlParameter("idPergunta", pergunta.idPergunta)
                    , new SqlParameter("idUsuario", idUsuario)
                    , new SqlParameter("idInscricao", idInscricao)
                    ).ToList();

                pergunta.Alternativas = listAlternativas;
            }

            return listPerguntas;
        }

    }
}
