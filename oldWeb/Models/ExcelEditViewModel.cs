namespace OfficeIMO.Collaborative.Web.Models;

public class ExcelEditViewModel
{
    public string SessionId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string SheetName { get; set; } = string.Empty;
    public int Rows { get; set; }
    public int Columns { get; set; }
    public string[,] Cells { get; set; } = new string[0,0];
}
