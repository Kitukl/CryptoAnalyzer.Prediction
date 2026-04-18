using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using CryptoAnalyzer.Prediction.Core.DTOs;
using CryptoAnalyzer.Prediction.Domain.Entities;
using CryptoAnalyzer.Prediction.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CryptoAnalyzer.Prediction.Core.Queries;

public sealed class GetForecastForNDaysQuery : IRequest<PredictionForNDaysResponse>
{
    public string CoinId { get; set; }
    public int DaysToPredict { get; set; }
    public int HistoryDays { get; set; }
}

public class GetForecastForNDaysQueryHanlder : IRequestHandler<GetForecastForNDaysQuery, PredictionForNDaysResponse>
{
    private readonly IDistributedCache _cache;
    private readonly HttpClient _httpClient;
    private readonly INewsRepository _newsRepository;

    public GetForecastForNDaysQueryHanlder(IDistributedCache cache, HttpClient httpClient, INewsRepository newsRepository)
    {
        _cache = cache;
        _httpClient = httpClient;
        _newsRepository = newsRepository;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoAnalyzer");
    }

    public async Task<PredictionForNDaysResponse> Handle(GetForecastForNDaysQuery request, CancellationToken cancellationToken)
    {
        var key = $"forecast:{request.CoinId}:{request.DaysToPredict}:{request.HistoryDays}";

        var cachedData = await _cache.GetStringAsync(key, cancellationToken);

        if (cachedData is not null)
        {
            var data = JsonSerializer.Deserialize<Coin>(cachedData);

            if (DateTime.UtcNow.AddMinutes(-15) <= data?.UpdatedAt)
            {
                return new PredictionForNDaysResponse
                {
                    CoinId = data.Id,
                    Predictions = data.PredictedData,
                    UpdatedAt = data.UpdatedAt
                };
            }
        }
        
        var externalData = await _httpClient.GetFromJsonAsync<JsonNode>(
            $"https://api.coingecko.com/api/v3/coins/{request.CoinId}/market_chart?vs_currency=usd&days={request.HistoryDays}&interval=daily");

        var historicalDataRaw = externalData?["prices"]?.AsArray();
        if (historicalDataRaw is null) throw new Exception("Failed to fetch data from CoinGecko");

        var historicalData = historicalDataRaw.Select(c => new PricePoint
        {
            Date = DateTimeOffset.FromUnixTimeMilliseconds(c![0]!.GetValue<long>()).DateTime,
            Price = c[1]!.GetValue<decimal>()
        }).ToList();

        var allNews = await _newsRepository.GetNewsAsync(request.HistoryDays); 
        
        var newsLookup = allNews
            .GroupBy(n => n.Date.Date)
            .ToDictionary(
                g => g.Key, 
                g => (float)g.Average(n => n.Grade)
            );

        var predictionRequest = new PredictiopnForOneDayRequest
        {
            CoinId = request.CoinId,
            History = historicalData.Select(p => new History
            {
                Date = p.Date.Date,
                Price = p.Price,
                Sentiment = newsLookup.TryGetValue(p.Date.Date, out var s) ? s : 0f
            }),
            DaysToPredict = request.DaysToPredict
        };
        
        var response = await _httpClient.PostAsJsonAsync("http://localhost:8000/forecast", predictionRequest);

        if (!response.IsSuccessStatusCode) throw new Exception("ML Prediction service error");

        var predictedData = await response.Content.ReadFromJsonAsync<IEnumerable<PricePoint>>();
        
        var coin = new Coin
        {
            Id = request.CoinId,
            HistoricalData = historicalData,
            PredictedData = predictedData,
            UpdatedAt = DateTime.UtcNow
        };

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(coin));

        return new PredictionForNDaysResponse
        {
            CoinId = request.CoinId,
            Predictions = predictedData,
            UpdatedAt = coin.UpdatedAt
        };
    }
}