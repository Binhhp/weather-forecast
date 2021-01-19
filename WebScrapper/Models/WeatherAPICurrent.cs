namespace WebScrapper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WeatherAPICurrent")]
    public partial class WeatherAPICurrent
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Ten { get; set; }

        [StringLength(150)]
        public string Datnuoc { get; set; }

        public double? Lat { get; set; }

        public double? Long { get; set; }

        public double? Nhietdo { get; set; }

        [StringLength(1500)]
        public string Anh { get; set; }

        public double? Doam { get; set; }

        public double? Tocdogio { get; set; }

        public double? Apsuat { get; set; }

        public double? May { get; set; }

        [StringLength(150)]
        public string MoTa { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTao { get; set; }
    }
}
