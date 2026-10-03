using LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Common.Interfaces
{
    public interface IMonoService
    {  // Wraps calls to Mono's API. Keeping this as an interface means the handler
       // never touches HttpClient directly, and we can easily mock this in tests later.
         Task<MonoExchangeTokenResult> ExchangeTokenAsync(string code, CancellationToken cancellationToken);
        

        
    }
}
