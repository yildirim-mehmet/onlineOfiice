using Microsoft.AspNetCore.Mvc;
using OfficeIMO.Collaborative.Web.Models;
using OfficeIMO.Collaborative.Web.Services;

namespace OfficeIMO.Collaborative.Web.Controllers;

public class ExcelController : Controller
{
    private readonly DocumentSessionService _sessionService;

    public ExcelController(DocumentSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Lütfen bir Excel dosyası seçiniz.");
            return View();
        }

        var allowed = new[] { ".xlsx" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowed.Contains(ext))
        {
            ModelState.AddModelError(string.Empty, "Sadece .xlsx uzantılı dosyalar desteklenmektedir.");
            return View();
        }

        var uploadPath = _sessionService.UploadPath;
        Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadPath, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Oturum oluştur
        var sessionId = _sessionService.CreateExcelSession(fullPath, file.FileName);

        return RedirectToAction("Edit", new { sessionId });
    }

    [HttpGet]
    public IActionResult Edit(string sessionId)
    {
        var session = _sessionService.GetExcelSession(sessionId);
        if (session == null) return NotFound();

        var sheet = session.Document.Workbook.Worksheets.First();
        const int maxRows = 20;
        const int maxCols = 10;

        var rows = maxRows;
        var cols = maxCols;
        var data = new string[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var cellAddress = GetCellAddress(r + 1, c + 1);
                var cell = sheet.Cells[cellAddress];
                data[r, c] = cell?.Value?.ToString() ?? string.Empty;
            }
        }

        var model = new ExcelEditViewModel
        {
            SessionId = session.SessionId,
            FileName = session.OriginalFileName,
            SheetName = sheet.Name,
            Rows = rows,
            Columns = cols,
            Cells = data
        };

        return View(model);
    }

    private static string GetCellAddress(int row, int column)
    {
        // Örnek: 1,1 -> A1; 1,27 -> AA1
        string col = string.Empty;
        int dividend = column;
        while (dividend > 0)
        {
            int modulo = (dividend - 1) % 26;
            col = Convert.ToChar('A' + modulo) + col;
            dividend = (dividend - modulo) / 26;
        }
        return $"{col}{row}";
    }
}
