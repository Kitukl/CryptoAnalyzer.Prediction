using CryptoAnalyzer.Prediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Prediction.Domain.Repositories;

public class PredictionHistoryRepository : IPredictionHistoryRepository
{
    private readonly PredictionDbContext _context;

    public PredictionHistoryRepository(PredictionDbContext context)
    {
        _context = context;
    }
    public async Task<List<PredictionHistory>> GetAllPredictionForUser(string user)
    {
        return await _context.PredictionHistories
            .Where(c => c.UserEmail == user)
            .ToListAsync();
    }

    public async Task<PredictionHistory> GetPredictionForId(Guid id)
    {
        return await _context.PredictionHistories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<PredictionHistory> CreatePredictionForUser(PredictionHistory predictionHistory)
    {
        await _context.PredictionHistories.AddAsync(predictionHistory);
        await _context.SaveChangesAsync();
        return predictionHistory;
    }
}