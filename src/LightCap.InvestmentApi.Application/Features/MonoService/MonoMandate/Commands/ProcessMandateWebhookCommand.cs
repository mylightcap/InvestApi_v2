using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Domain.Entities;
using LightCap.InvestmentApi.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoMandate.Commands
{
    public record ProcessMandateWebhookCommand(MonoMandateWebhookPayload Payload) : IRequest<Result>;

    public class ProcessMandateWebhookHandler(IRepository<DirectDebitMandate> mandateRepository
    ) : IRequestHandler<ProcessMandateWebhookCommand, Result>
    {
        public async Task<Result> Handle(ProcessMandateWebhookCommand request, CancellationToken cancellationToken)
        {
            var mandate = await mandateRepository.GetSingleAsync(
                x => x.MonoMandateId == request.Payload.Data.Id);

            if (mandate == null)
                return Result.Fail("No matching mandate found for this webhook.");

            // NOTE: confirm the exact event name against Mono's dashboard/docs when testing -
            // "mandate.ready_to_debit" is the documented concept, exact string may differ.
            if (request.Payload.Event.Contains("ready", StringComparison.OrdinalIgnoreCase))
            {
                mandate.Status = MandateStatus.ReadyToDebit;
                mandate.ReadyAt = DateTime.UtcNow;
            }
            else if (request.Payload.Event.Contains("fail", StringComparison.OrdinalIgnoreCase))
            {
                mandate.Status = MandateStatus.Failed;
            }

            await mandateRepository.UpdateAsync(mandate);
            await mandateRepository.SaveChanges(cancellationToken);

            return Result.Ok();
        }
    }
}
