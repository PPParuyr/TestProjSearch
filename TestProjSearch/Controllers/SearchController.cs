using BussinesLayer.Models;
using BussinesLayer.Models.Search;
using BussinesLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestProjSearch.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<ActionResult<SearchResponse>> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _searchService.SearchAsync(request, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ping")]
        public async Task<ActionResult<bool>> IsAvailableAsync(CancellationToken cancellationToken)
        {
            try
            {
                var isAvailable = await _searchService.IsAvailableAsync(cancellationToken);
                return Ok(isAvailable);
            }
            catch
            {
                return StatusCode(500, false);
            }
        }
    }
}
