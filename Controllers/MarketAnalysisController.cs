using AgriMarketAnalysis.Models;
using AgriMarketAnalysis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reactive.Linq;

namespace AgriMarketAnalysis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketAnalysisController : ControllerBase
    {
        private readonly MarketDataService _marketDataService;

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
                .Where(good => good.Timestamp >= DateTime.UtcNow.AddHours(-1)) // Last hour's data
                .GroupBy(good => good.Name)
                .Select(group => new
                {
                    Good = group.Key,
                    AveragePrice = group.Average(g => g.Price)
                })
                .ToList();

            return Ok(trends);
        }
    }
}
