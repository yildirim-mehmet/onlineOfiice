namespace OnlineOfficeWeb.Services;

public class PdfRenderService
{
    // TODO: OfficeIMO.Pdf ile gerçek pdf okuma/parse burada yapılacak

    public string ExtractText(string filePath)
    {
        // #if OFFICEIMO_ENABLED
        // var reader = new PdfReadDocument(filePath);
        // var text = PdfTextExtractor.ReadAllText(reader);
        // return text;
        // #else
        return "PDF dosyası yüklendi. Bu aşamada yalnızca örnek bir metin gösterilmektedir.";
        // #endif
    }
}
