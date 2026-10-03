namespace LightCap.InvestmentApi.Infrastructure.Configurations;

public record JwtSettings
{
    public const string Path = "Security:Jwt";

    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}