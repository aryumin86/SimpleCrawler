using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using TheCrawler.Lib.Config;
using TheCrawler.Lib.Repositories;

namespace TheCrawler.Lib.CollectorStaff
{
    public class Collector
    {
        public int Id { get; set; }
        public bool NeedGetNewSource { get; set; }

        private AppConfig _appConfig;

        /// <summary>
        /// Data from this site is collecting now.
        /// </summary>
        private SiteCollectionTask _currentSiteCollectionTask;

        /// <summary>
        /// LInks of current site that were processed
        /// </summary>
        private HashSet<string> _processedUrls;

        /// <summary>
        /// Links of current site that were not processed yet.
        /// </summary>
        private HashSet<string> _nonProcessedUrls;

        private ICollectionSoursesRepository _sourcesRepo;

        public Collector(ICollectionSoursesRepository sourcesRepo, AppConfig appCOnfig, string baseUrl)
        {
            _appConfig = appCOnfig;
            _sourcesRepo = sourcesRepo;
            _processedUrls = new HashSet<string>();
            if(baseUrl != null)
                _nonProcessedUrls = new HashSet<string>(new string[] { baseUrl });
        }

        /// <summary>
        /// Starts the process of collection data from new source.
        /// </summary>
        private void ClearData()
        {
            _nonProcessedUrls.Clear();
            _processedUrls.Clear();
        }

        public IObservable<CollectionTask> Collect(CancellationToken cancellationToken)
        {
            var subject = new ReplaySubject<CollectionTask>();
            var intervalSource = Observable.Interval(TimeSpan.FromMilliseconds(_appConfig.CollectionIntervalInMs))
                .Subscribe(async res =>
                {
                    if(NeedGetNewSource == true)
                    {
                        _currentSiteCollectionTask = new SiteCollectionTask();
                        _currentSiteCollectionTask.CollectionSource = _sourcesRepo.GetNotProcessedCollectionSource();
                        if(_currentSiteCollectionTask.CollectionSource == null)
                        {
                            subject.OnNext(new SiteCollectionTask() {
                                Comment = "No new sources to collect data",
                                CollectionTaskState = Enums.CollectionTaskState.CANCELLED
                            });
                        }
                        else
                        {
                            _nonProcessedUrls.Add(_currentSiteCollectionTask.CollectionSource.Host);
                        }

                        NeedGetNewSource = false;
                    }

                    if (_processedUrls.Count >= _appConfig.MaxLinksPerSource || _nonProcessedUrls.Count == 0)
                    {
                        _currentSiteCollectionTask.CollectionTaskState = 
                            Enums.CollectionTaskState.COMPLETED_WITH_SUCCESS;
                        subject.OnNext(_currentSiteCollectionTask);
                        ClearData();
                        NeedGetNewSource = true;
                    }
                    else
                    {
                        var u = _nonProcessedUrls.FirstOrDefault();
                        PageCollectionTask pageTask = new PageCollectionTask();
                        pageTask.CollectionTaskState = Enums.CollectionTaskState.STARTED;
                        pageTask.WhenStarted = DateTime.Now;

                        try
                        {
                            HttpClient client = new HttpClient();
                            var html = await client.GetStringAsync(u);
                            var doc = new HtmlDocument();
                            doc.LoadHtml(html);

                            // get all page links
                            var links = doc.DocumentNode.Descendants("a")
                                .Select(a => a.GetAttributeValue("href", null))
                                .Where(a => !string.IsNullOrWhiteSpace(a));

                            foreach(var link in links)
                            {
                                if(Uri.TryCreate(link, UriKind.Absolute, out Uri absUrl))
                                {
                                    if(!_nonProcessedUrls.Contains(absUrl.AbsoluteUri) && !_processedUrls.Contains(absUrl.AbsoluteUri))
                                        _nonProcessedUrls.Add(absUrl.AbsoluteUri);
                                }
                                else
                                {
                                    var absFromRelUrl = new Uri(new Uri(_currentSiteCollectionTask.CollectionSource.Host), link);
                                    if (!_nonProcessedUrls.Contains(absFromRelUrl.AbsoluteUri) && !_processedUrls.Contains(absFromRelUrl.AbsoluteUri))
                                        _nonProcessedUrls.Add(absUrl.AbsoluteUri);
                                }
                            }

                            pageTask.SourcePage = new Entities.SourcePage()
                            {
                                CollectionSource = _currentSiteCollectionTask.CollectionSource,
                                Html = doc.DocumentNode.OuterHtml,
                                SourceId = _currentSiteCollectionTask.CollectionSource.Id,
                                Text = doc.DocumentNode.InnerText,
                                Url = u,
                                WhenCollected = DateTime.Now
                            };
                            pageTask.CollectionTaskState = Enums.CollectionTaskState.COMPLETED_WITH_SUCCESS;                            
                        }
                        catch (Exception ex)
                        {
                            pageTask.Exception = ex;
                            pageTask.CollectionTaskState = Enums.CollectionTaskState.FAILED;
                        }

                        subject.OnNext(pageTask);
                        _nonProcessedUrls.Remove(u);
                        _processedUrls.Add(u);
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        subject.OnCompleted();
                        subject.Dispose();
                    }
                });


            return subject;

        }
    }
}
