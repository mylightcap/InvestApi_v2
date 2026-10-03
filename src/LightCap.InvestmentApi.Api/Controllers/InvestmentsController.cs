using LightCap.InvestmentApi.Api.Common;
using LightCap.InvestmentApi.Application.Features.Investment.InvestmentPreference.Commands;
using LightCap.InvestmentApi.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LightCap.InvestmentApi.Application.Features.Investment.InvestmentPreference.Commands.UpdateInvestmentPreferences;

namespace LightCap.InvestmentApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserPreferencesController(ISender sender) : ControllerBase
{
    

    [Authorize]
    [HttpPost("investment-preferences")]
    public async Task<IActionResult> UpdateInvestmentPreferences(UpdatePreferencesRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await sender.Send(new UpdateInvestmentPreferencesCommand(userId, request.AutoInvestPercentage, request.InvestmentMode));

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Errors.Select(e => e.Message));
    }
}