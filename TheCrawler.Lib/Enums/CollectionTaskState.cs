using System;
using System.Collections.Generic;
using System.Text;

namespace TheCrawler.Lib.Enums
{
    public enum CollectionTaskState
    {
        CREATED = 1,
        STARTED = 2,
        COMPLETED_WITH_SUCCESS = 3,
        FAILED = 4,
        CANCELLED = 5
    }
}
