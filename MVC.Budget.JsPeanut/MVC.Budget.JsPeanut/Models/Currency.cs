using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MVC.Budget.JsPeanut.Models
{
    [NotMapped]
    public class Currency
    {
        public int? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("symbol_native")]
        public string? NativeSymbol { get; set; }
        [JsonPropertyName("code")]
        public string? CurrencyCode { get; set; }
    }
}
