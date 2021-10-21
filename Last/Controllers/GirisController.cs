using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Last.Models.Entity;

namespace Last.Controllers
{
    public class GirisController : Controller
    {
        [HttpGet]
        [ActionName("Kayit")]
        public ActionResult Kayit_Get()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Kayit")]
        public ActionResult Kayit_Post(tblKullaniciSet kullanici)
        {
            using (var context = new Context())
            {
                if (kullanici.kullaniciadi != null && kullanici.eposta != null && kullanici.sifre != null)
                {
                    if (kullanici.kullaniciadi.Trim() == kullanici.kullaniciadi)
                    {
                        try
                        {
                            var email = new MailAddress(kullanici.eposta);
                        }
                        catch (FormatException)
                        {
                            ViewBag.Mesaj = "Geçerli email adresi yazın.";
                            return View();
                            throw;
                        }

                        var hesap = (from i in context.tblKullaniciSet
                                     where i.kullaniciadi == kullanici.kullaniciadi
                                     select i).Count();

                        if (hesap == 0)
                        {
                            kullanici.kayittarihi = DateTime.Now;
                            kullanici.kullanicituru = "aktifdegil";
                            context.tblKullaniciSet.Add(kullanici);
                            context.SaveChanges();
                            return RedirectToAction("Giris");
                        }

                        else
                        {
                            ViewBag.Mesaj = "Bu kullanıcı adı daha önce alınmış.";
                        }
                        return View();
                    }

                    else
                    {
                        ViewBag.Mesaj = "Lütfen kullanıcı adınızın başında ve sonunda boşluk bırakmayınız.";
                        return View();
                    }
                }

                else
                {
                    ViewBag.Mesaj = "Lütfen Boşluk Bırakmayınız.";
                    return View();
                }
            }
        }

        [HttpGet]
        [ActionName("Giris")]
        public ActionResult Giris_Get()
        {
            if (Session["kullanici"] != null)
            {
                return RedirectToAction("Main", "List");
            }

            else
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("Giris")]
        public ActionResult Giris_Post(tblKullaniciSet kullanici)
        {
            using (var context = new Context())
            {
                var hesap = (from i in context.tblKullaniciSet
                             where i.kullaniciadi == kullanici.kullaniciadi
                             select i).SingleOrDefault();
                if (hesap == null)
                {
                    ViewBag.Mesaj = "Böyle bir kullanıcı yok.";
                    return View();
                }

                else
                {
                    if (hesap.kullaniciadi == kullanici.kullaniciadi && hesap.sifre == kullanici.sifre)
                    {
                        if (hesap.kullanicituru == "aktifdegil")
                        {
                            return View();
                        }
                        else
                        {
                            Session["kullanici"] = hesap.id;
                            context.SaveChanges();
                            return RedirectToAction("Liste", "Ana");
                        }
                    }

                    else
                    {
                        ViewBag.Mesaj = "Kullanıcı veya Şifre Hatalı.";
                    }

                }
                return View();
            }
        }

        public ActionResult Cikis()
        {
            Session["kullanici"] = null;
            return RedirectToAction("Giris");
        }
    }
}