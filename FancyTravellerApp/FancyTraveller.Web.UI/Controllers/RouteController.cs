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
        
        [ActionName("AvailableCitites")]
        public IEnumerable<AvailableCityResponse> GetAvailableCities()
        {
            return service.AvailableCities.Select(c => new AvailableCityResponse(){ Name = c });
        }

        [HttpPost]
        public ShortestRouteResponse FindShortestRoute(ShortestRouteRequest request)
        {
            var result = service.FindShortestRoute(request.SourceCity, request.DestinationCity, request.CitiesToSkip);
            
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