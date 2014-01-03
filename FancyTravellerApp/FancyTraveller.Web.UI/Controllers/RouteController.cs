using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Logic;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.Services;
using FancyTraveller.Web.UI.ViewModels;

namespace FancyTraveller.Web.UI.Controllers
{
    public class RouteController : ApiController
    {
        private readonly RouteService service;

        public RouteController()
        {
            service = new RouteService(new VerticesRepository(ConfigurationManager.AppSettings, new ServerFileReader()), new DijkstraRouteFinder());
        }
        
        [ActionName("AvailableCities")]
        public IEnumerable<AvailableCityResponse> GetAvailableCities()
        {
            return service.AvailableCities.Select(c => new AvailableCityResponse() { Name = c.Name, Id = c.Id, Location = c.Location });
        }

        [HttpPost]
        public ShortestRouteResponse FindShortestRoute(ShortestRouteRequest request)
        {
            var allDistancesBetweenCities = service.LoadDistancesBetweenCities(request.CitiesToSkip);
            var result = service.FindShortestRoute(request.SourceCity.Id, request.DestinationCity.Id, allDistancesBetweenCities);

            return new ShortestRouteResponse()
            {
                SourceCity = request.SourceCity,
                DestinationCity = request.DestinationCity,
                Distance = result[result.Count - 1]
            };
        }
    }
}