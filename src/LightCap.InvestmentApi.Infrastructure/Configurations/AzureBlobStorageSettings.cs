namespace LightCap.InvestmentApi.Infrastructure.Configurations;

public record AzureBlobStorageSettings
{
    public const string Path = "AzureBlobStorage";

    public string BackOfficeMediaBlobContainerName { get; init; } = string.Empty;
}