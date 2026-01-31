using System.Text.Json.Serialization;

namespace CryptoAnalyzer.Prediction.Core.DTOs;

public class PredictiopnForOneDayRequest
{
    [JsonPropertyName("coin_id")]
    public string CoinId { get; set; }
    [JsonPropertyName("history")]
    public IEnumerable<History> History { get; set; }
    [JsonPropertyName("days_to_predict")]
    public int DaysToPredict { get; set; }
}

public class History
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    [JsonPropertyName("sentiment")]
    public double Sentiment { get; set; }
}