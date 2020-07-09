using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCrawler.Lib.CollectorStaff;

namespace TheCrawler.Service.Services
{
    public class CollectionProcessAlertsService : ICollectionProcessAlertsService
    {
        public Task<IEnumerable<SiteCollectionTask>> GetAllSiteCollectionTasksStateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SiteCollectionTask> OnSiteWasProcessedAsync()
        {
            throw new NotImplementedException();
        }

        public Task SendPageCollectionTaskStateAsync(CollectionTask collectionTask)
        {
            throw new NotImplementedException();
        }
    }
}
