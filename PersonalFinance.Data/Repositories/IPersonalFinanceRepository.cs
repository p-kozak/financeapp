using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Data.Repositories
{
    public interface IPersonalFinanceRepository
    {
        void AddCustomer(Customer user);
        void AddTransaction(Transaction transaction);

        void OpenBalance(Customer user, int currency);
        bool CustomerExists(Customer user);
        CustomerBalance GetBalance(Customer user, int currency);
        ICollection<BalanceHistory> GetBalanceHistory(Customer customer, int currency);

        ICollection<CustomerBalance> GetCustomerBalances(Customer customer);

        Customer GetCustomerByIdentityId(string identityId);
        void CloseBalance(CustomerBalance balance);
        void CloseBalanceById(int balanceId);
    }
}
