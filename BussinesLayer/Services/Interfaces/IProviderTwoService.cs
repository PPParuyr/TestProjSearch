using BussinesLayer.Models.ProviderTwo;
using BussinesLayer.Models.Search;

namespace BussinesLayer.Services
{
    public interface IProviderTwoService
    {
        Task<ProviderTwoSearchResponse> GetRoutesAsync(SearchRequest request, CancellationToken cancellationToken);
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
