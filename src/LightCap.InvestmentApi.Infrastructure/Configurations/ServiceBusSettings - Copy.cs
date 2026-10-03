namespace LightCap.InvestmentApi.Infrastructure.Configurations;

public record ServiceBusSettings
{
    public const string Path = "AzureServiceBus";
    public string SubscriptionName { get; init; } = string.Empty;
    public Dictionary<string, NamespaceSettings> Namespaces { get; init; } = new();
}

public record NamespaceSettings
{
    public string ConnectionString { get; init; } = string.Empty;
    public Dictionary<string, string> Queues { get; init; } = new();
    public Dictionary<string, string> Topics { get; init; } = new();
}

// Access topics
// var residentialTopic = settings.Namespaces["Alat1"].Topics["ResidentialAddressFilledV2"];
// var addressesTopic = settings.Namespaces["Alat2"].Topics["AddressesUpload"];

// Access queues
// var feedbackQueue = settings.Namespaces["Alat2"].Queues["AddressVendorFeedback"];