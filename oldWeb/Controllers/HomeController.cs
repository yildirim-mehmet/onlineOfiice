using Microsoft.AspNetCore.Mvc;
using OnlineOfficeWeb.Models;
using OnlineOfficeWeb.Services;

namespace OnlineOfficeWeb.Controllers;

public class HomeController : Controller
{
    private readonly WordRenderService _wordService;
    private readonly ExcelRenderService _excelService;
    private readonly PdfRenderService _pdfService;
    private readonly PptRenderService _pptService;
    private readonly VisioRenderService _visioService;
    private readonly IWebHostEnvironment _env;

    public HomeController(
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

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.Error = "Lütfen bir dosya seçiniz.";
            return View();
        }

        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsRoot);

        var storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsRoot, storedFileName);
        using (var stream = System.IO.File.Create(filePath))
        {
            file.CopyTo(stream);
        }

        var fileType = DetectFileType(file.FileName);
        var model = new UploadedFileInfo
        {
            OriginalFileName = file.FileName,
            StoredFileName = storedFileName,
            RelativePath = $"/uploads/{storedFileName}",
            FileType = fileType
        };

        // Yüklenen dosyayı uygun viewer'a yönlendir
        return RedirectToAction("ViewFile", "Viewer", new
        {
            fileName = model.StoredFileName,
            originalName = model.OriginalFileName,
            type = model.FileType
        });
    }

    private OfficeFileType DetectFileType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".docx" => OfficeFileType.Word,
            ".doc" => OfficeFileType.Word,
            ".xlsx" => OfficeFileType.Excel,
            ".xls" => OfficeFileType.Excel,
            ".pptx" => OfficeFileType.PowerPoint,
            ".ppt" => OfficeFileType.PowerPoint,
            ".pdf" => OfficeFileType.Pdf,
            ".vsdx" => OfficeFileType.Visio,
            _ => OfficeFileType.Unknown
        };
    }
}
