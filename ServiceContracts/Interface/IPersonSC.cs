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

        //bool DeletePerson(Guid personId);

        //PersonResponse GetPersonByEMPID(Guid employeeId);

        //List<PersonResponse> GetFilterPerson(string Searchby, string Searchstring);

        //List<PersonResponse> GetSortedPerson(List<PersonResponse> PersonResponse, string sortBy, SortOrderOptions sortOrderOptions);



    }
}
