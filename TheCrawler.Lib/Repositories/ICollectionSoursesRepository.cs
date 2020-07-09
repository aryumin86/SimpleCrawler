using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TheCrawler.Lib.Entities;

namespace TheCrawler.Lib.Repositories
{
    public interface ICollectionSoursesRepository
    {
        public CollectionSource GetNotProcessedCollectionSource();
        public IEnumerable<CollectionSource> GetSources(Expression<Func<CollectionSource, bool>> predicate);
        public void UpdateSource(CollectionSource collectionSource);
        public CollectionSource CreateCollectionSource(CollectionSource collectionSource);
    }
}
