using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Web.UI.ViewModels
{
    public class ShortestRouteRequest
    {
        public City SourceCity { get; set; }
        public City DestinationCity { get; set; }
        public List<string> CitiesToSkip { get; set; }
    }
}