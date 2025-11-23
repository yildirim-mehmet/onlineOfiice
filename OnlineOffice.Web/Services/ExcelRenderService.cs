// 📁 OnlineOfficeWeb/Services/ExcelRenderService.cs

using OfficeIMO.Excel;
using System.Collections.Generic;

namespace OnlineOfficeWeb.Services {

    public record ExcelCellDto(int Row, int Column, string? Text);
    public record ExcelSheetDto(string Name, List<ExcelCellDto> Cells);
    public record ExcelWorkbookDto(List<ExcelSheetDto> Sheets);

    public class ExcelRenderService {

        // NOT: Burada henüz Excel'in gerçek Dimension bilgisini kullanmıyoruz,
        // TryGetCellText ile mantıklı bir aralık tarıyoruz.
        private const int MaxRowsToScan = 1000;  // İhtiyaca göre arttırılabilir
        private const int MaxColsToScan = 50;    // İlk 50 sütun yeterli bizim senaryoda

        public ExcelWorkbookDto RenderToDto(string filePath) {

            var sheetsDto = new List<ExcelSheetDto>();

            // Excel dosyasını yükle
            var doc = ExcelDocument.Load(filePath);

            foreach (var sheet in doc.Sheets) {

                var cells = new List<ExcelCellDto>();

                // ✔ Sadece public API: TryGetCellText kullanıyoruz
                // ✔ satır/sütunları sabit bir aralıkta tarıyoruz
                int consecutiveEmptyRows = 0;

                for (int r = 1; r <= MaxRowsToScan; r++) {
                    bool rowHasAnyData = false;

                    for (int c = 1; c <= MaxColsToScan; c++) {
                        sheet.TryGetCellText(r, c, out string text);

                        if (!string.IsNullOrWhiteSpace(text)) {
                            rowHasAnyData = true;
                            cells.Add(new ExcelCellDto(
                                Row: r,
                                Column: c,
                                Text: text
                            ));
                        }
                    }

                    if (!rowHasAnyData) {
                        consecutiveEmptyRows++;
                    } else {
                        consecutiveEmptyRows = 0;
                    }

                    // Arka arkaya 10 boş satır gördüysek muhtemelen sonuna geldik → erken çıkış
                    if (consecutiveEmptyRows >= 10 && r > 50) {
                        break;
                    }
                }

                sheetsDto.Add(new ExcelSheetDto(sheet.Name, cells));
            }

            return new ExcelWorkbookDto(sheetsDto);
        }
    }
}
