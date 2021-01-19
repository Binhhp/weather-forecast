namespace WebScrapper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScrapperCurent")]
    public partial class ScrapperCurent
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string Ten { get; set; }

        [StringLength(250)]
        public string smallTxt { get; set; }

        [StringLength(500)]
        public string Anh { get; set; }

        [StringLength(250)]
        public string DoF { get; set; }

        [StringLength(250)]
        public string DoC { get; set; }

        public string Chitiet { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTao { get; set; }

        [StringLength(150)]
        public string Mota { get; set; }
    }
}
