using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace Test
{
    public class TestPersonalFinanceRepository
    {
        public PersonalFinanceRepository Repository { get; set; }
        public Customer John { get; set; }
        public TestPersonalFinanceRepository()
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


    }


}
