using Fullbar.Olympus.Dominio.Contexto;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.Dominio.Interface;
using Fullbar.Olympus.Dominio.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Security;

namespace Fullbar.Olympus.Dominio.Repositorio
{
    public class RepositorioUsuario : IUsuario
    {
        private readonly EFDbContexto _db = new EFDbContexto();

        public RetornoPadrao Cadastro(UsuarioCadastro usuario)
        {
            var senha = Crypto.Encrypt(usuario.Senha, Util.Crypto.Key256, 256);
            var btLincencaEUA = (usuario.LicencaMedicaEUA == 2) ? false : true;
            var idIdioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);

            try
            {
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_UsuarioCadastro @idNacionalidade, @dsCPF, @idPais, @dsPassaporte, @dsNomeCompleto, @dsTelefone, @dsCelular, @dsEmail , @dsSenha ,  @dsTituloProfissional, @dsTituloProfissionalOutros, @dsNumConselho , @dsEspecialidade, @dsCNPJ, @dsNomeFantasia, @dsRazaoSocial, @dsCEP, @dsUF, @dsCidade, @dsBairro, @dsEndereco, @dsNumero, @btLicencaEUA, @dsNumEstado, @idIdioma",

                                                                                    new SqlParameter("idNacionalidade", usuario.Nacionalidade)

                                                                                  , (!String.IsNullOrEmpty(usuario.CPF)) ? new SqlParameter("dsCPF", usuario.CPF.Replace("-", "").Replace(".", "")) : new SqlParameter("dsCPF", string.Empty)

                                                                                  , new SqlParameter("idPais", usuario.idPais)

                                                                                  , (!String.IsNullOrEmpty(usuario.NumeroPassaporte)) ? new SqlParameter("dsPassaporte", usuario.NumeroPassaporte) : new SqlParameter("dsPassaporte", string.Empty)

                                                                                  , new SqlParameter("dsNomeCompleto", usuario.Nome)

                                                                                  , (!String.IsNullOrEmpty(usuario.Telefone)) ? new SqlParameter("dsTelefone", usuario.Telefone) : new SqlParameter("dsTelefone", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Celular)) ? new SqlParameter("dsCelular", usuario.Celular) : new SqlParameter("dsCelular", string.Empty)

                                                                                  , new SqlParameter("dsEmail", usuario.EmailCadastro)

                                                                                  , new SqlParameter("dsSenha", senha)

                                                                                  , (!String.IsNullOrEmpty(usuario.TituloProfissional)) ? new SqlParameter("dsTituloProfissional", usuario.TituloProfissional) : new SqlParameter("dsTituloProfissional", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.TituloProfissionalOutros)) ? new SqlParameter("dsTituloProfissionalOutros", usuario.TituloProfissionalOutros) : new SqlParameter("dsTituloProfissionalOutros", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.NumeroConselho)) ? new SqlParameter("dsNumConselho", usuario.NumeroConselho) : new SqlParameter("dsNumConselho", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Especialidade)) ? new SqlParameter("dsEspecialidade", usuario.Especialidade) : new SqlParameter("dsEspecialidade", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.CNPJ)) ? new SqlParameter("dsCNPJ", usuario.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "")) : new SqlParameter("dsCNPJ", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.NomeInstituicao)) ? new SqlParameter("dsNomeFantasia", usuario.NomeInstituicao) : new SqlParameter("dsNomeFantasia", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.NomeInstituicao)) ? new SqlParameter("dsRazaoSocial", usuario.NomeInstituicao) : new SqlParameter("dsRazaoSocial", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Cep)) ? new SqlParameter("dsCEP", usuario.Cep.Replace("-", "")) : new SqlParameter("dsCEP", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.UF)) ? new SqlParameter("dsUF", usuario.UF) : new SqlParameter("dsUF", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Cidade)) ? new SqlParameter("dsCidade", usuario.Cidade) : new SqlParameter("dsCidade", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Bairro)) ? new SqlParameter("dsBairro", usuario.Bairro) : new SqlParameter("dsBairro", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Endereco)) ? new SqlParameter("dsEndereco", usuario.Endereco) : new SqlParameter("dsEndereco", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Numero)) ? new SqlParameter("dsNumero", usuario.Numero) : new SqlParameter("dsNumero", string.Empty)

                                                                                  , new SqlParameter("btLicencaEUA", btLincencaEUA)

                                                                                  , (!String.IsNullOrEmpty(usuario.NumeroEstado)) ? new SqlParameter("dsNumEstado", usuario.NumeroEstado) : new SqlParameter("dsNumEstado", string.Empty)

                                                                                  , new SqlParameter("idIdioma", idIdioma)).FirstOrDefault();


               
                return retorno;
            }
            catch (Exception ex)
            {
                new RetornoPadrao { id = -5, dsMensagem = "" };
            }

            return null;

        }

        public List<Pais> ListaPais()
        {
            return _db.Database.SqlQuery<Pais>("SP_PaisLista @idIdioma", new SqlParameter("idIdioma", 2)).ToList();
        }

        public RetornoCNPJ BuscaCNPJ(string cnpj)
        {
            var retornoCNPJ = _db.Database.SqlQuery<RetornoCNPJ>("SP_EmpresaListaPorCNPJ @dsCNPJ", new SqlParameter("dsCNPJ", cnpj.Replace(".", "").Replace("/", "").Replace("-", ""))).FirstOrDefault();

            if (retornoCNPJ == null)
            {
                return new RetornoCNPJ { dsBairro = "", dsCep = "", dsCidade = "", dsCNPJ = "", dsComplemento = "", dsEndereco = "", dsMensagem = "", dsNomeFantasia = "", dsNumero = "", dsUF = "" };
            }

            return retornoCNPJ;
        }

        public Usuario ListaUsuarioPorId(int id)
        {

            var retornoUsuario = _db.Database.SqlQuery<Usuario>("SP_UsuarioListaPorID @idUsuario", new SqlParameter("idUsuario", id)).FirstOrDefault();
            var retornoCNPJ = _db.Database.SqlQuery<RetornoCNPJ>("SP_EmpresaListaPorCNPJ @dsCNPJ", new SqlParameter("dsCNPJ", retornoUsuario.dsCNPJ.Replace(".", "").Replace("/", "").Replace("-", ""))).FirstOrDefault();

            var usuarioLogin = new Usuario
            {

                btLicencaEUA = retornoUsuario.btLicencaEUA
                ,
                dsCNPJ = retornoUsuario.dsCNPJ
                ,
                dsCPF = retornoUsuario.dsCPF
                ,
                dsEmail = retornoUsuario.dsEmail
                ,
                dsEspecialidade = retornoUsuario.dsEspecialidade
                ,
                dsNacionalidade = retornoUsuario.dsNacionalidade
                ,
                dsNomeCompleto = retornoUsuario.dsNomeCompleto
                ,
                dsNumConselho = retornoUsuario.dsNumConselho
                ,
                dsNumEstado = retornoUsuario.dsNumEstado
                ,
                dsTelefone = retornoUsuario.dsTelefone
                ,
                dsCelular = retornoUsuario.dsCelular
                ,
                dsTituloProfissional = retornoUsuario.dsTituloProfissional
                ,
                dsTituloProfissionalOutros = retornoUsuario.dsTituloProfissionalOutros
                ,
                idNacionalidade = retornoUsuario.idNacionalidade
                ,
                idusuario = retornoUsuario.idusuario
                ,
                Passaporte = retornoUsuario.Passaporte
                ,
                RetornoCNPJ = retornoCNPJ
                ,
                dsSenha = retornoUsuario.dsSenha

            };

            return usuarioLogin;
        }

        public RetornoPadrao Editar(UsuarioEditar usuario)
        {
            var senha = Crypto.Encrypt(usuario.Senha, Util.Crypto.Key256, 256);
            var btLincencaEUA = (usuario.LicencaMedicaEUA == 0) ? false : true;

            try
            {
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_UsuarioAlterar @idUsuario, @dsNomeCompleto, @dsTelefone, @dsCelular, @dsSenha ,  @dsTituloProfissional, @dsTituloProfissionalOutros, @dsNumConselho , @dsEspecialidade, @dsCNPJ, @btLicencaEUA, @dsNumEstado",

                                                                                    new SqlParameter("idUsuario", usuario.idUsuario)

                                                                                  , new SqlParameter("dsNomeCompleto", usuario.Nome)

                                                                                  , (!String.IsNullOrEmpty(usuario.Telefone)) ? new SqlParameter("dsTelefone", usuario.Telefone) : new SqlParameter("dsTelefone", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Celular)) ? new SqlParameter("dsCelular", usuario.Celular) : new SqlParameter("dsCelular", string.Empty)

                                                                                  , new SqlParameter("dsSenha", senha)

                                                                                  , (!String.IsNullOrEmpty(usuario.TituloProfissional)) ? new SqlParameter("dsTituloProfissional", usuario.TituloProfissional) : new SqlParameter("dsTituloProfissional", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.TituloProfissionalOutros)) ? new SqlParameter("dsTituloProfissionalOutros", usuario.TituloProfissionalOutros) : new SqlParameter("dsTituloProfissionalOutros", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.NumeroConselho)) ? new SqlParameter("dsNumConselho", usuario.NumeroConselho) : new SqlParameter("dsNumConselho", string.Empty)

                                                                                  , (!String.IsNullOrEmpty(usuario.Especialidade)) ? new SqlParameter("dsEspecialidade", usuario.Especialidade) : new SqlParameter("dsEspecialidade", string.Empty)

                                                                                  , new SqlParameter("dsCNPJ", usuario.CNPJ.Replace(".", "").Replace("/", "").Replace("-", ""))

                                                                                  , new SqlParameter("btLicencaEUA", btLincencaEUA)

                                                                                  , (!String.IsNullOrEmpty(usuario.NumeroEstado)) ? new SqlParameter("dsNumEstado", usuario.NumeroEstado) : new SqlParameter("dsNumEstado", string.Empty)).FirstOrDefault();



                return retorno;
            }
            catch (Exception ex)
            {
                return new RetornoPadrao { id = -2, dsMensagem = "" };
            }

            return null;
        }

        public UsuarioEditar ListaEditar(int idUsuario)
        {
            var usuario = ListaUsuarioPorId(idUsuario);
            var retornoCNPJ = (!string.IsNullOrEmpty(usuario.dsCNPJ)) ? BuscaCNPJ(usuario.dsCNPJ) : null;

            var usuarioEditar = new UsuarioEditar()
            {

                CNPJ = usuario.dsCNPJ
                ,
                CPF = usuario.dsCPF
                ,
                Email = usuario.dsEmail
                ,
                Especialidade = usuario.dsEspecialidade
                ,
                idNacionalidade = usuario.idNacionalidade
                ,
                idUsuario = usuario.idusuario
                ,
                LicencaMedicaEUA = (usuario.btLicencaEUA) ? 1 : 2
                ,
                Nome = usuario.dsNomeCompleto
                ,
                NumeroConselho = usuario.dsNumConselho
                ,
                NumeroEstado = usuario.dsNumEstado
                ,
                Telefone = usuario.dsTelefone
                ,
                Celular = usuario.dsCelular
                ,
                TituloProfissional = usuario.dsTituloProfissional
                ,
                TituloProfissionalOutros = usuario.dsTituloProfissionalOutros
                ,
                Bairro = (retornoCNPJ != null) ? retornoCNPJ.dsBairro : string.Empty
                ,
                Cep = (retornoCNPJ != null) ? retornoCNPJ.dsCep : string.Empty
                ,
                Cidade = (retornoCNPJ != null) ? retornoCNPJ.dsCidade : string.Empty
                ,
                Endereco = (retornoCNPJ != null) ? retornoCNPJ.dsEndereco : string.Empty
                ,
                UF = (retornoCNPJ != null) ? retornoCNPJ.dsUF : string.Empty
                ,
                NomeInstituicao = (retornoCNPJ != null) ? retornoCNPJ.dsNomeFantasia : string.Empty
                ,
                Complemento = (retornoCNPJ != null) ? retornoCNPJ.dsComplemento : string.Empty
                ,
                Numero = (retornoCNPJ != null) ? retornoCNPJ.dsNumero : string.Empty
            };

            return usuarioEditar;
        }

        public RetornoPadrao EnviarSenha(string cpf)
        {
            var _cpf = cpf.Replace("-", "").Replace(".", "");
            var idioma = Convert.ToInt16(WebConfigurationManager.AppSettings["idIdioma"]);
            var retornoUsuario = _db.Database.SqlQuery<UsuarioSenha>("SP_UsuarioRecuperaSenha @dsCPF, @idIdioma", new SqlParameter("dsCPF", _cpf), new SqlParameter("idIdioma", idioma)).FirstOrDefault();

            if (retornoUsuario != null && retornoUsuario.idUsuario != null)
            {
                var isEmail = new Util.Email().EnviarSenha(retornoUsuario.dsEmail, retornoUsuario.dsSenha, idioma, retornoUsuario.dsNomeCompleto);

                if (isEmail)
                {
                    return new RetornoPadrao { id = 1, dsMensagem = "", dsTexto = retornoUsuario.dsEmail };
                }
                else
                {
                    return new RetornoPadrao { id = -2, dsMensagem = "" };
                }
            }

            return new RetornoPadrao { id = -1, dsMensagem = "" };
        }

        public RetornoPadrao EditaNoTreinamento(UsuarioEditar usuario)
        {
            try
            {
                var retorno = _db.Database.SqlQuery<RetornoPadrao>("SP_UsuarioAlteraDadosNoTreinamento @idUsuario, @dsNomeCompleto, @dsEmail, @dsCelular",

                                                                                    new SqlParameter("idUsuario", usuario.idUsuario)

                                                                                  , new SqlParameter("dsNomeCompleto", usuario.Nome)

                                                                                  , new SqlParameter("dsEmail", usuario.Email)

                                                                                  , (!String.IsNullOrEmpty(usuario.Celular)) ? new SqlParameter("dsCelular", usuario.Celular) : new SqlParameter("dsCelular", string.Empty)

                                                                                  ).FirstOrDefault();



                return retorno;
            }
            catch (Exception ex)
            {
                return new RetornoPadrao { id = -2, dsMensagem = "" };
            }

            return null;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
