namespace OnlineOfficeWeb.Services;

public class PptRenderService
{
    // TODO: OfficeIMO.PowerPoint entegrasyonu burada yapılacak (slaytları image'e çevirme vb.)

    public string GetSummary(string filePath)
    {
        // #if OFFICEIMO_ENABLED
        // using var ppt = PowerPointPresentation.Load(filePath);
        // return $"Sunumda {ppt.Slides.Count} adet slayt var.";
        // #else
        return "PowerPoint dosyası yüklendi. Bu aşamada yalnızca özet bilgisi örnek olarak gösteriliyor.";
        // #endif
    }
}
