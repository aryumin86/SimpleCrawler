using System;
using System.Collections.Generic;
using System.Text;

namespace TheCrawler.Lib.Config
{
    public class AppConfig
    {
        public int Concurrency { get; set; }

        /// <summary>
        /// Default data only for testing.
        /// </summary>
        public IEnumerable<string> DefaultSources { get; set; }
        public int CollectionIntervalInMs { get; set; }
        public string DefaultSearchStrRegex { get; set; }
        public int MaxLinksPerSource { get; set; }
        public string DbConnString { get; set; }
    }
}
