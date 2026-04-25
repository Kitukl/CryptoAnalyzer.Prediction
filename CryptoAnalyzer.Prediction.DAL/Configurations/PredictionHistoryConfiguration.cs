using System.Text.Json;
using CryptoAnalyzer.Prediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Prediction.Domain.Configurations;

public class PredictionHistoryConfiguration : IEntityTypeConfiguration<PredictionHistory>
{
    public void Configure(EntityTypeBuilder<PredictionHistory> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.PricePoints)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<IEnumerable<PricePoint>>(v, (JsonSerializerOptions)null)
            );
    }
}