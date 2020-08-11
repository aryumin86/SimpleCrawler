using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TheCrawler.Lib.Config;
using TheCrawler.Lib.Db;
using TheCrawler.Lib.Entities;
using System.Linq;

namespace TheCrawler.Lib.Repositories
{
    public class CollectionSoursesRepository : ICollectionSoursesRepository
    {
        private AppConfig _appConfig;
        private static object obj = new object();

        public CollectionSoursesRepository(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }
        public CollectionSource CreateCollectionSource(CollectionSource collectionSource)
        {
            using (var ctx = new CollectionsContxext(_appConfig.DbConnString))
            {
                collectionSource.Host = new Uri(collectionSource.Host).Host;
                if (ctx.CollectionSources.FirstOrDefault(x => x.Host == collectionSource.Host) == null)
                {
                    ctx.CollectionSources.Add(collectionSource);
                    ctx.SaveChanges();
                }
            }
            return collectionSource;
        }

        public CollectionSource GetNotProcessedCollectionSource()
        {
            CollectionSource res = null;
            lock (obj)
            {
                using (CollectionsContxext context = new CollectionsContxext(_appConfig.DbConnString))
                {
                    res = context.CollectionSources.FirstOrDefault(s => s.EnabledForCollecting == true && (s.WhenCollected == null
                   || s.WhenCollected.Year < DateTime.Now.AddYears(-100).Year));

                    if(res != null)
                    {
                        res.EnabledForCollecting = false;
                        context.SaveChanges();
                    }
                }
            }

            return res;
        }

        public IEnumerable<CollectionSource> GetSources(Expression<Func<CollectionSource, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateSource(CollectionSource collectionSource)
        {
            using CollectionsContxext context = new CollectionsContxext(_appConfig.DbConnString);
            context.CollectionSources.Update(collectionSource);
            context.SaveChanges();
        }

        public void SeedCollectionSources(IEnumerable<CollectionSource> sources)
        {
            using CollectionsContxext context = new CollectionsContxext(_appConfig.DbConnString);
            if (!context.CollectionSources.Any())
            {
                foreach (var s in sources)
                    context.CollectionSources.Add(s);
            }
            
            context.SaveChanges();
        }
    }
}
