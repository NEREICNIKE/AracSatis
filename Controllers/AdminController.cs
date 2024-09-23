using AracSatis.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AracSatis.Session;
using System.Security;

namespace AracSatis.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [AdminAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        AracSatisDBContext _dbContext = new AracSatisDBContext();

        [AdminAuthorize]
        public ActionResult KullaniciEkle(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    kullanici.yetkiID = 2;
                    // Kullanıcıyı veritabanına ekleyin
                    _dbContext.Kullanici.Add(kullanici);
                    _dbContext.SaveChanges();

                    // Başarılı mesajını TempData'ya ekle
                    TempData["SuccessMessageEkleme"] = "Kullanıcı başarılı bir şekilde eklendi.";

                    // Kullanıcıyı ekleme formuna yeniden yönlendir
                    return RedirectToAction("KullaniciEkle");
                }
                catch (Exception ex)
                {
                    // Hata durumunda hatayı TempData'ya ekle
                    TempData["ErrorMessageEkleme"] = "Kullanıcı eklenirken bir hata oluştu: " + ex.Message;
                }
            }

            // Eğer model geçerli değilse veya işlem başarısız olursa, aynı sayfaya geri dönün
            return View(kullanici);
        }

        [AdminAuthorize]
        public ActionResult KullaniciDuzenle()
        {
            var kullaniciData = _dbContext.Kullanici.Where(k => k.yetkiID == 2).ToList();
            return View(kullaniciData);
        }

        [AdminAuthorize]
        public ActionResult KullaniciGuncelle(int id)
        {
            try
            {
                // Veritabanında kullanıcıyı bul
                var kullanici = _dbContext.Kullanici.SingleOrDefault(k => k.kullaniciID == id);

                if (kullanici != null)
                {
                    // Kullanıcıyı güncelleme sayfasına yönlendir
                    return View(kullanici);
                }
                // Kullanıcı bulunamadıysa hata mesajı göster
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";

            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını göster
                TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu: " + ex.Message;
            }
            // Eğer ModelState geçerli değilse veya işlem başarısızsa aynı sayfaya dön
            return RedirectToAction("KullaniciDuzenle");
        }

        [HttpPost]
        public ActionResult KullaniciGuncelle(int id, Kullanici model)
        {
            try
            {
                // Veritabanında kullanıcıyı bul
                var kullanici = _dbContext.Kullanici.SingleOrDefault(k => k.kullaniciID == model.kullaniciID);

                if (kullanici is null || kullanici.kullaniciID != id)
                {
                    // Kullanıcı bulunamadıysa hata mesajı göster
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return View();
                }

                model.yetkiID = 2;
                var entry = _dbContext.Entry(kullanici);
                entry.CurrentValues.SetValues(model);
                entry.State = EntityState.Modified;
                _dbContext.SaveChanges();
                TempData["SuccesMessage"] = "Kullanıcı başarılı bir şekilde güncellendi.";

            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını göster
                TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu: " + ex.Message;
            }


            // Eğer ModelState geçerli değilse veya işlem başarısızsa aynı sayfaya dön
            return RedirectToAction("KullaniciDuzenle");
        }

        [AdminAuthorize]
        public ActionResult KullaniciSil(int id)
        {
            var kullanici = _dbContext.Kullanici.Where(k => k.kullaniciID == id);

            _dbContext.Kullanici.RemoveRange(kullanici);
            _dbContext.SaveChanges();

            // Başarılı mesajını TempData'ya ekle
            TempData["SuccessMessage"] = "Kullanıcı başarılı bir şekilde silindi.";

            return RedirectToAction("KullaniciDuzenle");
        }

        [AdminAuthorize]
        public ActionResult AracEkle()
        {
            ViewBag.Marka = _dbContext.Marka.ToList();
            ViewBag.Renk = _dbContext.Renk.ToList();
            ViewBag.Sanziman = _dbContext.Sanziman.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AracEkle(HttpPostedFileBase resim, int markaID, int renkID, int sanzimanID, string aciklama, string modelYili, string satisFiyati, string adet)
        {
            try
            {
                Arac arac = new Arac();
                if (resim != null && resim.ContentLength > 0)
                {
                    // Dosya türünü kontrol etme
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(resim.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["ErrorMessageEkleme"] = "Sadece JPG, JPEG ve PNG dosyaları yüklenebilir.";
                        return View();
                    }

                    // Dosyayı byte array'e çevirme
                    using (var binaryReader = new BinaryReader(resim.InputStream))
                    {
                        arac.resim = binaryReader.ReadBytes(resim.ContentLength);
                    }
                }

                arac.markaID = markaID;
                arac.renkID = renkID;
                arac.sanzimanID = sanzimanID;
                arac.aciklama = aciklama;
                arac.modelYili = Convert.ToInt32(modelYili);    
                arac.satisFiyati = Convert.ToDecimal(satisFiyati);
                arac.adet = Convert.ToInt32(adet);

                _dbContext.Arac.Add(arac);
                _dbContext.SaveChanges();

                // Başarılı mesajını TempData'ya ekle
                TempData["SuccessMessageEkleme"] = "Araç başarılı bir şekilde eklendi.";

                // Kullanıcıyı ekleme formuna yeniden yönlendir
                return RedirectToAction("AracEkle");
            }
            catch (Exception ex)
            {
                // Hata durumunda hatayı TempData'ya ekle
                TempData["ErrorMessageEkleme"] = "Araç eklenirken bir hata oluştu: " + ex.Message;
            }
                        
            // Eğer model geçerli değilse veya işlem başarısız olursa, aynı sayfaya geri dönün
            return View();
        }

        [AdminAuthorize]
        public ActionResult AracDuzenle()
        {
            var aracData = _dbContext.Arac.ToList();
            return View(aracData);
        }

        [AdminAuthorize]
        public ActionResult AracGuncelle(int id)
        {
            try
            {
                // Veritabanında kullanıcıyı bul
                var arac = _dbContext.Arac.SingleOrDefault(k => k.aracID == id);

                if (arac != null)
                {
                    // Kullanıcıyı güncelleme sayfasına yönlendir
                    return View(arac);
                }
                // Kullanıcı bulunamadıysa hata mesajı göster
                TempData["ErrorMessage"] = "Araç bulunamadı.";

            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını göster
                TempData["ErrorMessage"] = "Araç güncellenirken bir hata oluştu: " + ex.Message;
            }
            // Eğer ModelState geçerli değilse veya işlem başarısızsa aynı sayfaya dön
            return RedirectToAction("AracDuzenle");
        }

        [HttpPost]
        public ActionResult AracGuncelle(int id, Arac model)
        {
            try
            {
                // Veritabanında kullanıcıyı bul
                var arac = _dbContext.Arac.SingleOrDefault(k => k.aracID == model.aracID);

                if (arac is null || arac.aracID != id)
                {
                    // Kullanıcı bulunamadıysa hata mesajı göster
                    TempData["ErrorMessage"] = "Araç bulunamadı.";
                    return View();
                }

                // İlgili Marka, Renk, Şanzıman bilgilerini veritabanından bul
                var marka = _dbContext.Marka.SingleOrDefault(k => k.markaAdi == model.Marka.markaAdi);
                var renk = _dbContext.Renk.SingleOrDefault(k => k.renkAdi == model.Renk.renkAdi);
                var sanziman = _dbContext.Sanziman.SingleOrDefault(k => k.sanzimanTuru == model.Sanziman.sanzimanTuru);

                // Marka, Renk veya Şanzıman bilgileri bulunamazsa hata mesajı göster ve geri dön
                if (marka == null || renk == null || sanziman == null)
                {
                    TempData["ErrorMessage"] = "Geçerli Marka, Renk veya Şanzıman bilgisi bulunamadı.";
                    return RedirectToAction("AracDuzenle", new { id = model.aracID });
                }

                model.markaID = marka.markaID;
                model.renkID = renk.renkID;
                model.sanzimanID = sanziman.sanzimanID;
                model.resim = arac.resim;
                var entry = _dbContext.Entry(arac);
                entry.CurrentValues.SetValues(model);
                entry.State = EntityState.Modified;
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Araç başarılı bir şekilde güncellendi.";

            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını göster
                TempData["ErrorMessage"] = "Araç güncellenirken bir hata oluştu: " + ex.Message;
            }


            // Eğer ModelState geçerli değilse veya işlem başarısızsa aynı sayfaya dön
            return RedirectToAction("AracDuzenle");
        }

        [AdminAuthorize]
        public ActionResult AracSil(int id)
        {
            var arac = _dbContext.Arac.Where(k => k.aracID == id);

            _dbContext.Arac.RemoveRange(arac);
            _dbContext.SaveChanges();

            // Başarılı mesajını TempData'ya ekle
            TempData["SuccessMessage"] = "Araç başarılı bir şekilde silindi.";

            return RedirectToAction("AracDuzenle");
        }

        [AdminAuthorize]
        public ActionResult Satislar()
        {
            var satisData = _dbContext.Satis.ToList();
            return View(satisData);
        }

        [AdminAuthorize]
        public ActionResult SatisSil(int id)
        {
            var satis = _dbContext.Satis.Where(k => k.satisID == id);

            _dbContext.Satis.RemoveRange(satis);
            _dbContext.SaveChanges();

            // Başarılı mesajını TempData'ya ekle
            TempData["SuccessMessage"] = "Satış başarılı bir şekilde silindi.";

            return RedirectToAction("Satislar");
        }

        [AdminAuthorize]
        public ActionResult Cikis()
        {
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Index", "Login");
        }
    }
}