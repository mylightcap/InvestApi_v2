using LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit.MonoDebitWebhook.Commands;
using LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command;
using LightCap.InvestmentApi.Application.Features.MonoService.MonoMandate;
using LightCap.InvestmentApi.Application.Features.MonoService.MonoMandate.Commands;
using LightCap.InvestmentApi.Application.Features.MonoService.Transaction.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command.MonoExchangeTokenHandler;

namespace LightCap.InvestmentApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonoController(ISender sender) : ControllerBase
    {
        public record ExchangeTokenRequest(string Code);

        [Authorize]
        [HttpPost("exchange-token")]
        public async Task<IActionResult> ExchangeToken(ExchangeTokenRequest request)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await sender.Send(new MonoExchangeTokenCommand(userId, request.Code));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors.Select(e => e.Message));
        }


        // NOTE: No [Authorize] here - this is called by Mono's servers, not
        // a logged-in user. Instead, this endpoint MUST verify the request
        // actually came from Mono - see the signature-check note below.
        [HttpPost("transactions")]
        public async Task<IActionResult> ReceiveTransaction(MonoTransactionWebhookPayload payload)
        {
            var result = await sender.Send(new ProcessMonoTransactionCommand(payload));

            // Always return 200 OK even on a "failure" Result, as long as the
            // request itself was well-formed - otherwise Mono will keep
            // retrying a webhook that failed for a reason retrying won't fix
            // (e.g. "no linked account found"). Only return a non-200 for
            // genuinely malformed requests, which ASP.NET model binding
            // already handles before this method even runs.
            return Ok();
        }





        //Mandate Controller

        [HttpPost("mandate")]
        public async Task<IActionResult> ReceiveMandateUpdate(MonoMandateWebhookPayload payload)
        {
            await sender.Send(new ProcessMandateWebhookCommand(payload));
            return Ok(); // always 200 so Mono doesn't endlessly retry
        }


        public record SetupMandateRequest(decimal MaxAmount);
        [Authorize]
        [HttpPost("setup")]
        public async Task<IActionResult> Setup(SetupMandateRequest request)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await sender.Send(new SetupMandateCommand(userId, request.MaxAmount));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors.Select(e => e.Message));
        }


        [HttpPost("debit-confirmation")]
        public async Task<IActionResult> ReceiveDebitConfirmation(MonoDebitWebhookPayload payload)
        {
            await sender.Send(new ProcessDebitWebhookCommand(payload));
            return Ok(); // always 200 so Mono doesn't endlessly retry
        }
    }
}