using LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;

namespace LightCap.InvestmentApi.Infrastructure.Services;
         
public class ApiClient(
    IHttpClientFactory httpClientFactory,
    ILogger<ApiClient> logger) : IApiClient
{
    private static readonly string Separator = new('*', 110);
    private readonly Stopwatch _stopwatch = new();

    public async Task<TResponse> GetAsync<TResponse>(string uri, string clientName = "",
        Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        var client = ResolveHttpClient(clientName);
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        ApplyHeaders(request, headers);
        return await SendAsync<TResponse>(client, request, cancellationToken);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data, string clientName = "",
        Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        var client = ResolveHttpClient(clientName);
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = JsonContent.Create(data)
        };

        ApplyHeaders(request, headers);

        return await SendAsync<TResponse>(client, request, cancellationToken);
    }

    public async Task<TResponse> PostStringAsync<TResponse>(
    string uri,
    string data,
    string clientName = "",
    Dictionary<string, string>? headers = null,
    CancellationToken cancellationToken = default)
    {
        var client = ResolveHttpClient(clientName);

        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(
            $"\"{data}\"",
            Encoding.UTF8,
            "application/json")
        };

        ApplyHeaders(request, headers);

        return await SendAsync<TResponse>(client, request, cancellationToken);
    }

    public async Task<TResponse> PostEmptyJsonAsync<TResponse>(
    string uri,
    string clientName = "",
    Dictionary<string, string>? headers = null,
    CancellationToken cancellationToken = default)
    {
        var client = ResolveHttpClient(clientName);

        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent("\"\"", Encoding.UTF8, "application/json")
        };

        ApplyHeaders(request, headers);

        return await SendAsync<TResponse>(client, request, cancellationToken);
    }


    public async Task<TResponse> PostFormAsync<TResponse>(
    string uri,
    Dictionary<string, string> formData,
    string clientName = "",
    Dictionary<string, string>? headers = null,
    CancellationToken cancellationToken = default)
    {
        var client = ResolveHttpClient(clientName);

        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new FormUrlEncodedContent(formData)
        };

        ApplyHeaders(request, headers);

        return await SendAsync<TResponse>(client, request, cancellationToken);
    }

    private async Task<TResponse> SendAsync<TResponse>(HttpClient httpClient, HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("""
                              {separator}
                              Sending HTTP {Method} request to {Uri}

                              Request Body: {@Body}
                              {separator}
                              """, Separator, request.Method, request.RequestUri,
            request.Content is null ? "nil" : await request.Content.ReadAsStringAsync(cancellationToken), Separator);

        _stopwatch.Restart();
        var response = await httpClient.SendAsync(request, cancellationToken);
        _stopwatch.Stop();

        var httpContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("""
                            {separator}
                            Http {Method} request to {Uri} failed with {StatusCode}

                            Duration: {Duration}ms 

                            Response: {@ErrorContent}
                            {separator}
                            """, Separator, request.Method, request.RequestUri, response.StatusCode,
                _stopwatch.ElapsedMilliseconds,
                httpContent,
                Separator);

            response.EnsureSuccessStatusCode();
        }

        logger.LogInformation("""
                              {separator}
                              HTTP {Method} request to {Uri} was successful with {StatusCode} 

                              Duration: {Duration}ms

                              Response: {@Response}
                              {separator}
                              """, Separator, request.Method, request.RequestUri, response.StatusCode,
            _stopwatch.ElapsedMilliseconds,
            httpContent, Separator);

        var result = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        return result;
    }

    private HttpClient ResolveHttpClient(string clientName)
    {
        return httpClientFactory.CreateClient(!string.IsNullOrWhiteSpace(clientName)
            ? clientName
            : string.Empty);
    }

    private static void ApplyHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
    {
        if (headers == null) return;
        foreach (var (key, value) in headers)
        {
            request.Headers.Add(key, value);
        }
    }

    //New methods:
    private HttpClient ResolveHttpClientv2(string clientName) =>
      httpClientFactory.CreateClient(string.IsNullOrWhiteSpace(clientName) ? "" : clientName);

    private void ApplyHeadersv2(HttpRequestMessage request, Dictionary<string, string>? headers)
    {
        if (headers == null) return;
        foreach (var (key, value) in headers)
            request.Headers.Add(key, value);
    }

    private async Task<TResponse> SendAsyncv2<TResponse>(HttpClient client, HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken)
               ?? throw new InvalidOperationException("Empty response body");
    }

    // GET with query string
    public async Task<TResponse> GetAsyncv2<TResponse>(
        string uri,
        string clientName = "",
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        var client = ResolveHttpClientv2(clientName);
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        ApplyHeadersv2(request, headers);
        return await SendAsync<TResponse>(client, request, cancellationToken);
    }

    // POST with empty JSON body and query string
    public async Task<TResponse> PostEmptyJsonAsyncv2<TResponse>(
        string uri,
        string clientName = "",
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        var client = ResolveHttpClientv2(clientName);
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent("\"\"", Encoding.UTF8, "application/json")
        };
        ApplyHeadersv2(request, headers);
        return await SendAsyncv2<TResponse>(client, request, cancellationToken);
    }
}