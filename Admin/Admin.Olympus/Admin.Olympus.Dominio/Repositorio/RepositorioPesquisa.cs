using Admin.Olympus.Dominio.Contexto;
using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Admin.Olympus.Dominio.Repositorio
{
    public class RepositorioPesquisa 
    {
        EFDbContexto _db = new EFDbContexto();
        ConsultaSQL sql = new ConsultaSQL();

        public List<Pesquisa> ListaPesquisaTipo()
        {
            return _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPesquisaTipoLista").ToList();
        }
        public List<Pesquisa> ListaPesquisaPre()
        {
            return _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPrePesquisaLista").ToList();
        }

        public List<Pesquisa> ListaPesquisaObrigatoria()
        {
            return _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPrePesquisaLista  @btObrigatoria"
                , new SqlParameter("btObrigatoria", true)).ToList();
        }

        public List<Pesquisa> ListaPesquisaPos()
        {
            return _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPosPesquisaLista").ToList();
        }

        public bool ValidaCadastroPesquisa(Pesquisa pesquisa)
        {
            return _db.Database.SqlQuery<bool>("SP_TreinamentoPrePosPesquisaValida @idTipo, @idTreinamento"
                , new SqlParameter("idTipo", pesquisa.idTipo)
                , new SqlParameter("idTreinamento", pesquisa.idTreinamento)
                ).FirstOrDefault();
        }

        public RetornoPadrao VincularPesquisa(Pesquisa pesquisa)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPrePosPesquisaCadastro @idTipo, @idPesquisa, @idTreinamento, @btAtivo"
                    , new SqlParameter("idTipo", pesquisa.idTipo)
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    , new SqlParameter("idTreinamento", pesquisa.idTreinamento)
                    , new SqlParameter("btAtivo", pesquisa.btAtivo)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }

        public bool ValidaEditarPesquisa(Pesquisa pesquisa)
        {
            return _db.Database.SqlQuery<bool>("SP_TreinamentoPesquisaAlteraValida @idTipo, @idTreinamento"
                , new SqlParameter("idTipo", pesquisa.idTipo)
                , new SqlParameter("idTreinamento", pesquisa.idTreinamento)
                ).FirstOrDefault();
        }

        public Pesquisa ListaPerguntaEditar(Pesquisa pesquisa)
        {
            if (pesquisa.idTipo == 1 || pesquisa.idTipo == 3)
            {
                var pergunta = _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPrePerguntaLista @idPesquisa"
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    ).FirstOrDefault(x => x.idPergunta == pesquisa.idPergunta);

                if (pergunta.idTipo == 1)
                {
                    var listAlternativas = _db.Database.SqlQuery<Alternativa>("SP_TreinamentoPreAlternativaLista @idPergunta"
                        , new SqlParameter("idPergunta", pergunta.idPergunta)
                        ).ToList();

                    pergunta.idTipoPergunta = 1;
                    pergunta.Alternativas = listAlternativas;

                    var contator = pergunta.Alternativas.Count();
                    for (var i = 0; i < (5 - contator); i++)
                    {
                        pergunta.Alternativas.Add(new Alternativa()
                        {
                            btCorreta = false,
                            btCorretaUsuario = false,
                            idPergunta = pergunta.idPergunta
                        });
                    }
                }

                return pergunta;
            }
            else
            {
                var pergunta = _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPosPerguntaLista @idPesquisa"
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    ).FirstOrDefault(x => x.idPergunta == pesquisa.idPergunta);

                if (pergunta.idTipo == 1)
                {
                    var listAlternativas = _db.Database.SqlQuery<Alternativa>("SP_TreinamentoPosAlternativaLista @idPergunta"
                        , new SqlParameter("idPergunta", pergunta.idPergunta)
                        ).ToList();

                    pergunta.idTipoPergunta = 1;
                    pergunta.Alternativas = listAlternativas;

                    var contator = pergunta.Alternativas.Count();
                    for (var i = 0; i < (5 - contator); i++)
                    {
                        pergunta.Alternativas.Add(new Alternativa()
                        {
                            btCorreta = false,
                            btCorretaUsuario = false,
                            idPergunta = pergunta.idPergunta
                        });
                    }
                }

                return pergunta;
            }
        }

        public List<Pesquisa> ListaPesquisa(int idTreinamento, int? idPesquisa)
        {
            return _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPesquisaLista @idTreinamento, @idPesquisa"
                , new SqlParameter("idTreinamento", idTreinamento)
                , (idPesquisa != null) ? new SqlParameter("idPesquisa", idPesquisa) : new SqlParameter("idPesquisa", DBNull.Value)
                ).ToList();
        }

        public List<Pesquisa> ListaPerguntaPre(int idPesquisa)
        {
            var listPerguntas = _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPrePerguntaLista @idPesquisa"
                , new SqlParameter("idPesquisa", idPesquisa)
                ).ToList();


            foreach (var pergunta in listPerguntas)
            {
                var listAlternativas = _db.Database.SqlQuery<Alternativa>("SP_TreinamentoPreAlternativaLista @idPergunta"
                    , new SqlParameter("idPergunta", pergunta.idPergunta)
                    ).ToList();

                pergunta.Alternativas = listAlternativas;
            }

            return listPerguntas;
        }
        public List<Pesquisa> ListaPerguntaPos(int idPesquisa)
        {
            var listPerguntas = _db.Database.SqlQuery<Pesquisa>("SP_TreinamentoPosPerguntaLista @idPesquisa"
                , new SqlParameter("idPesquisa", idPesquisa)
                ).ToList();


            foreach (var pergunta in listPerguntas)
            {
                var listAlternativas = _db.Database.SqlQuery<Alternativa>("SP_TreinamentoPosAlternativaLista @idPergunta"
                    , new SqlParameter("idPergunta", pergunta.idPergunta)
                    ).ToList();

                pergunta.Alternativas = listAlternativas;
            }

            return listPerguntas;
        }

        public RetornoPadrao CadastrarPesquisaPre(Pesquisa pesquisa)
        {
            try
            {
                //Cadastra Pesquisa Padrão 'Pré Treinamento'
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPrePesquisaCadastro").FirstOrDefault();

                if (retorno != null && retorno.id > 0)
                {
                    pesquisa.idPesquisa = retorno.id;
                    pesquisa.idTipo = 1; //Pre Treinamento
                    pesquisa.btAtivo = false;

                    //Vincula ao Treinamento
                    retorno = VincularPesquisa(pesquisa);

                    if (retorno != null && retorno.id > 0)
                    {
                        retorno.id = pesquisa.idPesquisa;
                        return retorno;
                    }
                    else
                        return new RetornoPadrao();
                }
                else
                    return new RetornoPadrao();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }
        public RetornoPadrao CadastrarPesquisaPos(Pesquisa pesquisa)
        {
            try
            {
                //Cadastra Pesquisa Padrão 'Pós Treinamento'
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPosPesquisaCadastro").FirstOrDefault();

                if (retorno != null && retorno.id > 0)
                {
                    pesquisa.idPesquisa = retorno.id;
                    pesquisa.idTipo = 2; //Pos Treinamento
                    pesquisa.btAtivo = false;

                    //Vincula ao Treinamento
                    retorno = VincularPesquisa(pesquisa);

                    if (retorno != null && retorno.id > 0)
                    {
                        retorno.id = pesquisa.idPesquisa;
                        return retorno;
                    }
                    else
                        return new RetornoPadrao();
                }
                else
                    return new RetornoPadrao();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }



        public RetornoPadrao CadastrarPesquisaObrigatoria(Pesquisa pesquisa)
        {
            try
            {
                //Cadastra Pesquisa Padrão 'Obrigatoria'
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPrePesquisaCadastro  @btObrigatoria"
                , new SqlParameter("btObrigatoria", true)).FirstOrDefault();

                if (retorno != null && retorno.id > 0)
                {
                    pesquisa.idPesquisa = retorno.id;
                    pesquisa.idTipo = 3; //Obrigatoria
                    pesquisa.btAtivo = false;

                    //Vincula ao Treinamento
                    retorno = VincularPesquisa(pesquisa);

                    if (retorno != null && retorno.id > 0)
                    {
                        retorno.id = pesquisa.idPesquisa;
                        return retorno;
                    }
                    else
                        return new RetornoPadrao();
                }
                else
                    return new RetornoPadrao();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }


        public RetornoPadrao CadastrarPerguntaPre(Pesquisa pesquisa)
        {
            try
            {
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPrePerguntaCadastro @idPesquisa, @idTipo, @dsDescricao"
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    , new SqlParameter("idTipo", pesquisa.idTipoPergunta)
                    , new SqlParameter("dsDescricao", pesquisa.dsDescricao)
                    ).FirstOrDefault();

                if (retorno != null && retorno.id > 0)
                {
                    if (pesquisa.idTipoPergunta == 1)
                    {
                        foreach (var alternativa in pesquisa.Alternativas)
                        {
                            _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPreAlternativaCadastro @idPergunta, @dsDescricao, @btCorreta"
                                , new SqlParameter("idPergunta", retorno.id)
                                , new SqlParameter("dsDescricao", alternativa.dsDescricao)
                                , new SqlParameter("btCorreta", alternativa.btCorreta)
                                ).FirstOrDefault();
                        }

                        return retorno;
                    }
                    else
                    {
                        return retorno;
                    }                    
                }
                else
                    return new RetornoPadrao();
            }
            catch(Exception ex)
            {
                return new RetornoPadrao();
            }            
        }
        public RetornoPadrao CadastrarPerguntaPos(Pesquisa pesquisa)
        {
            try
            {
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPosPerguntaCadastro @idPesquisa, @idTipo, @dsDescricao"
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    , new SqlParameter("idTipo", pesquisa.idTipoPergunta)
                    , new SqlParameter("dsDescricao", pesquisa.dsDescricao)
                    ).FirstOrDefault();

                if (retorno != null && retorno.id > 0)
                {
                    if (pesquisa.idTipoPergunta == 1)
                    {
                        foreach (var alternativa in pesquisa.Alternativas)
                        {
                            _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPosAlternativaCadastro @idPergunta, @dsDescricao, @btCorreta"
                                , new SqlParameter("idPergunta", retorno.id)
                                , new SqlParameter("dsDescricao", alternativa.dsDescricao)
                                , new SqlParameter("btCorreta", alternativa.btCorreta)
                                ).FirstOrDefault();
                        }

                        return retorno;
                    }
                    else
                    {
                        return retorno;
                    }
                }
                else
                    return new RetornoPadrao();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }
        
        public RetornoPadrao FinalizarPesquisa(Pesquisa pesquisa)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPesquisaProcessa @idTreinamento, @idPesquisa, @idTipo"
                    , new SqlParameter("idTreinamento", pesquisa.idTreinamento)
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    , new SqlParameter("idTipo", pesquisa.idTipo)
                    ).FirstOrDefault();
            }
            catch
            {
                return new RetornoPadrao();
            }
        }

        public List<Pesquisa> RelatorioResultado(Pesquisa pesquisa)
        {
            if (pesquisa.idTipo == 1) // PRE
                return _db.Database.SqlQuery<Pesquisa>("SP_RelatorioTreinamentoPrePesquisa @idTreinamento, @btExcel"
                    , new SqlParameter("idTreinamento", pesquisa.idTreinamento)
                    , new SqlParameter("btExcel", pesquisa.btExcel)
                    ).ToList();
            else if (pesquisa.idTipo == 2)                     // PÓS
                return _db.Database.SqlQuery<Pesquisa>("SP_RelatorioTreinamentoPosPesquisa @idTreinamento, @btExcel"
                    , new SqlParameter("idTreinamento", pesquisa.idTreinamento)
                    , new SqlParameter("btExcel", pesquisa.btExcel)
                    ).ToList();
            else  // OBRIGATORIO
                return _db.Database.SqlQuery<Pesquisa>("SP_RelatorioTreinamentoPrePesquisa @idTreinamento, @btExcel, @btObrigatoria"
                    , new SqlParameter("idTreinamento", pesquisa.idTreinamento)
                    , new SqlParameter("btExcel", pesquisa.btExcel)
                    , new SqlParameter("btObrigatoria", true)
                    ).ToList();
            
        }

        public RetornoPadrao AlterarPergunta(Pesquisa pesquisa)
        {
            if (pesquisa.idTipo == 1 || pesquisa.idTipo == 3)
            {
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPrePerguntaAltera @idPergunta, @idPesquisa, @dsDescricao"
                    , new SqlParameter("idPergunta", pesquisa.idPergunta)
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    , new SqlParameter("dsDescricao", pesquisa.dsDescricao)
                    ).FirstOrDefault();

                if (retorno != null && retorno.id > 0)
                {
                    if (pesquisa.idTipoPergunta == 1)
                    {
                        foreach (var alternativa in pesquisa.Alternativas)
                        {
                            _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPreAlternativaAltera @idAlternativa, @idPergunta, @dsDescricao, @btCorreta, @btExcluir"
                                , new SqlParameter("idAlternativa", alternativa.idAlternativa)
                                , new SqlParameter("idPergunta", pesquisa.idPergunta)
                                , sql.CreateStringParameter("dsDescricao", alternativa.dsDescricao)
                                , new SqlParameter("btCorreta", alternativa.btCorreta)
                                , new SqlParameter("btExcluir", alternativa.btExcluir)
                                ).FirstOrDefault();
                        }

                        return retorno;
                    }
                    else
                    {
                        return retorno;
                    }
                }
                else
                    return new RetornoPadrao();
            }
            else
            {
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPosPerguntaAltera @idPergunta, @idPesquisa, @dsDescricao"
                    , new SqlParameter("idPergunta", pesquisa.idPergunta)
                    , new SqlParameter("idPesquisa", pesquisa.idPesquisa)
                    , new SqlParameter("dsDescricao", pesquisa.dsDescricao)
                    ).FirstOrDefault();

                if (retorno != null && retorno.id > 0)
                {
                    if (pesquisa.idTipoPergunta == 1)
                    {
                        foreach (var alternativa in pesquisa.Alternativas)
                        {
                            _db.Database.SqlQuery<RetornoPadrao>("SP_TreinamentoPosAlternativaAltera @idAlternativa, @idPergunta, @dsDescricao, @btCorreta, @btExcluir"
                                , new SqlParameter("idAlternativa", alternativa.idAlternativa)
                                , new SqlParameter("idPergunta", pesquisa.idPergunta)
                                , sql.CreateStringParameter("dsDescricao", alternativa.dsDescricao)
                                , new SqlParameter("btCorreta", alternativa.btCorreta)
                                , new SqlParameter("btExcluir", alternativa.btExcluir)
                                ).FirstOrDefault();
                        }

                        return retorno;
                    }
                    else
                    {
                        return retorno;
                    }
                }
                else
                    return new RetornoPadrao();
            }
        }
    }
}
