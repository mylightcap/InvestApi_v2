using LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Common.Interfaces
{
    public interface IMonoDebitService
    {
        Task<MonoDebitResult> DebitAsync(MonoDebitRequest request, CancellationToken cancellationToken);
    }
}
