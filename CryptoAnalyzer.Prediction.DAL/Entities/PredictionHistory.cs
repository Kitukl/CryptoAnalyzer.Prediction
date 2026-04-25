namespace CryptoAnalyzer.Prediction.Domain.Entities;

public class PredictionHistory
{
    public Guid Id { get; set; }
    public string CoinId { get; set; }
    public IEnumerable<PricePoint> PricePoints { get; set; }
    public string UserEmail { get; set; }
    public DateTime CreatedAt { get; set; }
}