using System.Text.Json.Serialization;

namespace ExpressionParserApi.DtoModels;

internal record ExpressionDtoModel
{
    [JsonPropertyName("expression")] public string? Expression { get; set; }
}