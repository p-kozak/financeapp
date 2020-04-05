using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;
using System.Collections.Generic;

namespace PersonalFinance.Data.Repositories
{
    public interface IPersonalFinanceRepository
    {
        void AddCustomer(Customer user);
        void AddTransaction(Transaction transaction);

        void OpenBalance(Customer user, int currency);
        bool CustomerExists(Customer user);
        CustomerBalance GetBalance(Customer user, int currency);
        ICollection<BalanceHistory> GetBalanceHistory(CustomerBalance balance);
        ICollection<BalanceHistory> GetBalanceHistoryById(int balanceId);


        ICollection<CustomerBalance> GetCustomerBalances(Customer customer);

        Customer GetCustomerByIdentityId(string identityId);
        void CloseBalance(CustomerBalance balance);
        void CloseBalanceById(int balanceId);
        ICollection<Transaction> GetCustomerTransactions(Customer customer);

        void EditBalance(CustomerBalance balance, decimal newValue);
        void EditTransaction(Transaction oldTransaction, TransactionDTO newTransaction);
        Transaction GetTransactionById(int id);


    }
}
