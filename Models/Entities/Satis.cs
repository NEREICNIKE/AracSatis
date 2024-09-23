namespace AracSatis.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Satis
    {
        public int satisID { get; set; }

        public int kullaniciID { get; set; }

        public int aracID { get; set; }

        public int teslimatID { get; set; }

        [Column(TypeName = "money")]
        public decimal fiyat { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime satisTarihi { get; set; }

        public virtual Arac Arac { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Teslimat Teslimat { get; set; }
    }
}
