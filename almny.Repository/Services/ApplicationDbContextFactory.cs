using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using almny.Repository;

namespace _3almny.Repository.Services
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {

            var configBuilder = new ConfigurationBuilder();
            var config = configBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DBConnection"));

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
