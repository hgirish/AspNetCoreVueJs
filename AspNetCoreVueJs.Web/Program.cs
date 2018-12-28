using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;

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
        
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.File(hostingContext.Configuration["Serilog:LogFileName"]));
        }
    }
}
