using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceContracts.DTO;
using ServiceContracts.Enum;
using ServiceContracts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PersonService : IPersonSC
    {
        private readonly DbContextdata _db;

        public PersonService(DbContextdata db)
        {
            _db = db;
        }

        //-----------------------------------------------------------------------------------------------------------
        public PersonResponse AddPerson(PersonAddRequest personAddRequest)
        {

            // Check whether the input request is null
            if (personAddRequest == null)
            {
                // Throw exception if request is null
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            if (personAddRequest.PersonName == null)
            {
                throw new ArgumentException("PersonName cannot be null.", nameof(personAddRequest.PersonName));
            }

            Person person = new Person();
            person.PersonId = Guid.NewGuid();
            person.PersonName = personAddRequest.PersonName;
            person.PersonEmail = personAddRequest.PersonEmail;
            person.DateOfBirth = personAddRequest.DateOfBirth;
            person.Gender = personAddRequest.Gender;
            person.CountryId = personAddRequest.CountryId; ;
            //person.Country = personAddRequest.Country;
            person.Address = personAddRequest.Address;
            person.ReceiveNewsLetters = personAddRequest.ReceiveNewsLetters;


            _db.Persons.Add(person);
           // _db.sp_insert_person(person);
           _db.SaveChanges();

            //--------------------------------------------------------------
            // Demo: Pause for 3 minutes.
            // Update the record in SQL Server during this time.
            // Then fetch the latest data from the database.

            //Thread.Sleep(TimeSpan.FromMinutes(1));

            //--------------------------------------------------------------




            //------------------------------------------------------------------
            // Example:
            // If "Soumya" is changed to "Sony" in the database,
            // this code will still return "Soumya"
            // because it reads data from the entity object.


            PersonResponse personResponse = new PersonResponse();
            personResponse.PersonId = person.PersonId;
            personResponse.PersonName = person.PersonName;
            personResponse.PersonEmail = person.PersonEmail;
            personResponse.DateOfBirth = person.DateOfBirth;
            personResponse.Gender = person.Gender;
            personResponse.CountryId = person.CountryId;
           // personResponse.Country = person.Country;
            personResponse.Address = person.Address;
            personResponse.ReceiveNewsLetters = person.ReceiveNewsLetters;

            // -----------------------------------------------------------------------------



            // Read the latest data from the database and return

            //  Person latestPerson = _db.Persons.AsNoTracking().FirstOrDefault(p => p.PersonId == person.PersonId);

            //AsNoTracking() tells EF Core:
            //"Do not track this entity. Read a fresh copy directly from the database."

            // FirstOrDefault() is a LINQ method that returns the first record matching the given condition.
            // If no record matches, it returns null.In this example, it fetches the latest Person record from the Persons table whose PersonId matches the current person's PersonId.

            /*  PersonResponse response = new PersonResponse();

              response.PersonId = latestPerson.PersonId;
              response.PersonName = latestPerson.PersonName;
              response.PersonEmail = latestPerson.PersonEmail;
              response.DateOfBirth = latestPerson.DateOfBirth;
              response.Gender = latestPerson.Gender;
              response.CountryId = latestPerson.CountryId;
              response.Country = latestPerson.Country;
              response.Address = latestPerson.Address;
              response.ReceiveNewsLetters = latestPerson.ReceiveNewsLetters;*/



            return personResponse;
         }

        //-----------------------------------------------------------------------------------------------------------

        public List<PersonResponse> GetAllPersons()
        {
            // Fetch all person records from the database
            List<Person> person = _db.Persons.ToList();

            // Create an empty list of PersonResponse DTOs
            List<PersonResponse> personResponses = new List<PersonResponse>();

            // Read each person record one by one
            foreach (Person person1 in person)
            {
                // Create a new DTO object
                PersonResponse personResponse = new PersonResponse();

                // Copy data from Entity to DTO
                personResponse.PersonId = person1.PersonId;
                personResponse.PersonName = person1.PersonName;
                personResponse.PersonEmail = person1.PersonEmail;
                personResponse.DateOfBirth = person1.DateOfBirth;
                personResponse.Gender = person1.Gender;
               // personResponse.Country = person1.Country;
                personResponse.Address = person1.Address;
                personResponse.ReceiveNewsLetters = person1.ReceiveNewsLetters;

                personResponses.Add(personResponse);

            }
            // Return all person details
            return personResponses;
        }

        //-----------------------------------------------------------------------------------------------------------

        public PersonResponse GetPersonById(Guid PersonId)
        {

            // Check whether PersonId is null
            if (PersonId == null)
            {
                return null;
            }
            // Find the person from database
            Person? person1 = _db.Persons
                .Where(p => p.PersonId == PersonId)
                .FirstOrDefault();

            // Check whether person exists
            if (person1 == null)
            {
                return null;
            }




            Person person = _db.Persons.Where(p => p.PersonId == PersonId).FirstOrDefault();

            PersonResponse obj = new PersonResponse();
            obj.PersonId = person.PersonId; 
            obj.PersonName = person.PersonName;
            obj.PersonEmail = person.PersonEmail;
            obj.DateOfBirth = person.DateOfBirth;
            obj.Gender = person.Gender;
            //obj.Country = person.Country;
            obj.Address = person.Address;
            obj.ReceiveNewsLetters = person.ReceiveNewsLetters;

            return obj;
        }

        //-----------------------------------------------------------------------------------------------------------

        public PersonResponse UpdatePerson(PersonUpdateRequest personupdateRequest)
        {
            // Find the existing person from the database
 
            Person? person = _db.Persons.Where(p => p.PersonId == personupdateRequest.PersonId).FirstOrDefault();

            if(person == null)
            {
                throw new ArgumentException("Person not found");// Person not found
            }
           
            // Update the entity with new values
            person.PersonName = personupdateRequest.PersonName;
            person.PersonEmail = personupdateRequest.PersonEmail;
            person.DateOfBirth = personupdateRequest.DateOfBirth;
            person.Gender = personupdateRequest.Gender;
            person.CountryId = personupdateRequest.CountryId;
            //person.Country = personupdateRequest.Country;
            person.Address = personupdateRequest.Address;
            person.ReceiveNewsLetters = personupdateRequest.ReceiveNewsLetters;


            // Save changes to the database
            _db.SaveChanges();

            //Convert Entity to DTO
            PersonResponse personResponse = new PersonResponse();

            personResponse.PersonId = person.PersonId;
            personResponse.PersonName = person.PersonName;
            personResponse.PersonEmail = person.PersonEmail;
            personResponse.DateOfBirth = person.DateOfBirth;
            personResponse.Gender = person.Gender;
            personResponse.CountryId = person.CountryId;
          //  personResponse.Country = person.Country;
            personResponse.Address = person.Address;
            personResponse.ReceiveNewsLetters = person.ReceiveNewsLetters;

            return personResponse;


        }

        //-----------------------------------------------------------------------------------------------------------

        public async Task<bool> DeletePerson(Guid personId)
        {
            Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);

            if (personId == null)
            {
                return false;
            }


            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        public async Task<List<PersonResponse>> GetFilterPerson(string Searchby, string Searchstring)
        {
            // Get all persons
            List<PersonResponse> ActuvalData = GetAllPersons();
            List<PersonResponse> FilteredData = ActuvalData;
            if(string.IsNullOrEmpty(Searchby) || (Searchstring == null || Searchstring=="")) 
                return FilteredData;

            switch (Searchby)
            {
                case "PersonName":
                    FilteredData = ActuvalData.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ?
                       temp.PersonName.Contains(Searchstring, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case "Gender":
                    FilteredData = ActuvalData.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ?
                       temp.Gender.Contains(Searchstring, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: FilteredData = ActuvalData;
                    break;
            }

            return FilteredData;
        }

        //-----------------------------------------------------------------------------------------------------------

        public async Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPersons,string sortBy,SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedData = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                _ => allPersons
            };

            return sortedData;
        }
    }


}

