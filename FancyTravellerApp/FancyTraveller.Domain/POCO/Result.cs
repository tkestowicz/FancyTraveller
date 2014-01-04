using System.Collections.Generic;

namespace FancyTraveller.Domain.POCO
{
    public class Result
    {
        public int Distance { get; set; }
        public IList<City> VisitedCities { get; set; }
    }
}
