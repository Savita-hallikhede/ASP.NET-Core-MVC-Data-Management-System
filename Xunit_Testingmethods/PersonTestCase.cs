using Entity;
using Microsoft.EntityFrameworkCore;
using Service;
using ServiceContracts.DTO;
using ServiceContracts.Interface;

namespace Xunit_Testingmethods
{
    public class PersonTestCase
    {
        private readonly IPersonSC _personservice;

        public PersonTestCase()
        {

            DbContextdata obj1 = new DbContextdata(new DbContextOptionsBuilder<DbContextdata>().Options);
            PersonService personService = new PersonService(obj1);
            _personservice = personService;
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







    }
}
