using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Util
{
    public static class Data
    {
        public static bool ValidaData(string data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    Convert.ToDateTime(data);
                    return true;
                }
                else
                {
                    return true;
                }
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
    }
}