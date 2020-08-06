using Domain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class TestsHelper
    {
        public static Database CreateInMemoryDbContext() => new TestsHelper().CreateDbContext();

        internal Database CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<Database>()
                       .UseInMemoryDatabase($"PaymentGateway-{ Guid.NewGuid()}")
                       .Options;

            return new Database(options);
        }
    }
}
