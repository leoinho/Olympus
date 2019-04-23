using Admin.Olympus.Dominio.Entidade;
using Admin.Olympus.Dominio.Repositorio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Olympus.MVC.Controllers
{
    public class RelatorioController : Controller
    {
        RepositorioUsuario _usuario = new RepositorioUsuario();

        public ActionResult Cadastro()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                ViewBag.Site = ddlSite();
            }
            else
            {
                //OLA e OMS

                ViewBag.Pais = new SelectList(_usuario.ListaPais().Where(x => x.dsNome != "BRAZIL").OrderBy(x => x.dsNome), "idPais", "dsNome");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Cadastro(Usuario usuario)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var admin = (Administrador)Session["Administrador"];
            var idIdioma = admin.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                ViewBag.Site = ddlSite();
            }
            else
            {
                //OLA e OMS

                ViewBag.Pais = new SelectList(_usuario.ListaPais().Where(x => x.dsNome != "BRAZIL").OrderBy(x => x.dsNome), "idPais", "dsNome");

                usuario.idIdioma = idIdioma;
            }

            var lista = _usuario.RelatorioUsuario(usuario);
            ViewBag.Cadastros = lista;

            return View();
        }
        public ActionResult CadastroExcel(Usuario usuario)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var admin = (Administrador)Session["Administrador"];

            if (admin.idTipo == 2 || admin.idTipo == 3)
                usuario.idIdioma = admin.idTipo;

            var cadastros = _usuario.RelatorioUsuario(usuario);

            // Create file
            DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time"));
            string filename = "Relatorio_Cadastro_" + now.ToString("ddMMyyyyHHmm") + ".xlsx";

            // Criar excel
            MemoryStream stream = new MemoryStream();
            ExcelStructure excel = new ExcelStructure("Cadastros", ref stream);

            excel.setColumnWidth(1, 30); 
            excel.setColumnWidth(2, 15); 
            excel.setColumnWidth(3, 10); 
            excel.setColumnWidth(4, 10); 
            excel.setColumnWidth(5, 15); 
            excel.setColumnWidth(6, 20); 
            excel.setColumnWidth(7, 30); 
            //excel.setColumnWidth(8, 20); 
            excel.setColumnWidth(8, 25); 
            excel.setColumnWidth(9, 25);
            excel.setColumnWidth(10, 20);
            excel.setColumnWidth(11, 25);
            excel.setColumnWidth(12, 15);
            excel.setColumnWidth(13, 15);
            excel.setColumnWidth(14, 20);
            excel.setColumnWidth(15, 35);
            excel.setColumnWidth(16, 35);
            excel.setColumnWidth(17, 20);
            excel.setColumnWidth(18, 10);
            excel.setColumnWidth(19, 10);
            excel.setColumnWidth(20, 30);
            excel.setColumnWidth(21, 15);
            excel.setColumnWidth(22, 15);
            excel.setColumnWidth(23, 20);
            excel.setColumnWidth(24, 30);
            excel.setColumnWidth(25, 20);
            excel.setColumnWidth(26, 20);

            excel.createSheetData();

            IList<string> headers = new List<string>();

            headers = new List<string>();

            headers.Add("NOME");
            headers.Add("CPF");
            headers.Add("TELEFONE");
            headers.Add("CELULAR");
            headers.Add("E-MAIL");
            headers.Add("NACIONALIDADE");
            headers.Add("PAÍS");
            //headers.Add("PASSAPORTE");
            headers.Add("TITULO PROFISSIONAL");
            headers.Add("TITULO PROFISSIONAL OUTROS");
            headers.Add("NUM CONSELHO");
            headers.Add("ESPECIALIDADE");
            headers.Add("LICENSA EUA");
            headers.Add("NUM ESTADO");

            headers.Add("CNPJ");
            headers.Add("NOME FANTASIA");
            headers.Add("RAZÃO SOCIAL");
            headers.Add("CIDADE");
            headers.Add("ESTADO");
            headers.Add("CEP");
            headers.Add("ENDEREÇO");
            headers.Add("NÚMERO");
            headers.Add("COMPLEMENTO");
            headers.Add("BAIRRO");
            headers.Add("DATA DE CADASTRO");
            headers.Add("STATUS");
            headers.Add("NOVA INSTITUIÇÃO");

            excel.CreateColumnHeader(1, headers);

            IList<string> valores = new List<string>();
            IList<int> tipos = new List<int>();

            uint row = 2;

            foreach (var m in cadastros)
            {
                valores = new List<string>();
                tipos = new List<int>();

                valores.Add(m.dsNomeCompleto);
                tipos.Add(ExcelStructure.TYPE_STRING);
                
                valores.Add(m.dsCPF);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTelefone);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCelular);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsEmail);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNacionalidade);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsPais);
                tipos.Add(ExcelStructure.TYPE_STRING);

                //valores.Add(m.dsPassaporte);
                //tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTituloProfissional);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTituloProfissionalOutros);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNumConselho);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsEspecialidade);
                tipos.Add(ExcelStructure.TYPE_STRING);
                
                if (m.btLicencaEUA)
                    valores.Add("SIM");
                else
                    valores.Add("NÃO");

                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNumEstado);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCNPJ);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNomeFantasia);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsRazaoSocial);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCidade);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsUF);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCEP);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsEndereco);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsNumero);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsComplemento);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsBairro);
                tipos.Add(ExcelStructure.TYPE_STRING);
                
                valores.Add(m.dtCadastro.ToString());
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsStatus);
                tipos.Add(ExcelStructure.TYPE_STRING);
                
                if (m.btNovaEmpresa)
                    valores.Add("SIM");
                else
                    valores.Add("NÃO");
                tipos.Add(ExcelStructure.TYPE_STRING);

                excel.CreateRowWithContent(row, valores, tipos);
                row++;
            }

            excel.save();
            excel.close();

            stream.Position = 0;

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = filename
            };
        }

        public SelectList ddlSite()
        {
            var listSite = new List<Banner>();
            listSite.Add(new Banner { idIdioma = 1, dsSite = "OBL" });
            listSite.Add(new Banner { idIdioma = 3, dsSite = "OMS" });
            listSite.Add(new Banner { idIdioma = 2, dsSite = "OLA" });

            return new SelectList(listSite, "idIdioma", "dsSite", 0);
        }
	}
}