using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Logic;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.POCO;
using FancyTraveller.Domain.Services;
using FancyTraveller.Web.UI.ViewModels;
using Newtonsoft.Json;

namespace FancyTraveller.Web.UI.Controllers
{
    public class RouteController : ApiController
    {
        private readonly RouteService service;

        public RouteController()
        {
            service = new RouteService(new VerticesRepository(ConfigurationManager.AppSettings, new ServerFileReader()), new DijkstraRouteFinder());
        }
        
        [ActionName("AvailableCitites")]
        public IEnumerable<AvailableCityResponse> GetAvailableCities()
        {
            return service.AvailableCities.Select(c => new AvailableCityResponse(){ Name = c });
        }

        [HttpPost]
        public ShortestRouteResponse FindShortestRoute([FromBody] string jsonBody)
        {
            var request = JsonConvert.DeserializeObject<ShortestRouteRequest>(jsonBody);

            var result = service.FindShortestRoute(request.SourceCity, request.DestinationCity, null);
            
            return new ShortestRouteResponse()
            {
                SourceCity = new City()
                {
                    Name = request.SourceCity,
                    Location = service.GetLocationOf(request.SourceCity)
                },
                DestinationCity = new City()
                {
                    Name = request.DestinationCity,
                    Location = service.GetLocationOf(request.DestinationCity)
                },
                Distance = result
            };
        }
    }
}