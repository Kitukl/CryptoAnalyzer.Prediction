using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using CryptoAnalyzer.Prediction.Core.DTOs;
using CryptoAnalyzer.Prediction.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CryptoAnalyzer.Prediction.Core.Queries;

public sealed class GetForecastForOneDayQuery : IRequest<PredictionForOneDayResponse>
{
    public string CoinId { get; set; }
    public int Days { get; set; }
}

public class GetForecastForOneDayQueryHandler : IRequestHandler<GetForecastForOneDayQuery, PredictionForOneDayResponse>
{
    private readonly IDistributedCache _cache;
    private readonly HttpClient _httpClient;

    public GetForecastForOneDayQueryHandler(IDistributedCache cache, HttpClient httpClient)
    {
        _cache = cache;
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoAnalyzer");
    }
    public async Task<PredictionForOneDayResponse> Handle(GetForecastForOneDayQuery request, CancellationToken cancellationToken)
    {
        var key = $"prediction:{request.CoinId}";

        var cachedData = await _cache.GetStringAsync(key, cancellationToken);

        if (cachedData is not null)
        {
            var data = JsonSerializer.Deserialize<Coin>(cachedData);

            if (DateTime.UtcNow.AddMinutes(-15) <= data?.UpdatedAt)
            {
                return new PredictionForOneDayResponse
                {
                    CoinId = data.Id,
                    PredictedPrice = data.PredictedData?.LastOrDefault()?.Price
                };
            }
        }

        var externalData = await _httpClient.GetFromJsonAsync<JsonNode>($"https://api.coingecko.com/api/v3/coins/{request.CoinId}/market_chart?vs_currency=usd&days=365&interval=daily");

        var historicalDataRaw = externalData?["prices"]?.AsArray();

        if (historicalDataRaw is null) throw new Exception("External API");

        var historicalData = historicalDataRaw.Select(c => new PricePoint
        {
            Date = DateTimeOffset.FromUnixTimeMilliseconds(c![0]!.GetValue<long>()).DateTime,
            Price = c[1]!.GetValue<decimal>()
        }).ToList();

        var predictionRequest = new PredictiopnForOneDayRequest
        {
            CoinId = request.CoinId,
            History = historicalData.Select(p => new History
            {
                Date = p.Date.Date,
                Price = p.Price,
                Sentiment = 0
            }),
            DaysToPredict = request.Days
        };

        var response = await _httpClient.PostAsJsonAsync("http://localhost:8000/forecast", predictionRequest);

        if (!response.IsSuccessStatusCode) throw new Exception("Predicting service broken");

        var predictedData = await response.Content.ReadFromJsonAsync<IEnumerable<PricePoint>>();
        
        var coin = new Coin
        {
            Id = request.CoinId,
            HistoricalData = historicalData,
            PredictedData = predictedData,
            UpdatedAt = DateTime.UtcNow
        };

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(coin));

        return new PredictionForOneDayResponse
        {
            CoinId = request.CoinId,
            PredictedPrice = predictedData.LastOrDefault()?.Price
        };
    }
}