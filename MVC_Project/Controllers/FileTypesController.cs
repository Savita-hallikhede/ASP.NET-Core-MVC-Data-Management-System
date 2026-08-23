using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.Interface;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using ServiceContracts.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Project.Controllers
{
    public class FileTypesController : Controller
    {
        // Store the IFileData service object
        private IFileData filedata;

        // Constructor injection of IFileData
        public FileTypesController(IFileData data)
        {
            this.filedata = data;
        }



        // HTTP GET method for PDF generation
        public async Task<IActionResult> PDFformat()
        {
            // Get person data from the service
            List<PersonResponse> persondata =   await filedata.PersonData_PDF();

            // Create PDF using the Razor view
            ViewAsPdf pdf = new ViewAsPdf("displayDataPDF", persondata);
            return pdf;
        }

        public async Task<IActionResult> CSVformat()
        {
            // Get person data from the service
            MemoryStream persondata = await filedata.persondata_CSV();

            return File(persondata, "application/octet-stream", "PersonData.CSV");
        }

        public async Task<IActionResult> ExcelFormat()
        {
            MemoryStream excel = await filedata.Persondata_Excel();

            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Person.xlsx");


        }

        [HttpGet]
        public IActionResult ExcelToDatabase()
        {
            // Displays the ExcelToDatabase view/page
            return View();
        }

        [HttpPost]
        public async Task<string> ExcelToDatabase(IFormFile EXCEL)
        {
            // Send the uploaded Excel file to the service layer
            int? count = await filedata.ExcelData_to_DB(EXCEL);

            // Return the number of records inserted into the database
            return "Records Count is " + count.ToString();
        }


       
    }
}
