using DinkToPdf;
using DinkToPdf.Contracts;
using HtmlToPdfApp.Models;
using HtmlToPdfApp.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HtmlToPdfApp.Controllers
{
    public class HomeController : Controller
    {
        private IConverter _converter;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConverter converter)
        {
            _logger = logger;
            _converter = converter;
        }

        public IActionResult Index()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                //// If you don't set Out value it will saved in downloads -- Prefered not to set
                //Out = @"D:\PDFCreator\Employee_Report.pdf"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            //// this will download directly
            //_converter.Convert(pdf);
            //return Ok("Successfully created PDF document.");

            // this will show the document on the browser
            var file=_converter.Convert(pdf);
            //return File(file, "application/pdf");
            return File(file, "application/pdf", "EmployeeReport.pdf");
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