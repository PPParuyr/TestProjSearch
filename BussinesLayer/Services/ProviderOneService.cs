using BussinesLayer.Models.ProviderOne;
using BussinesLayer.Models.Search;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;


namespace BussinesLayer.Services;

public class ProviderOneService : IProviderOneService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProviderOneService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task<ProviderOneSearchResponse> GetRoutesAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var requestContent = new StringContent(JsonConvert.SerializeObject(new ProviderOneSearchRequest
        {
            From = request.Origin,
            To = request.Destination,
            DateFrom = request.OriginDateTime,
            DateTo = request.Filters?.DestinationDateTime,
            MaxPrice = request.Filters?.MaxPrice
        }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_configuration.GetSection("providerOneSearchUrl").Value, requestContent, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ProviderOneSearchResponse>(responseContent);
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(_configuration.GetSection("providerOnePingUrl").Value, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }
}
