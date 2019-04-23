using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string dsNomeCompleto { get; set; }
        public string dsCPF { get; set; }
        public string dsCNPJ { get; set; }
        public string dsCodigoTreinamento { get; set; }
        public int idPais { get; set; }
        public string dsPais { get; set; }
        public string dsNomeFantasia { get; set; }
        public string dsUF { get; set; }
        public DateTime dtCadastro { get; set; }
        public int idIdioma { get; set; }
        public string dsStatus { get; set; }

        public string dsNacionalidade { get; set; }
        public string dsPassaporte { get; set; }
        public string dsTelefone { get; set; }
        public string dsCelular { get; set; }
        public string dsEmail { get; set; }
        public string dsTituloProfissional { get; set; }
        public string dsTituloProfissionalOutros { get; set; }
        public string dsNumConselho { get; set; }
        public string dsEspecialidade { get; set; }
        public bool btLicencaEUA { get; set; }
        public string dsNumEstado { get; set; }
        public string dsRazaoSocial { get; set; }
        public string dsCEP { get; set; }
        public string dsCidade { get; set; }
        public string dsBairro { get; set; }
        public string dsEndereco { get; set; }
        public string dsNumero { get; set; }
        public string dsComplemento { get; set; }
        public string dsRegiao { get; set; }
        public bool btNovaEmpresa { get; set; }
    }
}
