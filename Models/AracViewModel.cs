using AracSatis.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AracSatis.Models
{
    public class AracViewModel : DbContext
    {
        public List<int> Years { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Brands { get; set; }
        public List<string> Gearboxs { get; set; }

        // Filtreleme i�in kullan�lacak de�i�kenler
        public int SelectedYear { get; set; }
        public string SelectedColor { get; set; }
        public string SelectedBrand { get; set; }
        public string SelectedGearbox { get; set; }

        // Filtrelenmi� ara�lar
        public List<Arac> Araclar { get; set; }
    }
}