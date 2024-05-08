using Microsoft.Extensions.Configuration;

namespace Bonum.Shared;

public static class ConfigurationExtensions
{
    public static T GetRequired<T>(this IConfiguration configuration, string key)
    {
        var valueString = configuration[key];
        if (string.IsNullOrWhiteSpace(valueString))
        {
            throw new Exception($"Missing value for key: {key}");
        }

        var value = Convert.ChangeType(valueString, typeof(T));
        return (T)value;
    }
}