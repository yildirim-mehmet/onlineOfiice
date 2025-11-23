using ClosedXML.Excel;

namespace OnlineOffice.Web.Services;

public class OfficeDocumentInfo
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string FileName { get; set; } = string.Empty; // physical name
    public string OriginalName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

public class DocumentStoreService
{
    private readonly string _dataPath;
    private readonly List<OfficeDocumentInfo> _docs = new();

    public DocumentStoreService(IWebHostEnvironment env)
    {
        _dataPath = Path.Combine(env.WebRootPath, "data");
        Directory.CreateDirectory(_dataPath);
    }

    public IReadOnlyList<OfficeDocumentInfo> GetAll() => _docs.OrderByDescending(d => d.UploadedAt).ToList();

    public async Task<OfficeDocumentInfo> SaveAsync(IFormFile file)
    {
        var id = Guid.NewGuid().ToString("N");
        var ext = Path.GetExtension(file.FileName);
        var savedName = $"{id}{ext}";
        var fullPath = Path.Combine(_dataPath, savedName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var doc = new OfficeDocumentInfo
        {
            Id = id,
            FileName = savedName,
            OriginalName = file.FileName,
            ContentType = file.ContentType,
            UploadedAt = DateTime.UtcNow
        };
        _docs.Add(doc);
        return doc;
    }

    public string? GetPhysicalPath(string id)
    {
        var doc = _docs.FirstOrDefault(d => d.Id == id);
        if (doc == null) return null;
        return Path.Combine(_dataPath, doc.FileName);
    }

    public XLWorkbook LoadExcel(string id)
    {
        var path = GetPhysicalPath(id) ?? throw new FileNotFoundException("Dosya bulunamadı");
        return new XLWorkbook(path);
    }
}