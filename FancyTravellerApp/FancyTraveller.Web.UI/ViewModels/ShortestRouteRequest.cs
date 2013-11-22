using System.Collections.Generic;

namespace FancyTraveller.Web.UI.ViewModels
{
    public class ShortestRouteRequest
    {
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
        public List<string> CitiesToSkip { get; set; }
    }
}