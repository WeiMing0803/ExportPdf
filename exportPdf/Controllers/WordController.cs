using Microsoft.AspNetCore.Mvc;
using Spire.Doc;
using Xenirio.Component.Gutenberg;

namespace exportPdf.Controllers
{
    public class WordController : Controller
    {
        public IActionResult Index()
        {
            var converter = new Document();
            byte[] reportBinary = Generate();

            Stream reportStream = new MemoryStream(reportBinary);
            converter.InsertTextFromStream(reportStream, FileFormat.Docx);
            using (var stream = new MemoryStream())
            {
                converter.SaveToStream(stream, FileFormat.PDF);
                return File(stream.ToArray(), "application/pdf", $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.pdf");
            }
        }

        private byte[] Generate(EntityInfoReportModel? report = null)
        {
            return generateReport("Views/Report/tf00002110_wac.docx", report);
        }

        private byte[] generateReport(string templatePath, EntityInfoReportModel report)
        {
            var img = System.IO.File.ReadAllBytes(@"wwwroot\img\pengyuyan.png");

            var generator = new ReportGenerator(templatePath);
            //generator.setParagraph("Person.Name", "Hello World");
            generator.setImage("Person.Name", img);

            return generator.GenerateToByte();
        }
    }

    public class EntityInfoReportModel
    {

    }
}
