using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZamaraWebApp.Data;

namespace ZamaraWebApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
		private readonly AppDbContext _context;

		public ReportsController(IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            this.webHostEnvironment = webHostEnvironment;
			_context = context;
		}
        ////public IActionResult Index()
        ////{
        ////    string mimtype = "";
        ////    int extension = 1;
        ////    var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\Report1.rdlc";
        ////    Dictionary<string, string> parameters = new Dictionary<string, string>();
        ////    parameters.Add("RP1", "Welcome to rdlc reporting");
        ////    LocalReport localReport = new LocalReport(path);
        ////    var result = localReport.Execute(RenderType.Pdf,extension ,parameters,mimtype);
        ////    return File(result.MainStream, "application/pdf");
            
        ////}

        public async Task<IActionResult> ViewPdf()
        {
            string mimtype = "";
            int extension = 1;
            var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\Report11.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var users = await _context.Users.ToListAsync();

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("SystemUsers", users);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
            
            return File(result.MainStream, "application/pdf");
            
        }

		public async Task<IActionResult> ViewWord()
		{
			string mimtype = "";
			int extension = 1;
			var path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\Report21.rdlc";
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			var staffs = await _context.Staffs.ToListAsync();

			LocalReport localReport = new LocalReport(path);
			localReport.AddDataSource("StaffDataSet", staffs);
			var result = localReport.Execute(RenderType.Word, extension, parameters, mimtype);

			return File(result.MainStream, "application/docx");

		}
	}
}
