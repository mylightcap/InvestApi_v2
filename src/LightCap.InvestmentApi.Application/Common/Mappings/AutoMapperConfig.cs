using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace LightCap.InvestmentApi.Application.Common.Mappings;

public static class AutoMapperConfig
{
    public static IMapper Configure()
    {
        var config = new MapperConfiguration(cfg =>
        {           

            // Enable record mapping support
            cfg.ShouldUseConstructor = constructor =>
                constructor.IsPublic;

            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;
        }, new NullLoggerFactory());

        // Add detailed validation
        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }
}