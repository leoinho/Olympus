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
    public class FaleConoscoController : Controller
    {
        RepositorioFaleConosco _faleConosco = new RepositorioFaleConosco();

        public ActionResult Index()
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            if (usuario.idPerfil == 2) { return RedirectToAction("Index", "Dashboard"); }

            var idIdioma = usuario.idTipo;

            FaleConosco faleConosco = new FaleConosco();

            if (idIdioma == 1)
            {
                //OBL
                
                //DROP PAIS
                var contato = new List<FaleConosco>();

                contato = new List<FaleConosco>() { 
                    new FaleConosco { dsContato = "Brasil" }, 
                    new FaleConosco { dsContato = "México" }, 
                    new FaleConosco { dsContato = "América Latina" }};

                ViewBag.Pais = new SelectList(contato, "dsContato", "dsContato");
            }
            else if (idIdioma == 2)
                faleConosco.dsContato = "América Latina";
            else
                faleConosco.dsContato = "México";
            
            return View(faleConosco);
        }
        [HttpPost]
        public ActionResult Index(FaleConosco faleConosco)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var usuario = (Administrador)Session["Administrador"];
            var idIdioma = usuario.idTipo;

            if (idIdioma == 1)
            {
                //OBL

                //DROP PAIS
                var contato = new List<FaleConosco>();

                contato = new List<FaleConosco>() { 
                    new FaleConosco { dsContato = "Brasil" }, 
                    new FaleConosco { dsContato = "México" }, 
                    new FaleConosco { dsContato = "América Latina" }};

                ViewBag.Pais = new SelectList(contato, "dsContato", "dsContato");
            }
            else if (idIdioma == 2)
                faleConosco.dsContato = "América Latina";
            else
                faleConosco.dsContato = "México";

            ViewBag.FaleConosco = _faleConosco.ListaFaleConosco(faleConosco);

            return View(faleConosco);
        }
        public ActionResult FaleConoscoExcel(FaleConosco faleConosco)
        {
            if (Session["Administrador"] == null) { return RedirectToAction("Index", "Home"); }
            var admin = (Administrador)Session["Administrador"];

            var mensagens = _faleConosco.ListaFaleConosco(faleConosco);

            // Create file
            DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time"));
            string filename = "Relatorio_SAC_" + now.ToString("ddMMyyyyHHmm") + ".xlsx";

            // Criar excel
            MemoryStream stream = new MemoryStream();
            ExcelStructure excel = new ExcelStructure("SAC", ref stream);

            excel.setColumnWidth(1, 35);
            excel.setColumnWidth(2, 25);
            excel.setColumnWidth(3, 35);
            excel.setColumnWidth(4, 25);
            excel.setColumnWidth(5, 25);
            excel.setColumnWidth(6, 80);
            excel.setColumnWidth(7, 25);

            excel.createSheetData();

            IList<string> headers = new List<string>();

            headers = new List<string>();

            headers.Add("NOME");
            headers.Add("CPF | PASSAPORTE");
            headers.Add("E-MAIL");
            headers.Add("TELEFONE");
            headers.Add("ASSUNTO");
            headers.Add("MENSAGEM");
            headers.Add("DATA");

            excel.CreateColumnHeader(1, headers);

            IList<string> valores = new List<string>();
            IList<int> tipos = new List<int>();

            uint row = 2;

            foreach (var m in mensagens)
            {
                valores = new List<string>();
                tipos = new List<int>();

                valores.Add(m.dsNomeCompleto);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsCPF);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsEmail);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsTelefone);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsAssunto);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dsMensagem);
                tipos.Add(ExcelStructure.TYPE_STRING);

                valores.Add(m.dtCadastro.ToString());
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
	}
}