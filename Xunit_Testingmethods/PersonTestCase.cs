using Entity;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Service;
using ServiceContracts.DTO;
using ServiceContracts.Interface;

namespace Xunit_Testingmethods
{
    public class PersonTestCase
    {
        private readonly IPersonSC _personservice;
        private readonly ICountrySC _countryservice;

        public PersonTestCase()
        {
            List<Person> personsInitialData = new List<Person>();
            List<Country> countriesInitialData = new List<Country>();

            DbContextMock<DbContextdata> dbContextMock = 
                new DbContextMock<DbContextdata>(
                    new DbContextOptionsBuilder<DbContextdata>().Options);

            DbContextdata dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

            _personservice = new PersonService(dbContext);
            _countryservice = new CountryService(dbContext);

        }
        //--------------------------------------------------------------------------------------------------------------------
        //1)To verify whether the AddPerson() method throws an ArgumentNullException when the user provides null data.

        [Fact]//will be discovered and executed by XUnit.
        public void AddPerson_Null()
        {
            // Check whether AddPerson() throws ArgumentNullException
            PersonAddRequest? personAddRequest = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Call AddPerson() by passing the null request.
                _personservice.AddPerson(personAddRequest);
            });
        }
        //--------------------------------------------------------------------------------------------------------------------





        //--------------------------------------------------------------------------------------------------------------------
        //2)  To verify whether the AddPerson() method throws an ArgumentException when the PersonName is null.
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _personservice.AddPerson(personAddRequest);
            });
        }
        //--------------------------------------------------------------------------------------------------------------------




        //--------------------------------------------------------------------------------------------------------------------
        //3)To verify whether a PersonId is generated and the person data is successfully added to the database when valid details are provided.
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            // Arrange:
            // Create a PersonAddRequest object with valid person details.
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                PersonEmail = "person@example.com",
                Address = "sample address",
                Country = "us",
                Gender = "Male",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            // Act:
            // Call AddPerson() with the valid person details.
            // The method should add the person and return PersonResponce.
            PersonResponse person_response_from_add =
                _personservice.AddPerson(personAddRequest);

            // Get all persons from the database.
            List<PersonResponse> persons_list =
                _personservice.GetAllPersons();

            // Assert:
            // Verify that the returned PersonId is not empty.
            // This confirms that a PersonId was generated.
            Assert.True(person_response_from_add.PersonId != Guid.Empty);

            // Verify that the newly added person exists in the  list.
            Assert.Contains(person_response_from_add, persons_list);
        }
        //--------------------------------------------------------------------------------------------------------------------





        //--------------------------------------------------------------------------------------------------------------------

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            // Arrange
            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                PersonEmail = "smith@example.com",
                Gender = "Male",
                Address = "Address of Smith",
                Country = "USA",
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                PersonEmail = "mary@example.com",
                Gender = "Female",
                Address = "Address of Mary",
                Country = "USA",
                DateOfBirth = DateTime.Parse("2000-02-02"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                PersonEmail = "rahman@example.com",
                Gender = "Male",
                Address = "Address of Rahman",
                Country = "India",
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            // Store all person requests in a list
            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
            person_request_1,
            person_request_2,
            person_request_3
            };

            // Store the responses returned by AddPerson()
            List<PersonResponse> person_response_list_from_add =
                new List<PersonResponse>();

            // Add each person
            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response =
                    _personservice.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            // Act
            // Get all persons from the database
            List<PersonResponse> persons_list_from_get =
                _personservice.GetAllPersons();

            // Assert
            // Check whether every added person exists in the returned list
            foreach (PersonResponse person_response_from_add
                     in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------------------------


        //5)To verify whether GetPersonById() returns null when a null PersonId is provided.To verify whether GetPersonById() returns null when a null PersonId is provided.
        [Fact]
        public void GetPersonById_NullPersonId()
        {
            // Arrange
            Guid? personId = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personservice.GetPersonById(personId.Value);
            });
        }
        //--------------------------------------------------------------------------------------------------------------------

        //6) To verify whether DeletePerson() successfully deletes a person when a valid PersonId is provided.
        [Fact]

        //Arrange
        public async Task DeletePerson_validPersonID()
        {
            CountryAddRequest country_add_request = new CountryAddRequest()
            {
                CountryName = "USA"
            };

          
            CountryResponse country_response_from_add =  _countryservice.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "Jones",
                PersonEmail = "jones@example.com",
                CountryId = country_response_from_add.CountryId,
                Country = "USA",
                DateOfBirth = Convert.ToDateTime("2010-01-01"),
                Gender = "Male",
                Address = "address",
                ReceiveNewsLetters = true
            };

            PersonResponse person_response_from_add =  _personservice.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personservice.DeletePerson(person_response_from_add.PersonId);

            //Assert
            Assert.True(isDeleted);

        }


        //--------------------------------------------------------------------------------------------------------------------
        //We are checking whether our filtering data is working properly or not by using valid data.
        [Fact]
        public async Task GetFilterPerson_SearchByField()
        {
            // Arrange - Create Countries

            CountryAddRequest firstCountryRequest = new CountryAddRequest()
            {
                    CountryName = "USA"
            };

            CountryAddRequest secondCountryRequest = new CountryAddRequest()
            {
                    CountryName = "India"
            };

            CountryResponse firstCountryResponse = _countryservice.AddCountry(firstCountryRequest);

            CountryResponse secondCountryResponse = _countryservice.AddCountry(secondCountryRequest);


            // Arrange - Create Persons

            PersonAddRequest firstPersonRequest = new PersonAddRequest()
            {
                    PersonName = "Smith",
                    PersonEmail = "smith@example.com",
                    Gender = "Male",
                    Address = "Address of Smith",
                    CountryId = firstCountryResponse.CountryId,
                    DateOfBirth = DateTime.Parse("2002-05-06"),
                    ReceiveNewsLetters = true
            };

            PersonAddRequest secondPersonRequest = new PersonAddRequest()
            {
                    PersonName = "Mary",
                    PersonEmail = "mary@example.com",
                    Gender = "Female",
                    Address = "Address of Mary",
                    CountryId = secondCountryResponse.CountryId,
                    DateOfBirth = DateTime.Parse("2000-02-02"),
                    ReceiveNewsLetters = false
            };

            PersonAddRequest thirdPersonRequest = new PersonAddRequest()
            {
                    PersonName = "Rahman",
                    PersonEmail = "rahman@example.com",
                    Gender = "Male",
                    Address = "Address of Rahman",
                    CountryId = secondCountryResponse.CountryId,
                    DateOfBirth = DateTime.Parse("1999-03-03"),
                    ReceiveNewsLetters = true
            };


            // Add all persons

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>()
            {
                  firstPersonRequest,
                  secondPersonRequest,
                  thirdPersonRequest
            };


            // Store the persons that were actually added

            List<PersonResponse> addedPersons = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse addedPerson = _personservice.AddPerson(personRequest);

                addedPersons.Add(addedPerson);
            }


            // Act - Filter persons

            List<PersonResponse> filteredPersons = await _personservice.GetFilterPerson(nameof(Person.Gender),"Female");


            // Assert - Check the matching person

            foreach (PersonResponse addedPerson in addedPersons)
            {
                if (addedPerson.Gender != null)
                {
                    if (addedPerson.Gender.Contains("Female",StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(addedPerson, filteredPersons);
                    }
                }
            }
        }

 //--------------------------------------------------------------------------------------------------------------------


    }
}
