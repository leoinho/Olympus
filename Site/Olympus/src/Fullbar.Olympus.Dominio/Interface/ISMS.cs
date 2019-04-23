using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Interface
{
    public interface ISMS :IDisposable
    {
        List<SMS>Lista();
        List<SMS> ValidacaoLista();
        RetornoPadrao AtualizaStatus(int idAgendamento, int idStatus , string xml, string dsCodigo);
        RetornoPadrao AtualizaStatusValidacao(int idLog, int idStatus);
    }
}
