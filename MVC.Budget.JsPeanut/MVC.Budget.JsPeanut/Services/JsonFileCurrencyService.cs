using MVC.Budget.JsPeanut.Models;
using System.Text.Json;

namespace MVC.Budget.JsPeanut.Services
{
    public class JsonFileCurrencyService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }
        public JsonFileCurrencyService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        private string JsonFile => Path.Combine(WebHostEnvironment.WebRootPath, "data", "currency-data.json");

        public List<Currency> GetCurrencyList()
        {
            using var jsonFileReader = File.OpenText(JsonFile);
            var currencyDictionary = JsonSerializer.Deserialize<Dictionary<string, Currency>>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return currencyDictionary.Values.ToList();
        }
    }
}
