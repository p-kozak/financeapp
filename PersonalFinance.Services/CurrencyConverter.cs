using Newtonsoft.Json;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PersonalFinance.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly HttpClient client;
        private readonly string LatestRatesURL = "https://api.exchangeratesapi.io/latest";

        public CurrencyConverter()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }


        public decimal ConvertCustomerBalanceToGivenCurrency(CustomerBalance balance, Currency currencyTo)
        {
            var latestRates = GetLatestExchangeRates();
            //I didn't create DTO for this api call. Might add it later
            dynamic dic = JsonConvert.DeserializeObject(latestRates.Result);


            var toSymbol = Enum.GetName(typeof(Currency), currencyTo);
            var fromSymbol = Enum.GetName(typeof(Currency), balance.Currency);

            var toRate = dic["rates"][toSymbol];
            var fromRate = dic["rates"][fromSymbol];
            return toRate / fromRate * balance.Balance;



        }

        public decimal ConvertListOfBalancesToGivenCurrency(IEnumerable<CustomerBalance> balances, Currency currencyTo)
        {
            var latestRates = GetLatestExchangeRates();
            dynamic dic = JsonConvert.DeserializeObject(latestRates.Result);
            decimal finalBalance = 0;
            var toSymbol = Enum.GetName(typeof(Currency), currencyTo);
            var toRate = dic["rates"][toSymbol];

            foreach (var balance in balances)
            {
                var fromSymbol = Enum.GetName(typeof(Currency), balance.Currency);
                var fromRate = dic["rates"][fromSymbol];
                finalBalance += (decimal)(toRate / fromRate * balance.Balance);
            }
            return finalBalance;

        }

        public async Task<String> GetLatestExchangeRates()
        {
            var response = await client.GetAsync(LatestRatesURL);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
