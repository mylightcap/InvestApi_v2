using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Infrastructure.Services.BVN.Queries
{
    public record BvnQueryCommand(string bvn) : IRequest<Result<BvnQueryHandler>>;
    public class BvnQueryHandler(ILogger logger) : IRequestHandler<BvnQueryCommand, Result<BvnQueryHandler>>
    {
        public Task<Result<BvnQueryHandler>> Handle(BvnQueryCommand request, CancellationToken cancellationToken)
        {
            logger.LogWarning("BVN Verification | At this Point BNV is Checked");

            throw new NotImplementedException();
        }
    }
}
