using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Interface
{
    public interface IConteudoEducacional : IDisposable
    {
        List<ConteudoEducacional> ListaConteudoEducacional();
    }
}
