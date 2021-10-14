using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStorage_PoC.Stores
{
    public class EFContextBuilder : IDesignTimeDbContextFactory<EFStorage>
    {
        public EFStorage CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFStorage>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=MetricsDB;Trusted_Connection=True;");

            return new EFStorage(optionsBuilder.Options);
        }
    }
}
