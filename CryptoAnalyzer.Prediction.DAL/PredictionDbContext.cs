using CryptoAnalyzer.Prediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Prediction.Domain;

public class PredictionDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<News> News { get; set; }
    public DbSet<PredictionHistory> PredictionHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PredictionDbContext).Assembly);
    }
}