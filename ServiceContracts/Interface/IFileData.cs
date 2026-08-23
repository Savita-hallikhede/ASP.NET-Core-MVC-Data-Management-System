using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts.Interface
{
    public interface IFileData
    {
        Task<List<PersonResponse>> PersonData_PDF();

        Task<MemoryStream> persondata_CSV();

        Task<MemoryStream> Persondata_Excel();

        Task<int> ExcelData_to_DB(IFormFile Excelfile);
    }
}
