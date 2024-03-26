using BussinesLayer.Models.Search;

namespace BussinesLayer.Services
{
    public interface ISearchService
    {
        Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
