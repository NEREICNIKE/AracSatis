using AracSatis.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace AracSatis.Models
{
    public class AracSatisViewModel : DbContext
    {
        public Arac Arac { get; set; }
        public Satis Satis { get; set; }
        public List<TeslimatTuru> TeslimatTurleri { get; set; }
    }

    public class TeslimatTuru
    {
        public string teslimatTuru { get; set; }
    }
}