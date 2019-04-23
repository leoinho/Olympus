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
    public class RepositorioEmail : IEmail
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public RetornoPadrao Enviar()
        {
            try
            {
                var idStatus = 1;
                var idStatusEnvio = 2;

                var listaEmail = _db.Database.SqlQuery<Email>("SP_EmailAgendamentoLista @idStatus", new SqlParameter("idStatus", idStatus)).ToList();

                //CONFIRMAÇÂO DE INSCRIÇÃO
                foreach (var item in listaEmail.Where(x => x.dsTipo.Trim() == "CI").Where(x => x.btCadastrado == true).ToList())
                {
                    var email = new Email()
                    {
                        dsArquivoHTML = item.dsArquivoHTML,
                        dsAssunto = item.dsAssunto,
                        dsBairro = item.dsBairro,
                        dsCEP = item.dsCEP,
                        dsCidade = item.dsCidade,
                        dsCNPJ = item.dsCNPJ,
                        dsCPF = item.dsCPF,
                        dsEmail = item.dsEmail,
                        dsEndereco = item.dsEndereco,
                        dsNomeCompleto = item.dsNomeCompleto,
                        dsNumero = item.dsNumero,
                        dsPais = item.dsPais,
                        dsTelefone = item.dsTelefone,
                        dsTipo = item.dsTipo,
                        dsUF = item.dsUF,
                        dtTreinamento = item.dtTreinamento,
                        dsDataTreinamento = item.dtTreinamento.ToString("dd/MM/yy HH:mm"),
                        idIdioma = item.idIdioma,
                        idUsuario = item.idUsuario,
                        idInscricaoUsuarioConvidante = item.idInscricaoUsuarioConvidante,
                        idTreinamento = item.idTreinamento,
                        dsNomeTreinamento = item.dsNomeTreinamento,
                        btCadastrado = item.btCadastrado,
                        btTemConvidado = item.btTemConvidado,
                        dsLocalTreinamento = item.dsLocalTreinamento,
                        dsLatitude = item.dsLatitude,
                        dsLongitude = item.dsLongitude
                    };

                    List<Email> listaConvidados = new List<Email>();

                    //PEGA A LISTA DE CONVIDADOS CONVIDADOS
                    if (email.btTemConvidado)
                    {
                        listaConvidados = _db.Database.SqlQuery<Email>("SP_TreinamentoConviteLista @idInscricaoUsuarioConvidante, @idUsuarioConvidante, @idTreinamento",
                            new SqlParameter("idInscricaoUsuarioConvidante", email.idInscricaoUsuarioConvidante),
                            new SqlParameter("idUsuarioConvidante", email.idUsuario),
                            new SqlParameter("idTreinamento", email.idTreinamento)).ToList();
                    }

                    try
                    {
                        var isEmail = new Util.Email().EnviarEmailConfirmacaoInscricao(email, listaConvidados);
                        idStatusEnvio = (isEmail) ? 2 : 3;
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }


                    try
                    {
                        var retornoAtualizaStatus = _db.Database.SqlQuery<RetornoPadrao>("SP_EmailAgendamentoAlteraStatus @idAgendamento, @idStatus", new SqlParameter("idAgendamento", item.idAgendamento), new SqlParameter("idStatus", idStatusEnvio)).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

                //CONFIRMAÇÂO DE INSCRIÇÃO PARA CONVIDADOS
                foreach (var item in listaEmail.Where(x => x.dsTipo.Trim() == "CI").Where(x => x.btCadastrado == false).ToList())
                {
                    var email = new Email()
                    {
                        dsArquivoHTML = item.dsArquivoHTML,
                        dsAssunto = item.dsAssunto,
                        dsBairro = item.dsBairro,
                        dsCEP = item.dsCEP,
                        dsCidade = item.dsCidade,
                        dsCNPJ = item.dsCNPJ,
                        dsCPF = item.dsCPF,
                        dsEmail = item.dsEmail,
                        dsEndereco = item.dsEndereco,
                        dsNomeCompleto = item.dsNomeCompleto,
                        dsNomeConvidante = item.dsNomeConvidante,
                        dsNumero = item.dsNumero,
                        dsPais = item.dsPais,
                        dsTelefone = item.dsTelefone,
                        dsTipo = item.dsTipo,
                        dsUF = item.dsUF,
                        dtTreinamento = item.dtTreinamento,
                        dsDataTreinamento = item.dtTreinamento.ToString("dd/MM/yy HH:mm"),
                        idIdioma = item.idIdioma,
                        idUsuario = item.idUsuario,
                        idTreinamento = item.idTreinamento,
                        dsNomeTreinamento = item.dsNomeTreinamento,
                        btCadastrado = item.btCadastrado,
                        btTemConvidado = item.btTemConvidado,
                        dsLocalTreinamento = item.dsLocalTreinamento,
                        dsLatitude = item.dsLatitude,
                        dsLongitude = item.dsLongitude
                    };

                    try
                    {
                        var isEmail = new Util.Email().EnviarEmailConfirmacaoInscricaoConvidado(email);
                        idStatusEnvio = (isEmail) ? 2 : 3;
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }


                    try
                    {
                        var retornoAtualizaStatus = _db.Database.SqlQuery<RetornoPadrao>("SP_EmailAgendamentoAlteraStatus @idAgendamento, @idStatus", new SqlParameter("idAgendamento", item.idAgendamento), new SqlParameter("idStatus", idStatusEnvio)).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }


                foreach (var item in listaEmail.Where(e => e.dsTipo.Trim() == "LE" || e.dsTipo.Trim() == "LEC").ToList())
                {
                    var email = new Email()
                    {
                        dsArquivoHTML = item.dsArquivoHTML
                        ,
                        dsAssunto = item.dsAssunto
                        ,
                        dsBairro = item.dsBairro
                        ,
                        dsCEP = item.dsCEP
                        ,
                        dsCidade = item.dsCidade
                        ,
                        dsCNPJ = item.dsCNPJ
                        ,
                        dsCPF = item.dsCPF
                        ,
                        dsEmail = item.dsEmail
                        ,
                        dsEndereco = item.dsEndereco
                        ,
                        dsNomeCompleto = item.dsNomeCompleto
                        ,
                        dsNumero = item.dsNumero
                        ,
                        dsPais = item.dsPais
                        ,
                        dsTelefone = item.dsTelefone
                        ,
                        dsTipo = item.dsTipo
                        ,
                        dsUF = item.dsUF
                        ,
                        dsDataTreinamento = item.dtTreinamento.ToString("dd/MM/yy HH:mm")
                        ,
                        idIdioma = item.idIdioma
                        ,
                        btCadastrado = item.btCadastrado

                    };


                    var isEmail = new Util.Email().EnviarEmailListaEspera(email);
                    idStatusEnvio = (isEmail) ? 2 : 3;

                    try
                    {
                        var retornoAtualizaStatus = _db.Database.SqlQuery<RetornoPadrao>("SP_EmailAgendamentoAlteraStatus @idAgendamento, @idStatus", new SqlParameter("idAgendamento", item.idAgendamento), new SqlParameter("idStatus", idStatusEnvio)).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }


                foreach (var item in listaEmail.Where(e => e.dsTipo.Trim() == "CN" || e.dsTipo.Trim() == "CNT").ToList())
                {
                    var email = new Email()
                    {
                        dsArquivoHTML = item.dsArquivoHTML
                        ,
                        dsAssunto = item.dsAssunto
                        ,
                        dsBairro = item.dsBairro
                        ,
                        dsCEP = item.dsCEP
                        ,
                        dsCidade = item.dsCidade
                        ,
                        dsCNPJ = item.dsCNPJ
                        ,
                        dsCPF = item.dsCPF
                        ,
                        dsEmail = item.dsEmail
                        ,
                        dsEndereco = item.dsEndereco
                        ,
                        dsNomeCompleto = item.dsNomeCompleto
                        ,
                        dsNumero = item.dsNumero
                        ,
                        dsPais = item.dsPais
                        ,
                        dsTelefone = item.dsTelefone
                        ,
                        dsTipo = item.dsTipo
                        ,
                        dsUF = item.dsUF
                        ,
                        dsNomeTreinamento = item.dsNomeTreinamento
                        ,
                        dsDataTreinamento = item.dtTreinamento.ToString("dd/MM/yy")
                        ,
                        idIdioma = item.idIdioma
                        ,
                        btCadastrado = item.btCadastrado

                    };


                    var isEmail = new Util.Email().EnviarEmailCancelamento(email);
                    idStatusEnvio = (isEmail) ? 2 : 3;

                    try
                    {
                        var retornoAtualizaStatus = _db.Database.SqlQuery<RetornoPadrao>("SP_EmailAgendamentoAlteraStatus @idAgendamento, @idStatus", new SqlParameter("idAgendamento", item.idAgendamento), new SqlParameter("idStatus", idStatusEnvio)).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }





                foreach (var item in listaEmail.Where(e => e.dsTipo.Trim() == "TC").ToList())
                {
                    var email = new Email()
                    {
                        dsArquivoHTML = item.dsArquivoHTML
                        ,
                        dsAssunto = item.dsAssunto
                        ,
                        dsBairro = item.dsBairro
                        ,
                        dsCEP = item.dsCEP
                        ,
                        dsCidade = item.dsCidade
                        ,
                        dsCNPJ = item.dsCNPJ
                        ,
                        dsCPF = item.dsCPF
                        ,
                        dsEmail = item.dsEmail
                        ,
                        dsEndereco = item.dsEndereco
                        ,
                        dsNomeCompleto = item.dsNomeCompleto
                        ,
                        dsNumero = item.dsNumero
                        ,
                        dsPais = item.dsPais
                        ,
                        dsTelefone = item.dsTelefone
                        ,
                        dsTipo = item.dsTipo
                        ,
                        dsUF = item.dsUF
                        ,
                        dsNomeTreinamento = item.dsNomeTreinamento
                        ,
                        dsDataTreinamento = item.dtTreinamento.ToString("dd/MM/yy")
                        ,
                        idIdioma = item.idIdioma
                        ,
                        btCadastrado = item.btCadastrado

                    };


                    var isEmail = new Util.Email().EnviarEmailTreinamentoCancelado(email);
                    idStatusEnvio = (isEmail) ? 2 : 3;

                    try
                    {
                        var retornoAtualizaStatus = _db.Database.SqlQuery<RetornoPadrao>("SP_EmailAgendamentoAlteraStatus @idAgendamento, @idStatus", new SqlParameter("idAgendamento", item.idAgendamento), new SqlParameter("idStatus", idStatusEnvio)).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }



            }
            catch (Exception ex)
            {

                throw;
            }

            return new RetornoPadrao() { id = 1, dsMensagem = "" };
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
