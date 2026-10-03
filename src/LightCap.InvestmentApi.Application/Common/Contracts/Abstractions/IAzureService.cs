namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions
{
    public interface IAzureService
    {
        Task<bool> SendObjectCreatedTopic<T>(T obj, string topicName, string azureServiceKey);
        Task<bool> SendObjectToQueue<T>(T payload, string queueName, string queueConnectionKey);
    }
}
