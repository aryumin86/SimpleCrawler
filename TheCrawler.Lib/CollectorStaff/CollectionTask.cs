using System;
using System.Collections.Generic;
using System.Text;
using TheCrawler.Lib.Enums;

namespace TheCrawler.Lib.CollectorStaff
{
    public abstract class CollectionTask
    {
        public DateTime? WhenStarted { get; set; }
        public DateTime? WhenEnded { get; set; }
        public CollectionTaskState CollectionTaskState { get; set; }
        public string Comment { get; set; }

        public CollectionTask()
        {
            CollectionTaskState = CollectionTaskState.CREATED;
        }
    }
}
