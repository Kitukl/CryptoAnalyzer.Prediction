using CryptoAnalyzer.Prediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Prediction.Domain.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly PredictionDbContext _context;

    public NewsRepository(PredictionDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<News>> GetNewsAsync(int days)
    {
        var news = await _context.News.ToListAsync();

        return news.Where(c => c.Date.AddDays(days) >= DateTime.UtcNow);
    }
}