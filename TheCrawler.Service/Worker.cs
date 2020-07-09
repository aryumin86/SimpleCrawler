using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheCrawler.Lib.CollectorStaff;
using TheCrawler.Lib.Config;
using TheCrawler.Lib.Repositories;
using TheCrawler.Service.Services;

namespace TheCrawler.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private AppConfig _appConfig;
        private ICollectionSoursesRepository _sourcesRepo;
        private ISoursePagesRepository _pagesRepo;
        private ICollectionProcessAlertsService _collectionProcessAlertsService;

        public Worker(ILogger<Worker> logger, AppConfig appConfig, 
            ICollectionSoursesRepository sourcesRepo, ISoursePagesRepository pagesRepo,
            ICollectionProcessAlertsService collectionProcessAlertsService)
        {
            _logger = logger;
            _appConfig = appConfig;
            _sourcesRepo = sourcesRepo;
            _pagesRepo = pagesRepo;
            _collectionProcessAlertsService = collectionProcessAlertsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Creating collectors
#if DEBUG
                IEnumerable<Collector> collectors = Enumerable
                    .Range(0, _appConfig.Concurrency)
                    .Select(x => new Collector(_sourcesRepo, _appConfig, _appConfig.DefaultSources.ToArray()[x])
                    { Id = x });
#else
IEnumerable<Collector> collectors = Enumerable
                    .Range(0, _appConfig.Concurrency)
                    .Select(x => new Collector(_sourcesRepo, _appConfig, null)
                    { Id = x });
#endif

                // Each collector starts work parallely
                Parallel.ForEach(collectors, collector =>
                {
                    collector.Collect(stoppingToken)
                    .Do(x => Console.WriteLine($"{x.WhenEnded}: {x.Comment}"))
                    .Subscribe(res =>
                    {
                        Console.WriteLine(res.CollectionTaskState);
                    },
                    ex => _logger.LogError(ex.StackTrace),
                    () => _logger.LogInformation($"Collecting stopped for collector {collector.Id}"));
                });


                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
