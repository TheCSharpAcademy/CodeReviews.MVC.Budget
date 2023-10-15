using MVC.Budget.JsPeanut.Models;
using RestSharp;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;

namespace MVC.Budget.JsPeanut.Services
{
    public class CurrencyConverterService
    {
        public decimal UsdCategoryCurrencyRate { get; set; }
        public decimal ConvertValueToUsd(string transactionCurrencyCode, decimal transactionValue, string categoryCurrencyCode)
        {
            int conversionValue = 0;

            var client = new RestClient("http://api.exchangeratesapi.io/");
            var requestUrl = "v1/latest?access_key=fa64348655a3b54b9d64101ab0de65dd&symbols=USD";
            requestUrl = requestUrl + $",{transactionCurrencyCode}" + $",{categoryCurrencyCode}";
            var request = new RestRequest(requestUrl);
            var response = client.ExecuteGet(request);

            var rates = JsonSerializer.Deserialize<ApiCurrencyRate>(response.Content);

            Console.WriteLine($"{rates.Rates.First().Key} {rates.Rates.First().Value}");

            var oneEuroToUsd = rates.Rates.Where(x => x.Key == "USD").First().Value;
            var oneEuroToCategoryCurrency = rates.Rates.Where(x => x.Key == categoryCurrencyCode).First().Value;

            //var oneEuroToTransactionCurrency = rates.Rates.Where(x => x.Key == transactionCurrencyCode).First().Value;

            //USD - Transaction's currency ratio
            //var oneUsdToTransactionCurrency = oneEuroToTransactionCurrency / oneEuroToUsd;

            // USD - Category's currency ratio
            var oneUsdToCategoryCurrency = oneEuroToCategoryCurrency / oneEuroToUsd;

            transactionValue = transactionValue * oneUsdToCategoryCurrency;

            UsdCategoryCurrencyRate = oneUsdToCategoryCurrency;

            return transactionValue;
        }
    }
}
