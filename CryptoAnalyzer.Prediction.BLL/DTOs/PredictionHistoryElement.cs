namespace CryptoAnalyzer.Prediction.Core.DTOs;

public class PredictionHistoryElement
{
    public Guid Id { get; set; }
    public string CoinId { get; set; }
    public DateTime CratedAt { get; set; }
}