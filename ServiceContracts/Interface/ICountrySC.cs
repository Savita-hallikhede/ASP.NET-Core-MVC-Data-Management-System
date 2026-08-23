using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Interface
{
    public interface ICountrySC
    {
        CountryResponse AddCountry(CountryAddRequest countryAddRequest);

        List<CountryResponse> GetAllCountries();

        CountryResponse GetCountryById(Guid countryId);

    }
}
