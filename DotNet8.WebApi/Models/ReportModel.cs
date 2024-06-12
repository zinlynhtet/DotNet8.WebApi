using DotNet8.WebApi.Models;

public class ReportModel<T>
{
    public string DataSetName { get; set; }
    public string ReportFileName { get; set; }
    public string ExportFileName { get; set; }
    public EnumFileType ReportType { get; set; }
    public Dictionary<string, string> Parameters { get; set; }
    public List<T> DataLst { get; set; }
}