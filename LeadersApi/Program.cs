using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LeadersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, b) =>
                {
                    var env = context.HostingEnvironment;

                    b
                        .AddJsonFile("secrets.json", false, true)
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        ;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
