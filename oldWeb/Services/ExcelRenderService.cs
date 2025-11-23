using System.Collections.Generic;

namespace OnlineOfficeWeb.Services;

public record ExcelCellDto(int Row, int Column, string? Text);
public record ExcelSheetDto(string Name, List<ExcelCellDto> Cells);
public record ExcelWorkbookDto(List<ExcelSheetDto> Sheets);

public class ExcelRenderService
{
    // TODO: OfficeIMO.Excel entegrasyonu burada yapılacak

    public ExcelWorkbookDto RenderToDto(string filePath)
    {
        // #if OFFICEIMO_ENABLED
        // var excel = ExcelDocument.Load(filePath);
        // var sheets = new List<ExcelSheetDto>();
        // foreach (var sheet in excel.Sheets)
        // {
        //     var cells = sheet.Cells
        //         .Select(c => new ExcelCellDto(c.Row, c.Column, c.Text))
        //         .ToList();
        //     sheets.Add(new ExcelSheetDto(sheet.Name, cells));
        // }
        // return new ExcelWorkbookDto(sheets);
        // #else
        var dummySheet = new ExcelSheetDto("ÖrnekSayfa", new List<ExcelCellDto>
        {
            new ExcelCellDto(1, 1, "OfficeIMO entegrasyonu"),
            new ExcelCellDto(1, 2, "bu aşamada örnek veri ile çalışıyor.")
        });
        return new ExcelWorkbookDto(new List<ExcelSheetDto> { dummySheet });
        // #endif
    }
}
