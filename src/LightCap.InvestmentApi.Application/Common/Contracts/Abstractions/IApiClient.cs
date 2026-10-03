namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;

public interface IApiClient
{
    Task<TResponse> GetAsync<TResponse>(string uri, string clientName = "",
        Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

    Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data, string clientName = "",
        Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

    Task<TResponse> PostStringAsync<TResponse>(
    string uri,
    string data,
    string clientName = "",
    Dictionary<string, string>? headers = null,
    CancellationToken cancellationToken = default);

    Task<TResponse> PostFormAsync<TResponse>(
    string uri,
    Dictionary<string, string> formData,
    string clientName = "",
    Dictionary<string, string>? headers = null,
    CancellationToken cancellationToken = default);

    Task<TResponse> PostEmptyJsonAsyncv2<TResponse>(
        string uri,
        string clientName = "",
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default);
    Task<TResponse> PostEmptyJsonAsync<TResponse>(
    string uri,
    string clientName = "",
    Dictionary<string, string>? headers = null,
    CancellationToken cancellationToken = default);
    Task<TResponse> GetAsyncv2<TResponse>(
        string uri,
        string clientName = "",
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default);  
}