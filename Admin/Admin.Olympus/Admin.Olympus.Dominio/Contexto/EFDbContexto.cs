using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Admin.Olympus.Dominio.Contexto
{
    public class EFDbContexto: DbContext
    {
        public EFDbContexto()
            : base("EFDbContexto")
        {

        }
    }
}
