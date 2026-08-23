
using ServiceContracts.Interface;

using ServiceContracts.DTO;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CountryService : ICountrySC
    {

       // ---------------------------------------------------------------------------------------------------
        private List<Country> _countries;

        private readonly DbContextdata _db;

        public CountryService(DbContextdata db)
        {
            _db = db;
        }

        public CountryResponse AddCountry(CountryAddRequest countryAddRequest) 
        {
         Country countryobj = new Country();

         countryobj.CountryName = countryAddRequest.CountryName;

            // countryobj.CountryId = Guid.Parse("2712D163-E2AA-4307-8A63-406A3853F2CB");
            countryobj.CountryId = Guid.NewGuid();
            //_countries = new List<Country>();
            //_countries.Add(countryobj);


            try
            {
                _db.Countries.Add(countryobj);
                int rows = _db.SaveChanges();
                Console.WriteLine(rows);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            CountryResponse countryresp = new CountryResponse();
            countryresp.CountryId = countryobj.CountryId;
            countryresp.CountryName = countryobj.CountryName;
            return countryresp;

        }

        // ---------------------------------------------------------------------------------------------------


        public List<CountryResponse> GetAllCountries()
        {


            //----display hardcoded data
            /*List<CountryResponse> countryResponsesObj = new List<CountryResponse>();

            CountryResponse obj = new CountryResponse();
            obj.CountryId = Guid.Parse("6DDA6667-6B67-454B-8DF0-40B0DC72505D");
            obj.CountryName = "Austrelia";

            CountryResponse obj1 = new CountryResponse();
            obj1.CountryId = Guid.Parse("2712D163-E2AA-4307-8A63-406A3853F2CB");
            obj1.CountryName = "UK";

            CountryResponse obj2 = new CountryResponse();
            obj2.CountryId = Guid.Parse("2712D163-E2AA-4307-8A63-406A3853F2CB");
            obj2.CountryName = "Newzland";


            countryResponsesObj.Add(obj);
            countryResponsesObj.Add(obj1);
            countryResponsesObj.Add(obj2);


            return countryResponsesObj;*/


            //--------------Fetching data from database
            List<Country> countries = _db.Countries.ToList();
            List<CountryResponse> countryResponsesObj = new List<CountryResponse>();
            foreach (Country country in countries)
            {
                CountryResponse response = new CountryResponse();

                response.CountryId = country.CountryId;
                response.CountryName = country.CountryName;

                countryResponsesObj.Add(response);
            }

            return countryResponsesObj;
        }
        //---------------------------------------------------------------------------------------------------

        public CountryResponse GetCountryById(Guid countryId)
        {
            Country countries = _db.Countries.Where(temp => temp.CountryId == countryId).SingleOrDefault();


            CountryResponse obj = new CountryResponse
            { CountryName = countries.CountryName, CountryId = countries.CountryId };
            return obj;

        }
    }


    }
    
