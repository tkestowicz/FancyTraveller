using System.IO;

namespace FancyTraveller.Domain.Infrastracture
{
    public class FileReader : IDataReader
    {
        #region Implementation of IDataReader

        public string ReadData(string resource)
        {
            return File.ReadAllText(resource);
        }

        #endregion
    }
}