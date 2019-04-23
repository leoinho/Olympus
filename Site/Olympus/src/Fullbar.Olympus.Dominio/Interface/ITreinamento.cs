using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Interface
{
    public interface ITreinamento : IDisposable
    {

        List<TreinamentoHome> ListaTreinamentoPais(int? idIdioma);

        List<TreinamentoHome> ListaHome(int? idDivisao, int? idCategoria, int? idIdioma, string dsPais, int? idUsuario);
        RetornoPadrao Cadastrar(TreinamentoInscricao inscricao);
        RetornoPadrao CadastrarConvite(TreinamentoInscricao inscricao, RetornoPadrao retornoTreinamento);
        TreinamentoDetalhe ListaPorId(int id, int idUsuario);
        TreinamentoInscricaoResultado ListaResultado(int idTreinamento, int idEmpresa, int idUsuario, int idInscricaoUsuarioConvidante);
        RetornoPadrao CadastrarEstrangeiro(TreinamentoInscricaoEstrangeiro inscricao);
        List<TreinamentoUsuario> ListaPorUsuario(int idUsuario);
        TreinamentoInscricaoUsuario ListaInscricaoPorUsuario(Usuario usuario, int idTreinamento);
        RetornoPadrao CancelarInscricao(int idUsuario, int idTreinamento);
        RetornoPadrao CancelarInscricaoUsuarioConvidado(int idUsuario, int idTreinamento);

        List<TreinamentoPrePergunta> ListaPerguntas(int idTreinamento, int idTipo);
        RetornoPadrao CadastraRespostasQuestionario(TreinamentoPrePergunta treinamentoPrePergunta);
        RetornoPadrao AlternativaCadastro(List<TreinamentoAlternativaCadastro> alternativas);
        List<TreinamentoCalendario> TreinamentoCalendario(int mes, int ano, int idUsuario);

        bool QuestionarioRespondido(int idTreinamento, int idUsuario, int inQuestionario);

        int ListaTreinamentoInscricaoUsuario(int idUsuario, int idTreinamento);
        int ListaTreinamentoInscricaoStatusUsuario(int idUsuario, int idTreinamento);

        bool CadastraLogTreinamentoExterno(TreinamentoExterno treinamentoExterno);


        List<TreinamentoConvite> ListaConvidados(int idUsuario, int idTreinamento);
        List<TreinamentoConvite> ListaConvidadosCertificado(int idUsuario, int idTreinamento);

        RetornoPadrao ConfirmaProximoListaEspera(int idInscricao, int idAdministrador);
    }
}
