using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Admin.Olympus.Dominio.Util
{
    public static class Geral
    {
        public static bool ValidaData(string data)
        {
            try
            {
                var _data = Convert.ToDateTime(data);

                if (_data > Convert.ToDateTime("1/1/1753"))
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        public static bool ValidaIdade(string data)
        {
            DateTime DataNascimento = Convert.ToDateTime(data);

            int AnoBase = DateTime.Today.Year - 18;

            if (DataNascimento.Year < AnoBase)
            {
                return true;
            }

            if (AnoBase == DataNascimento.Year)
            {
                if (DataNascimento.Month < DateTime.Now.Month)
                {
                    return true;
                }

                if (DataNascimento.Month == DateTime.Now.Month)
                {
                    if (DataNascimento.Day <= DateTime.Now.Day)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool ValidaCPF(string value)
        {
            if (value != null)
            {
                string valor = value.ToString().Replace(".", "").Replace("-", "");

                if (valor.Length != 11)
                    return false;

                bool igual = true;

                for (int i = 1; i < 11 && igual; i++)
                    if (valor[i] != valor[0])
                        igual = false;

                if (igual || valor == "12345678909")
                    return false;

                int[] numeros = new int[11];

                for (int i = 0; i < 11; i++)
                    numeros[i] = int.Parse(valor[i].ToString());

                int soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += (10 - i) * numeros[i];

                int resultado = soma % 11;

                if (resultado == 1 || resultado == 0)
                {
                    if (numeros[9] != 0)
                        return false;
                }
                else if (numeros[9] != 11 - resultado)
                    return false;

                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += (11 - i) * numeros[i];

                resultado = soma % 11;

                if (resultado == 1 || resultado == 0)
                {
                    if (numeros[10] != 0)
                        return false;
                }
                else if (numeros[10] != 11 - resultado)
                    return false;

                return true;
            }
            else
            {
                return true;
            }
        }

        public static bool ValidaCNPJ(string value)
        {
            if (value != null)
            {
                string CNPJ = value.ToString().Replace(".", "").Replace("/", "").Replace("-", "");

                int[] digitos, soma, resultado;
                int nrDig;
                string ftmt;
                bool[] CNPJOk;

                ftmt = "6543298765432";
                digitos = new int[14];
                soma = new int[2];
                soma[0] = 0;
                soma[1] = 0;
                resultado = new int[2];
                resultado[0] = 0;
                resultado[1] = 0;
                CNPJOk = new bool[2];
                CNPJOk[0] = false;
                CNPJOk[1] = false;

                try
                {
                    for (nrDig = 0; nrDig < 14; nrDig++)
                    {
                        digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));

                        if (nrDig <= 11)
                            soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));

                        if (nrDig <= 12)
                            soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                    }

                    for (nrDig = 0; nrDig < 2; nrDig++)
                    {
                        resultado[nrDig] = (soma[nrDig] % 11);

                        if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                            CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                        else
                            CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                    }

                    return (CNPJOk[0] && CNPJOk[1]);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool ValidaEmail(string email)
        {
            Regex rg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (rg.IsMatch(email))
                return true;
            else
                return false;
        }

        /* RESOURCE */
        public static string RetornaTexto(string resourceName)
        {
            try
            {
                //Arquivo de Resource
                string resource = (string)HttpContext.Current.Session["ArquivoResource"];

                //Idioma
                string idioma = (string)HttpContext.Current.Session["CultureIdioma"];
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(idioma);

                return HttpContext.GetGlobalResourceObject(resource, resourceName, ci).ToString();
            }
            catch
            {
                return resourceName;
            }
        } 
    }
}
