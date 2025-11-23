
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OnlineOfficeWeb.Models;
using OnlineOfficeWeb.Services;
using System.IO;



namespace OnlineOfficeWeb.Controllers;

public class ViewerController : Controller {
    private readonly IWebHostEnvironment _env;
    private readonly ExcelRenderService _excel;
    private readonly WordRenderService _word;
    private readonly PdfRenderService _pdf;
    private readonly PowerPointRenderService _ppt;
    private readonly VisioRenderService _visio;
    private readonly PowerPointStructureService _pptStructureService;


    public ViewerController(
        IWebHostEnvironment env,
        ExcelRenderService excel,
        WordRenderService word,
        PdfRenderService pdf,
        PowerPointRenderService ppt,
        VisioRenderService visio
    ) {
        _env = env;
        _excel = excel;
        _word = word;
        _pdf = pdf;
        _ppt = ppt;
        _visio = visio;

    }

    public IActionResult ViewFile(string fileName, string originalName, OfficeFileType type) {
        var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
        if (!System.IO.File.Exists(path)) return Content("Dosya yok");

        ViewBag.OriginalName = originalName;

        return type switch {
            OfficeFileType.Word => View("ViewWord", _word.RenderToHtml(path)),
            OfficeFileType.Excel => View("ViewExcel", _excel.RenderToDto(path)),
            OfficeFileType.Pdf => View("ViewPdf", _pdf.ExtractText(path)),
            OfficeFileType.PowerPoint => View("ViewPpt", _ppt.RenderSlidesAsBase64(path)),
            OfficeFileType.Visio => View("ViewVisio", _visio.RenderPagesAsSvg(path)),
            _ => Content("Desteklenmeyen dosya")
        };
    }


    [HttpGet]
    public IActionResult ViewPowerPoint(string fileName, string originalName) {

        string uploads = Path.Combine(_env.WebRootPath, "uploads");
        string filePath = Path.Combine(uploads, fileName);

        if (!System.IO.File.Exists(filePath))
            return Content("PPTX dosyasý bulunamadý");

        // klasör adý = dosya adý (extension hariç)
        string outputDir = Path.GetFileNameWithoutExtension(fileName);

        var slides = PowerPointRenderService.RenderAllSlides(filePath, outputDir);

        return View("ViewPowerPoint", slides);
    }


    [HttpGet]
    public IActionResult ViewPowerPointStructure(string fileName) {
        string uploads = Path.Combine(_env.WebRootPath, "uploads");
        string filePath = Path.Combine(uploads, fileName);

        if (!System.IO.File.Exists(filePath))
            return Content("PowerPoint dosyasý bulunamadý.");

        var structure = _pptStructureService.ExtractStructure(filePath);

        return View("ViewPowerPointStructure", structure);
    }


}
