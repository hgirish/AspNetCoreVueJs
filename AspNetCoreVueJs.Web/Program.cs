using AspNetCoreVueJs.Web.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;

namespace AspNetCoreVueJs.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {          
           
            var host = CreateWebHostBuilder(args).Build();

            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var dbContext = services.GetRequiredService<EcommerceContext>();

            //    dbContext.Database.Migrate();
            //    dbContext.EnsureSeeded();
            //}
            var seed = args.Any(x => x == "/seed");
            if (seed)
            {
                args = args.Except(new[] { "/seed" }).ToArray();
            }
            if (seed)
            {
                Console.WriteLine("Seeding data");
                SeedData.EnsureSeedData(host.Services);
                return;
            }


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                );
        }
    }
}
