using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TheCrawler.Lib.Entities;

namespace TheCrawler.Lib.Repositories
{
    public class CollectionSoursesRepository : ICollectionSoursesRepository
    {
        public CollectionSource CreateCollectionSource(CollectionSource collectionSource)
        {
            throw new NotImplementedException();
        }

        public CollectionSource GetNotProcessedCollectionSource()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CollectionSource> GetSources(Expression<Func<CollectionSource, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateSource(CollectionSource collectionSource)
        {
            throw new NotImplementedException();
        }
    }
}
