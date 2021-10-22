using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dal
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("secrets.json", false, true)
                .Build();



            return new(
                new DbContextOptionsBuilder<AppDbContext>()
               // .UseInMemoryDatabase("lks").Options);
                   .UseSqlServer(string.Format(configuration.GetConnectionString("Azure"), configuration["SqlUserId"], configuration["SqlPassword"])).Options);
        }
    }
}
