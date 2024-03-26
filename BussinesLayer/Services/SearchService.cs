using BussinesLayer.Models.ProviderOne;
using BussinesLayer.Models.ProviderTwo;
using BussinesLayer.Models.Search;

namespace BussinesLayer.Services;

public class SearchService : ISearchService
{
    private readonly IProviderOneService _providerOneService;
    private readonly IProviderTwoService _providerTwoService;
    private readonly ICacheService<List<Route>> _cacheService;

    public SearchService(IProviderOneService providerOneService,
                         IProviderTwoService providerTwoService,
                         ICacheService<List<Route>> cacheService)
    {
        _providerOneService = providerOneService;
        _providerTwoService = providerTwoService;
        _cacheService = cacheService;
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        List<Route> allRoutes;
        if (request.Filters?.OnlyCached == true)
        {
            allRoutes = _cacheService.Get("Routes").Where(x => x.TimeLimit > DateTime.Now).ToList();
        }
        else
        {
            allRoutes = await GetRoutesAsync(request, cancellationToken);
        }
        SearchResponse searchResponse = new SearchResponse();
        if (allRoutes.Count != 0)
        {
            if (request.Filters?.OnlyCached != true)
            {
                List<Route> allRoutesFromCach = _cacheService.Get("Routes").Where(x => x.TimeLimit > DateTime.Now).ToList();

                if (allRoutesFromCach != null && allRoutesFromCach.Count > 0)
                {
                    allRoutes.AddRange(allRoutesFromCach);
                }
            }
            _cacheService.Create("Routes", allRoutes);

            searchResponse = new SearchResponse
            {
                Routes = [.. allRoutes],
                MinPrice = allRoutes.Min(r => r.Price),
                MaxPrice = allRoutes.Max(r => r.Price),
                MinMinutesRoute = allRoutes.Min(r => (r.DestinationDateTime - r.OriginDateTime).Minutes),
                MaxMinutesRoute = allRoutes.Max(r => (r.DestinationDateTime - r.OriginDateTime).Minutes)
            };
        }
        return searchResponse;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        bool providerOneAvailable = await _providerOneService.IsAvailableAsync(cancellationToken);
        bool providerTwoAvailable = await _providerTwoService.IsAvailableAsync(cancellationToken);

        return providerOneAvailable || providerTwoAvailable;
    }

    private async Task<List<Route>> GetRoutesAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        List<Route> routes = new List<Route>();

        if (await _providerOneService.IsAvailableAsync(cancellationToken))
        {
            ProviderOneSearchResponse responseOne = await _providerOneService.GetRoutesAsync(request, cancellationToken);

            foreach (ProviderOneRoute oneRouts in responseOne.Routes)
            {
                Route route = new Route
                {
                    Id = Guid.NewGuid(),
                    Price = oneRouts.Price,
                    Origin = oneRouts.From,
                    Destination = oneRouts.To,
                    OriginDateTime = oneRouts.DateFrom,
                    DestinationDateTime = oneRouts.DateTo,
                    TimeLimit = oneRouts.TimeLimit
                };
                routes.Add(route);
            }
        }
        if (await _providerTwoService.IsAvailableAsync(cancellationToken))
        {
            ProviderTwoSearchResponse responseTwo = await _providerTwoService.GetRoutesAsync(request, cancellationToken);
            foreach (ProviderTwoRoute oneRouts in responseTwo.Routes)
            {
                Route route = new Route
                {
                    Id = Guid.NewGuid(),
                    Price = oneRouts.Price,
                    Origin = oneRouts.Departure?.Point,
                    Destination = oneRouts.Arrival?.Point,
                    OriginDateTime = oneRouts.Departure.Date,
                    DestinationDateTime = oneRouts.Arrival.Date,
                    TimeLimit = oneRouts.TimeLimit
                };
                routes.Add(route);
            }
        }
        return routes;
    }

}
