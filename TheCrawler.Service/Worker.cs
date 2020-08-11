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
using TheCrawler.Lib.Entities;
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
            Task t = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                    if(stoppingToken.IsCancellationRequested)
                        await StopAsync(stoppingToken);
                }
            });
            // Creating collectors
#if DEBUG
            var defSources = _appConfig.DefaultSources.Select(x => new CollectionSource()
            { Host = new Uri(x).Host, EnabledForCollecting = true, WhenAdded = DateTime.Now });
            _sourcesRepo.SeedCollectionSources(defSources);
#endif

            IEnumerable<Collector> collectors = Enumerable
                    .Range(0, _appConfig.Concurrency)
                    //.Select(x => new Collector(_sourcesRepo, _appConfig, _appConfig.DefaultSources.ToArray()[x])
                    .Select(x => new Collector(_sourcesRepo, _appConfig, null)
                    { Id = x });

            // Each collector starts work parallely
            Parallel.ForEach(collectors, collector =>
                {
                    collector.Collect(stoppingToken)
                    //.Do(x => Console.WriteLine($"{x.WhenEnded}: {x.Comment}"))
                    .Subscribe(res =>
                    {
                        if(res is PageCollectionTask && (res as PageCollectionTask).SourcePage != null)
                        {
                            var resPageTask = res as PageCollectionTask;
                            Console.WriteLine($"Page collected. Page link: {resPageTask.SourcePage.Url}, state: {resPageTask.CollectionTaskState}, site url: {resPageTask.SourcePage?.CollectionSource?.Host}");
                            if (resPageTask.SourcePage != null && new Uri(resPageTask.SourcePage.Url).Host != resPageTask.SourcePage.CollectionSource.Host)
                            {
                                _sourcesRepo.CreateCollectionSource(new Lib.Entities.CollectionSource() { Host = resPageTask.SourcePage.Url, EnabledForCollecting = true });
                            }
                                
                        }
                        else if (res is SiteCollectionTask && (res as SiteCollectionTask).CollectionSource != null)
                        {
                            var resSourceTask = res as SiteCollectionTask;
                            Console.WriteLine($"Site data collected. Site link: {resSourceTask.CollectionSource.Host}, state: {resSourceTask.CollectionTaskState}");
                            _sourcesRepo.UpdateSource(resSourceTask.CollectionSource);
                        }
                        
                    },
                    ex => _logger.LogError(ex.StackTrace),
                    () => _logger.LogInformation($"Collecting stopped for collector {collector.Id}"));
                });


                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }
    }
}
