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
            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            var user = transaction.Customer;
           
            var balance = context.UserBalance.Where(s => (s.Customer == user) 
            && s.Currency == transaction.Currency).FirstOrDefault();

            //Balance is tracked and saved automatically
            balance.Balance += transaction.Amount;

            var history = new BalanceHistory()
            {
                Date = DateTime.Now,
                Currency = transaction.Currency,
                Balance = balance.Balance,
                Customer = user,
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

        public CustomerBalance GetBalance(Customer user, int currency)
        {
            if (!UserExists(user))
            {
                throw new ArgumentException("User does not exists");
            }

            //Tro to get the balance in a given currency. If null, throw an exception
            var balance = context.UserBalance.Where(s => s.Customer == user && s.Currency == currency).FirstOrDefault();
            
            if (balance == null)
            {
                throw new ArgumentException($"User {user.Id.ToString()} does not have balance in {Enum.GetName(typeof(Currency), currency)}");
            }
            return balance;

        }

        public bool UserExists(Customer user)
        {
            return context.Customers.Any(s => s.Id == user.Id);

        }


        public IEnumerable<BalanceHistory> GetBalanceHistory(Customer user, int currency)
        {
            if(!UserExists(user))
            {
                throw new ArgumentException("User does not exist.");
            }

            var balance = context.UserBalance.Where(s => s.Customer == user && s.Currency == currency).FirstOrDefault();

            if (balance == null)
            {
                throw new ArgumentException($"User {user.Id.ToString()} does not have balance in {Enum.GetName(typeof(Currency), currency)}");
            }

            var history = context.BalanceHistories.Where(s => s.Customer == user && s.Currency == currency).OrderBy(s => s.Date).AsEnumerable();

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
    }
}
