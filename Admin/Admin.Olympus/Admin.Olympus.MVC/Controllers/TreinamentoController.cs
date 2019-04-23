using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Repositorio;
using Admin.Olympus.Dominio.Util;
using Admin.Olympus.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Olympus.MVC.Controllers
{
    public class TreinamentoController : Controller
    {
        RepositorioTreinamento _treinamento = new RepositorioTreinamento();

        #region Listar

        public ActionResult Index()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            Treinamento treinamento = new Treinamento();

            if (idIdioma == 1)
            {
                //OBL
                ViewBag.Site = ddlSite();
            }
            else
            {
                //OLA e OMS
                treinamento.idIdioma = idIdioma;
            }

            if ((Session["CultureIdioma"]) == "es")
            {
                ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNomeES");
                ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNomeES");
            }
            else
            {
                ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
                ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            }



            ViewBag.Status = _treinamento.ListaTreinamentoStatus().Where(s => s.idStatus != 1); 

            if (TempData["Alerta"] != null)
                ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];

            if (Session["TreinamentoFiltrado"] != null)
            {
                treinamento = (Treinamento)Session["TreinamentoFiltrado"];
                ViewBag.Treinamentos = _treinamento.ListaTreinamento(treinamento);
            }

            //Exclui sessões de Palestrantes e Agenda
            Session.Remove("listPalestrantes");
            Session.Remove("listAgenda");

            return View(treinamento);
        }
        [HttpPost]
        public ActionResult Index(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            Session.Add("TreinamentoFiltrado", treinamento);
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
                ViewBag.Site = ddlSite(); 
            }
            else
            {
                //OLA e OMS
                treinamento.idIdioma = idIdioma;
            }

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            ViewBag.Status = _treinamento.ListaTreinamentoStatus().Where(s => s.idStatus != 1);

            ViewBag.Treinamentos = _treinamento.ListaTreinamento(treinamento);

            //Exclui sessões de Palestrantes e Agenda
            Session.Remove("listPalestrantes");
            Session.Remove("listAgenda");

            return View(treinamento);
        }

        #endregion

        #region Cadastro/Edição

        public ActionResult Cadastrar()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
            }
            else
            {
                //OLA e OMS
            }

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            //ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");

            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
            }
            else
            {
                //OLA e OMS
            }

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");

            if (treinamento != null && treinamento.idDivisao != 0)
            {
                if (treinamento.idDivisao == 3)
                {
                    //EDUCAÇÃO PROFISSIONAL
                    ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");

                    return View("CadastrarHCP", treinamento);
                }
                else
                {
                    //LIMPEZA, DESINFECÇÃO E ESTERILIZAÇÃO

                    return View("CadastrarCPC", treinamento);
                }
            }
            else
            {
                return View();
            }
        }
        
        [HttpPost]
        public ActionResult CadastrarHCP(Treinamento treinamento, HttpPostedFileBase dsImagem, HttpPostedFileBase dsMaterial)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");            

            //EDUCAÇÃO PROFISSIONAL
            var msg = ValidaTreinamentoHCP(treinamento, dsImagem, dsMaterial);

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                //Erro na validação
                ViewBag.Alerta = msg;
                return View("CadastrarHCP", treinamento);
            }
            else
            {
                treinamento = SalvaArquivosHCP(treinamento, dsImagem, dsMaterial);

                if (treinamento.btErro)
                {
                    //Erro ao salvar arquivo
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErroArquivo") };

                    ViewBag.Alerta = msg;
                    return View("CadastrarHCP", treinamento);
                }
                else
                {
                    //Salva Treinamento
                    treinamento.idAdministrador = usuario.idAdministrador;
                    treinamento.btBrasil = true;
                    treinamento.dsPais = "Brasil";
                    treinamento.idStatus = 3; //ENTRA COMO INATIVO
                    treinamento.idIdioma = idIdioma;
                    treinamento.inQuantidadeVagasPorCNPJ = 0;

                    var retorno = _treinamento.CadastraTreinamento(treinamento);

                    if (retorno != null && retorno.id != 0)
                    {
                        //SUCESSO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemOK") };

                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //ERRO AO SALVAR TREINAMENTO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErro") };

                        ViewBag.Alerta = msg;
                        return View("CadastrarHCP", treinamento);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult CadastrarCPC(Treinamento treinamento, HttpPostedFileBase dsImagem, HttpPostedFileBase dsMaterial)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");

            //LIMPEZA, DESINFECÇÃO E ESTERILIZAÇÃO
            var msg = ValidaTreinamentoCPC(treinamento, dsImagem, dsMaterial);

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                //Erro na validação
                ViewBag.Alerta = msg;
                return View("CadastrarCPC", treinamento);
            }
            else
            {
                treinamento = SalvaArquivosCPC(treinamento, dsImagem, dsMaterial);

                if (treinamento.btErro)
                {
                    //Erro ao salvar arquivo
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErroArquivo") };

                    ViewBag.Alerta = msg;
                    return View("CadastrarCPC", treinamento);
                }
                else
                {
                    //Salva Treinamento
                    treinamento.idAdministrador = usuario.idAdministrador;
                    treinamento.btBrasil = true;
                    treinamento.dsPais = "Brasil";
                    treinamento.idStatus = 3; //ENTRA COMO INATIVO
                    treinamento.idIdioma = idIdioma;

                    var retorno = _treinamento.CadastraTreinamento(treinamento);

                    if (retorno != null && retorno.id != 0)
                    {
                        //SUCESSO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemOK") };

                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //ERRO AO SALVAR TREINAMENTO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErro") };

                        ViewBag.Alerta = msg;
                        return View("CadastrarCPC", treinamento);
                    }
                }
            }
        }
        
        public ActionResult CadastrarInternacional()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
            }
            else
            {
                //OLA e OMS
            }           

            if ((Session["CultureIdioma"]) == "es")
            {
                ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNomeES");
                ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNomeES");
            }
            else
            {
                ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
                ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            }

            ViewBag.Pais = new SelectList(_treinamento.ListaPais(idIdioma).Where(x => x.dsNome != "BRAZIL").Where(x => x.dsNome != "BRASIL").OrderBy(x => x.dsNome), "dsNome", "dsNome");

            return View();
        }

        [HttpPost]
        public ActionResult CadastrarInternacional(Treinamento treinamento, HttpPostedFileBase dsImagem)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            ViewBag.Pais = new SelectList(_treinamento.ListaPais(idIdioma).Where(x => x.dsNome != "BRAZIL").Where(x => x.dsNome != "BRASIL").OrderBy(x => x.dsNome), "dsNome", "dsNome");

            var msg = ValidaTreinamentoInternacional(treinamento, dsImagem);

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                //Erro na validação
                ViewBag.Alerta = msg;
                return View("CadastrarInternacional", treinamento);
            }
            else
            {
                treinamento = SalvaArquivoInternacional(treinamento, dsImagem);

                if (treinamento.btErro)
                {
                    //Erro ao salvar arquivo
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErroArquivo") };

                    ViewBag.Alerta = msg;
                    return View("CadastrarInternacional", treinamento);
                }
                else
                {
                    //Salva Treinamento
                    treinamento.idAdministrador = usuario.idAdministrador;
                    treinamento.btBrasil = false;
                    treinamento.idStatus = 4; //SEMPRE ENTRA COMO EM BREVE
                    treinamento.idIdioma = idIdioma;
                    treinamento.inQuantidadeVagasPorCNPJ = 0;

                    var retorno = _treinamento.CadastraTreinamentoInternacional(treinamento);

                    if (retorno != null && retorno.id != 0)
                    {
                        //SUCESSO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemOK") };

                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //ERRO AO SALVAR TREINAMENTO
                        msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErro") };

                        ViewBag.Alerta = msg;
                        return View("CadastrarInternacional", treinamento);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Editar(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
            }
            else
            {
                //OLA e OMS
            }

            //Exclui sessões de Palestrantes e Agenda
            Session.Remove("listPalestrantes");
            Session.Remove("listAgenda");

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            treinamento = _treinamento.ListaTreinamentoPorId(treinamento.idTreinamento);

            if (treinamento.btBrasil)
            {
                //TREINAMENTO BRASIL

                if (treinamento.idDivisao == 3)
                {
                    //EDUCAÇÃO PROFISSIONAL

                    ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
                    ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");

                    treinamento.dsCodigoTreinamento = treinamento.dsCodigo;
                    treinamento.dsInicio = treinamento.dtTreinamento.ToString();
                    treinamento.dsTermino = treinamento.dtTreinamentoFim.ToString();
                    treinamento.dsInscInicio = treinamento.dtInscricaoInicio.ToString();
                    treinamento.dsInscFim = treinamento.dtInscricaoFim.ToString();
                    treinamento.dsLocal = treinamento.dsLocalTreinamento;

                    return View("EditarHCP", treinamento);
                }
                else
                {
                    //LIMPEZA, DESINFECÇÃO E ESTERILIZAÇÃO

                    ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");

                    treinamento.dsCodigoTreinamento = treinamento.dsCodigo;
                    treinamento.dsInicio = treinamento.dtTreinamento.ToString();
                    treinamento.dsTermino = treinamento.dtTreinamentoFim.ToString();
                    treinamento.dsInscInicio = treinamento.dtInscricaoInicio.ToString();
                    treinamento.dsInscFim = treinamento.dtInscricaoFim.ToString();
                    treinamento.dsLocal = treinamento.dsLocalTreinamento;

                    return View("EditarCPC", treinamento);
                }
            }
            else
            {
                //TREINAMENTO INTERNACIONAL

                ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
                ViewBag.Pais = new SelectList(_treinamento.ListaPais(idIdioma).Where(x => x.dsNome != "BRAZIL").Where(x => x.dsNome != "BRASIL").OrderBy(x => x.dsNome), "dsNome", "dsNome");

                treinamento.dsCodigoTreinamento = treinamento.dsCodigo;
                treinamento.dsInicio = treinamento.dtTreinamento.ToString();
                treinamento.dsInscInicio = treinamento.dtInscricaoInicio.ToString();
                treinamento.dsInscFim = treinamento.dtInscricaoFim.ToString();
                treinamento.dsLocal = treinamento.dsLocalTreinamento;

                return View("EditarInternacional", treinamento);
            }
        }

        [HttpPost]
        public ActionResult EditarHCP(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");

            //EDUCAÇÃO PROFISSIONAL
            var msg = ValidaTreinamentoHCP(treinamento);

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                //Erro na validação
                ViewBag.Alerta = msg;
                return View("EditarHCP", treinamento);
            }
            else
            {
                //Altera Treinamento
                treinamento.idIdioma = idIdioma;
                treinamento.dsPais = "Brasil";

                var retorno = _treinamento.AlteraTreinamento(treinamento);

                if (retorno != null && retorno.id != 0)
                {
                    //SUCESSO
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoOK") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
                else
                {
                    //ERRO AO SALVAR TREINAMENTO
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoErro") };

                    ViewBag.Alerta = msg;
                    return View("EditarHCP", treinamento);
                }
            }
        }

        [HttpPost]
        public ActionResult EditarCPC(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            ViewBag.Estado = new SelectList(_treinamento.ListaEstado(), "dsNome", "dsNome");

            //LIMPEZA, DESINFECÇÃO E ESTERILIZAÇÃO
            var msg = ValidaTreinamentoCPC(treinamento);

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                //Erro na validação
                ViewBag.Alerta = msg;
                return View("EditarCPC", treinamento);
            }
            else
            {
                //Altera Treinamento
                treinamento.idIdioma = idIdioma;
                treinamento.dsPais = "Brasil";

                var retorno = _treinamento.AlteraTreinamento(treinamento);

                if (retorno != null && retorno.id != 0)
                {
                    //SUCESSO
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoOK") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
                else
                {
                    //ERRO AO SALVAR TREINAMENTO
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoErro") };

                    ViewBag.Alerta = msg;
                    return View("EditarCPC", treinamento);
                }
            }
        }

        [HttpPost]
        public ActionResult EditarInternacional(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            ViewBag.Divisao = new SelectList(_treinamento.ListaDivisao(), "idDivisao", "dsNome");
            ViewBag.Categoria = new SelectList(_treinamento.ListaCategoria(), "idCategoria", "dsNome");
            ViewBag.Pais = new SelectList(_treinamento.ListaPais(idIdioma).Where(x => x.dsNome != "BRAZIL").Where(x => x.dsNome != "BRASIL").OrderBy(x => x.dsNome), "dsNome", "dsNome");

            //TREINAMENTO INTERNACIONAL
            var msg = ValidaTreinamentoInternacional(treinamento);

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                //Erro na validação
                ViewBag.Alerta = msg;
                return View("EditarInternacional", treinamento);
            }
            else
            {
                //Altera Treinamento
                var retorno = _treinamento.AlteraTreinamentoInternacional(treinamento);

                if (retorno != null && retorno.id != 0)
                {
                    //SUCESSO
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoOK") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
                else
                {
                    //ERRO AO SALVAR TREINAMENTO
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoErro") };

                    ViewBag.Alerta = msg;
                    return View("EditarInternacional", treinamento);
                }
            }
        }

        [HttpPost]
        public ActionResult AlterarStatus(Treinamento treinamentoAlteracao)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL
            }
            else
            {
                //OLA e OMS
            }

            var treinamento = _treinamento.ListaTreinamentoPorId(treinamentoAlteracao.idTreinamento);

            //VALIDA SE PODE OU NÃO ALTERAR O STATUS DO TREINAMENTO            

            //NÃO ATIVAR UM TREINAMENTO COM A DATA ANTERIOR A DATA ATUAL
            //if ((treinamento.idStatus == 1 || treinamento.idStatus == 3 || treinamento.idStatus == 4) && (treinamentoAlteracao.idStatus == 2))
            //{
                if (treinamento.dtTreinamento < System.DateTime.Now)
                {
                    //NÃO É POSSÍVEL ATIVAR TREINAMENTO
                    //var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoDataAnterior") };
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemEventoAnteriorDataAtual") };
                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
            //}


            //NÃO ATIVAR UM TREINAMENTO SEM PESQUISA CADASTRADA
            if ((treinamento.btBrasil == true && treinamento.btPesquisaObg == false) && (treinamentoAlteracao.idStatus == 2 || treinamentoAlteracao.idStatus == 4))
            {
                var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoCadastrarPesquisa") };
                TempData["Alerta"] = msg;
                return RedirectToAction("Index");
            }

            //TREINAMENTO Pendente, Inativo ou Em breve, não há problema em alterar entre eles
            if ((treinamento.idStatus == 1 || treinamento.idStatus == 3 || treinamento.idStatus == 4) && (treinamentoAlteracao.idStatus == 1 || treinamentoAlteracao.idStatus == 3))
            {
                var retorno = _treinamento.AlteraStatusTreinamento(treinamentoAlteracao);

                if (retorno != null && retorno.id != 0)
                {
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoOK") };
                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
                else
                {
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoErro") };
                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
            }

            //TREINAMENTO Pendente, Inativo ou Em breve para ATIVO, verifica se o treinamento já possui Palestrante e Agenda
            if ((treinamento.idStatus == 1 || treinamento.idStatus == 3 || treinamento.idStatus == 4) && (treinamentoAlteracao.idStatus == 2 || treinamentoAlteracao.idStatus == 4))
            {
                var listPalestrante = _treinamento.ListaPalestrante(treinamentoAlteracao);
                var listAgenda = _treinamento.ListaAgenda(treinamentoAlteracao);
                
                //TEM PALESTRANTE E AGENDA
                if (listPalestrante != null && listPalestrante.Count > 0 && listAgenda != null && listAgenda.Count > 0)
                {
                    var retorno = _treinamento.AlteraStatusTreinamento(treinamentoAlteracao);

                    if (retorno != null && retorno.id != 0)
                    {
                        var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoOK") };
                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoErro") };
                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //NÃO É POSSÍVEL ATIVAR UM TREINAMENTO SEM PALESTRANTE E/OU AGENDA
                    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoAgendaPalestrante") };
                    TempData["Alerta"] = msg;
                    return RedirectToAction("Index");
                }
            }

            //TREINAMENTO ATIVO para Pendente, Inativo ou Em breve, verifica se o treinamento já está aberto para inscrições
            if ((treinamento.idStatus == 2) && (treinamentoAlteracao.idStatus == 1 || treinamentoAlteracao.idStatus == 3 || treinamentoAlteracao.idStatus == 4))
            {
                //if (System.DateTime.Now < treinamento.dtInscricaoInicio)
                //{
                    var retorno = _treinamento.AlteraStatusTreinamento(treinamentoAlteracao);

                    if (retorno != null && retorno.id != 0)
                    {
                        var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloMensagem"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoOK") };
                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoErro") };
                        TempData["Alerta"] = msg;
                        return RedirectToAction("Index");
                    }
                //}
                //else
                //{
                //    //NÃO É POSSÍVEL INATIVAR TREINAMENTO. ABERTO PARA INSCRIÇÕES
                //    var msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoModalMensagemAlteracaoDataInscricao") };
                //    TempData["Alerta"] = msg;
                //    return RedirectToAction("Index");
                //}
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Detalhe

        public ActionResult Detalhe()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;
            var idTreinamento = (Session["idTreinamento"] != null) ? (int)Session["idTreinamento"] : 0;
                        

            if (Session["listPalestrantes"] != null || Session["listAgenda"] != null)
                ViewBag.Exclusao = true;

            Treinamento treinamento = new Treinamento();

            if (idIdioma == 1)
            {
                //OBL    
            }
            else
            {
                //OLA e OMS
                treinamento.idIdioma = idIdioma;
            }

            if (TempData["Alerta"] != null)
                ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];

            var idEditaDetalhe = 0;
            if(Session["idEditaDetalhe"] != null){
                TempData["Alerta"] = TempData["Alerta"];
                idEditaDetalhe = (int)Session["idEditaDetalhe"];
            }
            ViewBag.idEditaDetalhe = idEditaDetalhe;
            

            //Caso haja algum erro
            if (idTreinamento != 0)
            {
                Session.Remove("idTreinamento");

                var listTreinamento = _treinamento.ListaTreinamento(treinamento).
                    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).ToList();

                if (Session["listAgenda"] != null)
                    ViewBag.Agenda = (List<Treinamento>)Session["listAgenda"];

                ViewBag.Treinamento = new SelectList(listTreinamento.OrderBy(x => x.dtTreinamento), "idTreinamento", "dsNomeComData");
                ViewBag.Dias = new SelectList(_treinamento.ListaData(idTreinamento), "dtTreinamento", "dsDtTreinamento");

                treinamento = listTreinamento.FirstOrDefault(x => x.idTreinamento == idTreinamento);
                return View(treinamento);
            }

            //Se há Palestrante na Sessão, veio da inclusão de Palestrante (método AdicionaPalestrante)
            if (Session["listPalestrantes"] != null)
            {
                var listTreinamento = _treinamento.ListaTreinamento(treinamento).
                    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).ToList();
                var listPalestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];
                idTreinamento = listPalestrantes.FirstOrDefault().idTreinamento;

                ViewBag.Palestrantes = listPalestrantes;

                if (Session["listAgenda"] != null)
                    ViewBag.Agenda = (List<Treinamento>)Session["listAgenda"];

                ViewBag.Treinamento = new SelectList(listTreinamento.OrderBy(x => x.dtTreinamento), "idTreinamento", "dsNomeComData");
                ViewBag.Dias = new SelectList(_treinamento.ListaData(idTreinamento), "dtTreinamento", "dsDtTreinamento");

                treinamento = listTreinamento.FirstOrDefault(x => x.idTreinamento == idTreinamento);
                return View(treinamento);
            }
            else if (Session["listAgenda"] != null)
            {
                var listTreinamento = _treinamento.ListaTreinamento(treinamento).
                    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).ToList();
                var listAgenda = (List<Treinamento>)Session["listAgenda"];
                idTreinamento = listAgenda.FirstOrDefault().idTreinamento;

                ViewBag.Agenda = listAgenda;

                ViewBag.Treinamento = new SelectList(listTreinamento.OrderBy(x => x.dtTreinamento), "idTreinamento", "dsNomeComData");
                ViewBag.Dias = new SelectList(_treinamento.ListaData(idTreinamento), "dtTreinamento", "dsDtTreinamento");

                treinamento = listTreinamento.FirstOrDefault(x => x.idTreinamento == idTreinamento);
                return View(treinamento);
            }
            else
            {
                ViewBag.Treinamento = new SelectList(_treinamento.ListaTreinamento(treinamento).
                    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).
                    OrderBy(x => x.dtTreinamento), "idTreinamento", "dsNomeComData");
                
                return View();
            }           
        }

        [HttpPost]
        public ActionResult Detalhe(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            Treinamento treinamentoModel = new Treinamento();

            if (TempData["Alerta"] != null)
               ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];
                

            Session.Add("idEditaDetalhe", null);
            ViewBag.idEditaDetalhe = "";

            if (idIdioma == 1)
            {
                //OBL    
            }
            else
            {
                //OLA e OMS
                treinamentoModel.idIdioma = idIdioma;
            }

            var listTreinamento = _treinamento.ListaTreinamento(treinamentoModel).
                    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).ToList();
            var listPalestrantes = _treinamento.ListaPalestrante(treinamento);
            var listAgenda = _treinamento.ListaAgenda(treinamento);

            //Verifica se há palestrantes no BD
            if (listPalestrantes != null && listPalestrantes.Count > 0)
            {
                List<TreinamentoViewModel> listTreinamentoViewModel = new List<TreinamentoViewModel>();

                foreach (var item in listPalestrantes)
	            {
                    TreinamentoViewModel treinamentoViewModel = new TreinamentoViewModel();
                    treinamentoViewModel.Treinamento = item;

                    listTreinamentoViewModel.Add(treinamentoViewModel);		 
	            }

                ViewBag.Palestrantes = listTreinamentoViewModel;
                ViewBag.btAddPalestrante = false;
            }

            //Verifica se há agenda no BD
            if (listAgenda != null && listAgenda.Count > 0)
            {
                ViewBag.Agenda = listAgenda;
                ViewBag.btAddAgenda = false;
            }

            //Exclui sessões de Palestrantes e Agenda
            Session.Remove("listPalestrantes");
            Session.Remove("listAgenda");
            
            ViewBag.Treinamento = new SelectList(listTreinamento.OrderBy(x => x.dtTreinamento).
                    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).ToList(), "idTreinamento", "dsNomeComData");
            ViewBag.Dias = new SelectList(_treinamento.ListaData(treinamento.idTreinamento), "dtTreinamento", "dsDtTreinamento");

            treinamento = listTreinamento.FirstOrDefault(x => x.idTreinamento == treinamento.idTreinamento);

            ViewBag.Exclusao = false;

            return View(treinamento);
        }

        [HttpPost]
        public ActionResult AdicionaPalestrante(Treinamento treinamento, HttpPostedFileBase dsFoto)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            List<TreinamentoViewModel> listPalestrantes = new List<TreinamentoViewModel>();
            TreinamentoViewModel treinamentoViewModel = new TreinamentoViewModel();

            if (Session["listPalestrantes"] != null)
                listPalestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];

            var msg = ValidaPalestrante(treinamento, dsFoto);

            if (string.IsNullOrEmpty(msg.Mensagem))
            {
                treinamentoViewModel.idTreinamento = treinamento.idTreinamento;
                
                treinamentoViewModel.Treinamento = new Treinamento
                    {
                        idTreinamento = treinamento.idTreinamento,
                        dsPalestrante = treinamento.dsPalestrante,
                        dsPerfil = treinamento.dsPerfil
                    };

                //Salva foto (Não é possível armazenar foto na Sessão)
                treinamento = SalvaFotoPalestrante(treinamento, dsFoto);

                if (treinamento.btErro)
                {
                    //Erro ao salvar arquivo
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErroArquivo") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("Detalhe");
                }
                else
                {
                    //Pega o nome da foto
                    treinamentoViewModel.Treinamento.dsImagem = treinamento.dsImagem;
                }
            }
            else
            {
                treinamentoViewModel.idTreinamento = treinamento.idTreinamento;

                TempData["Alerta"] = msg;
            }

            listPalestrantes.Add(treinamentoViewModel);
            Session.Add("listPalestrantes", listPalestrantes);

            return RedirectToAction("Detalhe");
        }

        public ActionResult ExcluiPalestrante(string dsPalestrante)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            List<TreinamentoViewModel> listPalestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];

            var palestrante = listPalestrantes.FirstOrDefault(x => x.Treinamento != null && x.Treinamento.dsPalestrante == dsPalestrante);
            var idTreinamento = palestrante.idTreinamento;
            listPalestrantes.Remove(palestrante);

            listPalestrantes.Add(new TreinamentoViewModel());
            listPalestrantes.FirstOrDefault().idTreinamento = idTreinamento;
            Session["listPalestrantes"] = listPalestrantes;

            return RedirectToAction("Detalhe");
        }

        //public ActionResult AdicionaAgenda(int idTreinamento, string dtTreinamento, string dsHorario, string dsAtividade)
        [HttpPost]
        public ActionResult AdicionaAgenda(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            
            
            List<Treinamento> listAgenda = new List<Treinamento>();

            if (Session["listAgenda"] != null)
                listAgenda = (List<Treinamento>)Session["listAgenda"];


            if (string.IsNullOrEmpty(treinamento.dsHorario) || string.IsNullOrEmpty(treinamento.dsAtividade))
            {
                Session.Add("listAgenda", listAgenda);
                TempData["Alerta"] = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("ConteudoModalMensagem") };
                return RedirectToAction("Detalhe");
            }


            if (treinamento.dsHorario != null && treinamento.dsHorario != "") { 
                if (treinamento.dsHorario.Replace(":", "").Length == 3)
                    treinamento.dsHorario = treinamento.dsHorario + "0";
                else if (treinamento.dsHorario.Replace(":", "").Length == 2)
                    treinamento.dsHorario = treinamento.dsHorario.Replace(":", "") + ":00";
                else if (treinamento.dsHorario.Replace(":", "").Length == 1)
                    treinamento.dsHorario = "0" + treinamento.dsHorario.Replace(":", "") + ":00";
            }

            var IdentificadorAgenda = DateTime.Now.ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace(":", "");
            listAgenda.Add(new Treinamento { idTreinamento = treinamento.idTreinamento, dtTreinamento = Convert.ToDateTime(treinamento.dsDtTreinamento), dsHorario = treinamento.dsHorario, dsAtividade = treinamento.dsAtividade, dsIdAgenda = IdentificadorAgenda });
            Session.Add("listAgenda", listAgenda);

            //ViewBag.Dias = new SelectList(_treinamento.ListaTreinamento(new Treinamento()).Where(x => x.idTreinamento == treinamento..idTreinamento), "dtTreinamento", "dtTreinamento");
            //ViewBag.Agenda = listAgenda;

            return RedirectToAction("Detalhe");
            //return PartialView("_Agenda");
        }

        public ActionResult ExcluiAgenda(string dsIdAgenda, int idTreinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            List<Treinamento> listAgenda = (List<Treinamento>)Session["listAgenda"];

            if (listAgenda != null)
            {
                //AGENDA NÃO ESTÁ FINALIZADA

                var agenda = listAgenda.FirstOrDefault(x => x.dsIdAgenda == dsIdAgenda);
                listAgenda.Remove(agenda);

                if (listAgenda.Count > 0)
                    Session["listAgenda"] = listAgenda;
                else
                    Session["idTreinamento"] = idTreinamento;
            }

            return RedirectToAction("Detalhe");

            //else
            //{
                //AGENDA ESTÁ FINALIZADA (EXCLUI DO BANCO)

                //var listTreinamento = _treinamento.ListaTreinamento(new Treinamento()).
                //    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).ToList();
                //var Agenda = _treinamento.ListaAgenda(new Treinamento { idTreinamento = idTreinamento });
                //int idAgenda = Agenda.FirstOrDefault(x => x.dsAtividade == dsAtividade).idAgenda;

                //var retorno = _treinamento.ExcluiAgenda(new Treinamento { idTreinamento = idTreinamento, idAgenda = idAgenda });


                //var listPalestrantes = _treinamento.ListaPalestrante(new Treinamento { idTreinamento = idTreinamento });
                //Agenda = _treinamento.ListaAgenda(new Treinamento { idTreinamento = idTreinamento });

                ////Verifica se há palestrantes no BD
                //if (listPalestrantes != null && listPalestrantes.Count > 0)
                //{
                //    List<TreinamentoViewModel> listTreinamentoViewModel = new List<TreinamentoViewModel>();

                //    foreach (var item in listPalestrantes)
                //    {
                //        TreinamentoViewModel treinamentoViewModel = new TreinamentoViewModel();
                //        treinamentoViewModel.Treinamento = item;

                //        listTreinamentoViewModel.Add(treinamentoViewModel);
                //    }

                //    ViewBag.Palestrantes = listTreinamentoViewModel;
                //    ViewBag.btAddPalestrante = false;
                //}

                ////Verifica se há agenda no BD
                //if (Agenda != null && Agenda.Count > 0)
                //{
                //    ViewBag.Agenda = Agenda;
                //    ViewBag.btAddAgenda = false;
                //}

                //ViewBag.Treinamento = new SelectList(listTreinamento.OrderBy(x => x.dtTreinamento).
                //    Where(x => x.dtTreinamento.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0) > System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0)).ToList(), "idTreinamento", "dsNomeComData");
                //ViewBag.Dias = new SelectList(listTreinamento.Where(x => x.idTreinamento == idTreinamento), "dtTreinamento", "dtTreinamento");

                //var treinamento = listTreinamento.FirstOrDefault(x => x.idTreinamento == idTreinamento);

                //return View("Detalhe", treinamento);
            //}
        }

        [HttpPost]
        public ActionResult Finaliza(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            //Valida Palestrante e Agenda
            var msg = ValidaFinalizacao();

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                TempData["Alerta"] = msg;
                Session.Add("idTreinamento", treinamento.idTreinamento);

                return RedirectToAction("Detalhe");
            }
            else
            {
                var listPalestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];
                var listAgenda = (List<Treinamento>)Session["listAgenda"];

                List<Treinamento> palestrante = new List<Treinamento>();

                foreach (var item in listPalestrantes.Where(x => x.Treinamento != null))
                {
                    palestrante.Add(item.Treinamento);
                }

                //SALVA PALESTRANTE
                var retornoPalestrante = _treinamento.CadastraPalestrante(palestrante, "Cadastro");

                if (retornoPalestrante == null || retornoPalestrante.id == 0)
                {
                    //Erro ao salvar palestrante
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoDetalheModalMensagemErro") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("Detalhe");
                }

                //SALVA AGENDA
                var retornoAgenda = _treinamento.CadastraAgenda(listAgenda, "Cadastro");

                if (retornoAgenda == null || retornoAgenda.id == 0)
                {
                    //Erro ao salvar agenda
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoDetalheModalMensagemErro") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("Detalhe");
                }
                else
                {
                    if ((treinamento.idStatus == 3 || treinamento.idStatus == 1) && (treinamento.btPesquisaObg || treinamento.btBrasil != true))
                    {
                        if (treinamento.dtInscricaoInicio < DateTime.Now)
                            treinamento.idStatus = 2;
                        else
                            treinamento.idStatus = 4;

                        _treinamento.AlteraStatusTreinamento(treinamento);
                    }


                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoDetalheModalMensagemOK") };
                    TempData["Alerta"] = msg;

                    //Exclui sessões de Palestrantes e Agenda
                    Session.Remove("listPalestrantes");
                    Session.Remove("listAgenda");
                    
                    return RedirectToAction("Detalhe");
                }
            }
        }



        public ActionResult DetalheEditar(int id)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            Treinamento treinamento = new Treinamento { idTreinamento = id };

            if (TempData["Alerta"] != null)
                ViewBag.Alerta = (ModalMensagem)TempData["Alerta"];

            
            //PRIMEIRA VEZ QUE ENTRA NA PÁGINA = PEGA OS PALESTRANTES E AGENDA DO BD E JOGA NA SESSÃO
            if (Session["listPalestrantes"] == null || Session["listAgenda"] == null)
            {
                var listPalestrantes = _treinamento.ListaPalestrante(treinamento);
                var listAgenda = _treinamento.ListaAgenda(treinamento);

                foreach (var item in listAgenda)
                {
                    item.dsIdAgenda = DateTime.Now.ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace(":", "") + item.idAgenda.ToString();
                }


                //CASO NÃO TENHA AGENDA E OU PALESTRANTE
                if (listPalestrantes == null || listPalestrantes.Count == 0 || listAgenda == null || listAgenda.Count == 0)
                {
                    TempData["Alerta"] = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoDetalheEditarErroSemPalestranteAgenda") };
                    Session.Add("idEditaDetalhe", id);
                    return RedirectToAction("../Treinamento/Detalhe/", false);
                }

                //Verifica se há palestrantes no BD
                if (listPalestrantes != null && listPalestrantes.Count > 0)
                {
                    List<TreinamentoViewModel> listTreinamentoViewModel = new List<TreinamentoViewModel>();

                    foreach (var item in listPalestrantes)
                    {
                        TreinamentoViewModel treinamentoViewModel = new TreinamentoViewModel();
                        treinamentoViewModel.idTreinamento = id;
                        treinamentoViewModel.Treinamento = item;

                        listTreinamentoViewModel.Add(treinamentoViewModel);
                    }

                    ViewBag.Palestrantes = listTreinamentoViewModel;
                    Session["listPalestrantes"] = listTreinamentoViewModel;
                }

                //Verifica se há agenda no BD
                if (listAgenda != null && listAgenda.Count > 0)
                {
                    ViewBag.Agenda = listAgenda;
                    ViewBag.btAddAgenda = false;
                }

                Session["listAgenda"] = listAgenda;
            }
            else
            {
                ViewBag.Palestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];
                ViewBag.Agenda = (List<Treinamento>)Session["listAgenda"];
            }

            ViewBag.Dias = new SelectList(_treinamento.ListaData(treinamento.idTreinamento), "dtTreinamento", "dsDtTreinamento");
            
            return View(_treinamento.ListaTreinamentoPorId(treinamento.idTreinamento));
        }

        [HttpPost]
        public ActionResult AdicionaPalestranteEditar(Treinamento treinamento, HttpPostedFileBase dsFoto)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            List<TreinamentoViewModel> listPalestrantes = new List<TreinamentoViewModel>();
            TreinamentoViewModel treinamentoViewModel = new TreinamentoViewModel();

            if (Session["listPalestrantes"] != null)
                listPalestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];

            var msg = ValidaPalestrante(treinamento, dsFoto);

            if (string.IsNullOrEmpty(msg.Mensagem))
            {
                treinamentoViewModel.idTreinamento = treinamento.idTreinamento;

                treinamentoViewModel.Treinamento = new Treinamento
                {
                    idTreinamento = treinamento.idTreinamento,
                    dsPalestrante = treinamento.dsPalestrante,
                    dsPerfil = treinamento.dsPerfil
                };

                //Salva foto (Não é possível armazenar foto na Sessão)
                treinamento = SalvaFotoPalestrante(treinamento, dsFoto);

                if (treinamento.btErro)
                {
                    //Erro ao salvar arquivo
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoCadastrarModalMensagemErroArquivo") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("DetalheEditar", new { id = treinamento.idTreinamento });
                }
                else
                {
                    //Pega o nome da foto
                    treinamentoViewModel.Treinamento.dsImagem = treinamento.dsImagem;
                }
            }
            else
            {
                treinamentoViewModel.idTreinamento = treinamento.idTreinamento;

                TempData["Alerta"] = msg;
            }

            listPalestrantes.Add(treinamentoViewModel);
            Session.Add("listPalestrantes", listPalestrantes);

            return RedirectToAction("DetalheEditar", new { id = treinamento.idTreinamento });
        }

        public ActionResult ExcluiPalestranteEditar(string dsPalestrante)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            List<TreinamentoViewModel> listPalestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];

            var palestrante = listPalestrantes.FirstOrDefault(x => x.Treinamento != null && x.Treinamento.dsPalestrante == dsPalestrante);
            var idTreinamento = palestrante.idTreinamento;
            listPalestrantes.Remove(palestrante);

            listPalestrantes.Add(new TreinamentoViewModel());
            listPalestrantes.FirstOrDefault().idTreinamento = idTreinamento;
            Session["listPalestrantes"] = listPalestrantes;

            return RedirectToAction("DetalheEditar", new { id = idTreinamento });
        }

        [HttpPost]
        public ActionResult AdicionaAgendaEditar(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            if (string.IsNullOrEmpty(treinamento.dsHorario) || string.IsNullOrEmpty(treinamento.dsAtividade))
            {
                TempData["Alerta"] = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("ConteudoModalMensagem") };
                return RedirectToAction("DetalheEditar", new { id = treinamento.idTreinamento });
            }

            List<Treinamento> listAgenda = new List<Treinamento>();

            if (Session["listAgenda"] != null)
                listAgenda = (List<Treinamento>)Session["listAgenda"];


            if (treinamento.dsHorario != null && treinamento.dsHorario != "")
            {
                if (treinamento.dsHorario.Replace(":", "").Length == 3)
                    treinamento.dsHorario = treinamento.dsHorario + "0";
                else if (treinamento.dsHorario.Replace(":", "").Length == 2)
                    treinamento.dsHorario = treinamento.dsHorario.Replace(":", "") + ":00";
                else if (treinamento.dsHorario.Replace(":", "").Length == 1)
                    treinamento.dsHorario = "0" + treinamento.dsHorario.Replace(":", "") + ":00";
            }

            var IdentificadorAgenda = DateTime.Now.ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace(":", "");

            listAgenda.Add(new Treinamento { idTreinamento = treinamento.idTreinamento, dtTreinamento = Convert.ToDateTime(treinamento.dsDtTreinamento), dsHorario = treinamento.dsHorario, dsAtividade = treinamento.dsAtividade, dsIdAgenda = IdentificadorAgenda });
            Session.Add("listAgenda", listAgenda);

            return RedirectToAction("DetalheEditar", new { id = treinamento.idTreinamento });
        }

        public ActionResult ExcluiAgendaEditar(string dsIdAgenda, int idTreinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            List<Treinamento> listAgenda = (List<Treinamento>)Session["listAgenda"];

            if (listAgenda != null)
            {
                //AGENDA NÃO ESTÁ FINALIZADA

                var agenda = listAgenda.FirstOrDefault(x => x.dsIdAgenda == dsIdAgenda);
                listAgenda.Remove(agenda);

                if (listAgenda.Count > 0)
                    Session["listAgenda"] = listAgenda;
                else
                    Session["idTreinamento"] = idTreinamento;
            }

            return RedirectToAction("DetalheEditar", new { id = idTreinamento });
        }

        [HttpPost]
        public ActionResult FinalizaEditar(Treinamento treinamento)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];

            //Valida Palestrante e Agenda
            var msg = ValidaFinalizacao();

            if (!string.IsNullOrEmpty(msg.Mensagem))
            {
                TempData["Alerta"] = msg;

                return RedirectToAction("DetalheEditar", new { id = treinamento.idTreinamento });
            }
            else
            {
                var listPalestrantes = (List<TreinamentoViewModel>)Session["listPalestrantes"];
                var listAgenda = (List<Treinamento>)Session["listAgenda"];

                List<Treinamento> palestrante = new List<Treinamento>();

                foreach (var item in listPalestrantes.Where(x => x.Treinamento != null))
                {
                    palestrante.Add(item.Treinamento);
                }

                //SALVA PALESTRANTE
                var retornoPalestrante = _treinamento.CadastraPalestrante(palestrante, "Editar");

                if (retornoPalestrante == null || retornoPalestrante.id == 0)
                {
                    //Erro ao salvar palestrante
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoDetalheModalMensagemErro") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("DetalheEditar", new { id = treinamento.idTreinamento });
                }


                //SALVA AGENDA
                var retornoAgenda = _treinamento.CadastraAgenda(listAgenda, "Editar");

                if (retornoAgenda == null || retornoAgenda.id == 0)
                {
                    //Erro ao salvar agenda
                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoDetalheModalMensagemErro") };

                    TempData["Alerta"] = msg;
                    return RedirectToAction("DetalheEditar", new { id = treinamento.idTreinamento });
                }
                else
                {
                    if (treinamento.idStatus == 3 && (treinamento.btPesquisaObg ||  treinamento.btBrasil != true))
                    {
                        if(treinamento.dtInscricaoInicio < DateTime.Now)
                            treinamento.idStatus = 2;
                        else
                            treinamento.idStatus = 4;

                        _treinamento.AlteraStatusTreinamento(treinamento);
                    }

                    msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao"), Mensagem = Geral.RetornaTexto("TreinamentoDetalheModalMensagemOK") };
                    TempData["Alerta"] = msg;

                    //Exclui sessões de Palestrantes e Agenda
                    Session.Remove("listPalestrantes");
                    Session.Remove("listAgenda");

                    return RedirectToAction("Index");
                }
            }
        }

        #endregion

        #region Validação

        public SelectList ddlSite()
        {
            var listSite = new List<Banner>();
            listSite.Add(new Banner { idIdioma = 1, dsSite = "OBL" });
            listSite.Add(new Banner { idIdioma = 3, dsSite = "OMS" });
            listSite.Add(new Banner { idIdioma = 2, dsSite = "OLA" });

            return new SelectList(listSite, "idIdioma", "dsSite", 1);
        }

        public ModalMensagem ValidaTreinamentoHCP(Treinamento treinamento, HttpPostedFileBase dsImagem, HttpPostedFileBase dsMaterial)
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //CAMPOS EM BRANCO
            if (dsImagem == null || string.IsNullOrEmpty(treinamento.dsNome) || string.IsNullOrEmpty(treinamento.dsCodigoTreinamento)
                || treinamento.inQuantidadeVagas <= 0 || string.IsNullOrEmpty(treinamento.dsInicio) || string.IsNullOrEmpty(treinamento.dsTermino) || string.IsNullOrEmpty(treinamento.dsHoraInicio)
                || string.IsNullOrEmpty(treinamento.dsHoraTermino) || string.IsNullOrEmpty(treinamento.dsInscInicio) || string.IsNullOrEmpty(treinamento.dsInscFim)
                || string.IsNullOrEmpty(treinamento.dsDescricao) || string.IsNullOrEmpty(treinamento.dsLocal) || string.IsNullOrEmpty(treinamento.dsEndereco) || string.IsNullOrEmpty(treinamento.dsNumero)
                || string.IsNullOrEmpty(treinamento.dsBairro) || string.IsNullOrEmpty(treinamento.dsCEP) || string.IsNullOrEmpty(treinamento.dsCidade) || string.IsNullOrEmpty(treinamento.dsLatitude)
                || string.IsNullOrEmpty(treinamento.dsLongitude))
            {
                msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatorios") + " <br /> ";
                //if (dsImagem == null || dsPesquisa == null)
                if (dsImagem == null)
                    msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatoriosDocImg") + "<br />";
            }

            //FORMATOS DE ARQUIVOS
            //if (dsImagem != null && dsPesquisa != null)
            if (dsImagem != null)
            {
                if ((dsImagem.ContentType != "image/pjpeg" && dsImagem.ContentType != "image/jpeg" && dsImagem.ContentType != "image/png"))
                    //|| (dsPesquisa.ContentType != "application/msword" && dsPesquisa.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && dsPesquisa.ContentType != "application/pdf"))
                {
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemArquivoIncorreto") + " <br /> ";
                }
            }
            if (dsMaterial != null)
            {
                if (dsMaterial.ContentType != "application/msword" && dsMaterial.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && dsMaterial.ContentType != "application/pdf")
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemArquivoIncorreto") + " <br /> ";
            }

            //TAMANHO DA IMAGEM
            if (dsImagem != null)
            {
                var img = System.Drawing.Image.FromStream(dsImagem.InputStream, true, true);
                int width = img.Width;
                int height = img.Height;

                if (width < 446 || height < 238)
                {
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemTamanhoImagem") + " <br /> ";
                }
            }

            //DATAS
            if (!string.IsNullOrEmpty(treinamento.dsInicio) && !string.IsNullOrEmpty(treinamento.dsTermino) &&
                !string.IsNullOrEmpty(treinamento.dsInscInicio) && !string.IsNullOrEmpty(treinamento.dsInscFim))
            {
                if (Data.ValidaData(treinamento.dsInicio) && Data.ValidaData(treinamento.dsTermino) &&
                    Data.ValidaData(treinamento.dsInscInicio) && Data.ValidaData(treinamento.dsInscFim))
                {
                    var dtInicio = Convert.ToDateTime(treinamento.dsInicio);
                    var dtTermino = Convert.ToDateTime(treinamento.dsTermino);
                    var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
                    var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

                    if (dtInicio <= System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0))
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamentoAtual") + " <br /> ";

                    if (dtTermino < dtInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamento") + " <br /> ";

                    if (dtInscFim < dtInscInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataInscricao") + " <br /> ";

                    if (dtInscFim > dtTermino)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataFimInscricaoMaior") + " <br /> ";
                }
                else
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemData") + " <br /> ";
            }

            //HORA
            if (!string.IsNullOrEmpty(treinamento.dsHoraInicio) && !string.IsNullOrEmpty(treinamento.dsHoraTermino))
            {
                var horaInicial = Convert.ToInt32(treinamento.dsHoraInicio.Replace(":", ""));
                var horaFinal = Convert.ToInt32(treinamento.dsHoraTermino.Replace(":", ""));

                if (horaFinal < horaInicial)
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemHora") + " <br /> ";
            }

            return msg;
        }
        public Treinamento SalvaArquivosHCP(Treinamento treinamento, HttpPostedFileBase dsImagem, HttpPostedFileBase dsMaterial)
        {
            try
            {
                treinamento.btErro = false;

                //IMAGEM
                string nomeArquivo = Path.GetFileNameWithoutExtension(dsImagem.FileName);
                string extensao = Path.GetExtension(dsImagem.FileName);
                string dataHora = System.DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "").Replace("\\", "").Replace(".", "");
                string nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                //Salva arquivo
                var path = Path.Combine(Server.MapPath("~/assets/img/treinamento"), nomeCompleto);
                dsImagem.SaveAs(path);

                treinamento.dsImagem = nomeCompleto;

                //MATERIAL
                if (dsMaterial != null)
                {
                    nomeArquivo = Path.GetFileNameWithoutExtension(dsMaterial.FileName);
                    extensao = Path.GetExtension(dsMaterial.FileName);
                    nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                    //Salva arquivo
                    path = Path.Combine(Server.MapPath("~/assets/file/material"), nomeCompleto);
                    dsMaterial.SaveAs(path);

                    treinamento.dsMaterial = nomeCompleto;
                }

                //PESQUISA
                //nomeArquivo = Path.GetFileNameWithoutExtension(dsPesquisa.FileName);
                //extensao = Path.GetExtension(dsPesquisa.FileName);
                //nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                ////Salva arquivo
                //path = Path.Combine(Server.MapPath("~/assets/file/pesquisa"), nomeCompleto);
                //dsPesquisa.SaveAs(path);

                //treinamento.dsPesquisa = nomeCompleto;
            }
            catch
            {
                treinamento.btErro = true;
            }

            return treinamento;
        }

        public ModalMensagem ValidaTreinamentoCPC(Treinamento treinamento, HttpPostedFileBase dsImagem, HttpPostedFileBase dsMaterial)
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //CAMPOS EM BRANCO
            if (dsImagem == null || string.IsNullOrEmpty(treinamento.dsNome) || string.IsNullOrEmpty(treinamento.dsCodigoTreinamento)
                || treinamento.inQuantidadeVagas <= 0 || treinamento.inQuantidadeVagasPorCNPJ <= 0 || string.IsNullOrEmpty(treinamento.dsInicio) || string.IsNullOrEmpty(treinamento.dsTermino) || string.IsNullOrEmpty(treinamento.dsHoraInicio)
                || string.IsNullOrEmpty(treinamento.dsHoraTermino) || string.IsNullOrEmpty(treinamento.dsInscInicio) || string.IsNullOrEmpty(treinamento.dsInscFim)
                || string.IsNullOrEmpty(treinamento.dsDescricao) || string.IsNullOrEmpty(treinamento.dsLocal) || string.IsNullOrEmpty(treinamento.dsEndereco) || string.IsNullOrEmpty(treinamento.dsNumero)
                || string.IsNullOrEmpty(treinamento.dsBairro) || string.IsNullOrEmpty(treinamento.dsCEP) || string.IsNullOrEmpty(treinamento.dsCidade) || string.IsNullOrEmpty(treinamento.dsLatitude)
                || string.IsNullOrEmpty(treinamento.dsLongitude))
            {
                msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatorios") + " <br /> ";
            }

            //FORMATOS DE ARQUIVOS
            if (dsImagem != null)
            {
                if (dsImagem.ContentType != "image/pjpeg" && dsImagem.ContentType != "image/jpeg" && dsImagem.ContentType != "image/png")
                {
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemArquivoIncorreto") + " <br /> ";
                }
            }
            if (dsMaterial != null)
            {
                if (dsMaterial.ContentType != "application/msword" && dsMaterial.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && dsMaterial.ContentType != "application/pdf")
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemArquivoIncorreto") + " <br /> ";
            }


            //TAMANHO DA IMAGEM
            if (dsImagem != null)
            {
                var img = System.Drawing.Image.FromStream(dsImagem.InputStream, true, true);
                int width = img.Width;
                int height = img.Height;

                if (width < 446 || height < 238)
                {
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemTamanhoImagem") + " <br /> ";
                }
            }

            //DATAS
            if (!string.IsNullOrEmpty(treinamento.dsInicio) && !string.IsNullOrEmpty(treinamento.dsTermino) &&
                !string.IsNullOrEmpty(treinamento.dsInscInicio) && !string.IsNullOrEmpty(treinamento.dsInscFim))
            {
                if (Data.ValidaData(treinamento.dsInicio) && Data.ValidaData(treinamento.dsTermino) &&
                    Data.ValidaData(treinamento.dsInscInicio) && Data.ValidaData(treinamento.dsInscFim))
                {
                    var dtInicio = Convert.ToDateTime(treinamento.dsInicio);
                    var dtTermino = Convert.ToDateTime(treinamento.dsTermino);
                    var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
                    var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

                    if (dtInicio <= System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0))
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamentoAtual") + " <br /> ";

                    if (dtTermino < dtInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamento") + " <br /> ";

                    if (dtInscFim < dtInscInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataInscricao") + " <br /> ";

                    if (dtInscFim > dtTermino)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataFimInscricaoMaior") + " <br /> ";
                }
                else
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemData") + " <br /> ";
            }

            //HORA
            if (!string.IsNullOrEmpty(treinamento.dsHoraInicio) && !string.IsNullOrEmpty(treinamento.dsHoraTermino))
            {
                var horaInicial = Convert.ToInt32(treinamento.dsHoraInicio.Replace(":", ""));
                var horaFinal = Convert.ToInt32(treinamento.dsHoraTermino.Replace(":", ""));

                if (horaFinal < horaInicial)
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemHora") + " <br /> ";
            }

            return msg;
        }
        public Treinamento SalvaArquivosCPC(Treinamento treinamento, HttpPostedFileBase dsImagem, HttpPostedFileBase dsMaterial)
        {
            try
            {
                treinamento.btErro = false;

                //IMAGEM
                string nomeArquivo = Path.GetFileNameWithoutExtension(dsImagem.FileName);
                string extensao = Path.GetExtension(dsImagem.FileName);
                string dataHora = System.DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "").Replace("\\", "").Replace(".", "");
                string nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                //Salva arquivo
                var path = Path.Combine(Server.MapPath("~/assets/img/treinamento"), nomeCompleto);
                dsImagem.SaveAs(path);

                treinamento.dsImagem = nomeCompleto;

                //MATERIAL
                if (dsMaterial != null)
                {
                    nomeArquivo = Path.GetFileNameWithoutExtension(dsMaterial.FileName);
                    extensao = Path.GetExtension(dsMaterial.FileName);
                    nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                    //Salva arquivo
                    path = Path.Combine(Server.MapPath("~/assets/file/material"), nomeCompleto);
                    dsMaterial.SaveAs(path);

                    treinamento.dsMaterial = nomeCompleto;
                }
            }
            catch
            {
                treinamento.btErro = true;
            }

            return treinamento;
        }

        public ModalMensagem ValidaTreinamentoHCP(Treinamento treinamento)
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //CAMPOS EM BRANCO
            if (string.IsNullOrEmpty(treinamento.dsNome) || string.IsNullOrEmpty(treinamento.dsCodigoTreinamento)
                || treinamento.inQuantidadeVagas <= 0 || string.IsNullOrEmpty(treinamento.dsInicio) || string.IsNullOrEmpty(treinamento.dsTermino) || string.IsNullOrEmpty(treinamento.dsHoraInicio)
                || string.IsNullOrEmpty(treinamento.dsHoraTermino) || string.IsNullOrEmpty(treinamento.dsInscInicio) || string.IsNullOrEmpty(treinamento.dsInscFim)
                || string.IsNullOrEmpty(treinamento.dsDescricao) || string.IsNullOrEmpty(treinamento.dsLocal) || string.IsNullOrEmpty(treinamento.dsEndereco) || string.IsNullOrEmpty(treinamento.dsNumero)
                || string.IsNullOrEmpty(treinamento.dsBairro) || string.IsNullOrEmpty(treinamento.dsCEP) || string.IsNullOrEmpty(treinamento.dsCidade) || string.IsNullOrEmpty(treinamento.dsLatitude)
                || string.IsNullOrEmpty(treinamento.dsLongitude))
            {
                msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatorios") + " <br /> ";
            }

            //DATAS
            if (!string.IsNullOrEmpty(treinamento.dsInicio) && !string.IsNullOrEmpty(treinamento.dsTermino) &&
                !string.IsNullOrEmpty(treinamento.dsInscInicio) && !string.IsNullOrEmpty(treinamento.dsInscFim))
            {
                if (Data.ValidaData(treinamento.dsInicio) && Data.ValidaData(treinamento.dsTermino) &&
                    Data.ValidaData(treinamento.dsInscInicio) && Data.ValidaData(treinamento.dsInscFim))
                {
                    var dtInicio = Convert.ToDateTime(treinamento.dsInicio);
                    var dtTermino = Convert.ToDateTime(treinamento.dsTermino);
                    var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
                    var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

                    if (dtInicio <= System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0))
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamentoAtual") + " <br /> ";

                    if (dtTermino < dtInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamento") + " <br /> ";

                    if (dtInscFim < dtInscInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataInscricao") + " <br /> ";

                    if (dtInscFim > dtTermino)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataFimInscricaoMaior") + " <br /> ";
                    
                }
                else
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemData") + " <br /> ";
            }

            //HORA
            if (!string.IsNullOrEmpty(treinamento.dsHoraInicio) && !string.IsNullOrEmpty(treinamento.dsHoraTermino))
            {
                var horaInicial = Convert.ToInt32(treinamento.dsHoraInicio.Replace(":", ""));
                var horaFinal = Convert.ToInt32(treinamento.dsHoraTermino.Replace(":", ""));

                if (horaFinal < horaInicial)
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemHora") + " <br /> ";
            }

            return msg;
        }
        public ModalMensagem ValidaTreinamentoCPC(Treinamento treinamento)
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //CAMPOS EM BRANCO
            if (string.IsNullOrEmpty(treinamento.dsNome) || string.IsNullOrEmpty(treinamento.dsCodigoTreinamento) || treinamento.inQuantidadeVagas <= 0 || treinamento.inQuantidadeVagasPorCNPJ <= 0 
                || string.IsNullOrEmpty(treinamento.dsInicio) || string.IsNullOrEmpty(treinamento.dsTermino) || string.IsNullOrEmpty(treinamento.dsHoraInicio) || string.IsNullOrEmpty(treinamento.dsHoraTermino) 
                || string.IsNullOrEmpty(treinamento.dsInscInicio) || string.IsNullOrEmpty(treinamento.dsInscFim)
                || string.IsNullOrEmpty(treinamento.dsDescricao) || string.IsNullOrEmpty(treinamento.dsLocal) || string.IsNullOrEmpty(treinamento.dsEndereco) || string.IsNullOrEmpty(treinamento.dsNumero)
                || string.IsNullOrEmpty(treinamento.dsBairro) || string.IsNullOrEmpty(treinamento.dsCEP) || string.IsNullOrEmpty(treinamento.dsCidade) || string.IsNullOrEmpty(treinamento.dsLatitude)
                || string.IsNullOrEmpty(treinamento.dsLongitude))
            {
                msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatorios") + " <br /> ";
            }

            //DATAS
            if (!string.IsNullOrEmpty(treinamento.dsInicio) && !string.IsNullOrEmpty(treinamento.dsTermino) &&
                !string.IsNullOrEmpty(treinamento.dsInscInicio) && !string.IsNullOrEmpty(treinamento.dsInscFim))
            {
                if (Data.ValidaData(treinamento.dsInicio) && Data.ValidaData(treinamento.dsTermino) &&
                    Data.ValidaData(treinamento.dsInscInicio) && Data.ValidaData(treinamento.dsInscFim))
                {
                    var dtInicio = Convert.ToDateTime(treinamento.dsInicio);
                    var dtTermino = Convert.ToDateTime(treinamento.dsTermino);
                    var dtInscInicio = Convert.ToDateTime(treinamento.dsInscInicio);
                    var dtInscFim = Convert.ToDateTime(treinamento.dsInscFim);

                    if (dtInicio <= System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0))
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamentoAtual") + " <br /> ";

                    if (dtTermino < dtInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamento") + " <br /> ";

                    if (dtInscFim < dtInscInicio)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataInscricao") + " <br /> ";

                    if (dtInscFim > dtTermino)
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataFimInscricaoMaior") + " <br /> ";
                }
                else
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemData") + " <br /> ";
            }

            //HORA
            if (!string.IsNullOrEmpty(treinamento.dsHoraInicio) && !string.IsNullOrEmpty(treinamento.dsHoraTermino))
            {
                var horaInicial = Convert.ToInt32(treinamento.dsHoraInicio.Replace(":", ""));
                var horaFinal = Convert.ToInt32(treinamento.dsHoraTermino.Replace(":", ""));

                if (horaFinal < horaInicial)
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemHora") + " <br /> ";
            }

            return msg;
        }
        
        public ModalMensagem ValidaTreinamentoInternacional(Treinamento treinamento, HttpPostedFileBase dsImagem)
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //CAMPOS EM BRANCO
            if (dsImagem == null || string.IsNullOrEmpty(treinamento.dsNome) || string.IsNullOrEmpty(treinamento.dsCodigoTreinamento) || treinamento.inQuantidadeVagas <= 0 
                || string.IsNullOrEmpty(treinamento.dsInicio) || string.IsNullOrEmpty(treinamento.dsInscInicio) || string.IsNullOrEmpty(treinamento.dsInscFim)
                || string.IsNullOrEmpty(treinamento.dsUrlInscricao) || string.IsNullOrEmpty(treinamento.dsDescricao) || string.IsNullOrEmpty(treinamento.dsLocal) 
                || string.IsNullOrEmpty(treinamento.dsEndereco) || string.IsNullOrEmpty(treinamento.dsNumero) || string.IsNullOrEmpty(treinamento.dsBairro) 
                || string.IsNullOrEmpty(treinamento.dsCEP) || string.IsNullOrEmpty(treinamento.dsCidade) || string.IsNullOrEmpty(treinamento.dsPais)
                || string.IsNullOrEmpty(treinamento.dsLatitude) || string.IsNullOrEmpty(treinamento.dsLongitude) 
                || (treinamento.btSiteOBL == false && treinamento.btSiteOMS == false && treinamento.btSiteOLA == false)
                || (treinamento.idDivisao != 0 && treinamento.idDivisao == 3 && treinamento.idCategoria == 0))
            {
                msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatorios") + " <br /> ";
            }

            //FORMATOS DE ARQUIVOS
            if (dsImagem != null)
            {
                if ((dsImagem.ContentType != "image/pjpeg" && dsImagem.ContentType != "image/jpeg" && dsImagem.ContentType != "image/png"))
                {
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemArquivoIncorreto") + " <br /> ";
                }
            }

            //TAMANHO DA IMAGEM
            if (dsImagem != null)
            {
                var img = System.Drawing.Image.FromStream(dsImagem.InputStream, true, true);
                int width = img.Width;
                int height = img.Height;

                if (width < 446 || height < 238)
                {
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemTamanhoImagem") + " <br /> ";
                }
            }

            //DATA
            if (!string.IsNullOrEmpty(treinamento.dsInicio))
            {
                if (!Data.ValidaData(treinamento.dsInicio))
                    msg.Mensagem += Geral.RetornaTexto("ModalMensagemDataInvalida") + " <br /> ";


                if (Data.ValidaData(treinamento.dsInicio))
                {
                    if (Convert.ToDateTime(treinamento.dsInicio) <= System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0))
                        msg.Mensagem += Geral.RetornaTexto("ModalMensagemDataTreinamentoAtual") + " <br /> ";
                }
            }

            //HORA
            if (!string.IsNullOrEmpty(treinamento.dsHoraInicio) && !string.IsNullOrEmpty(treinamento.dsHoraTermino))
            {
                var horaInicial = Convert.ToInt32(treinamento.dsHoraInicio.Replace(":", ""));
                var horaFinal = Convert.ToInt32(treinamento.dsHoraTermino.Replace(":", ""));

                if (horaFinal < horaInicial)
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemHora") + " <br /> ";
            }

            return msg;
        }
        public ModalMensagem ValidaTreinamentoInternacional(Treinamento treinamento)
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //CAMPOS EM BRANCO
            if (string.IsNullOrEmpty(treinamento.dsNome) || string.IsNullOrEmpty(treinamento.dsCodigoTreinamento) || treinamento.inQuantidadeVagas <= 0
                || string.IsNullOrEmpty(treinamento.dsInicio) || string.IsNullOrEmpty(treinamento.dsInscInicio) || string.IsNullOrEmpty(treinamento.dsInscFim)
                || string.IsNullOrEmpty(treinamento.dsUrlInscricao) || string.IsNullOrEmpty(treinamento.dsDescricao) || string.IsNullOrEmpty(treinamento.dsLocal)
                || string.IsNullOrEmpty(treinamento.dsEndereco) || string.IsNullOrEmpty(treinamento.dsNumero) || string.IsNullOrEmpty(treinamento.dsBairro)
                || string.IsNullOrEmpty(treinamento.dsCEP) || string.IsNullOrEmpty(treinamento.dsCidade) || string.IsNullOrEmpty(treinamento.dsPais)
                || string.IsNullOrEmpty(treinamento.dsLatitude) || string.IsNullOrEmpty(treinamento.dsLongitude)
                || (treinamento.btSiteOBL == false && treinamento.btSiteOMS == false && treinamento.btSiteOLA == false)
                || (treinamento.idDivisao != 0 && treinamento.idDivisao == 3 && treinamento.idCategoria == 0))
            {
                msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatorios") + " <br /> ";
            }

            //DATA
            if (!string.IsNullOrEmpty(treinamento.dsInicio))
            {
                if (!Data.ValidaData(treinamento.dsInicio))
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemData") + " <br /> ";

                if (Data.ValidaData(treinamento.dsInicio))
                {
                    if (Convert.ToDateTime(treinamento.dsInicio) <= System.DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0))
                        msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemDataTreinamentoAtual") + " <br /> ";
                }
            }

            //HORA
            if (!string.IsNullOrEmpty(treinamento.dsHoraInicio) && !string.IsNullOrEmpty(treinamento.dsHoraTermino))
            {
                var horaInicial = Convert.ToInt32(treinamento.dsHoraInicio.Replace(":", ""));
                var horaFinal = Convert.ToInt32(treinamento.dsHoraTermino.Replace(":", ""));

                if (horaFinal < horaInicial)
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemHora") + " <br /> ";
            }

            return msg;
        }
        public Treinamento SalvaArquivoInternacional(Treinamento treinamento, HttpPostedFileBase dsImagem)
        {
            try
            {
                treinamento.btErro = false;

                //IMAGEM
                string nomeArquivo = Path.GetFileNameWithoutExtension(dsImagem.FileName);
                string extensao = Path.GetExtension(dsImagem.FileName);
                string dataHora = System.DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "").Replace("\\", "").Replace(".", "");
                string nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                //Salva arquivo
                var path = Path.Combine(Server.MapPath("~/assets/img/treinamento"), nomeCompleto);
                dsImagem.SaveAs(path);

                treinamento.dsImagem = nomeCompleto;
            }
            catch
            {
                treinamento.btErro = true;
            }

            return treinamento;
        }

        public ModalMensagem ValidaPalestrante(Treinamento treinamento, HttpPostedFileBase dsFoto)
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //FORMATOS DE FOTO
            if (dsFoto != null)
            {
                if (dsFoto.ContentType != "image/pjpeg" && dsFoto.ContentType != "image/jpeg" && dsFoto.ContentType != "image/png")
                {
                    msg.Mensagem += Geral.RetornaTexto("TreinamentoCadastrarModalMensagemArquivoIncorreto") + " <br /> ";
                }
            }

            //NOME E PERFIL
            if (string.IsNullOrEmpty(treinamento.dsPalestrante) || string.IsNullOrEmpty(treinamento.dsPerfil) || dsFoto == null)
                msg.Mensagem += Geral.RetornaTexto("ModalMensagemCamposObrigatorios") + " <br /> ";

            return msg;
        }

        public ModalMensagem ValidaFinalizacao()
        {
            ModalMensagem msg = new ModalMensagem { Titulo = Geral.RetornaTexto("ModalTituloAtencao") };

            //PALESTRANTE E AGENDA
            if (Session["listPalestrantes"] == null)
            {
                msg.Mensagem += Geral.RetornaTexto("TreinamentoDetalhePalestranteNulo") + " <br /> ";
            }

            if (Session["listAgenda"] == null)
            {
                msg.Mensagem += Geral.RetornaTexto("TreinamentoDetalheAgendaNulo") + " <br /> ";
            }
                        
            return msg;
        }
        public Treinamento SalvaFotoPalestrante(Treinamento treinamento, HttpPostedFileBase dsFoto)
        {
            try
            {
                treinamento.btErro = false;

                //FOTO
                string nomeArquivo = Path.GetFileNameWithoutExtension(dsFoto.FileName);
                string extensao = Path.GetExtension(dsFoto.FileName);
                string dataHora = System.DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "").Replace("\\", "").Replace(".", "");
                string nomeCompleto = string.Concat(nomeArquivo, dataHora, extensao);

                //Salva arquivo
                var path = Path.Combine(Server.MapPath("~/assets/img/treinamento/palestrante"), nomeCompleto);
                dsFoto.SaveAs(path);

                treinamento.dsImagem = nomeCompleto;
            }
            catch
            {
                treinamento.btErro = true;
            }

            return treinamento;
        }

        #endregion


        [HttpPost]
        public JsonResult Excluir(int idTreinamento)
        {
            string mensagem = string.Empty;

            var retorno = _treinamento.ExcluiTreinamento(idTreinamento);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Cancelar(int idTreinamento)
        {
            string mensagem = string.Empty;

            var retorno = _treinamento.CancelaTreinamento(idTreinamento);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

    }
}