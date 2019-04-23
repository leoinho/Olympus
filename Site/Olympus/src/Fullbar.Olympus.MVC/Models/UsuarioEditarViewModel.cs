using Fullbar.Olympus.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fullbar.Olympus.MVC.Models
{
    public class UsuarioEditarViewModel
    {
        public int idUsuario { get; set; }

        public int idNacionalidade { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
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
        public string Numero { get; set; }
        public string Complemento { get; set; }
        

        public int LicencaMedicaEUA { get; set; }
        public string NumeroEstado { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha { get; set; }

        public string ConfirmaEmail { get; set; }

        public string EmailCadastroEstrangeiro { get; set; }
        public string ConfirmaEmailEstrangeiro { get; set; }

        public string SenhaEstrangeiro { get; set; }
        public string ConfirmaSenhaEstrangeiro { get; set; }
        public string NomeEstrangeiro { get; set; }
        public string TelefoneEstrangeiro { get; set; }

        public string NumeroLicencaMedicaEUA { get; set; }
        public string Estado { get; set; }

        public string ValidaTituloProfissional()
        {

            if (String.IsNullOrEmpty(this.TituloProfissional) && String.IsNullOrEmpty(this.TituloProfissionalOutros))
            {
                return Fullbar.Olympus.MVC.Resource.TituloProfissional + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaLicencaMedicaEUA()
        {
            if (this.LicencaMedicaEUA == 1 && string.IsNullOrEmpty(this.NumeroLicencaMedicaEUA))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroLicencaMedicaEUANumero + "<br/>";
            }



            return string.Empty;
        }

        public string ValidaLicencaMedicaEUAEstado()
        {
            if (this.LicencaMedicaEUA == 1 && string.IsNullOrEmpty(this.Estado))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroLicencaMedicaEUAEstado + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaNome()
        {
            if (string.IsNullOrEmpty(this.Nome))
            {
                return Resource.PaginaCadastroNome + "\n";
            }

            return string.Empty;
        }

        public string ValidaCNPJ()
        {
            if (string.IsNullOrEmpty(this.CNPJ))
            {
                return Fullbar.Olympus.MVC.Resource.CnpjCadastroObrigatorio + "\n";
            }

            return string.Empty;
        }

        public string ValidaNomeInstituicao()
        {
            if (string.IsNullOrEmpty(this.NomeInstituicao))
            {
                return Fullbar.Olympus.MVC.Resource.InstituicaoCadastroObrigatorio + "\n";
            }

            return string.Empty;
        }

        public string ValidaSenha()
        {
            if (string.IsNullOrEmpty(this.Senha))
            {
                return Fullbar.Olympus.MVC.Resource.SenhaCadastroObrigatorio + "\n";
            }
            else
            {

                if (this.Senha.Length < 6)
                {
                    return Fullbar.Olympus.MVC.Resource.ValorMinimoSenhaCadastroObrigatorio + "\n";
                }
                else if (this.Senha != this.ConfirmaSenha)
                {
                    return Fullbar.Olympus.MVC.Resource.ConfirmaCadastroObrigatorio + "\n";
                }

            }

            return string.Empty;
        }

        public string ValidaCep()
        {
            if (string.IsNullOrEmpty(this.Cep))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroCepInstituicao + "\n";
            }
            return string.Empty;
        }

        public string ValidaUF()
        {
            if (string.IsNullOrEmpty(this.UF))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroUF + "\n";
            }
            return string.Empty;
        }

        public string ValidaCidade()
        {
            if (string.IsNullOrEmpty(this.Cidade))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroCidade + "\n";
            }
            return string.Empty;
        }

        public string ValidaBairro()
        {
            if (string.IsNullOrEmpty(this.Bairro))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroBairro + "\n";
            }
            return string.Empty;
        }

        public string ValidaEndereco()
        {
            if (string.IsNullOrEmpty(this.Endereco))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroEndereco + "\n";
            }
            return string.Empty;
        }

        public string ValidaNumero()
        {
            if (string.IsNullOrEmpty(this.Numero))
            {
                return Fullbar.Olympus.MVC.Resource.Numero + "\n";
            }
            return string.Empty;
        }

        public string ValidaMensagemRetorno(int id)
        {
            switch (id)
            {

                case -1:
                    return Resource.MensagemRetornoCNPJ;
                    break;

                case -2:
                    return Resource.MensagemRetornoGenerico;
                    break;

                case -3:
                    return Resource.MensagemRetornoEmail;
                    break;

                case -4:
                    return Resource.MensagemRetornoCNPJ;
                    break;

                case -5:
                    return Resource.MensagemRetornoGenerico;
                    break;

            }

            return string.Empty;
        }

        public string ValidaTelefone()
        {
            if (string.IsNullOrEmpty(this.Telefone))
            {
                return Fullbar.Olympus.MVC.Resource.Telefone + "\n";
            }
            return string.Empty;
        }

        public string ValidaCelular()
        {
            if (string.IsNullOrEmpty(this.Celular))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroTelefoneCelular + "\n";
            }
            return string.Empty;
        }

        public string ValidaEmail()
        {
            if (String.IsNullOrEmpty(this.Email))
            {
                return Fullbar.Olympus.MVC.Resource.EmailCadastroObrigatorio + "\n";
            }
            else
            {
                bool isEmail = Regex.IsMatch(this.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                {
                    return Fullbar.Olympus.MVC.Resource.EmailInvalido + "\n";
                }
                else if (this.Email != this.ConfirmaEmail)
                {
                    return Resource.PaginaCadastroEmailConfirmaEmail + "\n";
                }

            }

            return string.Empty;
        }

        public string ValidaNomeEstrangeiro()
        {
            if (string.IsNullOrEmpty(this.NomeEstrangeiro))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroNome + "\n";
            }

            return string.Empty;
        }
        public string ValidaTelefoneEstrangeiro()
        {
            if (string.IsNullOrEmpty(this.TelefoneEstrangeiro))
            {
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroTelefone + "\n";
            }
            return string.Empty;
        }
        public string ValidaEmailEstrangeiro()
        {
            if (String.IsNullOrEmpty(this.EmailCadastroEstrangeiro))
            {
                return Fullbar.Olympus.MVC.Resource.EmailCadastroObrigatorio + "\n";
            }
            else
            {
                bool isEmail = Regex.IsMatch(this.EmailCadastroEstrangeiro, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                {
                    return Fullbar.Olympus.MVC.Resource.EmailInvalido + "\n";
                }
                else if (this.EmailCadastroEstrangeiro != this.ConfirmaEmailEstrangeiro)
                {
                    return Resource.PaginaCadastroEmailConfirmaEmail + "\n";
                }

            }

            return string.Empty;
        }
        public string ValidaSenhaEstrangeiro()
        {
            if (string.IsNullOrEmpty(this.SenhaEstrangeiro))
            {
                return Fullbar.Olympus.MVC.Resource.SenhaCadastroObrigatorio + "\n";
            }
            else
            {

                if (this.SenhaEstrangeiro.Length < 6)
                {
                    return Fullbar.Olympus.MVC.Resource.ValorMinimoSenhaCadastroObrigatorio + "\n";
                }
                else if (this.SenhaEstrangeiro != this.ConfirmaSenhaEstrangeiro)
                {
                    return Fullbar.Olympus.MVC.Resource.ConfirmaCadastroObrigatorio + "\n";
                }

            }

            return string.Empty;
        }

       
    }
}