namespace OnlineOfficeWeb.Models;

public enum OfficeFileType
{
    Word,
    Excel,
    PowerPoint,
    Pdf,
    Visio,
    Unknown
}

public class UploadedFileInfo
{
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public OfficeFileType FileType { get; set; }
}
