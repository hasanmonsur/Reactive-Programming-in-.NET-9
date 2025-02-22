using AgriMarketAnalysis.Models;
using AgriMarketAnalysis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AgriMarketAnalysis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketAnalysisController : ControllerBase
    {
        private readonly MarketDataService _marketDataService;
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public MarketAnalysisController(MarketDataService marketDataService)
        {
            _marketDataService = marketDataService;
        }

        // Endpoint to add new market data
        [HttpPost("add")]
        public IActionResult AddMarketData([FromBody] AgriculturalGood good)
        {
            _marketDataService.AddMarketData(good);
            return Ok();
        }

        // Endpoint to get real-time market trends
        [HttpGet("trends")]
        public async Task<IActionResult> GetMarketTrends()
        {
            var trends = await _marketDataService.MarketDataStream
            .Where(good => good.Timestamp >= DateTime.UtcNow.AddHours(-1))
            .GroupBy(good => good.Name)
            .SelectMany(group => group
            .ToList()
            .Select(goods => new
            {
                Good = group.Key,
                AveragePrice = goods.Average(g => g.Price),
                Count = goods.Count
            }))
            .ToList();

            return Ok(trends);
        }
    }
}