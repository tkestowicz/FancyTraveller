using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Logic;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.POCO;
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
            return service.AvailableCitiesNew.Select(c => new AvailableCityResponse() { Name = c.Name, Id = c.Id });
        }

        [HttpPost]
        public ShortestRouteResponse FindShortestRoute(ShortestRouteRequest request)
        {
            var allDistancesBetweenCities = service.LoadDistancesBetweenCities(request.CitiesToSkip);
            var result = service.FindShortestRoute(request.SourceCity.Id, request.DestinationCity.Id, allDistancesBetweenCities);

            return new ShortestRouteResponse()
            {
                SourceCity = new City()
                {
                    Id = request.SourceCity.Id,
                    Name = request.SourceCity.Name,
                    Location = service.GetLocationOf(request.SourceCity.Name)
                },
                DestinationCity = new City()
                {
                    Id = request.DestinationCity.Id,
                    Name = request.DestinationCity.Name,
                    Location = service.GetLocationOf(request.DestinationCity.Name)
                },
                Distance = result[result.Count - 1]
            };
        }
    }
}