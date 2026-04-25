using System.Security.Claims;
using CryptoAnalyzer.Prediction.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAnalyzer.Prediction.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PredictionHistory : ControllerBase
{
    private readonly ISender _mediator;

    public PredictionHistory(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPredictionForUser()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "kitoleg167@gmail.com";
        if (userEmail is null)
        {
            return Unauthorized("User email not provided in claims");
        }

        var response = await _mediator.Send(new GetHistoryForUserQuery
        {
            UserEmail = userEmail
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPredictionById(Guid id)
    {
        var response = await _mediator.Send(new GetPredictionForIdQuery
        {
            Id = id
        });
        return Ok(response);
    }
}