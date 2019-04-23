using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullbar.Olympus.Dominio.Repositorio
{
    public class RepositorioSMS : ISMS
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public List<SMS> Lista()
        {
            try
            {
                var idStatus = 1;

                return _db.Database.SqlQuery<SMS>("SP_SMSAgendamentoLista @idStatus", new SqlParameter("idStatus", idStatus)).ToList();

            }
            catch (Exception ex)
            {

            }

            return null;
        }



        public List<SMS> ValidacaoLista()
        {
            try
            {
                return _db.Database.SqlQuery<SMS>("SP_SMSValidacaoLista").ToList();

            }
            catch (Exception ex)
            {

            }

            return null;
        }



        //public void AtualizaStatusEnvio()
        //{
        //    var lista = _db.Database.SqlQuery<LogSMS>("SP_UsuarioConfirmaAcessoSMSLogLista").ToList();

        //    foreach (var item in lista)
        //    {
        //        var idStatus = 0;
        //        var statusEnvio = new StatusEnvioSMS();

        //        statusEnvio = SMS.VerificaEnvio(item.Codigo);
        //        idStatus = statusEnvio.Status.Replace(" ", "") == StatusEnvioSMS.StatusEnvio.Mensagemenviada.ToString().ToUpper() ? 2 : (statusEnvio.Status.Replace(" ", "") == StatusEnvioSMS.StatusEnvio.AguardandoRetornodaoperadora.ToString().ToUpper() ? 1 : 3);

        //        _db.Database.SqlQuery<RetornoPadrao>("SP_UsuarioConfirmaAcessoSMSLogAtualiza @idLog,@idStatus,@dsRetorno",

        //                                                                                     new SqlParameter("idLog", item.IdLog)
        //                                                                                    , new SqlParameter("idStatus", idStatus)
        //                                                                                    , new SqlParameter("dsRetorno", statusEnvio.Status)).FirstOrDefault();

        //    }


        //}



        public RetornoPadrao AtualizaStatus(int idAgendamento, int idStatus, string xml, string dsCodigo)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_SMSAgendamentoAlteraStatus @idAgendamento, @idStatus , @dsRetorno, @dsCodigo", 
                                                new SqlParameter("idAgendamento", idAgendamento), 
                                                new SqlParameter("idStatus", idStatus), 
                                                new SqlParameter("dsRetorno", xml),
                                                new SqlParameter("dsCodigo", dsCodigo)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return null;
        }


        public RetornoPadrao AtualizaStatusValidacao(int idLog, int idStatus)
        {
            try
            {
                return _db.Database.SqlQuery<RetornoPadrao>("SP_SMSAgendamentoAlteraStatusValidacao @idLog, @idStatus",
                                                new SqlParameter("idLog", idLog),
                                                new SqlParameter("idStatus", idStatus)).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return null;
        }


        public void Dispose()
        {
            _db.Dispose();
        }



    }
}
