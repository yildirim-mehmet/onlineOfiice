
using System.IO;

namespace OnlineOfficeWeb.Services;

public class PdfRenderService {
    public string ExtractText(string filePath) {
        var name = Path.GetFileName(filePath);
        return $"PDF yüklendi: {name} (PDF extract TODO)";
    }
}
