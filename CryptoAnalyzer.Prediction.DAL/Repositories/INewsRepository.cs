using CryptoAnalyzer.Prediction.Domain.Entities;

namespace CryptoAnalyzer.Prediction.Domain.Repositories;

public interface INewsRepository
{
    public Task<IEnumerable<News>> GetNewsAsync(int days);
}