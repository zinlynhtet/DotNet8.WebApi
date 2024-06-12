using AspNetCore.Reporting;
using DotNet8.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroceryDealingSystem.BackendApi.Features
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BaseController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult ExportReport<T>(ReportModel<T> requestModel)
        {
            string mimetype = "";
            int extension = 1;
            //var path = $"{this._webHostEnvironment.WebRootPath}\\ReportFiles\\{requestModel.ReportFileName}.rdlc";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportFiles", requestModel.ReportFileName + ".rdlc");
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource(requestModel.DataSetName, requestModel.DataLst);

            if (requestModel.ReportType == EnumFileType.Pdf)
            {
                ReportResult result = lr.Execute(RenderType.Pdf, extension, 
                                        requestModel.Parameters, mimetype);
                return File(result.MainStream, "application/pdf", $"{requestModel.ExportFileName}.pdf");
            }
            else if (requestModel.ReportType == EnumFileType.Excel)
            {
                ReportResult result = lr.Execute(RenderType.Excel, extension,
                                        requestModel.Parameters, mimetype);
                return File(result.MainStream, "application/msexcel", $"{requestModel.ExportFileName}.xls");
            }

            return Ok();
        }
    }
}
