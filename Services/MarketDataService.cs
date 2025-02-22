using AgriMarketAnalysis.Models;
using System.Reactive.Subjects;

namespace AgriMarketAnalysis.Services
{
    public class MarketDataService
    {
        private readonly Subject<AgriculturalGood> _marketDataStream = new();

        // Expose the data stream as an observable
        public IObservable<AgriculturalGood> MarketDataStream => _marketDataStream;

        // Method to add new market data to the stream
        public void AddMarketData(AgriculturalGood good)
        {
            _marketDataStream.OnNext(good);
        }
    }
}
