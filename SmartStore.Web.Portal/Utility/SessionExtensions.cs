using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SmartStore.Web.Portal.Utility
{
    public static class SessionExtensions
    {
        public const string SessionCart = "_Cart";

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }
}
