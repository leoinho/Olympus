﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class UsuarioEditar
    {
        public int idUsuario { get; set; }

        public int idNacionalidade { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string TituloProfissional { get; set; }
        public string TituloProfissionalOutros { get; set; }
        public string NumeroConselho { get; set; }
        public string Especialidade { get; set; }
        public string CNPJ { get; set; }
       
        public string NomeInstituicao { get; set; }
        public string Cep { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }

        public int LicencaMedicaEUA { get; set; }
        public string NumeroEstado { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }


    }
}