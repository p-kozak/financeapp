﻿using Microsoft.EntityFrameworkCore;
using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

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
                Amount = balance.Balance,
                TrackedBalance = balance,
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
            var balance = context.UserBalance
                .Where(s => s.Customer == customer && s.Currency == currency).FirstOrDefault();

            if (balance == null)
            {
                throw new ArgumentException($"User {customer.Id.ToString()} does not have balance in {Enum.GetName(typeof(Currency), currency)}");
            }
            return balance;

        }



        public bool CustomerExists(Customer user)
        {
            if (user == null)
            {
                throw new ArgumentException("User is null.");
            }

            return context.Customers.Any(s => s.Id == user.Id);

        }


        public ICollection<BalanceHistory> GetBalanceHistory(CustomerBalance balance)
        {

            var history = context.BalanceHistories.AsNoTracking().Where(s => s.TrackedBalance == balance).OrderBy(s => s.Date).ToList();

            return history;

        }
        public ICollection<BalanceHistory> GetBalanceHistoryById(int balanceId)
        {
            var history = context.BalanceHistories.AsNoTracking().Where(s => s.Id == balanceId).OrderBy(s => s.Date).ToList();

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
                TrackedBalance = balance,
                Amount = 0
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
            var transactions = GetCustomerTransactions(balance.Customer).Where(s => s.Currency == balance.Currency);
            context.Transactions.RemoveRange(transactions);
            context.UserBalance.Remove(balance);
            context.SaveChanges();
        }

        public void CloseBalanceById(int balanceId)
        {
            var obj = context.UserBalance.Where(s => s.Id == balanceId).Include(x => x.Customer).FirstOrDefault();

            var transactions = GetCustomerTransactions(obj.Customer).Where(s => s.Currency == obj.Currency);
            var history = GetBalanceHistoryById(balanceId);
            context.BalanceHistories.RemoveRange(history);
            context.Transactions.RemoveRange(transactions);
            context.UserBalance.Remove(obj);
            context.SaveChanges();
        }

        public ICollection<Transaction> GetCustomerTransactions(Customer customer)
        {
            if (!CustomerExists(customer))
            {
                throw new ArgumentException("User does not exist");
            }

            var transactions = context.Transactions
                .Where(s => s.Customer == customer).Include(s => s.Customer).ToList();
            return transactions;
        }

        public void EditBalance(CustomerBalance balance, decimal newValue)
        {
            var difference = newValue - balance.Balance;
            var comment = $"AUTOGENERATED {DateTime.Now.ToString()} changed to {newValue.ToString()} by adding {difference}";

            var transaction = new Transaction
            {
                Amount = difference,
                Currency = balance.Currency,
                Customer = balance.Customer,
                Date = DateTime.Now,
                Description = comment,
            };

            AddTransaction(transaction);
            //Persist the new balance value
            balance.Balance = newValue;
            context.UserBalance.Update(balance);
            context.SaveChanges();

        }

        /*This method takes a transaction and edits all future balances to adjust them by the change in the transaction.
         Used when some transaction is edited. If you want to delete, just make newTransactionValue equal to 0.0*/
        public void EditTransaction(Transaction oldTransaction, TransactionDTO newTransaction)
        {

            if (oldTransaction.Currency != newTransaction.Currency)
            {
                throw new ArgumentException("Transactions need to have the same currency");
            }

            //Check if we update description
            if (!string.IsNullOrEmpty(newTransaction.Description))
            {
                oldTransaction.Description = newTransaction.Description;
            }


            //Check if we update transaction value. If they are the same, just return
            if (Double.Equals(oldTransaction.Amount, newTransaction.Amount))
            {
                return;
            }

            var difference = newTransaction.Amount - oldTransaction.Amount;

            var history = context.BalanceHistories.Where(s => s.Customer == oldTransaction.Customer
            && s.Currency == oldTransaction.Currency
            && s.Date >= oldTransaction.Date);

            foreach (var record in history)
            {
                record.Amount += difference;
            }

            //You also need to update current balance
            var balance = GetBalance(oldTransaction.Customer, oldTransaction.Currency);
            balance.Balance += difference;

            //You need to persist the oldTransaction because it might not be tracked
            oldTransaction.Amount = newTransaction.Amount;

            //if new amount is 0 then delete it. Also, delete this BalanceHistory record
            if (Double.Equals(newTransaction.Amount, 0.0M))
            {
                //The first element in history is the only we edited, therefore one we want to edit
                var historyToDelete = history.OrderBy(x => x.Date).First();


                context.BalanceHistories.Remove(historyToDelete);
                context.Transactions.Remove(oldTransaction);
                context.SaveChanges();
                return;
            }


            context.Transactions.Update(oldTransaction);
            context.SaveChanges();


        }

        public Transaction GetTransactionById(int id)
        {
            var trans = context.Transactions.Where(s => s.Id == id).Include(s => s.Customer).FirstOrDefault(); ;
            if (trans == null)
            {
                throw new ArgumentException($"Transaction {id} does not exist");
            }
            return trans;
        }
    }
}
