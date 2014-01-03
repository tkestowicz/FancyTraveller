using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Web.UI.ViewModels
{
    public class AvailableCityResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Location Location { get; set; }
    }
}