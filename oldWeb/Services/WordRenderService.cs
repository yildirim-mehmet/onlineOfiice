namespace OnlineOfficeWeb.Services;

public class WordRenderService
{
    // TODO: OfficeIMO.Word entegrasyonu burada yapılacak
    // using OfficeIMO.Word;
    // using OfficeIMO.Converters.Html; vb.

    public string RenderToHtml(string filePath)
    {
        // #if OFFICEIMO_ENABLED
        // using var doc = WordDocument.Load(filePath);
        // var html = WordToHtmlConverter.Convert(doc);
        // return html;
        // #else
        return "<p><em>Word dosyası başarıyla yüklendi. OfficeIMO entegrasyonu bu aşamada henüz etkin değil.</em></p>";
        // #endif
    }
}
