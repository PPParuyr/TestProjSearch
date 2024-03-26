using BussinesLayer.Models.ProviderTwo;
using BussinesLayer.Models.Search;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace BussinesLayer.Services;

public class ProviderTwoService : IProviderTwoService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProviderTwoService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task<ProviderTwoSearchResponse> GetRoutesAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var requestContent = new StringContent(JsonConvert.SerializeObject(new ProviderTwoSearchRequest
        {
            Departure = request.Origin,
            Arrival = request.Destination,
            DepartureDate = request.OriginDateTime,
            MinTimeLimit = request.Filters?.MinTimeLimit
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_configuration.GetSection("providerTwoSearchUrl").Value, requestContent, cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ProviderTwoSearchResponse>(responseContent);
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(_configuration.GetSection("providerTwoPingUrl").Value, cancellationToken);
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
