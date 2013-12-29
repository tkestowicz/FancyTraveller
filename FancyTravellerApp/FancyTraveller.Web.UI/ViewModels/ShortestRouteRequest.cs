using System.Collections.Generic;

namespace FancyTraveller.Web.UI.ViewModels
{
    public class ShortestRouteRequest
    {
        public int SourceCityId { get; set; }
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
        public int DestinationCityId { get; set; }
        public List<string> CitiesToSkip { get; set; }
    }
}