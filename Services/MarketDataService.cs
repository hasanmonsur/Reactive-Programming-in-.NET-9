using AgriMarketAnalysis.Data;
using AgriMarketAnalysis.Models;
using System.Reactive.Subjects;

namespace AgriMarketAnalysis.Services
{
    public class MarketDataService
    {
        private readonly Subject<AgriculturalGood> _marketDataStream = new();
        private readonly AppDbContext _dbContext;

        public MarketDataService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IObservable<AgriculturalGood> MarketDataStream => _marketDataStream;

        public void AddMarketData(AgriculturalGood good)
        {
            try
            {
                // Save to the database
                _dbContext.AgriculturalGoods.Add(good);
                _dbContext.SaveChanges();

                // Push to the stream
                _marketDataStream.OnNext(good);
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                Console.WriteLine($"Error adding market data: {ex.Message}");
            }
        }
    }
}
