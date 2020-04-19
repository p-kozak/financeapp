using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinance.Services
{
    public interface ICurrencyConverter
    {
        Task<String> GetLatestExchangeRates();
        decimal ConvertCustomerBalanceToGivenCurrency(CustomerBalance balance, Currency currencyTo);
        decimal ConvertListOfBalancesToGivenCurrency(IEnumerable<CustomerBalance> balances, Currency currencyTo);


    }
}
