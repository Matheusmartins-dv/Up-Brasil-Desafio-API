using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Domain.Common.QueryObject;

public class Filter<TFields> : Pagination where TFields : Enum
{
    public List<FilterItem<TFields>> Items { get; set; } = new List<FilterItem<TFields>>();

    public static bool TryParse(string? value, [NotNullWhen(true)] out Filter<TFields>? result)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        result = new Filter<TFields>();
        return true;
    }

    try
    {
        // ESSA LINHA É CRUCIAL:
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        var items = JsonSerializer.Deserialize<List<FilterItem<TFields>>>(value, options);
        result = new Filter<TFields> { Items = items ?? new() };
        return true;
    }
    catch
    {
        result = new Filter<TFields>();
        return true;
    }
}
}
