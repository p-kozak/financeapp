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
        bool UserExists(Customer user);
        CustomerBalance GetBalance(Customer user, int currency);
        IEnumerable<BalanceHistory> GetBalanceHistory(Customer user, int currency);
    }
}
