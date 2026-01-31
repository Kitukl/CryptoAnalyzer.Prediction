namespace CryptoAnalyzer.Prediction.Domain.Entities;

public class Coin
{
    public required string Id { get; set; }
    public required IEnumerable<PricePoint> HistoricalData { get; set; }
    public IEnumerable<PricePoint>? PredictedData { get; set; }
    public string? MetaData { get; set; }
    public DateTime? UpdatedAt { get; set; }
}   