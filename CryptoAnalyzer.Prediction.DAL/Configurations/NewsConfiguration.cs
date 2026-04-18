using CryptoAnalyzer.Prediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Prediction.Domain.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Text);
    }
}