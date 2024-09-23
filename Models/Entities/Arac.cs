namespace AracSatis.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Arac")]
    public partial class Arac
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Arac()
        {
            Satis = new HashSet<Satis>();
        }

        public int aracID { get; set; }

        public int markaID { get; set; }

        public int sanzimanID { get; set; }

        public int renkID { get; set; }

        public int modelYili { get; set; }

        public int adet { get; set; }

        [Column(TypeName = "money")]
        public decimal satisFiyati { get; set; }

        [Required]
        [StringLength(50)]
        public string aciklama { get; set; }

        [Column(TypeName = "image")]
        public byte[] resim { get; set; }

        public virtual Marka Marka { get; set; }

        public virtual Renk Renk { get; set; }

        public virtual Sanziman Sanziman { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Satis> Satis { get; set; }
    }
}
