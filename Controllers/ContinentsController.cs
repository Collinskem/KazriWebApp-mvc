using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using System.ServiceModel;
using ZamaraWebApp.Models;

namespace ZamaraWebApp.Controllers
{
    [Authorize]
    //[Authorize(Policy = "posts")]
    public class ContinentsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            // Create a client for the web service
            var client = new CountryInfoServiceSoapTypeClient(
                new BasicHttpBinding(),
                new EndpointAddress("http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso"));

            // Call the ListOfContinentsByName method on the web service
            var response = await client.ListOfContinentsByNameAsync();

            // Convert the response to a list of Continent objects
            var continents = new List<Continent>();
            foreach (var continentInfo in response.Body.ListOfContinentsByNameResult)
            {
                continents.Add(new Continent
                {
                    Code = continentInfo.sCode,
                    Name = continentInfo.sName
                });
            }

            //// Pass the list of continents to the view
            return View(continents);

            
        }
    }
}
