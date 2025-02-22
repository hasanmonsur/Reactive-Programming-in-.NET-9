using System.ComponentModel.DataAnnotations.Schema;

namespace AgriMarketAnalysis.Models
{
    public class AgriculturalGood
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., Wheat, Corn, Rice

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; } // Time of data entry
        public string Region { get; set; } // Region of the market
    }
}
