using CryptoAnalyzer.Prediction.Domain.Entities;

namespace CryptoAnalyzer.Prediction.Domain.Repositories;

public interface IPredictionHistoryRepository
{
    public Task<List<PredictionHistory>> GetAllPredictionForUser(string user);
    public Task<PredictionHistory> GetPredictionForId(Guid id);
    public Task<PredictionHistory> CreatePredictionForUser(PredictionHistory predictionHistory);
}