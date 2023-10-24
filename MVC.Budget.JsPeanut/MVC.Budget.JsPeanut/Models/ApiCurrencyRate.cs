using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MVC.Budget.JsPeanut.Models
{
    [NotMapped]
    public class ApiCurrencyRate
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("base")]
        public string Base { get; set; }
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
