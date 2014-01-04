using System;
using System.Collections.Generic;

namespace FancyTraveller.Domain.POCO
{
    public class CityEqualityComparer : IEqualityComparer<City>
    {
        #region Implementation of IEqualityComparer<in City>

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(City x, City y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(City obj)
        {
            return obj.Id.GetHashCode() & obj.Name.GetHashCode();
        }

        #endregion
    }

    public class City : IEquatable<City>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        #region Implementation of IEquatable<City>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(City other)
        {
            return other != null && Id == other.Id && Name == other.Name && ((Location == null  && other.Location == null) || Location.Equals(other.Location));
        }

        #endregion
    }
}