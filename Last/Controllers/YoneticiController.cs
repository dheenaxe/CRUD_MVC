using Last.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Last.Controllers
{
    public class YoneticiController : Controller
    {
        Context context = new Context();

        public ActionResult Aktiflestir(int gelenid)
        {
            var kullanici = context.tblKullaniciSet.Find(Session["kullanici"]);

            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Giris", "Giris");
            }
            else
            {

                if (kullanici.kullanicituru == "yonetici")
                {
                    var hedef = context.tblKullaniciSet.Find(Convert.ToInt32(gelenid));
                    hedef.kullanicituru = "aktif";
                    context.SaveChanges();
                    return RedirectToAction("KullaniciListesi", "Ana");
                }

                else
                {
                    return RedirectToAction("Liste", "Ana");
                }
            }
        }

        public ActionResult KullaniciListesi()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Giris", "Giris");
            }

            else
            {
                var kullanici = context.tblKullaniciSet.Find(Session["kullanici"]);
                if (kullanici.kullanicituru == "yonetici")
                {
                    return View(context.tblKullaniciSet.ToList());
                }
                else
                {
                    return RedirectToAction("KullaniciListesi");
                }
            }
        }
    }
}