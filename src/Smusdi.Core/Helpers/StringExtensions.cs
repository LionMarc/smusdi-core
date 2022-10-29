using System.Text.Json;

namespace Smusdi.Core.Helpers;

public static class StringExtensions
{
    public static string ToCamelCase(this string value) => JsonNamingPolicy.CamelCase.ConvertName(value);
}
