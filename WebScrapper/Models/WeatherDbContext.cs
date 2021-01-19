namespace WebScrapper.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WeatherDbContext : DbContext
    {
        public WeatherDbContext()
            : base("name=WeatherDbContext")
        {
        }

        public virtual DbSet<Scrapper7day> Scrapper7day { get; set; }
        public virtual DbSet<ScrapperCurent> ScrapperCurents { get; set; }
        public virtual DbSet<ScrapperDetail> ScrapperDetails { get; set; }
        public virtual DbSet<WeatherAPICurrent> WeatherAPICurrents { get; set; }
        public virtual DbSet<WebAPI5Day> WebAPI5Day { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
