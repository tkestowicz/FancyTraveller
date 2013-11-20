using System.IO;
using System.Web;

namespace FancyTraveller.Domain.Infrastracture
{
    public class ServerFileReader : IDataReader
    {
        #region Implementation of IDataReader

        public string ReadData(string resource)
        {
            return File.ReadAllText(HttpContext.Current.Server.MapPath(resource));
        }

        #endregion
    }
}
