using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalFinance.Data.Repositories
{
    public class PersonalFinanceRepository : IPersonalFinanceRepository
    {
        private readonly FinanceContext context;

        public PersonalFinanceRepository(FinanceContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            var customer = transaction.Customer;
           
            var balance = context.UserBalance.Where(s => (s.Customer == customer) 
            && s.Currency == transaction.Currency).FirstOrDefault();

            //Balance is tracked and saved automatically
            balance.Balance += transaction.Amount;

            var history = new BalanceHistory()
            {
                Date = transaction.Date,
                Currency = transaction.Currency,
                Balance = balance.Balance,
                Customer = customer,
            };

            context.BalanceHistories.Add(history);
            context.Transactions.Add(transaction);
            context.SaveChanges();
        }

    

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException();
            }
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public CustomerBalance GetBalance(Customer customer, int currency)
        {
            if (!CustomerExists(customer))
            {
                throw new ArgumentException("User does not exists");
            }

            //Tro to get the balance in a given currency. If null, throw an exception
            var balance = context.UserBalance.Where(s => s.Customer == customer && s.Currency == currency).FirstOrDefault();
            
            if (balance == null)
            {
                throw new ArgumentException($"User {customer.Id.ToString()} does not have balance in {Enum.GetName(typeof(Currency), currency)}");
            }
            return balance;

        }

       

        public bool CustomerExists(Customer user)
        {
            return context.Customers.Any(s => s.Id == user.Id);

        }


        public ICollection<BalanceHistory> GetBalanceHistory(Customer user, int currency)
        {
            if(!CustomerExists(user))
            {
                throw new ArgumentException("User does not exist.");
            }

            var balance = context.UserBalance.Where(s => s.Customer == user && s.Currency == currency).FirstOrDefault();

            if (balance == null)
            {
                throw new ArgumentException($"User {user.Id.ToString()} does not have balance in {Enum.GetName(typeof(Currency), currency)}");
            }

            var history = context.BalanceHistories.Where(s => s.Customer == user && s.Currency == currency).OrderBy(s => s.Date).ToList();

            return history;

        }

        public void OpenBalance(Customer user, int currency)
        {
            //Check if user balance already exists

            var exists = context.UserBalance.Any(s => s.Currency == currency && s.Customer == user);
            if (exists)
            {
                throw new ArgumentException("User has ballance in this currency");
            }

            var balance = new CustomerBalance()
            {
                Customer = user,
                Currency = currency,
                Balance = 0,
            };

            var history = new BalanceHistory()
            {
                Currency = currency,
                Customer = user,
                Date = DateTime.Now,
                Balance = 0
            };

            context.AddRange(balance, history);
            context.SaveChanges();

        }

        public ICollection<CustomerBalance> GetCustomerBalances(Customer customer)
        {
            if (!CustomerExists(customer))
            {
                throw new ArgumentException("Customer does not exist");
            }
            var balances = context.UserBalance.Where(s => s.Customer == customer).ToList();
            return balances;
        }

        public Customer GetCustomerByIdentityId(string identityId)
        {
            var customer = context.Customers.Where(s => s.IdentityId == identityId).FirstOrDefault();
            if (customer == null)
            {
                throw new ArgumentException("Customer does not exist");
            }
            return customer;
        }

        public void CloseBalance(CustomerBalance balance)
        {
            context.UserBalance.Remove(balance);
            context.SaveChanges();
        }

        public void CloseBalanceById(int balanceId)
        {
            var obj = context.UserBalance.Find(balanceId);
            context.UserBalance.Remove(obj);
            context.SaveChanges();
        }

        public ICollection<Transaction> GetCustomerTransactions(Customer customer)
        {
            if (!CustomerExists(customer))
            {
                throw new ArgumentException("User does not exist");
            }

            var transactions = context.Transactions.Where(s => s.Customer == customer).ToList();
            return transactions;
        }
    }
}
