using CryptoAnalyzer.Prediction.Core.Queries;
using CryptoAnalyzer.Prediction.Domain;
using CryptoAnalyzer.Prediction.Domain.Repositories;
using CryptoAnalyzer.Prediction.Extensions;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IPredictionHistoryRepository, PredictionHistoryRepository>();

builder.Services.AddControllers();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(GetForecastForOneDayQueryHandler).Assembly));

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyMethod();
    policy.WithOrigins("http://localhost:5173");
    policy.AllowAnyHeader();
    policy.AllowCredentials();
}));

builder.Services.AddDbContext<PredictionDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "CryptoAnalyzer_";
});

builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddJwtAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JWTOptions>>());

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();