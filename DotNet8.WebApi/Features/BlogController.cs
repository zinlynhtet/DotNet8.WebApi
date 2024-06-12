using AspNetCore.Reporting;
using DotNet8.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8.WebApi.Features
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _webHostEnvironment = webHostEnvironment;
        }

        private static readonly List<BlogDataModel> lst = new List<BlogDataModel>
        {
            new BlogDataModel { BlogId = 1, BlogTitle = "First Blog",BlogAuthor="MgMg" ,BlogContent = "This is the content of the first blog." },
            new BlogDataModel { BlogId = 2, BlogTitle = "Second Blog",BlogAuthor="MgMg", BlogContent = "This is the content of the second blog." },
            new BlogDataModel { BlogId = 3, BlogTitle = "Third Blog",BlogAuthor="MgMg", BlogContent = "This is the content of the third blog." }
        };


        [HttpGet]
        public async Task<ActionResult<List<BlogDataModel>>> GetBlogs()
        {
            return Ok(lst);
        }

        [HttpGet("report")]
        public async Task<IActionResult> ExecuteAsync()
        {
            var _file = @"ReportFiles\BlogReport.rdlc";
            LocalReport localReport = new LocalReport(_file);
            var lstBlogs = lst;

            var reportParams = new Dictionary<string, string>();
            reportParams.Add("ReportTitle", "BlogList");
            var model = new ReportModel<BlogDataModel>()
            {
                DataSetName = "BlogReportDataSet",
                ReportFileName = "BlogReport",
                ExportFileName = Guid.NewGuid().ToString(),
                ReportType = EnumFileType.Pdf,
                Parameters = reportParams,
                DataLst = lstBlogs
            };

            ExportReport(model);

            var result = localReport.Execute(RenderType.Pdf, 1, parameters: reportParams);
            byte[] byteArray = result.MainStream;

            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExportFiles");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, Guid.NewGuid().ToString() + ".pdf");
            await System.IO.File.WriteAllBytesAsync(filePath, byteArray);
            return Ok();
        }
    }
}
