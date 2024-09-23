using AracSatis.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AracSatis.Controllers
{
    public class LoginController : Controller
    {
        AracSatisDBContext _dbContext = new AracSatisDBContext();

        // GET: Login
        public ActionResult Index(string kullaniciAdi, string sifre)
        {
            var kullanici = _dbContext.Kullanici.SingleOrDefault(x => x.kullaniciAdi == kullaniciAdi && x.sifre == sifre);

            if (kullanici != null)
            {
                try
                {
                    if (kullanici.yetkiID == 1)
                    {
                        Session["AdminID"] = kullanici.kullaniciID;
                        Session["AdminName"] = kullanici.ad + " " + kullanici.soyad;

                        return RedirectToAction("Index", "Admin");
                    }
                    else if (kullanici.yetkiID == 2)
                    {
                        Session["NormalUserID"] = kullanici.kullaniciID;
                        Session["NormalUserName"] = kullanici.ad + " " + kullanici.soyad;
                        Session["NormalUserAdres"] = kullanici.adres;
                        Session["NormalUserTelefon"] = kullanici.telefon;

                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Giriş yapılırken bir hata oluştu: " + ex.Message;
                }
            }


            return View();
        }

        public ActionResult KayitOl(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    kullanici.yetkiID = 2;

                    _dbContext.Kullanici.Add(kullanici);
                    _dbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Kayıt başarılı bir şekilde gerçekleşti.";

                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessageKayit"] = "Kayıt olunurken bir hata oluştu: " + ex.Message;
                }
            }

            return View(kullanici);
        }

        public ActionResult SifreSifirla(string kullaniciAdi, string tcNo, string sifre, string sifreTekrar)
        {
            var kullanici = _dbContext.Kullanici.SingleOrDefault(k => k.kullaniciAdi == kullaniciAdi && k.tcNo == tcNo);

            if (kullanici != null)
            {
                try
                {
                    if (sifre == sifreTekrar)
                    {
                        kullanici.sifre = sifre;

                        TempData["SuccessMessage"] = "Şifre başarılı bir şekilde sıfırlandı.";

                        _dbContext.SaveChanges();

                    }

                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    // Hata durumunda hatayı TempData'ya ekleyin
                    TempData["ErrorMessage"] = "Şifre sıfırlanırken bir hata oluştu: " + ex.Message;
                }
            }

            return View(kullanici);
        }
    }
}