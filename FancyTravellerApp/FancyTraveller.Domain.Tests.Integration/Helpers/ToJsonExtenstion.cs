using System.Web.Script.Serialization;

namespace FancyTraveller.Domain.Tests.Integration.Helpers
{
    public static class ToJsonExtenstion
    {
        public static string ToJson(this object toSerialize)
        {
            return new JavaScriptSerializer().Serialize(toSerialize);
        }
    }
}
