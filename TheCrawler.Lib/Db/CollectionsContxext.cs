using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TheCrawler.Lib.Config;
using TheCrawler.Lib.Entities;

namespace TheCrawler.Lib.Db
{
    public class CollectionsContxext : DbContext
    {
        private string _connString;

        public CollectionsContxext()
        {

        }

        //public CollectionsContxext(AppConfig appConfig)
        //{
        //    _connString = appConfig.DbConnString;
        //}

        public CollectionsContxext(string connString)
        {
            _connString = connString;
        }

        public CollectionsContxext(DbContextOptions<CollectionsContxext> options, IConfiguration configuration) : base(options)
        {
            _connString = configuration.GetConnectionString("DbConnString");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseNpgsql(_connString, o => o.SetPostgresVersion(new Version(9, 6)));
            options.UseNpgsql(_connString);
        }

        public DbSet<SourcePage> SourcePages { get; set; }
        public DbSet<CollectionSource> CollectionSources { get; set; }
    }
}
