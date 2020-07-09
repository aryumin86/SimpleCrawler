using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TheCrawler.Lib.Entities;

namespace TheCrawler.Lib.Repositories
{
    public interface ISoursePagesRepository
    {
        public SourcePage AddSourcePage(SourcePage sourcePage);
        public void UpdateSourcePage(SourcePage sourcePage);
        public IEnumerable<SourcePage> GetSourcePages(Expression<Func<SourcePage, bool>> predicate);
    }
}
