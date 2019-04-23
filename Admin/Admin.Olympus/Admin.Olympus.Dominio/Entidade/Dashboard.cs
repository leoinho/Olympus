using System;

namespace Admin.Olympus.Dominio.Entidade
{
    public class Dashboard
    {
        public int inTipo { get; set; }
        public string dsTipo { get; set; }
        public int inQtd { get; set; }
        public string dsSite { get; set; }
        public string dsDia { get; set; }
        public string dsData { get; set; }
        public string dsCodigoTreinamento { get; set; }

        public int inMes { get; set; }
        public string dsMes { get; set; }
    }
}