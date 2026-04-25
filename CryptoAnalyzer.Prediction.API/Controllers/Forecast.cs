using System.Security.Claims;
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
    
    [HttpGet("{id}/forecast/{daysToPredict}/{historyDays}")]
    public async Task<ActionResult<PredictionForNDaysResponse>> GetForecastList(
        [FromRoute] string id, 
        [FromRoute] int daysToPredict, 
        [FromRoute] int historyDays)
    {
        try
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "kitoleg167@gmail.com";
            var query = new GetForecastForNDaysQuery
            {
                CoinId = id,
                DaysToPredict = daysToPredict,
                UserEmail = userEmail,
                HistoryDays = historyDays
            };

            var result = await _mediatr.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}