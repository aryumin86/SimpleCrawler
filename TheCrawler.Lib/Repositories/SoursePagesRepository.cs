using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TheCrawler.Lib.Entities;

namespace TheCrawler.Lib.Repositories
{
    public class SoursePagesRepository : ISoursePagesRepository
    {
        public SourcePage AddSourcePage(SourcePage sourcePage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SourcePage> GetSourcePages(Expression<Func<SourcePage, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateSourcePage(SourcePage sourcePage)
        {
            throw new NotImplementedException();
        }
    }
}
