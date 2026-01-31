using CryptoAnalyzer.Prediction.Core.DTOs;
using CryptoAnalyzer.Prediction.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAnalyzer.Prediction.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Forecast : ControllerBase
{
    private readonly ISender _mediatr;

    public Forecast(ISender mediatr)
    {
        _mediatr = mediatr;
    }
    
    [HttpGet("{id}/predict/{days}")]
    public async Task<ActionResult<PredictionForOneDayResponse>> GetOneDayPrediction([FromRoute] string id, [FromRoute] int days)
    {
        try
        {
            return Ok(await _mediatr.Send(new GetForecastForOneDayQuery
            {
                CoinId = id,
                Days = days
            }));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}