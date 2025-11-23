
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineOfficeWeb.Models;
using System;
using System.IO;

namespace OnlineOfficeWeb.Controllers;

public class HomeController : Controller {
    private readonly IWebHostEnvironment _env;

    public HomeController(IWebHostEnvironment env) {
        _env = env;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(IFormFile file) {
        if (file == null || file.Length == 0) {
            ViewBag.Error = "Dosya seçilmedi.";
            return View();
        }

        var uploads = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        var stored = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var path = Path.Combine(uploads, stored);
        using (var fs = System.IO.File.Create(path)) file.CopyTo(fs);

        var type = DetectFileType(file.FileName);

        return RedirectToAction("ViewFile", "Viewer", new {
            fileName = stored,
            originalName = file.FileName,
            type = type
        });
    }

    private OfficeFileType DetectFileType(string name) {
        var ext = Path.GetExtension(name).ToLower();
        return ext switch {
            ".docx" => OfficeFileType.Word,
            ".xlsx" => OfficeFileType.Excel,
            ".pptx" => OfficeFileType.PowerPoint,
            ".pdf"  => OfficeFileType.Pdf,
            ".vsdx" => OfficeFileType.Visio,
            _ => OfficeFileType.Unknown
        };
    }
}
