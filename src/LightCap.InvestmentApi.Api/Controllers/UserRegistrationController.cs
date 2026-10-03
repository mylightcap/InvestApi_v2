using LightCap.InvestmentApi.Application.Features.Auth.Login.Commands;
using LightCap.InvestmentApi.Application.Features.Auth.Logout.Commands;
using LightCap.InvestmentApi.Application.Features.Auth.Registration.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LightCap.InvestmentApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistrationController(ISender sender) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationCommand command)
        {
            var result = await sender.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors.Select(e => e.Message));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginCommand command)
        {
            var result = await sender.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors.Select(e => e.Message));
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await sender.Send(new UserLogoutCommand(userId));
            return result.IsSuccess ? Ok() : BadRequest(result.Errors.Select(e => e.Message));
        }
    }
}
