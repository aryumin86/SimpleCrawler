using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheCrawler.Lib.Config;
using TheCrawler.Lib.Db;
using TheCrawler.Lib.Repositories;
using TheCrawler.Service.Services;

namespace TheCrawler.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    AppConfig appConfig = configuration.GetSection("AppConfig").Get<AppConfig>();
                    services.AddSingleton(appConfig);                    

                    services.AddHostedService<Worker>();
                    services.AddSingleton<ISoursePagesRepository, SoursePagesRepository>();
                    services.AddSingleton<ICollectionSoursesRepository, CollectionSoursesRepository>();
                    services.AddSingleton<ICollectionProcessAlertsService, CollectionProcessAlertsService>();

                    var connStr = configuration.GetConnectionString("DbConnString");
                    services.AddDbContext<CollectionsContxext>(options =>
                    {
                        options.UseNpgsql(connStr);
                    });

                });
    }
}
