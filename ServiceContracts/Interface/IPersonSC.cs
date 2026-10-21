using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using ServiceContracts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Interface
{
    public interface IPersonSC
    {
        PersonResponse AddPerson(PersonAddRequest personAddRequest);

        List<PersonResponse> GetAllPersons();

        PersonResponse GetPersonById(Guid personId);

        PersonResponse UpdatePerson(PersonUpdateRequest personupdateRequest);

        Task<bool> DeletePerson(Guid personId);

       

        Task<List<PersonResponse>> GetFilterPerson(string Searchby, string Searchstring);

        Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> PersonResponse, string sortBy, SortOrderOptions sortOrderOptions);



    }
}
