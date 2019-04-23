using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Fullbar.Olympus.Dominio.Repositorio
{
    public class RepositorioTreinamento : ITreinamento
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public List<TreinamentoHome> ListaTreinamentoPais(int? idIdioma)
        {
            return _db.Database.SqlQuery<TreinamentoHome>("SP_TreinamentoPaisLista @idIdioma",
                         (idIdioma != null && idIdioma != 0) ? new SqlParameter("idIdioma", idIdioma) : new SqlParameter("idIdioma", DBNull.Value)                                                        
                    ).ToList();
        }

        public List<TreinamentoHome> ListaHome(int? idDivisao, int? idCategoria, int? idIdioma, string dsPais, int? idUsuario)
        {
            var Idioma = WebConfigurationManager.AppSettings["Idioma"];

            var retornoLista = _db.Database.SqlQuery<TreinamentoHome>("SP_TreinamentoHomeLista @idDivisao , @idCategoria, @idIdioma, @dsPais, @idUsuario",
                                                                                   (idDivisao != null) ? new SqlParameter("idDivisao", idDivisao) : new SqlParameter("idDivisao", DBNull.Value),
                                                                                   (idCategoria != null) ? new SqlParameter("idCategoria", idCategoria) : new SqlParameter("idCategoria", DBNull.Value),
                                                                                   (idIdioma != null && idIdioma != 0) ? new SqlParameter("idIdioma", idIdioma) : new SqlParameter("idIdioma", DBNull.Value),
                                                                                   (!string.IsNullOrEmpty(dsPais) && dsPais != "Todos") ? new SqlParameter("dsPais", dsPais.ToLower()) : new SqlParameter("dsPais", DBNull.Value),
                                                                                    (idUsuario != null && idUsuario != 0) ? new SqlParameter("idUsuario", idUsuario) : new SqlParameter("idUsuario", DBNull.Value)).ToList();

            var listatreinamento = new List<TreinamentoHome>();

            foreach (var item in retornoLista)
            {
                var mes = new DateTime(1900, item.dtTreinamento.Month, 1).ToString("MMMM", new CultureInfo(Idioma));
                var diaMesPorExtenso = string.Empty;

                var modelTreinamento = new TreinamentoHome()
                {

                    dsCategoria = item.dsCategoria
                    ,
                    dsDivisao = item.dsDivisao
                    ,
                    dsImagem = item.dsImagem
                    ,
                    dsNome = item.dsNome
                    ,
                    dsStatus = item.dsStatus
                    ,
                    idStatus = item.idStatus
                    ,
                    dtTreinamento = item.dtTreinamento
                    ,
                    idTreinamento = item.idTreinamento
                    ,
                    idIdioma = item.idIdioma
                    ,
                    inQuantidadeInscricoes = item.inQuantidadeInscricoes
                    ,
                    inQuantidadeVagas = item.inQuantidadeVagas
                    ,
                    dsDataPorExtenso = (Idioma == "2") ? string.Format("{0} {1}", item.dtTreinamento.Day, char.ToUpper(mes[0]) + mes.Substring(1)) : string.Format("{0} {1}", item.dtTreinamento.Day, char.ToUpper(mes[0]) + mes.Substring(1))
                    ,
                    dsPais = item.dsPais
                    ,
                    dsCidade = item.dsCidade
                    ,
                    inUsuarioStatus = item.inUsuarioStatus
                };

                listatreinamento.Add(modelTreinamento);

            }



            return listatreinamento;

        }

        public RetornoPadrao Cadastrar(TreinamentoInscricao inscricao)
        {
            try
            {
                var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
                var retorno = new RetornoPadrao();
                //var isProcessa = true;
                var arquivoPesquisa = string.Empty;

                var retornoTreinamento = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoInscricaoCadastro @idUsuario, @idEmpresa, @idTreinamento, @btHospedagemPassagem , @idIdioma",
                                                                                                      new SqlParameter("idUsuario", inscricao.idUsuario)
                                                                                                    , new SqlParameter("idEmpresa", inscricao.idEmpresa)
                                                                                                    , new SqlParameter("idTreinamento", inscricao.idTreinamento)
                                                                                                    , new SqlParameter("btHospedagemPassagem", inscricao.hospedagemPassagem)
                                                                                                    , new SqlParameter("idIdioma", idioma)).FirstOrDefault();
                if (retornoTreinamento.id > 0)
                {
                    arquivoPesquisa = retornoTreinamento.dsPesquisa;

                    return new RetornoPadrao() { id = retornoTreinamento.id, dsMensagem = "Inscrição realizada com sucesso", idStatus = retornoTreinamento.idStatus, dsPesquisa = arquivoPesquisa };
                }
                else
                {
                    return new RetornoPadrao() { id = retornoTreinamento.id, dsMensagem = retornoTreinamento.dsMensagem, idStatus = retornoTreinamento.idStatus, dsPesquisa = arquivoPesquisa };
                }   

            }
            catch (Exception ex)
            {
                return new RetornoPadrao { id = -2, dsMensagem = "" };
            }
        }


        public RetornoPadrao CadastrarConvite(TreinamentoInscricao inscricao, RetornoPadrao retornoTreinamento)
        {
            try
            {
                var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
                var retorno = new RetornoPadrao();
                var isProcessa = true;
                var arquivoPesquisa = string.Empty;
               

                if (retornoTreinamento.id > 0)
                {
                    arquivoPesquisa = retornoTreinamento.dsPesquisa;

                    if (inscricao.Convites != null)
                    {
                        foreach (var item in inscricao.Convites)
                        {
                            var retornoTreionamentoConvite = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoConviteCadastro @idTreinamento ,@idEmpresa , @idInscricaoUsuarioConvidante, @idUsuarioConvidante , @dsCPF ,@dsNome ,@dsEmail, @dsCelular",
                                                                                                                             new SqlParameter("idTreinamento", inscricao.idTreinamento)
                                                                                                                           , new SqlParameter("idEmpresa", inscricao.idEmpresa)
                                                                                                                           , new SqlParameter("idInscricaoUsuarioConvidante", retornoTreinamento.id)
                                                                                                                           , new SqlParameter("idUsuarioConvidante", inscricao.idUsuario)
                                                                                                                           , new SqlParameter("dsCPF", item.CPF)
                                                                                                                           , new SqlParameter("dsNome", item.Nome)
                                                                                                                           , new SqlParameter("dsEmail", item.Email)
                                                                                                                           , new SqlParameter("dsCelular", item.Celular)).FirstOrDefault();


                            if (retornoTreionamentoConvite.id == 0)
                            {
                                isProcessa = false;
                                retorno = retornoTreinamento;
                                break;
                            }

                        }


                        if (isProcessa)
                        {
                            retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoConviteProcessa @idTreinamento ,@idEmpresa , @idUsuarioConvidante , @idIdioma, @idInscricaoUsuarioConvidante",
                                                                                                                               new SqlParameter("idTreinamento", inscricao.idTreinamento)
                                                                                                                             , new SqlParameter("idInscricaoUsuarioConvidante", retornoTreinamento.id)
                                                                                                                             , new SqlParameter("idEmpresa", inscricao.idEmpresa)
                                                                                                                             , new SqlParameter("idUsuarioConvidante", inscricao.idUsuario)
                                                                                                                             , new SqlParameter("idIdioma", idioma)).FirstOrDefault();
                        }

                    }
                    else
                    {
                        return new RetornoPadrao() { id = retornoTreinamento.id, dsMensagem = retornoTreinamento.dsMensagem, idStatus = retornoTreinamento.idStatus, dsPesquisa = arquivoPesquisa };
                    }
                }
                else
                {
                    return new RetornoPadrao() { id = retornoTreinamento.id, dsMensagem = retornoTreinamento.dsMensagem, idStatus = retornoTreinamento.idStatus, dsPesquisa = arquivoPesquisa };
                }


                if (retorno.id > 0)
                {
                    return new RetornoPadrao() { id = retornoTreinamento.id, dsMensagem = "Inscrição realizada com sucesso", idStatus = retornoTreinamento.idStatus, dsPesquisa = arquivoPesquisa };
                }


                return retorno;

            }
            catch (Exception ex)
            {
                return new RetornoPadrao { id = -2, dsMensagem = "" };
            }
        }



        public TreinamentoDetalhe ListaPorId(int id, int idUsuario)
        {
            var TreinamentoDetalhe = _db.Database.SqlQuery<TreinamentoDetalhe>("SP_TreinamentoListaPorID @idTreinamento, @idUsuario", new SqlParameter("idTreinamento", id),
                                        (idUsuario != null && idUsuario != 0) ? new SqlParameter("idUsuario", idUsuario) : new SqlParameter("idUsuario", DBNull.Value)).FirstOrDefault();
            var listaPalestranteTreinamento = _db.Database.SqlQuery<TreinamentoPalestrante>("SP_TreinamentoPalestranteLista @idTreinamento", new SqlParameter("idTreinamento", id)).ToList();
            var listaAgendaTreinamento = _db.Database.SqlQuery<TreinamentoAgenda>("SP_TreinamentoAgendaLista @idTreinamento", new SqlParameter("idTreinamento", id)).ToList();
            var treinamentoVagas = _db.Database.SqlQuery<TreinamentoVagas>("SP_TreinamentoQuantidadeVagasLista @idTreinamento", new SqlParameter("idTreinamento", id)).FirstOrDefault();

            var Idioma = WebConfigurationManager.AppSettings["Idioma"];

            var dia = TreinamentoDetalhe.dtTreinamento.Day;
            var mesPorExtenso = new DateTime(1900, TreinamentoDetalhe.dtTreinamento.Month, 1).ToString("MMMM", new CultureInfo(Idioma));
            var mesFinalPorExtenso = new DateTime(1900, TreinamentoDetalhe.dtTreinamentoFim.Month, 1).ToString("MMMM", new CultureInfo(Idioma));

            if (listaPalestranteTreinamento.Count() == 0)
            {
                listaPalestranteTreinamento = new List<TreinamentoPalestrante>() { new TreinamentoPalestrante() {dsImagem = "imagemDefault.png",dsNome = "" , dsPerfil = "" , idPalestrante = 0 , idTreinamento = id } };
            }

            TreinamentoDetalhe.dsDia = dia.ToString();
            TreinamentoDetalhe.dsMesPorExtenso = char.ToUpper(mesPorExtenso[0]) + mesPorExtenso.Substring(1);
            TreinamentoDetalhe.mesFinalPorExtenso = char.ToUpper(mesPorExtenso[0]) + mesFinalPorExtenso.Substring(1);
            TreinamentoDetalhe.Palestrantes = listaPalestranteTreinamento;
            TreinamentoDetalhe.Agendas = listaAgendaTreinamento;
            TreinamentoDetalhe.inQuantidadeTotalVagas = treinamentoVagas.inQuantidadeVagas;
            TreinamentoDetalhe.inQuantidadeVagas = treinamentoVagas.inQuantidadeVagasEmAberto;

            var listaDescricao = new List<string>();


            if (TreinamentoDetalhe.dsDescricao.Length > 690)
            {
                var descricao = TreinamentoDetalhe.dsDescricao.Substring(0, 690);
                var descricao2 = TreinamentoDetalhe.dsDescricao.Substring(690, TreinamentoDetalhe.dsDescricao.Length - 690);


                TreinamentoDetalhe.dsListaDescricao = new List<string> { descricao, descricao2 };
            }
            else
            {
                TreinamentoDetalhe.dsListaDescricao = new List<string> { TreinamentoDetalhe.dsDescricao };
            }


            return TreinamentoDetalhe;

        }

        public TreinamentoInscricaoResultado ListaResultado(int idTreinamento, int idEmpresa, int idUsuario, int idInscricaoUsuarioConvidante)
        {
            var treinamento = new TreinamentoInscricaoResultado();
            var listaTreinamentoConfirmado = new List<TreinamentoInscricaoResultadoConfirmado>();
            var listaTreinamentoEspera = new List<TreinamentoResultadoListaEspera>();

            var retorno = _db.Database.SqlQuery<TreinamentoInscricaoResultadoConfirmado>("SP_TreinamentoConviteResultado @idTreinamento ,@idEmpresa , @idUsuarioConvidante, @idInscricaoUsuarioConvidante ",
                                                                                                                        new SqlParameter("idTreinamento", idTreinamento)
                                                                                                                      , new SqlParameter("idEmpresa", idEmpresa)
                                                                                                                      , new SqlParameter("idUsuarioConvidante", idUsuario)
                                                                                                                      , new SqlParameter("idInscricaoUsuarioConvidante", idInscricaoUsuarioConvidante)
                                                                                                                      ).ToList();


            foreach (var item in retorno)
            {
                if (item.idStatus == 1 || item.idStatus == 2 || item.idStatus == 5 || item.idStatus == 6 || item.idStatus == 7 || item.idStatus == 8)
                {
                    listaTreinamentoConfirmado.Add(new TreinamentoInscricaoResultadoConfirmado() { idConvite = item.idConvite, dsCPF = item.dsCPF, dsNome = item.dsNome, dsEmail = item.dsEmail, dsStatus = item.dsStatus, idStatus = item.idStatus });
                }

                if (item.idStatus == 3 || item.idStatus == 4)
                {
                    listaTreinamentoEspera.Add(new TreinamentoResultadoListaEspera() { dsNome = item.dsNome });
                }

            }


            treinamento.Confirmado = listaTreinamentoConfirmado;
            treinamento.ListaEspera = listaTreinamentoEspera;



            return treinamento;

        }

        public RetornoPadrao CadastrarEstrangeiro(TreinamentoInscricaoEstrangeiro inscricao)
        {
            var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
            var retornoTreinamento = new RetornoPadrao();

            try
            {
                retornoTreinamento = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoInscricaoCadastro @idUsuario, @idEmpresa, @idTreinamento, @btHospedagemPassagem , @idIdioma, @dsCurriculo",
                                                                                                 new SqlParameter("idUsuario", inscricao.idUsuario)
                                                                                               , new SqlParameter("idEmpresa", inscricao.idEmpresa)
                                                                                               , new SqlParameter("idTreinamento", inscricao.idTreinamento)
                                                                                               , new SqlParameter("btHospedagemPassagem", inscricao.hospedagem)
                                                                                               , new SqlParameter("idIdioma", idioma),
                                                                                               (!string.IsNullOrEmpty(inscricao.dsCurriculo)) ? new SqlParameter("dsCurriculo", inscricao.dsCurriculo) : new SqlParameter("dsCurriculo", DBNull.Value)).FirstOrDefault();

            }
            catch (Exception ex)
            {

                return new RetornoPadrao() { id = -1, dsMensagem = "erro ao tentar cadastrar inscrição." };
            }

            return retornoTreinamento;

        }

        public List<TreinamentoUsuario> ListaPorUsuario(int idUsuario)
        {
            var listaTreinamento = new List<TreinamentoUsuario>();
            var Idioma = WebConfigurationManager.AppSettings["Idioma"];

            try
            {
                var retornoListaTreinamento = _db.Database.SqlQuery<TreinamentoUsuario>("SP_TreinamentoUsuarioListaPorId @idUsuario", new SqlParameter("idUsuario", idUsuario)).ToList();

                foreach (var item in retornoListaTreinamento)
                {

                    var retornoTreinamentoAgenda = _db.Database.SqlQuery<TreinamentoAgenda>("SP_TreinamentoAgendaLista @idTreinamento", new SqlParameter("idTreinamento", item.idTreinamento)).ToList();

                    var modelTreinamento = new TreinamentoUsuario()
                    {

                        Agenda = retornoTreinamentoAgenda
                        ,
                        dsBairro = item.dsBairro
                        ,
                        dsCidade = item.dsCidade
                        ,
                        dsEndereco = item.dsEndereco
                        ,
                        dsImagem = item.dsImagem
                        ,
                        dsNome = item.dsNome
                        ,
                        dsNumero = item.dsNumero
                        ,
                        dsUF = item.dsUF
                        ,
                        idTreinamento = item.idTreinamento
                        ,
                        idStatusInscricao = item.idStatusInscricao
                        ,
                        DataTreinamento = string.Format("{0}/{1}", item.dtTreinamento.Day, item.dtTreinamento.Month)
                        ,
                        idDivisao = item.idDivisao
                        ,
                        btMaterial  = item.btMaterial
                        ,
                        dsMaterial = item.dsMaterial
                        ,
                        btConvidado = item.btConvidado
                        ,
                        btQuestionarioPre = item.btQuestionarioPre
                        ,
                        btQuestionarioPos = item.btQuestionarioPos
                        ,
                        btQuestionarioPreRespondido = item.btQuestionarioPreRespondido
                        ,
                        btQuestionarioPosRespondido = item.btQuestionarioPosRespondido
                        , 
                        dtTreinamento = item.dtTreinamento
                        ,
                        dsMes = new DateTime(1900, item.dtTreinamento.Month, 1).ToString("MMMM", new CultureInfo(Idioma))
                        ,
                        dtTreinamentoFim = item.dtTreinamentoFim
                        ,
                        dsMesFim = new DateTime(1900, item.dtTreinamentoFim.Month, 1).ToString("MMMM", new CultureInfo(Idioma))
                    };

                    listaTreinamento.Add(modelTreinamento);
                }

                return listaTreinamento;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                return null;
            }
        }

        public TreinamentoInscricaoUsuario ListaInscricaoPorUsuario(Usuario usuario, int idTreinamento)
        {
            try
            {
                var treinamento = ListaPorId(idTreinamento, usuario.idusuario);

                var retornoInscricao = new TreinamentoInscricaoUsuario()
                {

                    CNPJ = (usuario.RetornoCNPJ != null) ? usuario.RetornoCNPJ.dsCNPJ : string.Empty
                    ,
                    CPF = (usuario.RetornoCNPJ != null) ? usuario.dsCPF : string.Empty
                    ,
                    Data = treinamento.dtTreinamento.ToString("dd/MM/yyyy")
                    ,
                    DataTreinamento = treinamento.dtTreinamento
                    ,
                    HoraInicio = treinamento.dsHoraInicio
                    ,
                    HoraFim = treinamento.dsHoraTermino
                    ,
                    Endereco = string.Format("{0} , {1}",treinamento.dsEndereco,treinamento.dsNumero)
                    
                    ,Bairro = treinamento.dsBairro
                    ,UF = treinamento.dsUF
                    ,Cidade = treinamento.dsCidade
                    ,
                    Nome = usuario.dsNomeCompleto
                    ,
                    Telefone = usuario.dsTelefone
                    ,
                    Treinamento = treinamento.dsNome
                };


                return retornoInscricao;

            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }


        public RetornoPadrao CancelarInscricao(int idUsuario, int idTreinamento)
        {
            var retorno = new RetornoPadrao();
            var idioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);
            var motivo = "Cancelamento realizado no site pelo usuário";

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoCancelamentoCadastro @idUsuario, @idTreinamento , @dsMotivo , @idIdioma",
                                                                                                      new SqlParameter("idUsuario", idUsuario)
                                                                                                    , new SqlParameter("idTreinamento", idTreinamento)
                                                                                                    , new SqlParameter("dsMotivo", motivo)
                                                                                                    , new SqlParameter("idIdioma", idioma)).FirstOrDefault();

            }
            catch (Exception ex)
            {

                new RetornoPadrao() { id = -1, dsMensagem = "Erro ao tentar realizar cancelamento" };
            }

            return retorno;
        }
        public RetornoPadrao CancelarInscricaoUsuarioConvidado(int idUsuario, int idTreinamento)
        {
            var retorno = new RetornoPadrao();
            var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
            var motivo = "Cancelamento realizado pelo usuário convidante";

            try
            {
                retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoCancelamentoCadastro @idUsuario, @idTreinamento , @dsMotivo , @idIdioma",
                                                                                                      new SqlParameter("idUsuario", idUsuario)
                                                                                                    , new SqlParameter("idTreinamento", idTreinamento)
                                                                                                    , new SqlParameter("dsMotivo", motivo)
                                                                                                    , new SqlParameter("idIdioma", idioma)).FirstOrDefault();

            }
            catch (Exception ex)
            {

                new RetornoPadrao() { id = -1, dsMensagem = "Erro ao tentar realizar cancelamento" };
            }

            return retorno;
        }

        public List<TreinamentoPrePergunta> ListaPerguntas(int idTreinamento, int idTipo)
        {
            var listaPerguntasRespostas = new List<TreinamentoPrePergunta>();

            try
            {
                var pesquisa = _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPesquisaLista @idTreinamento, @idPesquisa"
                    , new SqlParameter("idTreinamento", idTreinamento)
                    , new SqlParameter("idPesquisa", DBNull.Value)
                    ).FirstOrDefault(x => x.idTipo == idTipo);

                //PRÉ
                if (idTipo == 1 || idTipo == 3)
                {
                    var listaPergunta = _db.Database.SqlQuery<TreinamentoPrePergunta>("SP_TreinamentoPrePerguntaLista @idPesquisa"
                        , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                        ).ToList();


                    foreach (var itemPergunta in listaPergunta)
                    {
                        var listaAlternativas = _db.Database.SqlQuery<TreinamentoPreAlternativa>("SP_TreinamentoPreAlternativaLista @idPergunta"
                            , new SqlParameter("idPergunta", itemPergunta.idPergunta)
                            ).ToList();

                        var perguntaAlternativa = new TreinamentoPrePergunta()
                        {

                            Alternativas = listaAlternativas
                            ,
                            dsDescricao = itemPergunta.dsDescricao
                            ,
                            dsTipo = itemPergunta.dsTipo
                            ,
                            dtCadastro = itemPergunta.dtCadastro
                            ,
                            idPergunta = itemPergunta.idPergunta
                            ,
                            idTipo = itemPergunta.idTipo
                        };

                        listaPerguntasRespostas.Add(perguntaAlternativa);
                    }

                    return listaPerguntasRespostas;
                }
                else //PÓS
                {
                    var listaPergunta = _db.Database.SqlQuery<TreinamentoPrePergunta>("SP_TreinamentoPosPerguntaLista @idPesquisa"
                        , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                        ).ToList();

                    foreach (var itemPergunta in listaPergunta)
                    {
                        var listaAlternativas = _db.Database.SqlQuery<TreinamentoPreAlternativa>("SP_TreinamentoPosAlternativaLista @idPergunta"
                            , new SqlParameter("idPergunta", itemPergunta.idPergunta)
                            ).ToList();

                        var perguntaAlternativa = new TreinamentoPrePergunta()
                        {

                            Alternativas = listaAlternativas
                            ,
                            dsDescricao = itemPergunta.dsDescricao
                            ,
                            dsTipo = itemPergunta.dsTipo
                            ,
                            dtCadastro = itemPergunta.dtCadastro
                            ,
                            idPergunta = itemPergunta.idPergunta
                            ,
                            idTipo = itemPergunta.idTipo
                        };

                        listaPerguntasRespostas.Add(perguntaAlternativa);
                    }

                    return listaPerguntasRespostas;
                }                
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public RetornoPadrao AlternativaCadastro(List<TreinamentoAlternativaCadastro> alternativas)
        {
            var retorno = new RetornoPadrao();

            try
            {
                foreach (var itemAlternativa in alternativas)
                {
                    retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPreAlternativaCadastro @idTreinamento , @idPergunta , @dsDescricao , @btCorreta",
                                                                                                           new SqlParameter("idTreinamento", itemAlternativa.idTreinamento)
                                                                                                         , new SqlParameter("idPergunta", itemAlternativa.idPergunta)
                                                                                                         , new SqlParameter("dsDescricao", itemAlternativa.dsDescricao)
                                                                                                         , new SqlParameter("btCorreta", itemAlternativa.btCorreta)).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return new RetornoPadrao() { id = -1, dsMensagem = "erro ao tentar cadastrar alternativa" };
            }

            return retorno;
        }

        public RetornoPadrao CadastraRespostasQuestionario(TreinamentoPrePergunta treinamentoPrePergunta)
        {
            var retorno = new RetornoPadrao();

            try
            {
                if (treinamentoPrePergunta.idTipo == 1) //PRÉ
                {

                    foreach (var itemAlternativa in treinamentoPrePergunta.Alternativas)
                    {
                        retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPreRespostaCadastro @idTreinamento , @idInscricao , @idUsuario , @idPergunta, @idAlternativa, @dsDescricao",
                                                                                                                new SqlParameter("idTreinamento", treinamentoPrePergunta.idTreinamento)
                                                                                                                , new SqlParameter("idInscricao", treinamentoPrePergunta.idInscricao)
                                                                                                                , new SqlParameter("idUsuario", treinamentoPrePergunta.idUsuario)
                                                                                                                , new SqlParameter("idPergunta", itemAlternativa.idPergunta)
                                                                                                                , new SqlParameter("idAlternativa", itemAlternativa.idAlternativa)
                                                                                                                , (string.IsNullOrEmpty(itemAlternativa.dsDescricao)) ? new SqlParameter("dsDescricao", string.Empty) : new SqlParameter("dsDescricao", itemAlternativa.dsDescricao)).FirstOrDefault();
                    }
                }
                else
                {
                    foreach (var itemAlternativa in treinamentoPrePergunta.Alternativas)
                    {
                        retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPosRespostaCadastro @idTreinamento , @idInscricao , @idUsuario , @idPergunta, @idAlternativa, @dsDescricao",
                                                                                                                new SqlParameter("idTreinamento", treinamentoPrePergunta.idTreinamento)
                                                                                                                , new SqlParameter("idInscricao", treinamentoPrePergunta.idInscricao)
                                                                                                                , new SqlParameter("idUsuario", treinamentoPrePergunta.idUsuario)
                                                                                                                , new SqlParameter("idPergunta", itemAlternativa.idPergunta)
                                                                                                                , new SqlParameter("idAlternativa", itemAlternativa.idAlternativa)
                                                                                                                , (string.IsNullOrEmpty(itemAlternativa.dsDescricao)) ? new SqlParameter("dsDescricao", string.Empty) : new SqlParameter("dsDescricao", itemAlternativa.dsDescricao)).FirstOrDefault();
                    }
                }
            }
            catch
            {
                return new RetornoPadrao() { id = -1, dsMensagem = "Erro ao tentar cadastrar resposta." };
            }

            return retorno;
        }

        public List<TreinamentoCalendario> TreinamentoCalendario(int mes, int ano, int idUsuario)
        {
            //var listaTreinamentoCalendario = new List<TreinamentoCalendario>();
            var Idioma = WebConfigurationManager.AppSettings["Idioma"];

            var listaRetorno = _db.Database.SqlQuery<TreinamentoCalendario>("SP_TreinamentoCalendarioLista @inMes , @inAno , @idUsuario",
                new SqlParameter("inMes", mes), new SqlParameter("inAno", ano), (idUsuario != 0) ? new SqlParameter("idUsuario", idUsuario) : new SqlParameter("idUsuario", DBNull.Value)).ToList();

            //foreach (var item in listaRetorno)
            //{
            //    var treinamentoCalendario = new TreinamentoCalendario();

            //    treinamentoCalendario.Idioma = Idioma;
            //    treinamentoCalendario.dsNome = item.dsNome;
            //    treinamentoCalendario.dtTreinamento = item.dtTreinamento;
            //    treinamentoCalendario.idTreinamento = item.idTreinamento;
            //    treinamentoCalendario.Mes = item.dtTreinamento.Month;
            //    treinamentoCalendario.Ano = item.dtTreinamento.Year;
            //    treinamentoCalendario.DataTreinamento = item.dtTreinamento.ToString("dd/MM/yyyy");
            //    treinamentoCalendario.idStatus = (item.idStatusInscricao == 0) ? item.idStatusTreinamento : item.idStatusInscricao;
            //    treinamentoCalendario.dsStatusInscricao = item.dsStatusInscricao;
                

            //    listaTreinamentoCalendario.Add(treinamentoCalendario);
            //}

            return listaRetorno;
        }

        public int ListaTreinamentoInscricaoUsuario(int idUsuario, int idTreinamento)
        {
            return _db.Database.SqlQuery<int>("SP_TreinamentoListaInscricaoUsuario @idUsuario ,@idTreinamento",
                                                        new SqlParameter("idUsuario", idUsuario)
                                                        , new SqlParameter("idTreinamento", idTreinamento)).FirstOrDefault();
        }

        public int ListaTreinamentoInscricaoStatusUsuario(int idUsuario, int idTreinamento)
        {
            return _db.Database.SqlQuery<int>("SP_TreinamentoListaInscricaoStatusUsuario @idUsuario ,@idTreinamento",
                                                        new SqlParameter("idUsuario", idUsuario)
                                                        , new SqlParameter("idTreinamento", idTreinamento)).FirstOrDefault();
        }

        public RetornoPadrao VerificaInscricaoUsuario(string CPF, int idTreinamento)
        {
            return  _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoInscricaoUsuario @dsCPF ,@idTreinamento",
                                                                                                           new SqlParameter("dsCPF", CPF.Replace(".","").Replace("-",""))
                                                                                                         , new SqlParameter("idTreinamento", idTreinamento)).FirstOrDefault();
        }

        public bool QuestionarioRespondido(int idTreinamento, int idUsuario, int inQuestionario) //inQuestionario - 1.Pre ou 2.Pos
        {
            return _db.Database.SqlQuery<bool>("SP_TreinamentoUsuarioQuestionarioVerifica @idTreinamento, @idUsuario, @inQuestionario"
                                                    , new SqlParameter("idTreinamento", idTreinamento)
                                                    , new SqlParameter("idUsuario", idUsuario)
                                                    , new SqlParameter("inQuestionario", inQuestionario)).FirstOrDefault();
        }

        public bool CadastraLogTreinamentoExterno(TreinamentoExterno treinamentoExterno)
        {
            return _db.Database.SqlQuery<bool>("SP_TreinamentoExternoCadastraLog @idUsuario, @idTreinamento, @idIdioma"
                                                    , new SqlParameter("idUsuario", treinamentoExterno.idUsuario)
                                                    , new SqlParameter("idTreinamento", treinamentoExterno.idTreinamento)
                                                    , new SqlParameter("idIdioma", treinamentoExterno.idIdioma)
                                                    ).FirstOrDefault();
        }
        
        public List<TreinamentoConvite> ListaConvidados(int idUsuario, int idTreinamento)
        {
            return _db.Database.SqlQuery<TreinamentoConvite>("SP_TreinamentoConvidadosLista @idUsuarioConvidante, @idTreinamento",
                new SqlParameter("idUsuarioConvidante", idUsuario),
                new SqlParameter("idTreinamento", idTreinamento)).ToList();
        }

        public List<TreinamentoConvite> ListaConvidadosCertificado(int idUsuario, int idTreinamento)
        {
            return _db.Database.SqlQuery<TreinamentoConvite>("SP_TreinamentoConvidadosListaCertificados @idUsuarioConvidante, @idTreinamento",
                new SqlParameter("idUsuarioConvidante", idUsuario),
                new SqlParameter("idTreinamento", idTreinamento)).ToList();
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
        
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
