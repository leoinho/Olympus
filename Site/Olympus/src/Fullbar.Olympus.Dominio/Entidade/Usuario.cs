using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
   public  class Usuario
    {
        public int idusuario { get; set; }
        public int idNacionalidade { get; set; }
        public string dsNacionalidade { get; set; }
        public string dsCPF { get; set; }
        public string Passaporte { get; set; }
        public string dsNomeCompleto { get; set; }
        public string dsTelefone { get; set; }
        public string dsCelular { get; set; }
        public string dsEmail { get; set; }
        public string dsTituloProfissional { get; set; }
        public string dsTituloProfissionalOutros { get; set; }
        public string dsNumConselho { get; set; }
        public string dsEspecialidade { get; set; }
        public string dsCNPJ { get; set; }
        public bool btLicencaEUA  { get; set; }
        public string dsNumEstado { get; set; }
        public RetornoCNPJ RetornoCNPJ { get; set; }
        public string dsSenha { get; set; }
   
   }
}
