using BussinesLayer.Models.ProviderOne;
using BussinesLayer.Models.Search;

namespace BussinesLayer.Services
{
    public interface IProviderOneService
    {
        Task<ProviderOneSearchResponse> GetRoutesAsync(SearchRequest request, CancellationToken cancellationToken);
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
