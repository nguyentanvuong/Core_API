using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using WebApi.Helpers.Common;
using WebApi.Services;

namespace WebApi
{
    public class Program
    {
        private static AppSettings settings = new AppSettings();

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
            config.GetSection("AppSettings").Bind(settings);

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
            try
            {
                Log.Information("Starting CoreAPI");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls($"http://*:{settings.Port}");
                })
            .UseSerilog()
            .ConfigureServices(services => services.AddHostedService<DerivedBackgroundPrinter>());
    }
}
