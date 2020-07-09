using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TheCrawler.Lib.Enums;

namespace TheCrawler.Lib.Entities
{
    /// <summary>
    /// Web site to collect data.
    /// </summary>
    [Table("CollectionSource")]
    public class CollectionSource
    {
        [Key]
        public int Id { get; set; }
        public string Host { get; set; }
        public DateTime WhenAdded { get; set; }
        public DateTime WhenCollected { get; set; }
        public int PagesCollectedCount { get; set; }
        public bool EnabledForCollecting { get; set; }
    }
}
