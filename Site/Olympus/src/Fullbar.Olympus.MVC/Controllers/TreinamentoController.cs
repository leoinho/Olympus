using AutoMapper;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.Dominio.Util;
using Fullbar.Olympus.MVC.Models;
using Fullbar.Olympus.MVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Fullbar.Olympus.MVC.Controllers
{
    public class TreinamentoController : BaseController
    {
        private readonly ITreinamento _treinamento;
        private readonly IPagina _pagina;
        private readonly IUsuario _usuario;

        public TreinamentoController(ITreinamento treinamento, IPagina pagina, IUsuario usuario)
        {

            _treinamento = treinamento;
            _pagina = pagina;
            _usuario = usuario;
        }


        public ActionResult Index()
        {
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);
            var treinamento = new List<TreinamentoHome>();

            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;

            treinamento = (TempData["ListaTreinamentoPorCategoria"] == null) ? _treinamento.ListaHome(null, null, idIdioma, null, idUsuario) : TempData["ListaTreinamentoPorCategoria"] as List<TreinamentoHome>;
            
            var treinamentoViewModel = Mapper.Map<List<TreinamentoHome>, List<TreinamentoHomeViewModel>>(treinamento);

            _pagina.Log(idUsuario, 7);

            var listPais = _treinamento.ListaTreinamentoPais(idIdioma);
            ViewBag.Pais = new SelectList(listPais, "dsPais", "dsPais", idIdioma);
            ViewBag.idCategoria = (TempData["idCategoria"] == null) ? null : TempData["idCategoria"];

            return View(treinamentoViewModel);
        }

        public ActionResult InscricaoHCP()
        {
            return View();
        }

        public ActionResult InscricaoConfirma()
        {  
            return View();
        }

        public ActionResult InscricaoConvidar()
        {
            var cookie = new Cookie();
            var treinamento = TempData["Treinamento"].ToString();
            var usuario = cookie.cookieUsuario();

            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 9);

            var idStatus = Convert.ToInt32(TempData["TreinamentoResultadoStatus"]);
            if (idStatus != 2 && idStatus != 3 && idStatus != 4) return RedirectToAction("Index", "Home");
            TempData["TreinamentoResultadoStatus"] = idStatus;

            if (string.IsNullOrEmpty(treinamento)) return RedirectToAction("Index", "Treinamento");

            if (usuario == null) return RedirectToAction("Index", "Cadastro");

            var retornaTreinamento = _treinamento.ListaPorId(Convert.ToInt32(TempData["idTreinamento"]), idUsuario);

            ViewBag.idTreinamento = Convert.ToInt32(TempData["idTreinamento"]);
            ViewBag.idUsuario = usuario.idusuario;
            ViewBag.Treinamento = treinamento;

            if (retornaTreinamento.idStatus != 2) return Redirect("/Treinamento/Detalhes/" + retornaTreinamento.idTreinamento.ToString());
            
            TempData["Treinamento"] = TempData["Treinamento"];

            var treinamentoDetalhe = TempData["TreinamentoDetalhe"];
            TempData["TreinamentoDetalhe"] = treinamentoDetalhe;

            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];
            TempData["idTreinamento"] = TempData["idTreinamento"];
            TempData["Treinamento"] = TempData["Treinamento"];
            TempData["idEmpresa"] = TempData["idEmpresa"];
            TempData["TreinamentoResultado"] = TempData["TreinamentoResultado"];
            TempData["TreinamentoResultadoStatus"] = TempData["TreinamentoResultadoStatus"];

            cookie.LimpaCookieTreinamento();
            cookie.LimpaCookieListaTreinamento();


            return View();
        }

        public ActionResult InscricaoConfirmacaoTerceiros()
        {
            return View();
        }

        public ActionResult InscricaoConvidarConfirmacao()
        {
            var cookie = new Cookie();
            var treinamento = cookie.cookieTreinamento();
            var usuario = cookie.cookieUsuario();

            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 9);

            if (string.IsNullOrEmpty(treinamento)) return RedirectToAction("Index", "Treinamento");

            if (usuario == null) return RedirectToAction("Index", "Cadastro");

            var retornaTreinamento = _treinamento.ListaPorId(Convert.ToInt32(treinamento), idUsuario);


            //if (retornaTreinamento.idStatus != 2) return Redirect("/Treinamento/Detalhes/" + retornaTreinamento.idTreinamento.ToString());

            var idDivisao = retornaTreinamento.idDivisao;
            ViewBag.idDivisao = idDivisao;

            ViewBag.Nacionalidade = usuario.idNacionalidade;
            ViewBag.idTreinamento = cookie.cookieTreinamento();
            ViewBag.CNPJ = "";

            if (usuario.idNacionalidade == 2 && usuario.RetornoCNPJ != null)
            {
                ViewBag.CNPJ = usuario.RetornoCNPJ.dsCNPJ;
                ViewBag.Instituicao = usuario.RetornoCNPJ.dsNomeFantasia;
            }

            ViewBag.Nome = usuario.dsNomeCompleto;
            ViewBag.Celular = usuario.dsCelular;
            ViewBag.Email = usuario.dsEmail;


            ViewBag.DiaSemana = RetornaDia(Convert.ToString(retornaTreinamento.dtTreinamento.DayOfWeek));

            var listaConvidados = _treinamento.ListaConvidados(usuario.idusuario, retornaTreinamento.idTreinamento);
            ViewBag.ListaConvidados = listaConvidados.Where(t => t.idStatus == 2).ToList();
            ViewBag.ListaConvidadosEspera = listaConvidados.Where(t => t.idStatus != 2).ToList();

            ViewBag.Treinamento = TempData["Treinamento"];
            ViewBag.Treinamento = TempData["Treinamento"].ToString();
            TempData["Treinamento"] = TempData["Treinamento"];

            var treinamentoDetalhe = TempData["TreinamentoDetalhe"];
            ViewBag.TreinamentoDetalhe = treinamentoDetalhe;
            TempData["TreinamentoDetalhe"] = treinamentoDetalhe;

            cookie.LimpaCookieTreinamento();
            cookie.LimpaCookieListaTreinamento();


            return View();
        }

        public ActionResult InscricaoPendente()
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();

            if (usuario == null) return RedirectToAction("Index", "Home");

            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 12);

            var isResultado = TempData["TreinamentoResultado"];
            
            ViewBag.Treinamento = TempData["Treinamento"];
            ViewBag.dsPesquisa = TempData["dsPesquisa"];
            ViewBag.TreinamentoDetalhe = TempData["TreinamentoDetalhe"]; 

            ViewBag.CaminhoAdmin = WebConfigurationManager.AppSettings["caminhoImg"];

            var x = TempData["dsPesquisa"];

            return View();
        }

        public ActionResult InscricaoPesquisa()
        {
            var cookie = new Cookie();
            var treinamento = cookie.cookieTreinamento();
            var usuario = cookie.cookieUsuario();

             var retornaTreinamento = _treinamento.ListaPorId(Convert.ToInt32(TempData["idTreinamento"]), usuario.idusuario);

            var listaTreinamentoPergunta = _treinamento.ListaPerguntas(retornaTreinamento.idTreinamento, 1);
            var listaTreinamentoPerguntaViewModel = Mapper.Map<List<TreinamentoPrePergunta>, List<TreinamentoPrePerguntaViewModel>>(listaTreinamentoPergunta);

            listaTreinamentoPerguntaViewModel.FirstOrDefault().idTreinamento = retornaTreinamento.idTreinamento;
            listaTreinamentoPerguntaViewModel.FirstOrDefault().idUsuario = usuario.idusuario;
            listaTreinamentoPerguntaViewModel.FirstOrDefault().idInscricao = _treinamento.ListaTreinamentoInscricaoUsuario(usuario.idusuario, retornaTreinamento.idTreinamento);


            var treinamentoDetalhe = TempData["TreinamentoDetalhe"];
            TempData["TreinamentoDetalhe"] = treinamentoDetalhe;
            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];
            TempData["idTreinamento"] = TempData["idTreinamento"];
            TempData["Treinamento"] = TempData["Treinamento"];
            TempData["idEmpresa"] = TempData["idEmpresa"];
            TempData["TreinamentoResultado"] = TempData["TreinamentoResultado"];
            TempData["TreinamentoResultadoStatus"] = TempData["TreinamentoResultadoStatus"];
            cookie.LimpaCookieTreinamento();
            cookie.LimpaCookieListaTreinamento();


            return View(listaTreinamentoPerguntaViewModel);
        }

        [HttpPost]
        public ActionResult Index(TreinamentoHomeViewModel treinamentoHome)
        {
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);
            var treinamento = new List<TreinamentoHome>();            
            
            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;

            treinamento = (TempData["ListaTreinamentoPorCategoria"] == null) ? _treinamento.ListaHome(null, treinamentoHome.idCategoria, treinamentoHome.idIdioma, treinamentoHome.dsPais, idUsuario) : TempData["ListaTreinamentoPorCategoria"] as List<TreinamentoHome>;

            var treinamentoViewModel = Mapper.Map<List<TreinamentoHome>, List<TreinamentoHomeViewModel>>(treinamento);


            _pagina.Log(idUsuario, 7);

            var listPais = _treinamento.ListaTreinamentoPais(idIdioma);
            ViewBag.Pais = new SelectList(listPais, "dsPais", "dsPais");
            ViewBag.idCategoria = treinamentoHome.idCategoria;

            return View(treinamentoViewModel);
        }

        public ActionResult Categoria(int id)
        {
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);
            var usuario = new Cookie().cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;

            var treinamento = _treinamento.ListaHome(null, id, idIdioma, null, idUsuario);
            
            _pagina.Log(idUsuario, 7);

            TempData["idCategoria"] = id;
            TempData["ListaTreinamentoPorCategoria"] = treinamento;

            return RedirectToAction("Index");
        }

        public ActionResult Detalhes(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 8);

            var treinamentoDetalhe = _treinamento.ListaPorId((int)id, idUsuario);
            var treinamentoDetalheViewModel = Mapper.Map<TreinamentoDetalhe, TreinamentoDetalheViewModel>(treinamentoDetalhe);

            var dataFinal = DateTime.Now.AddDays(-1);
            if (treinamentoDetalheViewModel.dtInscricaoFim < dataFinal)
            {
                treinamentoDetalheViewModel.idStatus = 3; //status inativo caso tenha passado periodo de inscricao
            }

            TempData["TreinamentoDetalhe"] = treinamentoDetalhe;

           

            cookie.LimpaCookieTreinamento();

            //TREINAMENTO EM BREVE
            if (id != 57 && id != 58 && id != 59 && id != 66 && id != 67 && id != 68 && id != 69)
                cookie.CriarCookieIdTreinamento(id.ToString());

            TempData["Treinamento"] = treinamentoDetalheViewModel.dsNome;

            ViewBag.Mensagem = TempData["Mensagem"] as string;
            ViewBag.MensagemTitulo = TempData["MensagemTitulo"] as string;

            var retornaTreinamento = _treinamento.ListaPorId(Convert.ToInt32(id), idUsuario);
            ViewBag.TreinamentoDetalhe = retornaTreinamento;

            ViewBag.DiaSemana = RetornaDia(Convert.ToString(retornaTreinamento.dtTreinamento.DayOfWeek));

            return View(treinamentoDetalheViewModel);
        }

        public ActionResult Inscricao()
        {

            var cookie = new Cookie();
            var treinamento = cookie.cookieTreinamento();
            var usuario = cookie.cookieUsuario();
            
            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 9);

            if (string.IsNullOrEmpty(treinamento)) return RedirectToAction("Index", "Treinamento");

            if (usuario == null) return RedirectToAction("Index", "Cadastro");

            var retornaTreinamento = _treinamento.ListaPorId(Convert.ToInt32(treinamento), idUsuario);


            if (retornaTreinamento.idStatus != 2) return Redirect("/Treinamento/Detalhes/" + retornaTreinamento.idTreinamento.ToString());

            var idDivisao = retornaTreinamento.idDivisao;            
            ViewBag.idDivisao = idDivisao;

            ViewBag.Nacionalidade = usuario.idNacionalidade;
            ViewBag.idTreinamento = cookie.cookieTreinamento();
            ViewBag.CNPJ = "";

            if (usuario.idNacionalidade == 2 && usuario.RetornoCNPJ != null)
            {
                ViewBag.CNPJ = usuario.RetornoCNPJ.dsCNPJ;
                ViewBag.Instituicao = usuario.RetornoCNPJ.dsNomeFantasia;
            }

            ViewBag.Nome = usuario.dsNomeCompleto;
            ViewBag.Celular = usuario.dsCelular;
            ViewBag.Email = usuario.dsEmail;


            ViewBag.DiaSemana = RetornaDia(Convert.ToString(retornaTreinamento.dtTreinamento.DayOfWeek));

            if (usuario.idNacionalidade == 1)
            {
                if (usuario.RetornoCNPJ != null)
                {
                    ViewBag.CNPJ = usuario.RetornoCNPJ.dsCNPJ;
                    ViewBag.Instituicao = usuario.RetornoCNPJ.dsNomeFantasia;
                    ViewBag.idEmpresa = usuario.RetornoCNPJ.idEmpresa;
                }
                else
                {
                    ViewBag.CNPJ = usuario.dsCNPJ;
                    ViewBag.Instituicao = "";
                    ViewBag.idEmpresa = 0;
                }

                ViewBag.idUsuario = usuario.idusuario;
                
            }



            //PESQUISA HCP:
            var listaTreinamentoPergunta = _treinamento.ListaPerguntas(retornaTreinamento.idTreinamento, 3);
            var listaTreinamentoPerguntaViewModel = Mapper.Map<List<TreinamentoPrePergunta>, List<TreinamentoPrePerguntaViewModel>>(listaTreinamentoPergunta);

            if (listaTreinamentoPergunta != null && listaTreinamentoPergunta.Count > 0)
            {
                listaTreinamentoPerguntaViewModel.FirstOrDefault().idTreinamento = retornaTreinamento.idTreinamento;
                listaTreinamentoPerguntaViewModel.FirstOrDefault().idUsuario = usuario.idusuario;
                listaTreinamentoPerguntaViewModel.FirstOrDefault().idInscricao = _treinamento.ListaTreinamentoInscricaoUsuario(usuario.idusuario, retornaTreinamento.idTreinamento);
            }
            else
                ViewBag.BtPesquisa = false;

            ViewBag.IdDivisao = retornaTreinamento.idDivisao;
            //fim pesquisa


            ViewBag.Treinamento = TempData["Treinamento"];
            ViewBag.Treinamento = TempData["Treinamento"].ToString();
            TempData["Treinamento"] = TempData["Treinamento"];

            var treinamentoDetalhe = TempData["TreinamentoDetalhe"];
            ViewBag.TreinamentoDetalhe = treinamentoDetalhe;
            TempData["TreinamentoDetalhe"] = treinamentoDetalhe;

            cookie.LimpaCookieTreinamento();
            cookie.LimpaCookieListaTreinamento();


            return View(listaTreinamentoPerguntaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarInscricao(TreinamentoInscricaoViewModel treinamentoViewModel)
        {
            var cookie = new Cookie();
            cookie.CriarCookieIdTreinamento(treinamentoViewModel.idTreinamento.ToString());
            var cookieListaTreinamentoConvite = cookie.cookieTreinamentoConvite();

            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];
            TempData["idTreinamento"] = TempData["idTreinamento"];
            TempData["Treinamento"] = TempData["Treinamento"];
            TempData["idEmpresa"] = TempData["idEmpresa"];
            TempData["TreinamentoResultado"] = TempData["TreinamentoResultado"];

            var treinamentoDetalhe = _treinamento.ListaPorId(Convert.ToInt32(treinamentoViewModel.idTreinamento), 0);
            var idDivisao = treinamentoDetalhe.idDivisao;
            ViewBag.idDivisao = idDivisao;
            ViewBag.idUsuario = treinamentoViewModel.idUsuario;
            ViewBag.Treinamento = treinamentoViewModel.Treinamento;
            ViewBag.idTreinamento = treinamentoViewModel.idTreinamento;

            //ViewBag.Treinamento = treinamentoViewModel.Treinamento;
            //ViewBag.CNPJ = treinamentoViewModel.CNPJ;
            //ViewBag.Instituicao = treinamentoViewModel.Instituicao;

            ViewBag.idTreinamento = treinamentoViewModel.idTreinamento;
            //ViewBag.idUsuario = treinamentoViewModel.idUsuario;
            //ViewBag.idEmpresa = treinamentoViewModel.idEmpresa;
            //ViewBag.Hospegadem = treinamentoViewModel.hospedagemPassagem;

            TempData["Treinamento"] = treinamentoViewModel.Treinamento;
            //TempData["CNPJ"] = treinamentoViewModel.CNPJ;
            //TempData["Instituicao"] = treinamentoViewModel.Instituicao;

            TempData["idTreinamento"] = treinamentoViewModel.idTreinamento;
            TempData["idUsuario"] = treinamentoViewModel.idUsuario;
 
            //TempData["idEmpresa"] = treinamentoViewModel.idEmpresa;
            //TempData["hospedagem"] = treinamentoViewModel.hospedagemPassagem;

            var msg = string.Empty;

            msg += treinamentoViewModel.ValidaNome();
            msg += treinamentoViewModel.ValidaEmail();
            msg += treinamentoViewModel.ValidaCPF();
            msg += treinamentoViewModel.ValidaCelular();
            msg += treinamentoViewModel.VerificaUsuarioPossuiTreinamento();

            if (string.IsNullOrEmpty(msg))
            {

                if (cookieListaTreinamentoConvite != null && cookieListaTreinamentoConvite.Count > 0)
                {

                    var CPF = treinamentoViewModel.CPF.Replace("-", "").Replace(".", "");

                    var isCPF = cookieListaTreinamentoConvite.Where(x => x.CPF == CPF).FirstOrDefault();
                    var isNome = cookieListaTreinamentoConvite.Where(x => x.Nome == treinamentoViewModel.Nome).FirstOrDefault();
                    var isEmail = cookieListaTreinamentoConvite.Where(x => x.Email == treinamentoViewModel.Email).FirstOrDefault();
                    var isCelular = cookieListaTreinamentoConvite.Where(x => x.Celular == treinamentoViewModel.Celular).FirstOrDefault();

                    if (isNome != null) msg += Resource.TreinamentoCadastrarInscricaoNome;
                    if (isCPF != null) msg += Resource.TreinamentoCadastrarInscricaoCPF;
                    if (isEmail != null) msg += Resource.TreinamentoCadastrarInscricaoEmail;
                    if (isCelular != null) msg += Resource.TreinamentoCadastrarInscricaoCelular;

                    if (string.IsNullOrEmpty(msg))
                    {
                        var treinamentoConvite = new TreinamentoConvite();

                        treinamentoConvite.Nome = treinamentoViewModel.Nome;
                        treinamentoConvite.CPF = treinamentoViewModel.CPF.Replace("-", "").Replace(".", "");
                        treinamentoConvite.Email = treinamentoViewModel.Email;
                        treinamentoConvite.Celular = treinamentoViewModel.Celular;

                        cookieListaTreinamentoConvite.Add(treinamentoConvite);
                        cookie.LimpaCookieListaTreinamento();

                        cookie.CriarCookieTreinamentoConvite(cookieListaTreinamentoConvite);

                        treinamentoViewModel.Convites = cookieListaTreinamentoConvite;
                    }
                }
                else
                {

                    var treinamentoConvite = new TreinamentoConvite();
                    var listaTreinamentoConvite = new List<TreinamentoConvite>();

                    treinamentoConvite.Nome = treinamentoViewModel.Nome;
                    treinamentoConvite.CPF = treinamentoViewModel.CPF.Replace("-", "").Replace(".", "");
                    treinamentoConvite.Email = treinamentoViewModel.Email;
                    treinamentoConvite.Celular = treinamentoViewModel.Celular;

                    listaTreinamentoConvite.Add(treinamentoConvite);

                    cookie.CriarCookieTreinamentoConvite(listaTreinamentoConvite);

                    treinamentoViewModel.Convites = listaTreinamentoConvite;

                }
            }


            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.CPF = treinamentoViewModel.CPF;
                ViewBag.Nome = treinamentoViewModel.Nome;
                ViewBag.Email = treinamentoViewModel.Email;
                ViewBag.Celular = treinamentoViewModel.Celular;

                ViewBag.Mensagem = Resource.TreinamentoCadastroInscricaoTextoErro + "<br/><br/>" + msg;
                ViewBag.MensagemTitulo = Resource.ContatoTituloErro;

                treinamentoViewModel.Convites = cookieListaTreinamentoConvite;
            }

            ViewBag.TreinamentoDetalhe = treinamentoDetalhe;
            TempData["TreinamentoDetalhe"] = treinamentoDetalhe;
            
            return View("InscricaoConvidar", treinamentoViewModel);
        }

        public ActionResult Excluir(string CPF)
        {
            var cookie = new Cookie();

            var listaTreinamentoConvite = cookie.cookieTreinamentoConvite();
            var treinamentoConvite = listaTreinamentoConvite.Where(x => x.CPF == CPF).FirstOrDefault();

            var idDivisao = _treinamento.ListaPorId(Convert.ToInt32(TempData["idTreinamento"]), 0).idDivisao;
            ViewBag.idDivisao = idDivisao;

            listaTreinamentoConvite.Remove(treinamentoConvite);

            cookie.LimpaCookieListaTreinamento();
            cookie.CriarCookieTreinamentoConvite(listaTreinamentoConvite);

            var cookieListaTreinamentoConvite = cookie.cookieTreinamentoConvite();

            ViewBag.Treinamento = TempData["Treinamento"];
            //ViewBag.CNPJ = TempData["CNPJ"];
            //ViewBag.Instituicao = TempData["Instituicao"];

            ViewBag.idTreinamento = TempData["idTreinamento"];
            ViewBag.idUsuario = TempData["idUsuario"];
            //ViewBag.idEmpresa = TempData["idEmpresa"];
            //ViewBag.Hospegadem = TempData["hospedagem"];

            TempData["Treinamento"] = TempData["Treinamento"];
            TempData["CNPJ"] = TempData["CNPJ"];
            TempData["Instituicao"] = TempData["Instituicao"];

            TempData["idTreinamento"] = TempData["idTreinamento"];
            TempData["idUsuario"] = TempData["idUsuario"];
            TempData["idEmpresa"] = TempData["idEmpresa"];
            TempData["hospedagem"] = TempData["hospedagem"];


            var treinamentoViewModel = new TreinamentoInscricaoViewModel();

            treinamentoViewModel.Convites = cookieListaTreinamentoConvite;

            ViewBag.TreinamentoDetalhe = TempData["TreinamentoDetalhe"];
            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];

            return View("InscricaoConvidar", treinamentoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finaliza(TreinamentoInscricaoViewModel treinamentoViewModel)
        {
            var cookie = new Cookie();
            var treinamentoModel = Mapper.Map<TreinamentoInscricaoViewModel, TreinamentoInscricao>(treinamentoViewModel);
            var cookieListaTreinamentoConvite = cookie.cookieTreinamentoConvite();

            var idDivisao = _treinamento.ListaPorId(Convert.ToInt32(treinamentoViewModel.idTreinamento), treinamentoModel.idUsuario).idDivisao;
            ViewBag.idDivisao = idDivisao;

            ViewBag.Treinamento = treinamentoViewModel.Treinamento;
            ViewBag.CNPJ = treinamentoViewModel.CNPJ;
            ViewBag.Instituicao = treinamentoViewModel.Instituicao;

            ViewBag.idTreinamento = treinamentoViewModel.idTreinamento;
            ViewBag.idUsuario = treinamentoViewModel.idUsuario;
            ViewBag.idEmpresa = treinamentoViewModel.idEmpresa;

            ViewBag.Hospegadem = treinamentoViewModel.hospedagemPassagem;
            
            var inscricaoTreinamento = new TreinamentoInscricao()
            {

                idUsuario = treinamentoModel.idUsuario
                ,
                hospedagemPassagem = treinamentoModel.hospedagemPassagem
                ,
                idEmpresa = treinamentoModel.idEmpresa
                ,
                idTreinamento = treinamentoModel.idTreinamento
                ,
                Convites = cookieListaTreinamentoConvite
            };

            var retornoCadastroTreinamento = _treinamento.Cadastrar(inscricaoTreinamento);           


            if (retornoCadastroTreinamento.id > 0)
            {
                cookie.LimpaCookieListaTreinamento();
                cookie.LimpaCookieTreinamento();

                TempData["TreinamentoResultado"] = true;
                TempData["idStatus"] = retornoCadastroTreinamento.idStatus;

                TempData["dsPesquisa"] = retornoCadastroTreinamento.dsPesquisa;

                TempData["idTreinamento"] = treinamentoViewModel.idTreinamento;
                TempData["idEmpresa"] = treinamentoViewModel.idEmpresa;
                TempData["Treinamento"] = treinamentoViewModel.Treinamento;

                TempData["idInscricaoUsuarioConvidante"] = retornoCadastroTreinamento.id;

                return RedirectToAction("Resultado");
            }
            else
            {
                ViewBag.Mensagem = treinamentoViewModel.ValidaMensagemRetorno(retornoCadastroTreinamento.id);
                ViewBag.MensagemTitulo = Resource.ContatoTituloErro;
            }

            ViewBag.TreinamentoDetalhe = TempData["TreinamentoDetalhe"];
            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];

            treinamentoViewModel.Convites = cookieListaTreinamentoConvite;
            return View("Inscricao", treinamentoViewModel);
        }

        [Authorize]
        public ActionResult Resultado()
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();

            if (usuario == null) return RedirectToAction("Index", "Home");

            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 12);

            var isResultado = TempData["TreinamentoResultado"];


            if (isResultado == null) return RedirectToAction("Index", "Home");

            var idStatus = Convert.ToInt32(TempData["TreinamentoResultadoStatus"]);
            if (idStatus != 2 && idStatus != 3 && idStatus != 4) return RedirectToAction("Index", "Home");
            TempData["TreinamentoResultadoStatus"] = idStatus;

            if (usuario.idNacionalidade == 2)
            {

                var TreinamentoResultadoViewModelEstrangeiro = Mapper.Map<TreinamentoInscricaoResultado, TreinamentoInscricaoResultadoViewModel>(new TreinamentoInscricaoResultado());

                TreinamentoResultadoViewModelEstrangeiro.Confirmado = null;
                TreinamentoResultadoViewModelEstrangeiro.ListaEspera = new List<TreinamentoResultadoListaEspera>() { new TreinamentoResultadoListaEspera() { dsNome = usuario.dsNomeCompleto } };

         

                return View(TreinamentoResultadoViewModelEstrangeiro);
            }

            var idTreinamento = TempData["idTreinamento"];
            var idEmpresa = TempData["idEmpresa"];
            var idInscricaoUsuarioConvidante = TempData["idInscricaoUsuarioConvidante"];
            var x = TempData["TreinamentoDetalhe"];
            ViewBag.Treinamento = TempData["Treinamento"];
            ViewBag.TreinamentoDetalhe = TempData["TreinamentoDetalhe"];
            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];
            TempData["idTreinamento"] = TempData["idTreinamento"];
            TempData["Treinamento"] = TempData["Treinamento"];
            TempData["idEmpresa"] = TempData["idEmpresa"];
            TempData["TreinamentoResultado"] = TempData["TreinamentoResultado"];

            ViewBag.idStatus = TempData["idStatus"];
            ViewBag.ResultadoStatus = TempData["TreinamentoResultadoStatus"];
            TempData["TreinamentoResultadoStatus"] = TempData["TreinamentoResultadoStatus"];
            var xxx = TempData["TreinamentoResultadoStatus"];
            TempData["TreinamentoResultadoStatus"] = TempData["TreinamentoResultadoStatus"];
            var retornaTreinamento = _treinamento.ListaPorId(Convert.ToInt32(idTreinamento), usuario.idusuario);

            ViewBag.DiaSemana = RetornaDia(Convert.ToString(retornaTreinamento.dtTreinamento.DayOfWeek));

            var listaTreinamentoResultado = _treinamento.ListaResultado(Convert.ToInt16(idTreinamento), Convert.ToInt16(idEmpresa), usuario.idusuario, Convert.ToInt16(idInscricaoUsuarioConvidante));
            var listaTreinamentoResultadoViewModel = Mapper.Map<TreinamentoInscricaoResultado, TreinamentoInscricaoResultadoViewModel>(listaTreinamentoResultado);

             
            ViewBag.CaminhoAdmin = WebConfigurationManager.AppSettings["caminhoImg"];
            ViewBag.dsPesquisa = TempData["dsPesquisa"];

            return View(listaTreinamentoResultadoViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizaE(int? hospedagemPassagem, int idTreinamento, int editaDados, string nome, string email, string celular, string curriculo, TreinamentoPrePerguntaViewModel treinamentoPrePerguntaViewModel)
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();

            var idDivisao = _treinamento.ListaPorId(idTreinamento, usuario.idusuario).idDivisao;
            ViewBag.idDivisao = idDivisao;

            var inscricao = new TreinamentoInscricaoEstrangeiro()
            {

                hospedagem = (hospedagemPassagem == 1) ? true : false
                ,
                idEmpresa = (usuario.RetornoCNPJ != null) ? usuario.RetornoCNPJ.idEmpresa : 0
                ,
                idTreinamento = idTreinamento
                ,
                idUsuario = usuario.idusuario
                ,
                dsCurriculo = (curriculo != null) ? curriculo : ""
            };

            var retornoInscricao = _treinamento.CadastrarEstrangeiro(inscricao);

            TempData["dsPesquisa"] = retornoInscricao.dsPesquisa;

            ViewBag.Nome = usuario.dsNomeCompleto;
            ViewBag.Celular = usuario.dsCelular;
            ViewBag.Email = usuario.dsEmail;
            ViewBag.DiaSemana = RetornaDia(Convert.ToString(_treinamento.ListaPorId(idTreinamento, usuario.idusuario).dtTreinamento.DayOfWeek));

            if (TempData["Treinamento"] != null)
            {

                ViewBag.Treinamento = TempData["Treinamento"].ToString();
                TempData["Treinamento"] = TempData["Treinamento"].ToString();

                var treinamentoDetalhe = TempData["TreinamentoDetalhe"];
                ViewBag.TreinamentoDetalhe = treinamentoDetalhe;
                TempData["TreinamentoDetalhe"] = treinamentoDetalhe;
                TempData["idEmpresa"] = usuario.RetornoCNPJ.idEmpresa;
            }            

            var t = _treinamento.ListaPorId(idTreinamento, usuario.idusuario);
            TempData["idTreinamento"] = idTreinamento;           

            if (usuario.idNacionalidade == 1)
            {
                if (usuario.RetornoCNPJ != null)
                {
                    ViewBag.CNPJ = usuario.RetornoCNPJ.dsCNPJ;
                    ViewBag.Instituicao = usuario.RetornoCNPJ.dsNomeFantasia;
                    ViewBag.idEmpresa = usuario.RetornoCNPJ.idEmpresa;
                }
                else
                {
                    ViewBag.CNPJ = usuario.dsCNPJ;
                    ViewBag.Instituicao = "";
                    ViewBag.idEmpresa = 0;
                }

                ViewBag.idUsuario = usuario.idusuario;

            }

            if (retornoInscricao.id > 0)
            {

                Session.Add("inscricao", retornoInscricao);

                if (editaDados == 1)
                {
                    UsuarioEditar _edita = new UsuarioEditar();
                    _edita.idUsuario = usuario.idusuario;
                    _edita.Nome = nome;
                    _edita.Email = email;
                    _edita.Telefone = usuario.dsTelefone;
                    _edita.Celular = celular;
                    _edita.Senha = Crypto.Decrypt(usuario.dsSenha, Crypto.Key256, 256);
                    _edita.TituloProfissional = usuario.dsTituloProfissional;
                    _edita.TituloProfissionalOutros = usuario.dsTituloProfissionalOutros;
                    _edita.NumeroConselho = usuario.dsNumConselho;
                    _edita.Especialidade = usuario.dsEspecialidade;
                    _edita.CNPJ = usuario.dsCNPJ;
                    _edita.LicencaMedicaEUA = Convert.ToInt32(usuario.btLicencaEUA);
                    _edita.NumeroEstado = usuario.dsNumEstado;                    

                    var edita = _usuario.EditaNoTreinamento(_edita);

                    if (edita.id > 0)
                    {
                        Session.Remove("USUARIO");
                        var usuarioEdita = _usuario.ListaUsuarioPorId(usuario.idusuario);
                        var _cookie = new Cookie();
                        cookie.CriarCookie(usuarioEdita);
                    }
                    
                }

                TempData["TreinamentoResultado"] = retornoInscricao.id;

                TempData["TreinamentoResultadoStatus"] = retornoInscricao.idStatus;


                if (treinamentoPrePerguntaViewModel != null && idDivisao == 3 && (!string.IsNullOrEmpty(curriculo)))
                {
                    var treinamentoPrePergunta = Mapper.Map<TreinamentoPrePerguntaViewModel, TreinamentoPrePergunta>(treinamentoPrePerguntaViewModel);
                    treinamentoPrePergunta.idTipo = 1; //Pré
                    treinamentoPrePergunta.idUsuario = usuario.idusuario;
                    treinamentoPrePergunta.idInscricao = retornoInscricao.id;
                    var retornoPesquisa = _treinamento.CadastraRespostasQuestionario(treinamentoPrePergunta);
                }


                if (retornoInscricao.idStatus == 2 || retornoInscricao.idStatus == 3 || retornoInscricao.idStatus == 4)
                    return RedirectToAction("Resultado");
                else
                    return RedirectToAction("InscricaoPendente");
                
            }

            var msg = retornoInscricao.dsMensagem;

            ViewBag.Hospegadem = hospedagemPassagem;
            ViewBag.idTreinamento = idTreinamento;
            ViewBag.Mensagem = msg;
            ViewBag.MensagemTitulo = Resource.ContatoTituloErro;
            ViewBag.Nacionalidade = usuario.idNacionalidade;

            return View("Inscricao");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizaConvidar()
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            string msg = string.Empty;

            var cookieListaTreinamentoConvite = cookie.cookieTreinamentoConvite();
            ViewBag.Treinamento = TempData["Treinamento"];
            ViewBag.idTreinamento = TempData["idTreinamento"];
            ViewBag.idUsuario = TempData["idUsuario"];
            TempData["Treinamento"] = TempData["Treinamento"];
            TempData["CNPJ"] = TempData["CNPJ"];
            TempData["Instituicao"] = TempData["Instituicao"];
            TempData["idTreinamento"] = TempData["idTreinamento"];
            TempData["idUsuario"] = TempData["idUsuario"];
            TempData["idEmpresa"] = TempData["idEmpresa"];
            TempData["hospedagem"] = TempData["hospedagem"];

            TempData["Treinamento"] = TempData["Treinamento"];

            var treinamentoViewModel = new TreinamentoInscricaoViewModel();

            treinamentoViewModel.Convites = cookieListaTreinamentoConvite;

            ViewBag.TreinamentoDetalhe = TempData["TreinamentoDetalhe"];
            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];

            if (Session["inscricao"] != null)
            {
                RetornoPadrao retorno = (RetornoPadrao)Session["inscricao"];
                TreinamentoInscricao inscricao = new TreinamentoInscricao();
                inscricao.idEmpresa = Convert.ToInt32(TempData["idEmpresa"]);
                inscricao.idTreinamento = Convert.ToInt32(TempData["idTreinamento"]);
                inscricao.idUsuario = Convert.ToInt32(TempData["idUsuario"]);

                inscricao.Convites = cookieListaTreinamentoConvite; 

                var cadastro = _treinamento.CadastrarConvite(inscricao, retorno);

                if (cadastro.id > 0)
                {
                    Session.Remove("inscricao");
                    return RedirectToAction("InscricaoConvidarConfirmacao");
                }
                else
                {
                    ViewBag.Mensagem = cadastro.dsMensagem;
                    ViewBag.MensagemTitulo = Resource.ContatoTituloErro;

                    return View("InscricaoConvidar", treinamentoViewModel);
                }

            }
            else
            {
                return RedirectToAction("/Home/");
            }

            
        }

        [Authorize]
        public ActionResult MeusTreinamentos()
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();

            if (usuario == null) return RedirectToAction("Index", "Home");


            var idUsuario = (usuario != null) ? usuario.idusuario : 0;
            _pagina.Log(idUsuario, 10);


            var listaTreinamentos = _treinamento.ListaPorUsuario(usuario.idusuario);           

            foreach (var item in listaTreinamentos)
            {
                item.dsDia = RetornaDia(Convert.ToString(item.dtTreinamento.DayOfWeek));
                item.dsDiaFim = RetornaDia(Convert.ToString(item.dtTreinamentoFim.DayOfWeek));

                if (item.idStatusInscricao == 8 || item.idStatusInscricao == 5)
                {
                    if (_treinamento.ListaConvidadosCertificado(idUsuario, item.idTreinamento).Count > 0)
                    {
                        item.btCertificadoIndicado = true;
                    }
                    else
                    {
                        item.btCertificadoIndicado = false;
                    }
                }
                
            }            

            ViewBag.mensagem = TempData["mensagem"] as string;
            ViewBag.mensagemTitulo = TempData["mensagemTitulo"] as string;

            return View(listaTreinamentos);
        }

        [Authorize]
        [HttpPost]
        public JsonResult InscricaoUsuario(int idTreinamento)
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            var retorno = _treinamento.ListaInscricaoPorUsuario(usuario, idTreinamento);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CancelarInscricao(int? idTreinamento)
        //{
        //    var cookie = new Cookie();
        //    var usuario = cookie.cookieUsuario();

        //    var retorno = _treinamento.CancelarInscricao(usuario.idusuario, (int)idTreinamento);

        //    if (retorno.id > 0)
        //    {
        //        TempData["mensagem"] = Resource.PaginaMeusTreinamentosCanceladoSucesso;
        //        TempData["mensagemTitulo"] = Resource.TreinamentoTituloErro;

        //        return RedirectToAction("MeusTreinamentos");
        //    }

        //    ViewBag.mensagem = retorno.dsMensagem;
        //    TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];

        //    return View("MeusTreinamentos");
        //}


        [Authorize]
        [HttpPost]
        public JsonResult CancelarInscricao(int idTreinamento)
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();

            var status = _treinamento.ListaTreinamentoInscricaoStatusUsuario(usuario.idusuario, idTreinamento);

            var retorno = _treinamento.CancelarInscricao(usuario.idusuario, (int)idTreinamento);

            if (retorno.id > 0)
            {

                if (status == 2)
                {
                    try { _treinamento.ConfirmaProximoListaEspera(retorno.idReferencia, 0); }
                    catch (Exception ex) { var erro = ex.Message; }
                }             

                retorno.dsMensagem = Resource.PaginaMeusTreinamentosCanceladoSucesso;
                retorno.dsTitulo = Resource.TreinamentoTituloErro;
            }

            //ViewBag.mensagem = retorno.dsMensagem;
            TempData["TreinamentoDetalhe"] = TempData["TreinamentoDetalhe"];

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult Questionario(int id, int idTipo)
        {
            var usuario = new Cookie().cookieUsuario();

            if (usuario == null || usuario.idusuario == 0) return RedirectToAction("Index", "Home");

            //Verifica se usuário já respondeu questionário
            //if (_treinamento.QuestionarioRespondido(id, usuario.idusuario, inQuestionario)) { return RedirectToAction("Detalhes", "Treinamento", new { id = id }); }

            var listaTreinamentoPergunta = _treinamento.ListaPerguntas(id, idTipo);
            var listaTreinamentoPerguntaViewModel = Mapper.Map<List<TreinamentoPrePergunta>, List<TreinamentoPrePerguntaViewModel>>(listaTreinamentoPergunta);

            ViewBag.Tipo = idTipo;
            listaTreinamentoPerguntaViewModel.FirstOrDefault().idTreinamento = id;
            listaTreinamentoPerguntaViewModel.FirstOrDefault().idUsuario = usuario.idusuario;
            listaTreinamentoPerguntaViewModel.FirstOrDefault().idInscricao = _treinamento.ListaTreinamentoInscricaoUsuario(usuario.idusuario, id);
            
            //Mesagens de retorno
            ViewBag.Mensagem = Resource.MensagemQuestionarioAlerta;
            ViewBag.MensagemTitulo = Resource.MensagemQuestionarioAlertaTitulo;

            return View(listaTreinamentoPerguntaViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Questionario(TreinamentoPrePerguntaViewModel treinamentoPrePerguntaViewModel)
        {
            var treinamentoPrePergunta = Mapper.Map<TreinamentoPrePerguntaViewModel, TreinamentoPrePergunta>(treinamentoPrePerguntaViewModel);

            var retorno = _treinamento.CadastraRespostasQuestionario(treinamentoPrePergunta);

            ViewBag.Tipo = treinamentoPrePergunta.idTipo;

            if (retorno.id != 0)
            {
                TempData["Mensagem"] = Resource.MensagemQuestionarioRespondido;
                //TempData["MensagemTitulo"] = Resource.MensagemQuestionarioRespondidoTitulo;

                return RedirectToAction("MeusTreinamentos", "Treinamento");
            }

            return View();
        }

        [Authorize]
        public JsonResult Calendario()
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            var Idioma = WebConfigurationManager.AppSettings["Idioma"];


            var listaCalendario = _treinamento.TreinamentoCalendario(DateTime.Now.Month, DateTime.Now.Year, usuario.idusuario);
            var calendario = new List<TreinamentoCalendario>();

            foreach (var item in listaCalendario)
            {

                var treinamentoCalendario = new TreinamentoCalendario();

                treinamentoCalendario.Idioma = Idioma;
                treinamentoCalendario.dsNome = item.dsNome;
                treinamentoCalendario.dtTreinamento = item.dtTreinamento;
                treinamentoCalendario.idTreinamento = item.idTreinamento;
                treinamentoCalendario.Mes = item.dtTreinamento.Month;
                treinamentoCalendario.Ano = item.dtTreinamento.Year;
                treinamentoCalendario.DataTreinamento = item.dtTreinamento.ToString("dd/MM/yyyy");
                treinamentoCalendario.idStatus = (item.idStatusInscricao == 0) ? item.idStatusTreinamento : item.idStatusInscricao;
                treinamentoCalendario.dsStatusInscricao = item.dsStatusInscricao;
                treinamentoCalendario.dsUrl = string.Format("/{0}/{1}/{2}", Resource.PaginaTreinamentoController, Resource.PaginaTreinamentoDetalhe, item.idTreinamento);


                calendario.Add(treinamentoCalendario);
            }


            return Json(calendario, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PDF(int id)
        {

            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            var retorno = _treinamento.ListaInscricaoPorUsuario(usuario, id);



            var inscricao = new UsuarioInscricao()
            {

                CNPJ = (!string.IsNullOrEmpty(retorno.CNPJ)) ? Convert.ToUInt64(retorno.CNPJ).ToString(@"00\.000\.000\/0000\-00") : Convert.ToUInt64(usuario.dsCNPJ).ToString(@"00\.000\.000\/0000\-00")
                ,
                CPF = (!string.IsNullOrEmpty(retorno.CPF)) ? Convert.ToUInt64(retorno.CPF).ToString(@"000\.000\.000\-00") : Convert.ToUInt64(usuario.dsCPF).ToString(@"000\.000\.000\-00")
                ,
                Data = retorno.Data
                ,
                Endereco = retorno.Endereco
                ,
                Nome = retorno.Nome
                ,
                Telefone = retorno.Telefone
                ,
                TreinamentoNome = retorno.Treinamento
                ,
                Contato = Resource.PaginaContatoEmailEnvio
                ,
                Bairro = retorno.Bairro
                ,
                Cidade = retorno.Cidade
                ,
                UF = retorno.UF

            };

            return new Rotativa.ActionAsPdf("InscricaoPDF", inscricao);
        }
        [Authorize]
        public ActionResult InscricaoPDF(UsuarioInscricao usuarioInscricao)
        {
            return View(usuarioInscricao);
        }

        public ActionResult Certificado(int id, int? idUsuario)
        {
            var cookie = new Cookie();
            Usuario usuario = new Usuario();

            if (idUsuario != null)
            {
                usuario = _usuario.ListaUsuarioPorId(Convert.ToInt32(idUsuario));
            }
            else
            {
                usuario = cookie.cookieUsuario();
            }

            var retorno = _treinamento.ListaInscricaoPorUsuario(usuario, id);
            
            var inscricao = new TreinamentoInscricaoUsuario()
            {
                Nome = retorno.Nome,
                Treinamento = retorno.Treinamento,
                DataTreinamento = retorno.DataTreinamento,
                HoraInicio = retorno.HoraInicio,
                HoraFim = retorno.HoraFim,
                Cidade = retorno.Cidade,
                UF = retorno.UF
            };

            return new Rotativa.ActionAsPdf("CertificadoPDF", inscricao)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape, PageSize = Rotativa.Options.Size.A2
                
            };
        }
        [Authorize]
        public ActionResult CertificadoPDF(TreinamentoInscricaoUsuario inscricao)
        {
            return View(inscricao);
        }

        //Método que salva os cliques nos treinamentos externos
        public ActionResult TreinamentoExterno(string dsURL, int idTreinamento)
        {
            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            var idIdioma = Convert.ToInt32(WebConfigurationManager.AppSettings["idIdioma"]);

            //Salva log do clique
            TreinamentoExterno log = new TreinamentoExterno { idUsuario = (usuario == null) ? 0 : usuario.idusuario, idTreinamento = idTreinamento, idIdioma = idIdioma };
            _treinamento.CadastraLogTreinamentoExterno(log);

            return Redirect(dsURL);
        }

        public ActionResult Indicados(int idTreinamento)
        {
            var usuario = new Cookie().cookieUsuario();

            if (usuario == null || usuario.idusuario == 0) return RedirectToAction("Index", "Home");

            ViewBag.SubTitulo = Resource.TreinamentoConvidadoSubTitulo.Replace("#NOME#", usuario.dsNomeCompleto);
            ViewBag.Indicados = _treinamento.ListaConvidados(usuario.idusuario, idTreinamento);

            //MENSAGEM
            ViewBag.mensagem = TempData["mensagem"] as string;
            ViewBag.mensagemTitulo = TempData["mensagemTitulo"] as string;

            return View();
        }


        public ActionResult IndicadosCertificado(int idTreinamento)
        {
            var usuario = new Cookie().cookieUsuario();

            if (usuario == null || usuario.idusuario == 0) return RedirectToAction("Index", "Home");
            
            ViewBag.SubTitulo = Resource.TreinamentoConvidadoSubTitulo.Replace("#NOME#", usuario.dsNomeCompleto);

            var indicados = _treinamento.ListaConvidadosCertificado(usuario.idusuario, idTreinamento);
            ViewBag.Indicados = indicados;
            ViewBag.Titulo = indicados[0].dsTreinamento;

            //MENSAGEM
            ViewBag.mensagem = TempData["mensagem"] as string;
            ViewBag.mensagemTitulo = TempData["mensagemTitulo"] as string;

            return View();
        }



        //[HttpPost]
        //public ActionResult CancelarInscricaoUsuarioConvidado(TreinamentoConvite treinamentoConvite)
        //{
        //    var cookie = new Cookie();
        //    var usuario = cookie.cookieUsuario();

        //    var retorno = _treinamento.CancelarInscricaoUsuarioConvidado(treinamentoConvite.idUsuarioConvidado, treinamentoConvite.idTreinamento);

        //    if (retorno.id > 0)
        //    {
        //        TempData["mensagem"] = Resource.TreinamentoConvidadoModalMensagem;
        //        TempData["mensagemTitulo"] = Resource.TreinamentoTituloErro;

        //        var listConvidados = _treinamento.ListaConvidados(usuario.idusuario, treinamentoConvite.idTreinamento);

        //        if (listConvidados != null && listConvidados.Count > 0)
        //            return RedirectToAction("Indicados", new { idTreinamento = treinamentoConvite.idTreinamento });
        //        else
        //            return RedirectToAction("MeusTreinamentos");
        //    }

        //    ViewBag.mensagem = retorno.dsMensagem;

        //    return RedirectToAction("Indicados", new { idTreinamento = treinamentoConvite.idTreinamento });
        //}



        [Authorize]
        [HttpPost]
        public JsonResult CancelarInscricaoUsuarioConvidado(int idTreinamento, int idUsuario)
        {

            var cookie = new Cookie();
            var usuario = cookie.cookieUsuario();
            RetornoPadrao retorno = new RetornoPadrao();

            var status = _treinamento.ListaTreinamentoInscricaoStatusUsuario(usuario.idusuario, idTreinamento);

            try { 
                
                retorno = _treinamento.CancelarInscricaoUsuarioConvidado(idUsuario, idTreinamento);

                if (retorno.id > 0)
                {

                    if (status == 2)
                    {
                        try{_treinamento.ConfirmaProximoListaEspera(retorno.idReferencia, 0);}
                        catch (Exception ex){var erro = ex.Message;}
                    }   


                    var listConvidados = _treinamento.ListaConvidados(usuario.idusuario, idTreinamento);
                    retorno.idReferencia = listConvidados.Count();               
                }
            }
            catch(Exception ex)
            {   
                retorno.id = -1;
                retorno.dsMensagem = ex.Message;
            }

            //ViewBag.mensagem = retorno.dsMensagem;

            return Json(retorno, JsonRequestBehavior.AllowGet);            
          
        }


        public string RetornaDia(string texto)
        {
            string retorno = string.Empty;
            if (Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["idIdioma"]) == 1)
            {
                switch (texto.ToLower())
                {
                    case "sunday":
                        retorno = "Domingo";
                        break;
                    case "monday":
                        retorno = "Segunda-feira";
                        break;
                    case "tuesday":
                        retorno = "Terça-feira";
                        break;
                    case "wednesday":
                        retorno = "Quarta-feira";
                        break;
                    case "thursday":
                        retorno = "Quinta-feira";
                        break;
                    case "friday":
                        retorno = "Sexta-feira";
                        break;
                    case "saturday":
                        retorno = "Sábado";
                        break;


                }
            }
            else if (Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["idIdioma"]) == 2)
            {
                switch (texto.ToLower())
                {
                    case "sunday":
                        retorno = "Sunday";
                        break;
                    case "monday":
                        retorno = "Monday";
                        break;
                    case "tuesday":
                        retorno = "Tuesday";
                        break;
                    case "wednesday":
                        retorno = "Wednesday";
                        break;
                    case "thursday":
                        retorno = "Thursday";
                        break;
                    case "friday":
                        retorno = "Friday";
                        break;
                    case "saturday":
                        retorno = "Saturday";
                        break;


                }
            }
            else
            {
                switch (texto.ToLower())
                {
                    case "sunday":
                        retorno = "Domingo";
                        break;
                    case "monday":
                        retorno = "Lunes";
                        break;
                    case "tuesday":
                        retorno = "Martes";
                        break;
                    case "wednesday":
                        retorno = "Miércoles";
                        break;
                    case "thursday":
                        retorno = "Jueves";
                        break;
                    case "friday":
                        retorno = "Viernes";
                        break;
                    case "saturday":
                        retorno = "Sábado";
                        break;


                }
            }

            return retorno;

        }


        [HttpPost]
        public JsonResult UploadArquivo(string pasta, string id)
        {
            var caminhoArquivo = WebConfigurationManager.AppSettings["DiretorioUploads"] + pasta + "\\";
            var r = new UploadFilesResult();           

            var nome = pasta + "_" + id.ToString();

            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (hpf != null && hpf.ContentLength == 0)
                        continue;

                    if (hpf == null) continue;
                    var extensao = hpf.FileName.Split('.')[1].ToUpper();

                    if ((hpf.ContentLength <= 3061200))
                    {
                        if (extensao == "JPG" || extensao == "PNG" || extensao == "PPT" || extensao == "PPTX" || extensao == "DOCX" || extensao == "PDF" || extensao == "DOC" || extensao == "XLSX" || extensao == "XLS" || pasta.ToLower() == "report")
                        {
                            r.nomeImagem = SalvaArquivo(caminhoArquivo, nome, hpf);
                        }
                        else
                        {
                            r.nomeImagem = "";
                        }
                    }

                    r.id = !string.IsNullOrEmpty(r.nomeImagem) ? 1 : 0;

                    r.Name = hpf.FileName;
                    r.Length = hpf.ContentLength;
                    r.Type = hpf.ContentType;
                    r.extensao = extensao;
                }

            }
            catch(Exception ex)
            {
                r.id = -1;
                r.extensao = ex.Message;
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        private string SalvaArquivo(string caminho, string nome, HttpPostedFileBase file)
        {
            var nomeImagem = string.Empty;


            if (file.ContentLength > 0)
            {
                var extensao = file.FileName.Split('.');

                nomeImagem = string.Format("{0}{1}{2}", nome, ".", extensao[1].ToUpper());
                //var strCaminho = Path.Combine(Server.MapPath(caminho), nomeImagem);
                var strCaminho = caminho + nomeImagem;
                file.SaveAs(strCaminho);
            }

            return nomeImagem;
        }


    }
}
