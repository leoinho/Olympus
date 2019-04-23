﻿using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Interface
{
   public interface IFaleConosco :IDisposable
    {
       RetornoPadrao Cadastrar(FaleConosco dados);
       List<Pais> RetornaPais();
    }
}