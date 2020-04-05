using PersonalFinance.Domain;
using PersonalFinance.Services;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Test
{
    public class TestCurrencyConverter
    {
        private readonly ITestOutputHelper output;

        public TestCurrencyConverter(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async void GetLatestExchangeRates()
        {
            var curr = new CurrencyConverter();
            var body = await curr.GetLatestExchangeRates();
            output.WriteLine(body);
            Assert.False(string.IsNullOrEmpty(body));
        }

        [Fact]
        public void ConvertBalance()
        {
            var balance = new CustomerBalance
            {
                Currency = (int)Currency.GBP,
                Balance = 1000
            };

            var curr = new CurrencyConverter();
            var res = curr.ConvertCustomerBalanceToGivenCurrency(balance, Currency.PLN);
            output.WriteLine($"I got {res} PLN from 1000 GBP");
        }

        [Fact]
        public void ConvertListOfBalancesToGivenCurrency()
        {
            var balance1 = new CustomerBalance
            {
                Currency = (int)Currency.GBP,
                Balance = 1000
            };
            var balance2 = new CustomerBalance
            {
                Currency = (int)Currency.USD,
                Balance = 1000
            };

            var balances = new List<CustomerBalance> { balance1, balance2 };
            var curr = new CurrencyConverter();
            var res = curr.ConvertListOfBalancesToGivenCurrency(balances, Currency.PLN);
            output.WriteLine($"I got {res} PLN from those monies");

        }
    }
}
