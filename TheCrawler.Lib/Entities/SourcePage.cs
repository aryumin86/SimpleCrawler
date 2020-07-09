using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TheCrawler.Lib.Entities
{
    /// <summary>
    /// Source web site page.
    /// </summary>
    [Table("SourcePages")]
    public class SourcePage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime WhenAdded { get; set; }
        public DateTime WhenCollected { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }        
        public int SourceId { get; set; }

        [ForeignKey("SourceId")]
        public CollectionSource CollectionSource { get; set; }
    }
}
