using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AracSatis.Models.Entities
{
    public partial class AracSatisDBContext : DbContext
    {
        public AracSatisDBContext()
            : base("name=AracSatisDBContext")
        {
        }

        public virtual DbSet<Arac> Arac { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<Marka> Marka { get; set; }
        public virtual DbSet<Renk> Renk { get; set; }
        public virtual DbSet<Sanziman> Sanziman { get; set; }
        public virtual DbSet<Satis> Satis { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Teslimat> Teslimat { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arac>()
                .Property(e => e.satisFiyati)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Arac>()
                .HasMany(e => e.Satis)
                .WithRequired(e => e.Arac)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Satis)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Marka>()
                .HasMany(e => e.Arac)
                .WithRequired(e => e.Marka)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Renk>()
                .HasMany(e => e.Arac)
                .WithRequired(e => e.Renk)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sanziman>()
                .HasMany(e => e.Arac)
                .WithRequired(e => e.Sanziman)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Satis>()
                .Property(e => e.fiyat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Teslimat>()
                .HasMany(e => e.Satis)
                .WithRequired(e => e.Teslimat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Yetki>()
                .HasMany(e => e.Kullanici)
                .WithRequired(e => e.Yetki)
                .WillCascadeOnDelete(false);
        }
    }
}
