using System.Text.Json.Serialization;

namespace ExpressionParserApi.ApiModels;

internal record ExpressionApiModel
{
    [JsonPropertyName("expression")] public string? Expression { get; set; }
    [JsonPropertyName("result")] public decimal? Evaluated { get; set; }
}