using Entity;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Interface;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;


namespace Service
{
    // Implements the IFileData interface
    public class FilesFormatData : IFileData
    {
        // DbContext object for database access
        private readonly DbContextdata _db;

        // Constructor injection of DbContext
        public FilesFormatData(DbContextdata data)
        {
            _db = data;
        }

        // Implements the method declared in IFileData
        public async Task<List<PersonResponse>> PersonData_PDF()
        {
            // Retrieve all person records from database
            List<Person> obj = await _db.Persons.ToListAsync();

            // Create a list to store DTO objects
            List<PersonResponse> result = new List<PersonResponse>();

            // Process each person record
            foreach (Person person in obj)
            {
                // Create a PersonResponse DTO object
                PersonResponse response = new PersonResponse();

                // Copy Person data into DTO
                response.PersonId = person.PersonId;
                response.PersonName = person.PersonName;
                response.PersonEmail = person.PersonEmail;
                response.DateOfBirth = person.DateOfBirth;
                response.Gender = person.Gender;
                response.CountryId = person.CountryId;
                response.Country = person.Country;
                response.Address = person.Address;
                response.ReceiveNewsLetters = person.ReceiveNewsLetters;

                // Add DTO object to the result list
                result.Add(response);
            }

            // Return the list of person data
            return result;
        }

        public async Task<MemoryStream> persondata_CSV()
        {
            // Created a MemoryStream to store the CSV data temporarily
            MemoryStream stream = new MemoryStream();

            // Retrieve all person records from database
            List<Person> obj = await _db.Persons.ToListAsync();

            // Created a StreamWriter to write data into the MemoryStream
            StreamWriter streamobj = new StreamWriter(stream);

            // Created CSV configuration using the default/invariant culture
            CsvConfiguration csvConfigurationobj = new CsvConfiguration(CultureInfo.InvariantCulture);

            // Create CsvWriter to write data in CSV format
            CsvWriter csvWriterObj = new CsvWriter(streamobj, csvConfigurationobj);

            // Write the first column heading: PersonName
            csvWriterObj.WriteField(nameof(PersonResponse.PersonName));
            // Write the second column heading: PersonEmail
            csvWriterObj.WriteField(nameof(PersonResponse.PersonEmail));
            // Move to the next row in the CSV file
            csvWriterObj.NextRecord();

            // Loop through each person record retrieved from the database
            foreach (Person person in obj)
            {
                // Write the person's name into the CSV file
                csvWriterObj.WriteField($"{person.PersonName}");

                // Write the person's email into the CSV file
                csvWriterObj.WriteField($"{person.PersonEmail}");

                // Move to the next row in the CSV file
                csvWriterObj.NextRecord();

            }
            // Write all remaining data from CsvWriter into the stream
            csvWriterObj.Flush();

            // Reset the stream position to the beginning
            stream.Position = 0;

            // Return the MemoryStream containing the CSV data
            return stream;



        }

        public async Task<MemoryStream> Persondata_Excel()
        {
            // Create a MemoryStream to temporarily store the Excel file
            MemoryStream stream = new MemoryStream();

            // Set the EPPlus license for non-commercial use
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            // Create an Excel package
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Create a worksheet named "PersonRecord"
                ExcelWorksheet worksheetObj = excelPackage.Workbook.Worksheets.Add("PersonRecord");

                // Add column headings to the Excel worksheet
                worksheetObj.Cells["A1"].Value = "PersonName";
                worksheetObj.Cells["B1"].Value = "Country";

                // Retrieve all Person records from the database
                List<Person> obj = await _db.Persons.ToListAsync();

                // Start adding person data from the second row
                int row = 2;

                // Loop through each Person record
                foreach (Person person in obj)
                {
                    // Add PersonName to the first column
                    worksheetObj.Cells[row, 1].Value = person.PersonName;
                    // Add Country to the second column
                    worksheetObj.Cells[row, 2].Value = person.Country;
                    // Move to the next row
                    row++;
                }
                // Automatically adjust the column width according to the content
                worksheetObj.Cells[$"A1:B{row}"].AutoFitColumns();

                // Save the Excel package into the MemoryStream
                await excelPackage.SaveAsAsync(stream);
            }
            // Reset the stream position to the beginning
            stream.Position = 0;
            // Return the MemoryStream containing the Excel file
            return stream;
        }


        public async Task<int> ExcelData_to_DB(IFormFile Excelfile)
        {
            // Initialize the counter to keep track of inserted records
            int insertRecord = 0;

            // Set the EPPlus license for non-commercial use
            ExcelPackage.License.SetNonCommercialOrganization("Practice makes perfect");

            // Create a MemoryStream to temporarily store the uploaded Excel file
            MemoryStream memoryStream = new MemoryStream();

            // Copy the uploaded Excel file into the MemoryStream
            await Excelfile.CopyToAsync(memoryStream);

            // Create an ExcelPackage using the uploaded Excel file
            using (ExcelPackage excelobi = new ExcelPackage(memoryStream))
            {
                // Select the first worksheet from the Excel workbook
                ExcelWorksheet worksheet = excelobi.Workbook.Worksheets[0];

                // Get the total number of rows in the worksheet
                int RowsCount = worksheet.Dimension.Rows;

                // Loop through each row of the Excel worksheet
                for (int i = 1; i <= RowsCount; i++)
                {
                    // Read the value from the first column of the current row
                    string? CellValue = Convert.ToString(worksheet.Cells[i, 1].Value);

                    // Check whether the cell contains a value
                    if (CellValue != "" && CellValue != null)
                    {
                        // Create a new Country object
                        Country country = new Country();

                        // Assign the Excel cell value to the CountryName property
                        country.CountryName = CellValue;

                        // Add the Country object to the database context
                        _db.Add(country);

                        // Save the country record into the database
                        await _db.SaveChangesAsync();

                        // Increase the inserted record count
                        insertRecord++;
                    }
                }
            }

            // Return the total number of records inserted into the database
            return insertRecord;
        }
    }

}
