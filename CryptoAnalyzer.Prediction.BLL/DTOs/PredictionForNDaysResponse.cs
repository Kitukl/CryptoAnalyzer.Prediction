using CryptoAnalyzer.Prediction.Domain.Entities;

namespace CryptoAnalyzer.Prediction.Core.DTOs;

public class PredictionForNDaysResponse
{
    public string CoinId { get; set; }
    public IEnumerable<PricePoint> Predictions { get; set; }
    public DateTime? UpdatedAt { get; set; }
}