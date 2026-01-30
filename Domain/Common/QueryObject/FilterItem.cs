using System.Text.Json.Serialization;
using Domain.Enums;

namespace Domain.Common.QueryObject;
public class FilterItem<TField> where TField : Enum
{ 
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TField Field { get; set; } = default!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperationFilterEnum Operator { get; set; } = OperationFilterEnum.Equals;
    public object Value { get; set; } = default!;
    public bool IsRequired { get; set; } = false;
}
