using CryptoAnalyzer.Prediction.Core.DTOs;
using CryptoAnalyzer.Prediction.Domain.Repositories;
using MediatR;

namespace CryptoAnalyzer.Prediction.Core.Queries;

public class GetHistoryForUserQuery : IRequest<IEnumerable<PredictionHistoryElement>>
{
    public string UserEmail { get; set; }
}

public class GetHistoryForUserQueryHandler : IRequestHandler<GetHistoryForUserQuery, IEnumerable<PredictionHistoryElement>>
{
    private readonly IPredictionHistoryRepository _historyRepository;

    public GetHistoryForUserQueryHandler(IPredictionHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }
    public async Task<IEnumerable<PredictionHistoryElement>> Handle(GetHistoryForUserQuery request, CancellationToken cancellationToken)
    {
        var response = await _historyRepository.GetAllPredictionForUser(request.UserEmail);
        return response.Select(c => new PredictionHistoryElement
        {
            CoinId = c.CoinId,
            CratedAt = c.CreatedAt,
            Id = c.Id
        });
    }
}