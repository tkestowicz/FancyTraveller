using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Logic;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.POCO;
using FancyTraveller.Domain.Services;
using FancyTraveller.Domain.Tests.Integration.TestingData;
using NUnit.Framework;
using Should;

namespace FancyTraveller.Domain.Tests.Integration.Tests
{
    [TestFixture]
    public class RouteServiceTests
    {

        public class VerticesEqualityComparer : IEqualityComparer<IDictionary<int, IList<Vertex>>>
        {
            #region Implementation of IEqualityComparer<in IDictionary<int,IList<Vertex>>>

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
            public bool Equals(IDictionary<int, IList<Vertex>> x, IDictionary<int, IList<Vertex>> y)
            {
                if (x.Keys.Count != y.Keys.Count) return false;
                
                foreach (var key in x.Keys)
                {
                    if (!y.Keys.Contains(key))
                        return false;

                    if(x[key].Count != y[key].Count) return false;

                    for(int i = 0; i < x[key].Count; i++)
                    {
                        if(x[key].ElementAt(i).Equals(y[key].ElementAt(i)) == false) return false;
                    }
                }
                return true;
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <returns>
            /// A hash code for the specified object.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
            public int GetHashCode(IDictionary<int, IList<Vertex>> obj)
            {
                return obj.Keys.GetHashCode() & obj.Values.GetHashCode();
            }

            #endregion
        }

        public class CitiesEqualityComparer : IEqualityComparer<IList<City>>
        {
            #region Implementation of IEqualityComparer<in IList<City>>

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
            public bool Equals(IList<City> x, IList<City> y)
            {
                if(x.Count != y.Count) return false;

                for (int i = 0; i < x.Count; i++)
                    if (x.ElementAt(i).Equals(y.ElementAt(i)) == false) return false;

                return true;
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <returns>
            /// A hash code for the specified object.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
            public int GetHashCode(IList<City> obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }

        private IRouteService service;

        [SetUp]
        public void InitializeContext()
        {
            this.service = new RouteService(new VerticesRepository(ConfigurationManager.AppSettings, new FileReader()), new DijkstraRouteFinder());
        }

        [Test]
        public void get_available_cities__happy_path__list_of_available_cities_is_returned()
        {
            var result = service.AvailableCities;

            var expectedListOfCitites = RouteServiceTestingData.AvailableCities;
            
            result.ShouldEqual(expectedListOfCitites, new CitiesEqualityComparer());
        }

        [Test]
        public void get_all_verticies___happy_path_with_no_citites_to_skip___list_of_verticies_is_returned()
        {
            var noCititesToSkip = new int[0];

            var result = service.LoadDistancesBetweenCities(noCititesToSkip);

            var expectedListOfVerticies = RouteServiceTestingData.Vertices;

            result.ShouldEqual(expectedListOfVerticies, new VerticesEqualityComparer());
        }

        [Test]
        public void get_verticies___cities_to_skip_passed___list_of_verticies_without_cities_to_skip()
        {
            var cititesToSkip = new[] { 3, 7, 11 }; // 3- Athens, 7- Brussels, 19 - Luxembourg

            var result = service.LoadDistancesBetweenCities(cititesToSkip);

            var allVerticies = RouteServiceTestingData.Vertices;
            var expectedListOfVerticies = allVerticies
                .Where(v => cititesToSkip.Contains(v.Key) == false)
                .Select(v => new KeyValuePair<int, IList<Vertex>>(v.Key, v.Value.Where(element => cititesToSkip.Contains(element.DestinationCity.Id) == false && cititesToSkip.Contains(element.SourceCity.Id) == false).ToList()))
                .ToDictionary(key => key.Key, value => value.Value);

            result.ShouldEqual(expectedListOfVerticies, new VerticesEqualityComparer());
        }

    }
}
