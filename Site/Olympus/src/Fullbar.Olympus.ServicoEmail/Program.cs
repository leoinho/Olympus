using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.Dominio.Repositorio;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.ServicoEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var kernel = new StandardKernel();

                kernel.Bind<IEmail>().To<RepositorioEmail>();
                var appEmail = kernel.Get<IEmail>();

                appEmail.Enviar();
            }
            catch (Exception ex)
            {
                
                throw ex.InnerException;
            }
           
        }
    }
}
