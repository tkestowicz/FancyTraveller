using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Script.Serialization;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Model
{
    public class VerticesRepository : IVertexRepository
    {
        private readonly NameValueCollection appSettings;
        private readonly IDataReader dataReader;
        private IEnumerable<Vertex> allVertices;

        private const string DataFileKey = "citiesDataFile";

        public VerticesRepository(NameValueCollection appSettings, IDataReader dataReader)
        {
            this.appSettings = appSettings;
            this.dataReader = dataReader;
        }

        private string ReadJson(NameValueCollection settings)
        {
            if (settings == null || settings.Get(DataFileKey) == null)
                throw new ConfigurationErrorsException(string.Format("Path to the data file under '{0}' key is not set in configuration.", DataFileKey));

            return dataReader.ReadData(settings.Get(DataFileKey));
        }

        private IEnumerable<TPoco> ConvertJsonToEnumerable<TPoco>(string dataInJson)
        {
            return new JavaScriptSerializer().Deserialize<List<TPoco>>(dataInJson);
        }

        #region Implementation of IVertexRepository

        public IEnumerable<Vertex> GetAll()
        {
            return allVertices ?? (allVertices = ConvertJsonToEnumerable<Vertex>(ReadJson(appSettings)));
        }

        #endregion
    }
}