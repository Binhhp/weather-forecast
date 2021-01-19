namespace WebScrapper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScrapperDetail")]
    public partial class ScrapperDetail
    {
        public int Id { get; set; }

        public string Ten { get; set; }

        public string NoiDung { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTao { get; set; }

        public int? CurentId { get; set; }
    }
}
