using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LightCap.InvestmentApi.Api.Common;

[ApiController]
public abstract class BaseController(ISender mediator) : ControllerBase
{
    protected readonly ISender Mediator = mediator;

    protected async Task<IActionResult> Send<TResponse>(IRequest<TResponse> request)
    {
        var result = await Mediator.Send(request);
        return Ok(result);
    }

    protected async Task<IActionResult> Send(IRequest request)
    {
        await Mediator.Send(request);
        return NoContent();
    }
}