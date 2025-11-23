
namespace OnlineOfficeWeb.Models;

public enum OfficeFileType { Word, Excel, PowerPoint, Pdf, Visio, Unknown }

public class UploadedFileInfo {
    public string OriginalFileName { get; set; } = "";
    public string StoredFileName { get; set; } = "";
    public string RelativePath { get; set; } = "";
    public OfficeFileType FileType { get; set; }
}
