namespace OnlineOfficeWeb.Services;

public class VisioRenderService
{
    // TODO: OfficeIMO.Visio entegrasyonu burada yapılacak (şekilleri/bağlantıları okuma)

    public string GetSummary(string filePath)
    {
        // #if OFFICEIMO_ENABLED
        // using var visio = VisioDocument.Load(filePath);
        // return $"Visio diyagramında {visio.Pages.Count} sayfa bulunmaktadır.";
        // #else
        return "Visio dosyası yüklendi. Bu aşamada yalnızca özet bilgisi örnek olarak gösteriliyor.";
        // #endif
    }
}
