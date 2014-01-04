using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Web.UI.ViewModels
{
    public class ShortestRouteResponse
    {
        public City SourceCity { get; set; }
        public City DestinationCity { get; set; }
        public int Distance { get; set; }
        public IList<City> VisitedCities { get; set; }
    }
}