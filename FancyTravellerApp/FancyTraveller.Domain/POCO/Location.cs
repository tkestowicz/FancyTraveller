using System;

namespace FancyTraveller.Domain.POCO
{
    public class Location : IEquatable<Location>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        #region Implementation of IEquatable<Location>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Location other)
        {
            return other != null && Latitude == other.Latitude && Longitude == other.Longitude;
        }

        #endregion
    }
}