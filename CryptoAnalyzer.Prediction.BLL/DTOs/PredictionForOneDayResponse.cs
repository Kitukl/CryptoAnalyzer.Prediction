namespace CryptoAnalyzer.Prediction.Core.DTOs;

public class PredictionForOneDayResponse
{
    public string CoinId { get; set; }
    public decimal? PredictedPrice { get; set; }
}