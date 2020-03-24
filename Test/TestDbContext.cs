using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Test
{
    public class TestDbContext
    {
        DbContext context;
        public TestDbContext()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("MainTest");
            context = new FinanceContext(builder.Options);

        }
        public FinanceContext GetNewContext(string name)
        {
            return new FinanceContext(new DbContextOptionsBuilder().UseInMemoryDatabase(name).Options);
        }

        [Fact]
        public void EmptyDatabase()
        {
            using (var emptyContext = GetNewContext("Empty"))
            {
                var count = emptyContext.Customers.Where(o => o != null).Count();
                Assert.Equal(0, count);

            }
        }

        [Fact]
        public void AddCustomer()
        {
            using (var ctx = GetNewContext("ToAdd"))
            {
                var customer = new Customer
                {
                    IdentityId = "35",
                    FirstName = "a",
                    LastName = "b",
                    Email = "a@a.a"
                };
                ctx.Customers.Add(customer);
                ctx.SaveChanges();

                var count2 = ctx.Customers.Where(o => o != null).Count();
                Assert.Equal(1, count2);
            }
        }
    }
}
