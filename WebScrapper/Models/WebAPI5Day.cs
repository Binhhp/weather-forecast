namespace WebScrapper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WebAPI5Day
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Anh { get; set; }

        public double? Nhietdocao { get; set; }

        public double? Nhietdothap { get; set; }

        [StringLength(250)]
        public string MoTa { get; set; }

        public double? May { get; set; }

        public double? Tocdogio { get; set; }

        public double? Doam { get; set; }

        public DateTime? NgayTao { get; set; }

        [StringLength(1500)]
        public string Ten { get; set; }
    }
}
