using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fullbar.Olympus.MVC.Models
{
    public class UsuarioCadastroViewModel
    {

        public int nacionalidade { get; set; }
        public string NumeroPassaporte { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        
        public string EmailCadastro { get; set; }
        
        public string ConfirmaEmail { get; set; }
        public string TituloProfissional { get; set; }
        public string TituloProfissionalOutros { get; set; }
        public string NumeroConselho { get; set; }
        public string Especialidade { get; set; }
        public string CNPJ { get; set; }
        public string NomeInstituicao { get; set; }
        public int LicencaMedicaEUA { get; set; }

        public string NumeroEstado { get; set; }

        public string NumeroLicencaMedicaEUA { get; set; }
        public string Estado { get; set; }

        public bool InformacaoVerdadeira { get; set; }
        public bool Regulamento { get; set; }

        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmaSenha { get; set; }

        public string Cep { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string NomeEstrangeiro { get; set; }
        public string TelefoneEstrangeiro { get; set; }
        public string EmailCadastroEstrangeiro { get; set; }
        public string ConfirmaEmailEstrangeiro { get; set; }
        public string SenhaEstrangeiro { get; set; }
        public string ConfirmaSenhaEstrangeiro { get; set; }

        public string Pais { get; set; }
        public int idPais { get; set; }

        public string ValidaPais()
        {
            if (this.idPais == 0)
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Pais";
                return Fullbar.Olympus.MVC.Resource.CadastroPais + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaPassaporte()
        {
            var nacionalidade = this.nacionalidade;

            if (nacionalidade == 2 && string.IsNullOrEmpty(this.NumeroPassaporte))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Passaporte";
                return Fullbar.Olympus.MVC.Resource.Passaporte + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaCPF()
        {

            if (this.nacionalidade == 1 && string.IsNullOrEmpty(this.CPF))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#CPF";
                return "CPF<br/>";
            }
            else if (!isCPF(this.CPF.Replace(".", "").Replace("-", "")))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#CPF";
                return Resource.CPFInvalido + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaTituloProfissional()
        {

            if (String.IsNullOrEmpty(this.TituloProfissional) && String.IsNullOrEmpty(this.TituloProfissionalOutros))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#TituloProfissional";
                return Fullbar.Olympus.MVC.Resource.TituloProfissional + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaLicencaMedicaEUA()
        {
            if (this.LicencaMedicaEUA == 1 && string.IsNullOrEmpty(this.NumeroLicencaMedicaEUA))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#LicencaMedicaEUANumero";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroLicencaMedicaEUANumero + "<br/>";
            }



            return string.Empty;
        }

        public string ValidaLicencaMedicaEUAEstado()
        {
            if (this.LicencaMedicaEUA == 1 && string.IsNullOrEmpty(this.Estado))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#LicencaMedicaEUAEstado";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroLicencaMedicaEUAEstado + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaNome()
        {
            if (string.IsNullOrEmpty(this.Nome) || contemNumeros(this.Nome))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Nome";
                return Resource.PaginaCadastroNome + "<br/>";
            }

            return string.Empty;
        }        

        public string ValidaEmail()
        {
            if (String.IsNullOrEmpty(this.EmailCadastro))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#EmailCadastro";
                HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaEmail";
                return Fullbar.Olympus.MVC.Resource.EmailCadastroObrigatorio + "<br/>";
            }
            else
            {
                bool isEmail = Regex.IsMatch(this.EmailCadastro, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#EmailCadastro";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaEmail";
                    return Fullbar.Olympus.MVC.Resource.EmailInvalido + "<br/>";
                }
                else if (this.EmailCadastro != this.ConfirmaEmail)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#EmailCadastro";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaEmail";
                    return Resource.PaginaCadastroEmailConfirmaEmail + "<br/>";
                }

            }

            return string.Empty;
        }

        public string ValidaCNPJ()
        {
            if (string.IsNullOrEmpty(this.CNPJ))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#CNPJ";
                return Fullbar.Olympus.MVC.Resource.CnpjCadastroObrigatorio + "<br/>";
            }
            else if(!(Fullbar.Olympus.Dominio.Util.Geral.isCNPJ(this.CNPJ)))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#CNPJ";
                return Fullbar.Olympus.MVC.Resource.CNPJInvalido + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaNomeInstituicao()
        {
            if (string.IsNullOrEmpty(this.NomeInstituicao))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#NomeInstituicao";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroNomeInstituicao + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaInformacaoVerdadeira()
        {
            if (!this.InformacaoVerdadeira)
            {
                HttpContext.Current.Session["ErroCadastro"] += "#InformacaoVerdadeira";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroInformacaoVerdadeira + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaRegulamento()
        {
            if (!this.Regulamento)
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Regulamento";
                return Fullbar.Olympus.MVC.Resource.RegulamentoCadastroObrigatorio + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaSenha()
        {
            if (string.IsNullOrEmpty(this.Senha))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Senha";
                HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaSenha";
                return Fullbar.Olympus.MVC.Resource.SenhaCadastroObrigatorio + "<br/>";
            }
            else
            {

                if (this.Senha.Length < 6)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#Senha";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaSenha";
                    return Fullbar.Olympus.MVC.Resource.ValorMinimoSenhaCadastroObrigatorio + "<br/>";
                }
                else if (this.Senha != this.ConfirmaSenha)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#Senha";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaSenha";
                    return Fullbar.Olympus.MVC.Resource.ConfirmaCadastroObrigatorio + "<br/>";
                }

            }

            return string.Empty;
        }

        public string ValidaCep()
        {
            if (string.IsNullOrEmpty(this.Cep))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Cep";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroCepInstituicao + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaUF()
        {
            if (string.IsNullOrEmpty(this.UF) || contemNumeros(this.UF))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#UF";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroUF + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaCidade()
        {
            if (string.IsNullOrEmpty(this.Cidade) || contemNumeros(this.Cidade))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Cidade";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroCidade + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaBairro()
        {
            if (string.IsNullOrEmpty(this.Bairro))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Bairro";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroBairro + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaEndereco()
        {
            if (string.IsNullOrEmpty(this.Endereco))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Endereco";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroEndereco + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaNumero()
        {
            if (string.IsNullOrEmpty(this.Numero))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Numero";
                return Fullbar.Olympus.MVC.Resource.Numero + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaMensagemRetorno(int id)
        {
            switch (id)
            {

                case -1:
                    return Resource.MensagemRetornoCPF;
                    break;

                case -2:
                    return Resource.MensagemRetornoPassaporte;
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

                default:
                    break;
            }

            return string.Empty;
        }

        public string ValidaTelefone()
        {
            var _fone = this.Telefone;
            
            if(_fone != null)
                _fone = _fone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ","");

            if (string.IsNullOrEmpty(this.Telefone) || _fone.Length < 10 || _fone.Substring(0, 1) == "0")
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Telefone";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroTelefone + "<br/>";
            }
            return string.Empty;
        }

        public string ValidaCelular()
        {
            var _celular = this.Celular;
            
            if(_celular != null)
                _celular = _celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (string.IsNullOrEmpty(this.Celular) || _celular.Length < 11 || _celular.Substring(0, 1) == "0")
            {
                HttpContext.Current.Session["ErroCadastro"] += "#Celular";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroTelefoneCelular + "<br/>";
            }
            return string.Empty;
        }

        public static bool isCPF(string CPF)
        {
            if (CPF.Trim().Length == 0)
                return false;

            int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string TempCPF;
            string Digito;
            int soma;
            int resto;

            CPF = CPF.Trim();
            CPF = CPF.Replace(".", "").Replace("-", "");

            if (CPF.Length != 11)
                return false;

            if (CPF == "11111111111" || CPF == "22222222222" || CPF == "33333333333" || CPF == "44444444444" || CPF == "55555555555" || CPF == "66666666666" || CPF == "77777777777" || CPF == "88888888888" || CPF == "99999999999")
                return false;

            TempCPF = CPF.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = resto.ToString();
            TempCPF = TempCPF + Digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = Digito + resto.ToString();

            return CPF.EndsWith(Digito);
        }


        public string ValidaNomeEstrangeiro()
        {
            if (string.IsNullOrEmpty(this.NomeEstrangeiro))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#NomeEstrangeiro";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroNome + "<br/>";
            }

            return string.Empty;
        }
        public string ValidaTelefoneEstrangeiro()
        {
            if (string.IsNullOrEmpty(this.TelefoneEstrangeiro))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#TelefoneEstrangeiro";
                return Fullbar.Olympus.MVC.Resource.PaginaCadastroTelefone + "<br/>";
            }
            return string.Empty;
        }
        public string ValidaEmailEstrangeiro()
        {
            if (String.IsNullOrEmpty(this.EmailCadastroEstrangeiro))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#EmailCadastroEstrangeiro";
                HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaEmailEstrangeiro";
                return Fullbar.Olympus.MVC.Resource.EmailCadastroObrigatorio + "<br/>";
            }
            else
            {
                bool isEmail = Regex.IsMatch(this.EmailCadastroEstrangeiro, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#EmailCadastroEstrangeiro";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaEmailEstrangeiro";
                    return Fullbar.Olympus.MVC.Resource.EmailInvalido + "<br/>";
                }
                else if (this.EmailCadastroEstrangeiro != this.ConfirmaEmailEstrangeiro)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#EmailCadastroEstrangeiro";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaEmailEstrangeiro";
                    return Resource.PaginaCadastroEmailConfirmaEmail + "<br/>";
                }

            }

            return string.Empty;
        }
        public string ValidaSenhaEstrangeiro()
        {
            if (string.IsNullOrEmpty(this.SenhaEstrangeiro))
            {
                HttpContext.Current.Session["ErroCadastro"] += "#SenhaEstrangeiro";
                HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaSenhaEstrangeiro";
                return Fullbar.Olympus.MVC.Resource.SenhaCadastroObrigatorio + "<br/>";
            }
            else
            {

                if (this.SenhaEstrangeiro.Length < 6)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#SenhaEstrangeiro";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaSenhaEstrangeiro";
                    return Fullbar.Olympus.MVC.Resource.ValorMinimoSenhaCadastroObrigatorio + "<br/>";
                }
                else if (this.SenhaEstrangeiro != this.ConfirmaSenhaEstrangeiro)
                {
                    HttpContext.Current.Session["ErroCadastro"] += "#SenhaEstrangeiro";
                    HttpContext.Current.Session["ErroCadastro"] += "#ConfirmaSenhaEstrangeiro";
                    return Fullbar.Olympus.MVC.Resource.ConfirmaCadastroObrigatorio + "<br/>";
                }

            }

            return string.Empty;
        }



        public bool contemNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }

    }
}