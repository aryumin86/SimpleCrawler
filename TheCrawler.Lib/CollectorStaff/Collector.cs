using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;

namespace TheCrawler.Lib.CollectorStaff
{
    public class Collector
    {
        /// <summary>
        /// Urls of this site will be collected.
        /// </summary>
        private Uri _baseUri;

        /// <summary>
        /// Max number of urls of the site to open.
        /// </summary>
        private int _maxUrlsToOpen;

        /// <summary>
        /// Pase between opening urls of the site.
        /// </summary>
        private TimeSpan _interval;

        public Collector(string baseUrl, int maxUrlsToOpen, TimeSpan interval)
        {
            _baseUri = new Uri(baseUrl);
            _maxUrlsToOpen = maxUrlsToOpen;
            _interval = interval;
        }

        public IObservable<CollectionTask> Collect(CancellationToken cancellationToken)
        {
            var subject = new ReplaySubject<CollectionTask>();
            var intervalSource = Observable.Interval(_interval)
                .Take(_maxUrlsToOpen)
                .Subscribe(async res =>
                {
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
