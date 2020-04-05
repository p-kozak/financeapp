using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;
using PersonalFinance.Domain.DTOs;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Test
{
    public class TestPersonalFinanceRepository
    {
        private readonly ITestOutputHelper output;

        public PersonalFinanceRepository Repository { get; set; }
        public Customer John { get; set; }
        public TestPersonalFinanceRepository(ITestOutputHelper output)
        {
            Repository = new PersonalFinanceRepository(GetNewContext("Repo"));
            John = new Customer
            {
                Email = "john@doe.pl",
                FirstName = "John",
                LastName = "Doe",
                IdentityId = "meh"
            };
            //John by default has balance in currency 12345
            Repository.AddCustomer(John);
            Repository.OpenBalance(John, 12345);
            this.output = output;
        }

        public Customer GetCustomerHelper()
        {
            return new Customer
            {
                Email = "john@doe.pl",
                FirstName = "John",
                LastName = "Doe",
                IdentityId = "meh"
            };
        }

        public Transaction GetTransactionHelper(int currency, Customer customer, decimal amount = 50.05M)
        {
            return new Transaction
            {
                Customer = customer,
                Date = DateTime.Now,
                Amount = amount,
                Currency = currency,
                Description = DateTime.Now.ToShortTimeString()
            };

        }

        public FinanceContext GetNewContext(string name)
        {
            return new FinanceContext(new DbContextOptionsBuilder().UseInMemoryDatabase(name).Options);
        }


        [Fact]
        public void AddCustomer()
        {
            var customer = new Customer
            {
                IdentityId = "35",
                FirstName = "a",
                LastName = "b",
                Email = "a@a.a"
            };
            Repository.AddCustomer(customer);

            Assert.NotEqual(0, customer.Id);
        }
        [Fact]
        public void CustomerExists()
        {
            Assert.True(Repository.CustomerExists(John));
            Assert.False(Repository.CustomerExists(new Customer()));
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(10000)]
        [InlineData(500000)]
        public void NonExistentBalances(int currency)
        {
            //Add user that exists
            var customer = new Customer
            {
                IdentityId = "35",
                FirstName = "a",
                LastName = "b",
                Email = "a@a.a"
            };

            Repository.AddCustomer(customer);
            Assert.Throws<ArgumentException>(() => Repository.GetBalance(customer, currency));

        }

        [Theory]
        [InlineData(50)]
        [InlineData((int)Currency.GBP)]
        [InlineData(-50)]
        public void AddingBalances(int currency)
        {
            Repository.OpenBalance(John, currency);
            Assert.NotNull(Repository.GetBalance(John, currency));
        }

        [Fact]
        public void GetCustomerBalances()
        {
            var customer = GetCustomerHelper();
            Repository.AddCustomer(customer);

            //User should not have balances yet
            var balances = Repository.GetCustomerBalances(customer);
            Assert.Equal(0, balances.Count);

            Repository.OpenBalance(customer, 1);
            Repository.OpenBalance(customer, 2);

            var balances2 = Repository.GetCustomerBalances(customer);
            Assert.Equal(2, balances2.Count);

        }
        [Fact]
        public void GetCustomerByIdentityId()
        {
            var identityIdExists = "puppy";
            var identityIdNotExists = "kitty";

            var customer = GetCustomerHelper();
            customer.IdentityId = identityIdExists;

            Repository.AddCustomer(customer);

            Assert.Throws<ArgumentException>(() => Repository.GetCustomerByIdentityId(identityIdNotExists));

            var customerFromRepo = Repository.GetCustomerByIdentityId(identityIdExists);
            Assert.Equal(identityIdExists, customerFromRepo.IdentityId);


        }

        [Fact]
        public void DeleteBalance()
        {

            Repository.OpenBalance(John, 678);
            Repository.OpenBalance(John, 789);

            var bal1 = Repository.GetBalance(John, 678);
            var bal2 = Repository.GetBalance(John, 789);

            var balancesBeforeDelete = Repository.GetCustomerBalances(John);

            //Delete balances
            Repository.CloseBalance(bal1);
            Repository.CloseBalanceById(bal2.Id);

            var balancesAfterDelete = Repository.GetCustomerBalances(John);

            Assert.True(balancesBeforeDelete.Contains(bal1));
            Assert.True(balancesBeforeDelete.Contains(bal2));
            Assert.False(balancesAfterDelete.Contains(bal1));
            Assert.False(balancesAfterDelete.Contains(bal2));

        }
        [Fact]
        public void GetUsersTransactions()
        {
            var user = GetCustomerHelper();
            Repository.AddCustomer(user);
            Repository.OpenBalance(user, 1);
            Repository.OpenBalance(user, 2);

            Enumerable.Range(0, 10).
                ToList().ForEach(i =>
                Repository.AddTransaction(new Transaction
                {
                    Customer = user,
                    Date = DateTime.Now,
                    Amount = i,
                    Currency = i % 2 + 1
                })
                );


            var count = Repository.GetCustomerTransactions(user).Count();
            Assert.Equal(10, count);
        }

        [Fact]
        public void EditBalance()
        {
            var customer = GetCustomerHelper();
            Repository.OpenBalance(customer, 10);

            var transCountBefore = Repository.GetCustomerTransactions(customer).Count();

            //edit balance
            var balance = Repository.GetBalance(customer, 10);
            Repository.EditBalance(balance, 50);
            Repository.EditBalance(balance, -40);

            var transCountAfter = Repository.GetCustomerTransactions(customer).Count();

            Assert.Equal(0, transCountBefore);
            Assert.Equal(2, transCountAfter);
            Assert.Equal(-40, balance.Balance);

        }

        [Fact]
        public void EditTransaction()
        {
            var customer = GetCustomerHelper();
            Repository.AddCustomer(customer);

            var timeNow = DateTime.Now;

            var transactions = Enumerable.Range(5, 10).Select(
                i => new Transaction
                {
                    Date = timeNow.AddSeconds(i),
                    Currency = 5,
                    Customer = customer,
                    Amount = i
                }).ToList();

            Repository.OpenBalance(customer, 5);

            var balance = Repository.GetBalance(customer, 5);
            var endSum = (5 + 14) * 10 / 2;
            transactions.ForEach(x => Repository.AddTransaction(x));

            var balanceBeforeEdit = Repository.GetBalance(customer, 5).Balance;
            var historyBeforeEdit = Repository.GetBalanceHistory(balance);

            Assert.Equal(10, Repository.GetCustomerTransactions(customer).Count);
            //Edit the 3rd transaction from 7 to 17
            Repository.EditTransaction(transactions[2], new TransactionDTO
            { Currency = 5, Amount = 27 }
            );


            var balanceAfterEdit = Repository.GetBalance(customer, 5).Balance;
            var historyAfterEdit = Repository.GetBalanceHistory(balance);

            historyAfterEdit.ToList().ForEach(x => output.WriteLine($"{x.Id.ToString()}  {x.Amount} {x.Date}"));
            output.WriteLine("DUPA");
            historyBeforeEdit.ToList().ForEach(x => output.WriteLine($"{x.Id.ToString()}  {x.Amount} {x.Date}"));

            Assert.Equal(endSum, balanceBeforeEdit);
            Assert.Equal(endSum + 20, balanceAfterEdit);
            //check if balance beforeedit are the same
            historyBeforeEdit.Where(s => s.Date < transactions[2].Date).ToList()
                .ForEach(x => Assert.Equal(x.Amount, historyAfterEdit.Where(s => s.Id == x.Id).FirstOrDefault().Amount));

            historyBeforeEdit.Where(s => s.Date >= transactions[2].Date).ToList()
                .ForEach(x => Assert.Equal(x.Amount + 20, historyAfterEdit.Where(s => s.Id == x.Id).FirstOrDefault().Amount));


        }

        [Fact]
        public void EditTransactionDelete()
        {
            var customer = GetCustomerHelper();
            Repository.AddCustomer(customer);

            var timeNow = DateTime.Now;

            var transactions = Enumerable.Range(5, 10).Select(
                i => new Transaction
                {
                    Date = timeNow.AddSeconds(i),
                    Currency = 5,
                    Customer = customer,
                    Amount = i
                }).ToList();

            Repository.OpenBalance(customer, 5);

            var endSum = (5 + 14) * 10 / 2;
            transactions.ForEach(x => Repository.AddTransaction(x));
            var balance = Repository.GetBalance(customer, 5);

            var balanceBeforeEdit = Repository.GetBalance(customer, 5).Balance;
            var historyBeforeEdit = Repository.GetBalanceHistory(balance);

            Assert.Equal(10, Repository.GetCustomerTransactions(customer).Count);
            //Edit the 3rd transaction from 7 to 17
            Repository.EditTransaction(transactions[2], new TransactionDTO
            { Currency = 5, Amount = 0.0M }
            );


            var balanceAfterEdit = Repository.GetBalance(customer, 5).Balance;
            var historyAfterEdit = Repository.GetBalanceHistory(balance);

            historyAfterEdit.ToList().ForEach(x => output.WriteLine($"{x.Id.ToString()}  {x.Amount} {x.Date}"));
            output.WriteLine("DUPA");
            historyBeforeEdit.ToList().ForEach(x => output.WriteLine($"{x.Id.ToString()}  {x.Amount} {x.Date}"));

            Assert.Equal(endSum, balanceBeforeEdit);
            Assert.Equal(endSum - 7, balanceAfterEdit);

            //check if balance beforeedit are the same
            Assert.NotEqual(historyAfterEdit.Count, historyBeforeEdit.Count);

            historyBeforeEdit.Where(s => s.Date < transactions[2].Date).ToList()
                .ForEach(x => Assert.Equal(x.Amount, historyAfterEdit.Where(s => s.Id == x.Id).FirstOrDefault().Amount));

            historyBeforeEdit.Where(s => s.Date > transactions[2].Date).ToList()
                .ForEach(x => Assert.Equal(x.Amount - 7, historyAfterEdit.Where(s => s.Id == x.Id).FirstOrDefault().Amount));

            //Don't expect to see deleted date in the collection
            Assert.Empty(historyAfterEdit.Where(x => x.Date == transactions[2].Date));

        }

        [Fact]
        public void GetTransactionById()
        {
            var customer = GetCustomerHelper();
            Repository.AddCustomer(customer);
            Repository.OpenBalance(customer, 5);

            var trans = GetTransactionHelper(5, customer);
            Repository.AddTransaction(trans);
            output.WriteLine($"Trans is {trans.Id.ToString()}");

            var retrivedTrans = Repository.GetTransactionById(trans.Id);
            output.WriteLine($"retrived is {retrivedTrans.Id.ToString()}");

            Assert.NotNull(retrivedTrans);
            Assert.NotNull(retrivedTrans.Customer);
            Assert.Equal(customer.Id, retrivedTrans.Customer.Id);
            Assert.Equal("John", retrivedTrans.Customer.FirstName);
        }



    }


}
