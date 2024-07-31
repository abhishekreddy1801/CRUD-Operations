using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDPractice.Controllers
{
    [Route("persons")] //[Route("[controller]")] this will do the same
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;

        public PersonsController(IPersonService personService, ICountryService countryService)
        {
            _personService = personService;
            _countryService = countryService;
        }

        [Route("[action]")] //this route will be considered as ~persons/index, no need to mention specific action explicitly
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            //Search
            ViewBag.ComboBoxFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name"},
                { nameof(PersonResponse.Email), "Email"},
                { nameof(PersonResponse.DateOfBirth), "Date of Birth"},
                { nameof(PersonResponse.Gender), "Gender"},
                { nameof(PersonResponse.CountryId), "Country"},
                { nameof(PersonResponse.Address), "Address"},
            };

            List<PersonResponse> filteredPersons = _personService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = _personService.GetSortedPersons(filteredPersons, sortBy, sortOrder);

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(country => new SelectListItem() { Text = country.CountryName, Value = country.CountryId.ToString() });

            return View();
        }

        [HttpPost]
        [Route("create")] //url would be : persons/create, coz we applied route to whole class no need not to write controller name here it will be automatically taken, if you want to override and dont want to start from controller name start route from / it will do the work
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countryService.GetAllCountries();
                ViewBag.Countries = countries.Select(country => new SelectListItem() { Text = country.CountryName, Value = country.CountryId.ToString() });
                ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(er => er.ErrorMessage).ToList();
                return View();
            }
            _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Edit(Guid personId)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonId(personId);

            if (personResponse is null) return RedirectToAction("Index");

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(country => new SelectListItem() { Text = country.CountryName, Value = country.CountryId.ToString() });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonId(personUpdateRequest.PersonId);

            if (personResponse is null) return RedirectToAction("Index");

            if(ModelState.IsValid)
            {
                PersonResponse updatedPersson = _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countryService.GetAllCountries();
                ViewBag.Countries = countries.Select(country => new SelectListItem() { Text = country.CountryName, Value = country.CountryId.ToString() });
                ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(er => er.ErrorMessage).ToList();
                return View(personResponse.ToPersonUpdateRequest());
            }
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Delete(Guid personId) 
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonId(personId);

            if (personResponse is null) return RedirectToAction("Index");

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (personResponse is null) { return RedirectToAction("Index"); }

            _personService.DeletePerson(personResponse.PersonId);
            return RedirectToAction("Index");
        }
    }
}
