using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheCrawler.Lib.CollectorStaff;

namespace TheCrawler.Service.Services
{
    public interface ICollectionProcessAlertsService
    {
        /// <summary>
        /// Sends to client site's page collection task's state.
        /// </summary>
        /// <param name="collectionTask"></param>
        /// <returns></returns>
        public Task SendPageCollectionTaskStateAsync(CollectionTask collectionTask);

        /// <summary>
        /// Should be invoked by client to get state of all current collection tasks.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SiteCollectionTask>> GetAllSiteCollectionTasksStateAsync();

        /// <summary>
        /// Should be sent when work with site is ended.
        /// </summary>
        /// <returns></returns>
        public Task<SiteCollectionTask> OnSiteWasProcessedAsync();
    }
}
