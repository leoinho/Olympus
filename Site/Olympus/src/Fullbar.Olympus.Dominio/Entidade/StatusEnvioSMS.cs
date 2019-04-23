using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Entidade
{
    public class StatusEnvioSMS
    {
        public string Telefone { get; set; }
        public string Status { get; set; }

        public StatusEnvioSMS()
        {
            Telefone = String.Empty;
            Status = String.Empty;
        }

        public enum StatusEnvio
        {
            Pendente = 1,
            Enviado,
            ErroAoEnviar,
            Validando
        }
    }
}
