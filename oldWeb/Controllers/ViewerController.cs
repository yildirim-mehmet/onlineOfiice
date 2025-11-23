using Microsoft.AspNetCore.Mvc;
using OnlineOfficeWeb.Models;
using OnlineOfficeWeb.Services;

namespace OnlineOfficeWeb.Controllers;

public class ViewerController : Controller
{
    private readonly WordRenderService _wordService;
    private readonly ExcelRenderService _excelService;
    private readonly PdfRenderService _pdfService;
    private readonly PptRenderService _pptService;
    private readonly VisioRenderService _visioService;
    private readonly IWebHostEnvironment _env;

    public ViewerController(
        WordRenderService wordService,
        ExcelRenderService excelService,
        PdfRenderService pdfService,
        PptRenderService pptService,
        VisioRenderService visioService,
        IWebHostEnvironment env)
    {
        _wordService = wordService;
        _excelService = excelService;
        _pdfService = pdfService;
        _pptService = pptService;
        _visioService = visioService;
        _env = env;
    }

    public IActionResult ViewFile(string fileName, string originalName, OfficeFileType type)
    {
        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads");
        var filePath = Path.Combine(uploadsRoot, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Dosya bulunamadı.");
        }

        ViewBag.OriginalName = originalName;

        return type switch
        {
            OfficeFileType.Word => View("ViewWord", _wordService.RenderToHtml(filePath)),
            OfficeFileType.Excel => View("ViewExcel", _excelService.RenderToDto(filePath)),
            OfficeFileType.Pdf => View("ViewPdf", _pdfService.ExtractText(filePath)),
            OfficeFileType.PowerPoint => View("ViewPpt", _pptService.GetSummary(filePath)),
            OfficeFileType.Visio => View("ViewVisio", _visioService.GetSummary(filePath)),
            _ => Content("Bu dosya tipi şu anda desteklenmiyor.")
        };
    }
}
