namespace AracSatis.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanici")]
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            Satis = new HashSet<Satis>();
        }

        public int kullaniciID { get; set; }

        public int yetkiID { get; set; }

        [Required]
        [StringLength(11)]
        [Display(Name = "TC Kimlik")]
        public string tcNo { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Ad")]
        public string ad { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Soyad")]
        public string soyad { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name = "Adres")]
        public string adres { get; set; }

        [Required]
        [StringLength(11)]
        [Display(Name = "Telefon")]
        public string telefon { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Kullanýcý Adý")]
        public string kullaniciAdi { get; set; }

        [Required]
        [StringLength(50)]
        public string sifre { get; set; }

        public virtual Yetki Yetki { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Satis> Satis { get; set; }
    }
}
