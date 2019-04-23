using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Interface
{
    public interface ICategoria : IDisposable
    {
        List<Categoria> Lista (int idDivisao);
    }
}
