using System.Text.Json.Serialization;

namespace CryptoAnalyzer.Prediction.Domain.Entities;

public class PricePoint()
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}