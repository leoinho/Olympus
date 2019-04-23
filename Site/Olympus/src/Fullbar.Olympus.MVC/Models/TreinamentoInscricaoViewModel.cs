using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Repositorio;
using Fullbar.Olympus.MVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Fullbar.Olympus.MVC.Models
{
    public class TreinamentoInscricaoViewModel
    {
        public int idUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int idTreinamento { get; set; }

        public int hospedagemPassagem { get; set; }

        public string Treinamento { get; set; }
        public string CNPJ { get; set; }
        public string Instituicao { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Celular { get; set; }

        public List<TreinamentoConvite> Convites { get; set; }

        public string ValidaCPF()
        {
            var usuario = new Cookie().cookieUsuario();

            if (string.IsNullOrEmpty(this.CPF))
            {
                return "CPF <br/>";
            }
            else if (!isCPF(this.CPF.Replace(".", "").Replace("-", "")))
            {
                return Resource.CPFInvalido + "<br/>";
            }
            else if (usuario.dsCPF == this.CPF.Replace(".", "").Replace("-", ""))
            {
                return Resource.TreinamentoCadastrarInscricaoCPFUsuario + "<br/>";

            }

            return string.Empty;
        }

        public string ValidaNome()
        {
            if (string.IsNullOrEmpty(this.Nome))
            {
                return Fullbar.Olympus.MVC.Resource.TreinamentoInscricaoNome + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaEmail()
        {
            if (String.IsNullOrEmpty(this.Email))
            {
                return Fullbar.Olympus.MVC.Resource.EmailCadastroObrigatorio + "<br/>";
            }
            else
            {
                bool isEmail = Regex.IsMatch(this.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                {
                    return Fullbar.Olympus.MVC.Resource.EmailInvalido + "<br/>";
                }
            }

            return string.Empty;
        }

        public string ValidaCelular()
        {
            if (string.IsNullOrEmpty(this.Celular))
            {
                return Fullbar.Olympus.MVC.Resource.TreinamentoInscricaoCelular + "<br/>";
            }

            return string.Empty;
        }

        public string ValidaMensagemRetorno(int id)
        {
            switch (id)
            {

                case -1:
                    return Resource.MensagemRetornoInscricaoEncerradas;
                    break;

                case -2:
                    return Resource.MensagemRetornoInscricaoNaoIniciadas;
                    break;

                case -3:
                    return Resource.MensagemRetornoInscricaoEfetuada;
                    break;

                default:
                    break;
            }

            return string.Empty;
        }

        public string VerificaUsuarioPossuiTreinamento()
        {
            var obj = new RepositorioTreinamento();

            if (string.IsNullOrEmpty(this.CPF))
            {
                return string.Empty;
            }
            else
            {
                var retorno = obj.VerificaInscricaoUsuario(this.CPF, this.idTreinamento);

                if (retorno.id > 0)
                    return Resource.PaginaTreinamentoInscricaoCadastro;
                return string.Empty;
            }
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


    }
}