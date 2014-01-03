using System;

namespace FancyTraveller.Domain.POCO
{
    public class Vertex : IEquatable<Vertex>
    {
        public City SourceCity { get; set; }
        public City DestinationCity { get; set; }
        public int Distance { get; set; }

        #region Implementation of IEquatable<Vertex>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Vertex other)
        {
            return other != null && SourceCity.Equals(other.SourceCity) && DestinationCity.Equals(other.DestinationCity) && Distance == other.Distance;
        }

        #endregion
    }
}