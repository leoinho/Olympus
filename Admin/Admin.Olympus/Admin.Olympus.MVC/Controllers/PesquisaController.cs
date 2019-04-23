using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Repositorio;
using Admin.Olympus.Dominio.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Olympus.MVC.Controllers
{
    public class PesquisaController : Controller
    {
        RepositorioTreinamento _treinamento = new RepositorioTreinamento();
        RepositorioPesquisa _pesquisa = new RepositorioPesquisa();

        public ActionResult Index()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisa), "idTreinamento", "dsNomeComData");
                ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");

                if (TempData["PesquisaAlteracao"] != null)
                {
                    var pesquisa = (Pesquisa)TempData["PesquisaAlteracao"];

                    var treinamento = _treinamento.ListaTreinamentoPorId(pesquisa.idTreinamento);
                    pesquisa.dsNome = treinamento.dsNome;
                    pesquisa.dsCodigo = treinamento.dsCodigo;
                    pesquisa.dsDtTreinamento = treinamento.dtTreinamento.ToShortDateString();
                    pesquisa.idTreinamento = treinamento.idTreinamento;

                    //Verifica se há pesquisa cadastrada
                    var listPesquisa = _pesquisa.ListaPesquisa(pesquisa.idTreinamento, null);

                    if (listPesquisa != null && listPesquisa.Count > 0)
                    {
                        if (listPesquisa.Where(x => x.idTipo == pesquisa.idTipo).Any())
                        {
                            pesquisa.idPesquisa = listPesquisa.FirstOrDefault(x => x.idTipo == pesquisa.idTipo).idPesquisa;

                            if (pesquisa.idTipo == 1 || pesquisa.idTipo == 3)
                                ViewBag.Perguntas = _pesquisa.ListaPerguntaPre(listPesquisa.FirstOrDefault(x => x.idTipo == pesquisa.idTipo).idPesquisa);
                            else
                                ViewBag.Perguntas = _pesquisa.ListaPerguntaPos(listPesquisa.FirstOrDefault(x => x.idTipo == pesquisa.idTipo).idPesquisa);

                            //Verifica se a pesquisa pode ser alterada
                            ViewBag.EditarPesquisa = _pesquisa.ValidaEditarPesquisa(pesquisa);
                        }
                    }


                    if (TempData["Alerta"] != null)
                        ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];


                    return View(pesquisa);
                }
                else
                {
                    if (TempData["Alerta"] != null)
                        ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];
                }

                return View();
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }            
        }

        [HttpPost]
        public ActionResult Index(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisa), "idTreinamento", "dsNomeComData");
                ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");

                var treinamento = _treinamento.ListaTreinamentoPorId(pesquisa.idTreinamento);
                pesquisa.dsNome = treinamento.dsNome;
                pesquisa.dsCodigo = treinamento.dsCodigo;
                pesquisa.dsDtTreinamento = treinamento.dtTreinamento.ToShortDateString();
                pesquisa.idTreinamento = treinamento.idTreinamento;

                //Verifica se há pesquisa cadastrada
                var listPesquisa = _pesquisa.ListaPesquisa(pesquisa.idTreinamento, null);

                if (listPesquisa != null && listPesquisa.Count > 0)
                {
                    if (listPesquisa.Where(x => x.idTipo == pesquisa.idTipo).Any())
                    {
                        pesquisa.idPesquisa = listPesquisa.FirstOrDefault(x => x.idTipo == pesquisa.idTipo).idPesquisa;

                        if (pesquisa.idTipo == 1 || pesquisa.idTipo == 3)
                            ViewBag.Perguntas = _pesquisa.ListaPerguntaPre(listPesquisa.FirstOrDefault(x => x.idTipo == pesquisa.idTipo).idPesquisa);
                        else
                            ViewBag.Perguntas = _pesquisa.ListaPerguntaPos(listPesquisa.FirstOrDefault(x => x.idTipo == pesquisa.idTipo).idPesquisa);

                        //Verifica se a pesquisa pode ser alterada
                        ViewBag.EditarPesquisa = _pesquisa.ValidaEditarPesquisa(pesquisa);
                    }
                }

                return View(pesquisa);
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }

        public ActionResult Cadastrar()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisaPre == false || x.btPesquisaPos == false || x.btPesquisaObg == false), "idTreinamento", "dsNomeComData");

                return View();
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public ActionResult Cadastrar(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;
            
            if (idIdioma == 1)
            {
                //OBL    

                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisaPre == false || x.btPesquisaPos == false || x.btPesquisaObg == false), "idTreinamento", "dsNomeComData");
                
                ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPre().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");

                var treinamento = _treinamento.ListaTreinamentoPorId(pesquisa.idTreinamento);
                pesquisa.dsNome = treinamento.dsNome;
                pesquisa.dsCodigo = treinamento.dsCodigo;
                pesquisa.dsDtTreinamento = treinamento.dtTreinamento.ToShortDateString();
                pesquisa.dtTreinamento = treinamento.dtTreinamento;
                pesquisa.idTreinamento = treinamento.idTreinamento;

                if (treinamento.idDivisao == 4)//Limpeza, desinfecção e esterilização 
                {
                    ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo().Where(w => w.idTipo != 3/*OBRIGATÓRIO*/), "idTipo", "dsNome");
                }
                else
                {
                    ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");
                }

                    return View(pesquisa);
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public ActionResult VincularPesquisa(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                var msg = new ModalMensagem();

                //Verifica se treinamento já possui pesquisa
                if (_pesquisa.ValidaCadastroPesquisa(pesquisa) && pesquisa.idPesquisa > 0)
                {
                    //Se treinamento já ocorreu, não pode cadastrar pesquisa Pré
                    if (pesquisa.idTipo == 1 && pesquisa.dtTreinamento.Date < System.DateTime.Now.Date)
                    {
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemPesquisaPreTreinamentoJaOcorreu") };

                        ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisaPre == false || x.btPesquisaPos == false), "idTreinamento", "dsNomeComData");
                        ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");
                        ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPre().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");

                        var treinamento = _treinamento.ListaTreinamentoPorId(pesquisa.idTreinamento);
                        pesquisa.dsNome = treinamento.dsNome;
                        pesquisa.dsCodigo = treinamento.dsCodigo;
                        pesquisa.dsDtTreinamento = treinamento.dtTreinamento.ToShortDateString();
                        pesquisa.dtTreinamento = treinamento.dtTreinamento;
                        pesquisa.idTreinamento = treinamento.idTreinamento;

                        ViewBag.Alerta = msg;
                        return View("Cadastrar", pesquisa);
                    }

                    //Salva pesquisa ao treinamento
                    pesquisa.btAtivo = true;
                    var retorno = _pesquisa.VincularPesquisa(pesquisa);

                    if (retorno != null && retorno.id != 0)
                    {
                        //SUCESSO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemOK") };

                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //ERRO AO SALVAR TREINAMENTO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemErro") };

                        ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisaPre == false || x.btPesquisaPos == false), "idTreinamento", "dsNomeComData");
                        ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");
                        ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPre().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");

                        var treinamento = _treinamento.ListaTreinamentoPorId(pesquisa.idTreinamento);
                        pesquisa.dsNome = treinamento.dsNome;
                        pesquisa.dsCodigo = treinamento.dsCodigo;
                        pesquisa.dsDtTreinamento = treinamento.dtTreinamento.ToShortDateString();
                        pesquisa.dtTreinamento = treinamento.dtTreinamento;
                        pesquisa.idTreinamento = treinamento.idTreinamento;

                        ViewBag.Alerta = msg;
                        return View("Cadastrar", pesquisa);
                    }
                }
                else
                {
                    //TREINAMENTO JÁ POSSUI PESQUISA DESSE TIPO CADASTRADA
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemPesquisaJaCadastrada") };

                    ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisaPre == false || x.btPesquisaPos == false), "idTreinamento", "dsNomeComData");
                    ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");
                    ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPre().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");

                    var treinamento = _treinamento.ListaTreinamentoPorId(pesquisa.idTreinamento);
                    pesquisa.dsNome = treinamento.dsNome;
                    pesquisa.dsCodigo = treinamento.dsCodigo;
                    pesquisa.dsDtTreinamento = treinamento.dtTreinamento.ToShortDateString();
                    pesquisa.dtTreinamento = treinamento.dtTreinamento;
                    pesquisa.idTreinamento = treinamento.idTreinamento;

                    ViewBag.Alerta = msg;
                    return View("Cadastrar", pesquisa);
                }
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }

        public ActionResult CarregaPesquisa(int idTipo)
        {
            if (idTipo == 1)
                ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPre().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");
            else if (idTipo == 2)
                ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPos().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");
            else
            {
                var pes = _pesquisa.ListaPesquisaObrigatoria().OrderBy(x => x.dtTreinamento);
                if(pes.Count() == 0)
                    ViewBag.Pesquisas = new SelectList(pes, "idPesquisa", "dsNome");
                else
                {

                }
                    ViewBag.Pesquisas = new SelectList(pes, "idPesquisa", "dsNome");
            }
                

            return PartialView("_DivPesquisa");
        }

        public ActionResult InserirPesquisaManualmente(int idTipo, int idTreinamento, int? idPesquisa)
        {
            var msg = new ModalMensagem();
            Pesquisa pesquisa = new Pesquisa();

            var treinamento = _treinamento.ListaTreinamentoPorId(idTreinamento);
            pesquisa.dsNome = treinamento.dsNome;
            pesquisa.dsCodigo = treinamento.dsCodigo;
            pesquisa.dsDtTreinamento = treinamento.dtTreinamento.ToShortDateString();
            pesquisa.dtTreinamento = treinamento.dtTreinamento;
            pesquisa.idTreinamento = treinamento.idTreinamento;
            pesquisa.idTipo = idTipo;

            ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisaPre == false || x.btPesquisaPos == false), "idTreinamento", "dsNomeComData");

            //Verifica se treinamento já possui pesquisa
            if (_pesquisa.ValidaCadastroPesquisa(pesquisa) || idPesquisa != null)
            {
                //Se treinamento já ocorreu, não pode cadastrar pesquisa Pré
                if (pesquisa.idTipo == 1 && pesquisa.dtTreinamento.Date < System.DateTime.Now.Date)
                {
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemPesquisaPreTreinamentoJaOcorreu") };

                    ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");
                    ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPre().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");
                                        
                    ViewBag.Alerta = msg;
                    return View("Cadastrar", pesquisa);
                }


                //PEGA LISTA COM PERGUNTAS

                pesquisa.idPesquisa = (idPesquisa != null) ? Convert.ToInt32(idPesquisa) : 0;

                //Verifica se há pesquisa cadastrada
                var listPesquisa = _pesquisa.ListaPesquisa(idTreinamento, idPesquisa);

                if (listPesquisa != null && listPesquisa.Count > 0)
                {
                    if (listPesquisa.Where(x => x.idTipo == idTipo).Any())
                    {
                        pesquisa.idPesquisa = listPesquisa.FirstOrDefault(x => x.idTipo == idTipo).idPesquisa;

                        if (idTipo == 1 || idTipo == 3)
                            ViewBag.Perguntas = _pesquisa.ListaPerguntaPre(listPesquisa.FirstOrDefault(x => x.idTipo == idTipo).idPesquisa);
                        else
                            ViewBag.Perguntas = _pesquisa.ListaPerguntaPos(listPesquisa.FirstOrDefault(x => x.idTipo == idTipo).idPesquisa);
                    }
                }

                ViewBag.Tipo = idTipo;
                if (idTipo == 1 || idTipo == 3)
                    return View("CadastrarPre", pesquisa);
                else
                    return View("CadastrarPos", pesquisa);
            }
            else
            {
                //TREINAMENTO JÁ POSSUI PESQUISA DESSE TIPO CADASTRADA
                msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemPesquisaJaCadastrada") };

                ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");
                ViewBag.Pesquisas = new SelectList(_pesquisa.ListaPesquisaPre().OrderBy(x => x.dtTreinamento), "idPesquisa", "dsNome");

                ViewBag.Alerta = msg;
                return View("Cadastrar", pesquisa);
            }
        }

        [HttpPost]
        public ActionResult CadastrarPergunta(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1) //OBL
            {
                //Se não há pesquisa, cadastra uma
                if (pesquisa.idPesquisa == 0)
                {
                    if (pesquisa.idTipoPergunta == 1)
                    {
                        if (String.IsNullOrEmpty(pesquisa.dsAlternativaA) || String.IsNullOrEmpty(pesquisa.dsAlternativaB))
                        {
                            if (pesquisa.idTipo == 1 || pesquisa.idTipo == 3)
                                return View("CadastrarPre", pesquisa);
                            else
                                return View("CadastrarPos", pesquisa);
                        }
                    }


                    var retorno = new RetornoPadrao();

                    if (pesquisa.idTipo == 1) //PRE TREINAMENTO
                        retorno = _pesquisa.CadastrarPesquisaPre(pesquisa);
                    else if (pesquisa.idTipo == 2) //POS TREINAMENTO
                        retorno = _pesquisa.CadastrarPesquisaPos(pesquisa);
                    else if (pesquisa.idTipo == 3) //OBRIGATORIA
                        retorno = _pesquisa.CadastrarPesquisaObrigatoria(pesquisa);
                    
                    int idPesquisa = retorno.id;

                    if (retorno != null && retorno.id > 0)
                    {
                        //Alternativas                        
                        if (pesquisa.idTipoPergunta == 1)
                        {
                            

                            List<Alternativa> listAlternativas = new List<Alternativa>();

                            listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaA, btCorreta = pesquisa.btCorretaA });
                            listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaB, btCorreta = pesquisa.btCorretaB });

                            if(!string.IsNullOrEmpty(pesquisa.dsAlternativaC))
                                listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaC, btCorreta = pesquisa.btCorretaC });

                            if (!string.IsNullOrEmpty(pesquisa.dsAlternativaD))
                                listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaD, btCorreta = pesquisa.btCorretaD });

                            if (!string.IsNullOrEmpty(pesquisa.dsAlternativaE))
                                listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaE, btCorreta = pesquisa.btCorretaE });

                            pesquisa.Alternativas = listAlternativas;
                        }



                        
                        if (pesquisa.idTipo == 1) //PRE TREINAMENTO
                            retorno = _pesquisa.CadastrarPerguntaPre(pesquisa);
                        else if (pesquisa.idTipo == 2) //POS TREINAMENTO
                            retorno = _pesquisa.CadastrarPerguntaPos(pesquisa);
                        else if (pesquisa.idTipo == 3) //OBRIGATORIA
                            retorno = _pesquisa.CadastrarPerguntaPre(pesquisa);

                        
                        if (retorno != null && retorno.id > 0)
                        {
                            //SUCESSO
                            return RedirectToAction("InserirPesquisaManualmente", new { idTipo = pesquisa.idTipo, idTreinamento = pesquisa.idTreinamento, idPesquisa = idPesquisa });
                        }
                        else
                        {
                            //ERRO AO CADASTRAR PERGUNTA
                            var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemErroCadastrarPergunta") };
                            ViewBag.Alerta = msg;

                            return RedirectToAction("InserirPesquisaManualmente", new { idTipo = pesquisa.idTipo, idTreinamento = pesquisa.idTreinamento });
                        }
                    }
                    else
                    {
                        //ERRO AO CADASTRAR PERGUNTA
                        var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemErroCadastrarPergunta") };
                        ViewBag.Alerta = msg;

                        return RedirectToAction("InserirPesquisaManualmente", new { idTipo = pesquisa.idTipo, idTreinamento = pesquisa.idTreinamento });
                    }
                }
                else
                {
                    //Alternativas                        
                    if (pesquisa.idTipoPergunta == 1)
                    {
                        List<Alternativa> listAlternativas = new List<Alternativa>();

                        listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaA, btCorreta = pesquisa.btCorretaA });
                        listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaB, btCorreta = pesquisa.btCorretaB });

                        if (!string.IsNullOrEmpty(pesquisa.dsAlternativaC))
                            listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaC, btCorreta = pesquisa.btCorretaC });

                        if (!string.IsNullOrEmpty(pesquisa.dsAlternativaD))
                            listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaD, btCorreta = pesquisa.btCorretaD });

                        if (!string.IsNullOrEmpty(pesquisa.dsAlternativaE))
                            listAlternativas.Add(new Alternativa { dsDescricao = pesquisa.dsAlternativaE, btCorreta = pesquisa.btCorretaE });

                        pesquisa.Alternativas = listAlternativas;
                    }

                    var retorno = new RetornoPadrao();


                    if (pesquisa.idTipo == 1 || pesquisa.idTipo == 3) //PRE TREINAMENTO
                        retorno = _pesquisa.CadastrarPerguntaPre(pesquisa);
                    else if (pesquisa.idTipo == 2) //POS TREINAMENTO
                        retorno = _pesquisa.CadastrarPerguntaPos(pesquisa);


                    if (retorno != null && retorno.id > 0)
                    {
                        //SUCESSO
                        return RedirectToAction("InserirPesquisaManualmente", new { idTipo = pesquisa.idTipo, idTreinamento = pesquisa.idTreinamento, idPesquisa = pesquisa.idPesquisa });
                    }
                    else
                    {
                        //ERRO AO CADASTRAR PERGUNTA
                        var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemErroCadastrarPergunta") };
                        ViewBag.Alerta = msg;

                        return RedirectToAction("InserirPesquisaManualmente", new { idTipo = pesquisa.idTipo, idTreinamento = pesquisa.idTreinamento });
                    }
                }
            }
            else
            {
                //OLA e OMS
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public ActionResult FinalizarPesquisa(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                var retorno = _pesquisa.FinalizarPesquisa(pesquisa);

                if (retorno != null && retorno.id > 0)
                {
                    //SUCESSO
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemOK") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
                else
                {
                    //ERRO AO FINALIZAR
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemErroFinalizar") };
                    ViewBag.Alerta = msg;

                    return RedirectToAction("InserirPesquisaManualmente", new { idTipo = pesquisa.idTipo, idTreinamento = pesquisa.idTreinamento, idPesquisa = pesquisa.idPesquisa });
                }
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }

        public ActionResult Resultado()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisa), "idTreinamento", "dsNomeComData");
                ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");

                if (TempData["Alerta"] != null)
                    ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];

                return View();
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }

        }

        [HttpPost]
        public ActionResult Resultado(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).OrderBy(x => x.dtTreinamento).Where(x => x.btBrasil).Where(x => x.btPesquisa), "idTreinamento", "dsNomeComData");
                ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");

                if (TempData["Alerta"] != null)
                    ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];


                ViewBag.Resultados = _pesquisa.RelatorioResultado(pesquisa);


                return View();
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }
        public ActionResult ResultadoExcel(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var admin = (Administrador)Session["Administrador"];

            var resultado = _pesquisa.RelatorioResultado(pesquisa);

            // Create file
            DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time"));
            string filename = "Relatorio_Resultado_" + now.ToString("ddMMyyyyHHmm") + ".xlsx";

            // Criar excel
            MemoryStream stream = new MemoryStream();
            ExcelStructure excel = new ExcelStructure("Resultado", ref stream);

            excel.setColumnWidth(1, 30);
            excel.setColumnWidth(2, 25);
            excel.setColumnWidth(3, 30);
            excel.setColumnWidth(4, 23);
            excel.setColumnWidth(5, 25);
            excel.setColumnWidth(6, 80);
            excel.setColumnWidth(7, 45);
            excel.setColumnWidth(8, 45);

            excel.createSheetData();

            IList<string> headers = new List<string>();

            headers = new List<string>();

            headers.Add("CÓDIGO DO EVENTO");
            headers.Add("DATA DO EVENTO");
            headers.Add("NOME");
            headers.Add("CPF");
            headers.Add("DATA DE RESPOSTA");
            headers.Add("PERGUNTA");
            headers.Add("RESPOSTA");
            headers.Add("RESPOSTA CORRETA");

            excel.CreateColumnHeader(1, headers);

            IList<string> valores = new List<string>();
            IList<int> tipos = new List<int>();

            uint row = 2;

            foreach (var m in resultado)
            {
                valores = new List<string>();
                tipos = new List<int>();

                valores.Add(m.dsCodigo);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dtTreinamento.ToShortDateString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNomeCompleto);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCPF);
                tipos.Add(ExcelStructure.TYPE_STRING);
                
                valores.Add(m.dtResposta.ToString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsPergunta);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsAlternativa);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsRespostaCorreta);
                tipos.Add(ExcelStructure.TYPE_STRING);

                excel.CreateRowWithContent(row, valores, tipos);
                row++;
            }

            excel.save();
            excel.close();

            stream.Position = 0;

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = filename
            };
        }

        [HttpPost]
        public ActionResult Editar(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
                                
                ViewBag.Tipo = new SelectList(_pesquisa.ListaPesquisaTipo(), "idTipo", "dsNome");
                
                if (TempData["Alerta"] != null)
                    ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];


                var pergunta = _pesquisa.ListaPerguntaEditar(pesquisa);
                pesquisa.dsDescricao = pergunta.dsDescricao;
                pesquisa.idPergunta = pergunta.idPergunta;
                pesquisa.idTipoPergunta = pergunta.idTipo;

                if (pergunta.Alternativas != null && pergunta.Alternativas.Count > 0)
                    ViewBag.Alternativas = pergunta.Alternativas;

                return View(pesquisa);
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public ActionResult EditarPergunta(Pesquisa pesquisa)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                //Alternativas                        
                if (pesquisa.idTipoPergunta == 1)
                {
                    List<Alternativa> listAlternativas = new List<Alternativa>();

                    listAlternativas.Add(new Alternativa { idAlternativa = pesquisa.idAlternativaA, dsDescricao = pesquisa.dsAlternativaA, btCorreta = pesquisa.btCorretaA, btExcluir = string.IsNullOrEmpty(pesquisa.dsAlternativaA) ? true : false });
                    listAlternativas.Add(new Alternativa { idAlternativa = pesquisa.idAlternativaB, dsDescricao = pesquisa.dsAlternativaB, btCorreta = pesquisa.btCorretaB, btExcluir = string.IsNullOrEmpty(pesquisa.dsAlternativaB) ? true : false });
                    listAlternativas.Add(new Alternativa { idAlternativa = pesquisa.idAlternativaC, dsDescricao = pesquisa.dsAlternativaC, btCorreta = pesquisa.btCorretaC, btExcluir = string.IsNullOrEmpty(pesquisa.dsAlternativaC) ? true : false });
                    listAlternativas.Add(new Alternativa { idAlternativa = pesquisa.idAlternativaD, dsDescricao = pesquisa.dsAlternativaD, btCorreta = pesquisa.btCorretaD, btExcluir = string.IsNullOrEmpty(pesquisa.dsAlternativaD) ? true : false });
                    listAlternativas.Add(new Alternativa { idAlternativa = pesquisa.idAlternativaE, dsDescricao = pesquisa.dsAlternativaE, btCorreta = pesquisa.btCorretaE, btExcluir = string.IsNullOrEmpty(pesquisa.dsAlternativaE) ? true : false });

                    pesquisa.Alternativas = listAlternativas;
                }

                var retorno = _pesquisa.AlterarPergunta(pesquisa);

                if (retorno != null && retorno.id > 0)
                {
                    //SUCESSO
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemAlterarPerguntaOK") };

                    TempData["Alerta"] = msg;
                    TempData["PesquisaAlteracao"] = pesquisa;

                    return RedirectToAction("Index");
                }
                else
                {
                    //ERRO AO CADASTRAR PERGUNTA
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("PesquisaModalMensagemErroAlterarPergunta") };
                    
                    TempData["Alerta"] = msg;
                    TempData["PesquisaAlteracao"] = pesquisa;

                    return RedirectToAction("Index");
                }
            }
            else
            {
                //OLA e OMS

                return RedirectToAction("Index", "Dashboard");
            }
        }
	}
}