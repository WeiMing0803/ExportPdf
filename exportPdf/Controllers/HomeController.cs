using exportPdf.common;
using exportPdf.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using RazorEngine;
using RazorEngine.Templating;
using SelectPdf;
using System.Diagnostics;
using System.Text;

namespace exportPdf.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ViewRenderService _viewRenderService;

        public HomeController(ILogger<HomeController> logger,
            ViewRenderService viewRenderService)
        {
            _logger = logger;
            this._viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ToPdf()
        {
            PdfDocument pdfDocument = new PdfDocument();
            HtmlToPdf converter = new HtmlToPdf();//实例化一个html到pdf转换器对象
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;//设置页面方向
            converter.Options.PdfPageSize = PdfPageSize.A4;//设置页面大小
            converter.Options.MarginTop = 10;//设置页边距
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;

            PdfReportModel model = new PdfReportModel { Name = "彭于晏", Email = "pengyuyan@outlook.com" };
            //string htmlResult = readByEngineRazor(model);
            string htmlResult = await readCshtml(model);

            if (!string.IsNullOrEmpty(htmlResult))
            {
                pdfDocument = converter.ConvertHtmlString(htmlResult);
            }
            
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $@"ExportPDF\{DateTime.Now.ToString("yyyyMMdd")}");
            Directory.CreateDirectory(savePath);
            string filename = Path.Combine(savePath, $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.pdf");
            pdfDocument.Save(filename);

            byte[] bytes = System.IO.File.ReadAllBytes(filename);
            return File(bytes, "application/pdf", Path.GetFileName(filename));
        }

        private string readByEngineRazor(PdfReportModel model)
        {
            string template = System.IO.File.ReadAllText("Views/Report/PdfReport.cshtml");
            string htmlResult = Engine.Razor.RunCompile(template, "PdfReport", typeof(PdfReportModel), model);
            return htmlResult;
        }

        private async Task<string> readCshtml(PdfReportModel model)
        {
            string htmlResult = await _viewRenderService.RenderToStringAsync("Report/PdfReport", model);
            return htmlResult;
        }


        public IActionResult Download()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\86186\Desktop\ExportPDF\20230228\149c7f05-3234-446a-9e18-55e78f7fd629.pdf");
            return File(bytes, "application/pdf", "file.pdf");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}