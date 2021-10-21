using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Last.Models.Entity;

namespace Last.Controllers
{
    public class AnaController : Controller
    {
        Context context = new Context();

        public ActionResult Anasayfa()
        {
            return View();
        }

        public ActionResult Liste()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Giris", "Giris");
            }

            else
            {
                return View(context.tblUrunSet.ToList());
            }
        }

        public ActionResult DuzenlemeListesi()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Giris", "Giris");
            }

            else
            {
                return View(context.tblUrunSet.ToList());
            }
        }

        [HttpGet]
        [ActionName("Ekle")]
        public ActionResult Ekle_Get()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Giris", "Giris");
            }

            else
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("Ekle")]
        public ActionResult Ekle_Post(tblUrunSet urun, HttpPostedFileBase file)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Giris", "Giris");
            }

            else
            {
                var yol = Path.Combine(Server.MapPath("~/resim/Urun/"), file.FileName);
                file.SaveAs(yol);
                urun.resim = file.FileName;
                context.tblUrunSet.Add(urun);
                context.SaveChanges();
                return RedirectToAction("Liste");
            }
        }

        public ActionResult Sil(int? gelenid)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Giris", "Giris");
            }

            else
            {
                var urun = context.tblUrunSet.Find(gelenid);
                context.tblUrunSet.Remove(urun);
                context.SaveChanges();
                return RedirectToAction("Liste");
            }
        }
    }
}