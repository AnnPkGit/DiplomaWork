using Newtonsoft.Json;

namespace WebApp.Services;

public static class SessionExtensions
{
    public static T? GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}