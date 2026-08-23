using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts.DTO;
using ServiceContracts.Interface;
using System.Security.Cryptography;
namespace MVC_Project.Controllers
{
    public class PersonController : Controller
    {
        private IPersonSC Person;
        private ICountrySC countrydata;
        public PersonController(IPersonSC obj, ICountrySC data) 
        {
            this.Person = obj;
            this.countrydata = data;
        }

        //-----------------------------------------------------------------------------------------------------------


        //without dropdown
        //[HttpGet]
        //public IActionResult ADD()
        //{
        //    return View();
        //}


        //with dropdown
        [HttpGet]
        public IActionResult ADD()
        {
            // Get all countries from the Country table through Country Service
            List<CountryResponse> countriesobj = countrydata.GetAllCountries();

            // Create a list to store dropdown items
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            // Loop through each country
            foreach (CountryResponse country in countriesobj)
            {
                // Create one dropdown item
                SelectListItem Obj = new SelectListItem();

                // CountryName will be displayed to the user in the dropdown
                Obj.Text = country.CountryName;

                // CountryId will be used as the value when a country is selected
                Obj.Value = country.CountryId.ToString();

                // Add the country item to the dropdown list
                selectListItems.Add(Obj);
            }

            // Send the dropdown list from Controller to View using ViewBag
            ViewBag.AllCountrydata_viewbag = selectListItems;

            // Return the Add Person View
            return View();
        }


        [HttpPost]
        public IActionResult ADD(PersonAddRequest dataofPerson)
        {
            PersonResponse response = Person.AddPerson(dataofPerson);

            return View("PersonData",response);
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult GetPerson()
        {
           List<PersonResponse> personsdata = Person.GetAllPersons();
            return View(personsdata);
        }
        //-----------------------------------------------------------------------------------------------------------


        [HttpGet]
        public IActionResult GetDataByID()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetDataByID(PersonResponse response)
        {
            PersonResponse personresult = Person.GetPersonById(response.PersonId);

            return View("PersonDataById", personresult);
        }


        

        //-----------------------------------------------------------------------------------------------------------


        [HttpGet]
        public IActionResult Addsp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Addsp(PersonAddRequest personAddRequest)
        {
            PersonResponse data = Person.AddPerson(personAddRequest);
            return View("displaySpData",data);  
        }

        //-----------------------------------------------------------------------------------------------------------



        [HttpGet]
        public IActionResult PersonData1()
        {
            List<PersonResponse> person = Person.GetAllPersons();

            return View(person);
        }

        //-----------------------------------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            PersonResponse? person = Person.GetPersonById(id);

            if(person == null)
            {
                return NotFound();
            }

            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest();

            personUpdateRequest.PersonId = person.PersonId;
            personUpdateRequest.PersonName = person.PersonName;
            personUpdateRequest.PersonEmail = person.PersonEmail;
            personUpdateRequest.DateOfBirth = person.DateOfBirth;
            personUpdateRequest.Gender = person.Gender;
            personUpdateRequest.CountryId = person.CountryId;
            personUpdateRequest.Address = person.Address;
            personUpdateRequest.ReceiveNewsLetters = person.ReceiveNewsLetters;


            List<CountryResponse> countryobj = countrydata.GetAllCountries();
            List<SelectListItem> selectlist = new List<SelectListItem>();
            foreach(CountryResponse country in countryobj)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = country.CountryName;
                obj.Value = country.CountryId.ToString();

                selectlist.Add(obj);

            }

            ViewBag.AllCountrydata_viewbag = selectlist;    
            return View(personUpdateRequest);


           
        }


        [HttpPost]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
             Person.UpdatePerson(personUpdateRequest);

            return RedirectToAction("PersonData1");
              
            //return View("PersonData" , personResponse);
        }


    }
}

