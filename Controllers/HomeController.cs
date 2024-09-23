using AracSatis.Models;
using AracSatis.Models.Entities;
using AracSatis.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AracSatis.Controllers
{
    public class HomeController : Controller
    {
        AracSatisDBContext _dbContext = new AracSatisDBContext();

        // GET: Home
        [KullaniciAuthorize]
        public ActionResult Index()
        {
            var viewModel = new AracViewModel
            {
                Years = _dbContext.Arac.Select(a => a.modelYili).Distinct().ToList(),
                Colors = _dbContext.Arac.Select(a => a.Renk.renkAdi).Distinct().ToList(),
                Brands = _dbContext.Arac.Select(a => a.Marka.markaAdi).Distinct().ToList(),
                Gearboxs = _dbContext.Arac.Select(a => a.Sanziman.sanzimanTuru).Distinct().ToList(),
                Araclar = _dbContext.Arac.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(AracViewModel model)
        {
            var filteredCars = _dbContext.Arac.AsQueryable();

            if (model.SelectedYear != 0)
                filteredCars = filteredCars.Where(c => c.modelYili == model.SelectedYear);

            if (!string.IsNullOrEmpty(model.SelectedColor))
                filteredCars = filteredCars.Where(c => c.Renk.renkAdi == model.SelectedColor);

            if (!string.IsNullOrEmpty(model.SelectedBrand))
                filteredCars = filteredCars.Where(c => c.Marka.markaAdi == model.SelectedBrand);

            if (!string.IsNullOrEmpty(model.SelectedGearbox))
                filteredCars = filteredCars.Where(c => c.Sanziman.sanzimanTuru == model.SelectedGearbox);

            model.Years = _dbContext.Arac.Select(a => a.modelYili).Distinct().ToList();
            model.Colors = _dbContext.Arac.Select(a => a.Renk.renkAdi).Distinct().ToList();
            model.Brands = _dbContext.Arac.Select(a => a.Marka.markaAdi).Distinct().ToList();
            model.Gearboxs = _dbContext.Arac.Select(a => a.Sanziman.sanzimanTuru).Distinct().ToList();
            model.Araclar = filteredCars.ToList();

            return View(model);
        }

        [KullaniciAuthorize]
        public ActionResult AracAl(int id, string teslimatTuru)
        {
            var arac = _dbContext.Arac.SingleOrDefault(x => x.aracID == id);
            var teslimatTurleri = _dbContext.Teslimat.Select(x => new TeslimatTuru { teslimatTuru = x.teslimatTuru }).ToList();
            var teslimat = _dbContext.Teslimat.SingleOrDefault(x => x.teslimatTuru == teslimatTuru);
            var satis = new Satis();

            var viewModel = new AracSatisViewModel
            {
                Arac = arac,
                Satis = satis,
                TeslimatTurleri = teslimatTurleri
            };

            if (teslimat != null)
            {
                try
                {
                    viewModel.Satis.kullaniciID = Convert.ToInt32(Session["NormalUserID"]);
                    viewModel.Satis.aracID = arac.aracID;
                    viewModel.Satis.teslimatID = teslimat.teslimatID;
                    viewModel.Satis.fiyat = arac.satisFiyati;
                    viewModel.Satis.satisTarihi = DateTime.Now;
                    arac.adet--;
                    
                    _dbContext.Satis.Add(satis);
                    _dbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Satış başarılı bir şekilde gerçekleşti.";

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Satış işlemi sırasında bir hata oluştu: " + ex.Message;
                }
            }

            return View(viewModel);
        }

        [KullaniciAuthorize]
        public ActionResult Cikis()
        {
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Index", "Login");
        }
    }
}