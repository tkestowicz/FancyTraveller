namespace FancyTraveller.Domain.POCO
{
    public class Vertex
    {
        public City SourceCity { get; set; }
        public City DestinationCity { get; set; }
        public int Distance { get; set; }
    }
}