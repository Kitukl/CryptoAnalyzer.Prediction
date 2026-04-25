using CryptoAnalyzer.Prediction.Core.DTOs;
using CryptoAnalyzer.Prediction.Domain.Repositories;
using MediatR;

namespace CryptoAnalyzer.Prediction.Core.Queries;

public class GetPredictionForIdQuery : IRequest<PredictionForNDaysResponse>
{
    public Guid Id { get; set; }
}

public class GetPredictionForIdQueryHandler : IRequestHandler<GetPredictionForIdQuery, PredictionForNDaysResponse>
{
    private readonly IPredictionHistoryRepository _historyRepository;

    public GetPredictionForIdQueryHandler(IPredictionHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }
    public async Task<PredictionForNDaysResponse> Handle(GetPredictionForIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _historyRepository.GetPredictionForId(request.Id);
        return new PredictionForNDaysResponse
        {
            Predictions = response.PricePoints,
            CoinId = response.CoinId,
            UpdatedAt = response.CreatedAt
        };
    }
}