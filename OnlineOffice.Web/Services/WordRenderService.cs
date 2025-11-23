
using System.IO;

namespace OnlineOfficeWeb.Services;

public class WordRenderService {
    public string RenderToHtml(string filePath) {
        var name = Path.GetFileName(filePath);
        return $"<p>Word Dosyası: <b>{name}</b> (HTML render TODO)</p>";
    }
}
