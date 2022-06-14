using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale.BusinessLayer;
using Makale.Entities;
using Makale_Sayfasi.Filter;

namespace Makale_Sayfasi.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        // GET: Home
        MakaleYonet makaleYonet = new MakaleYonet();

        KategoriYonet kategoriYonet = new KategoriYonet();

        KullaniciYonet kullaniciYonet = new KullaniciYonet();
        public ActionResult Index()
        {
            //Test test = new Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.YorumTest();


            //int sayi = 0, sayi2 = 5;
            //int bolum = sayi2 / sayi;
            //olusturdugum exceptionu denemek için yazdık



            //return View(makaleYonet.MakaleGetir().OrderByDescending(x=>x.DegistirmeTarihi).ToList());
            

            return View(makaleYonet.MakaleGetirQueryable().Where(x => x.TaslakMi == false).OrderByDescending(x => x.DegistirmeTarihi).ToList());
        }

      
        public ActionResult Kategori(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Kategori kategori = kategoriYonet.KategoriBul(id.Value);

            if (kategori==null)
            {
                return HttpNotFound();
            }
            List<Metin> makaleler = makaleYonet.MakaleGetirQueryable().Where(x => x.TaslakMi == false && x.KategoriId==id).OrderByDescending(x => x.DegistirmeTarihi).ToList();

            return View("Index",makaleler);
        }
        public ActionResult EnBegenilenler()
        {
            return View("Index", makaleYonet.MakaleGetir().OrderByDescending(x => x.BegeniSayisi).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
               BusinessLayerResult<Kullanici> sonuc= kullaniciYonet.LoginKullanici(model);

                if (sonuc.hata.Count>0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                Session["login"] = sonuc.Sonuc;
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if (model.KullaniciAdi=="semih")
                //{
                //    ModelState.AddModelError("", "Bu kullanıcı adı kullanılıyor");
                //}
                //if (model.Email == "semih")
                //{
                //    ModelState.AddModelError("", "Bu mail adresi kullanılıyor");
                //}
                BusinessLayerResult<Kullanici> result= kullaniciYonet.KullaniciKaydet(model);
                if (result.hata.Count>0)
                {
                    result.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
               
                return RedirectToAction("RegisterOK");
            }
            return View();
        }
        public ActionResult RegisterOK()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        [Auth]
        public ActionResult ProfilGoster()
        {
            Kullanici user = Session["login"] as Kullanici;
            BusinessLayerResult<Kullanici>result= kullaniciYonet.KullaniciBul(user.Id);
            if (result.hata.Count>0)
            {
                //hata sayfasına yonlenebılır
            }
            return View(result.Sonuc);
        }
        [Auth]
        public ActionResult ProfilDuzenle()
        {
            Kullanici user = Session["login"] as Kullanici;
            BusinessLayerResult<Kullanici> result = kullaniciYonet.KullaniciBul(user.Id);
            if (result.hata.Count > 0)
            {
                //hata sayfasına yonlenebılır
            }
            return View(result.Sonuc);
        }
        [Auth]
        [HttpPost]
        public ActionResult ProfilDuzenle(Kullanici model,HttpPostedFileBase profilResim)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                if (profilResim != null && (profilResim.ContentType == "image/jpeg" || profilResim.ContentType == "image/jpg" || profilResim.ContentType == "image/png"))
                {
                    string dosyaAdi = $"user_{model.Id}.{profilResim.ContentType.Split('/')[1]}";

                    profilResim.SaveAs(Server.MapPath($"~/image/{dosyaAdi}"));
                    model.ProfilResmi = dosyaAdi;
                }

                BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.KullaniciUpdate(model);

                if (sonuc.hata.Count > 0)
                {
                    for (int i = 0; i < sonuc.hata.Count; i++)
                    {
                        ModelState.AddModelError("", (sonuc.hata)[i]);
                    }
                    return View(model);
                }

                Session["login"] = sonuc.Sonuc;

                return RedirectToAction("ProfilGoster");
            }


            return View(model);
            
        }
        [Auth]
        public ActionResult ProfilSil()
        {
            Kullanici user = Session["login"] as Kullanici;

            BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.KullaniciSil(user.Id);

            if (sonuc.hata.Count>0)
            {
                return RedirectToAction("ProfilGoster");
            }

            Session.Clear();
            return RedirectToAction("Index");
        }
        
        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.ActivateUser(id);
            if (sonuc.hata.Count>0)
            {
                TempData["error"] = sonuc.hata;
                    return RedirectToAction("UserActivateError");
            }
            return RedirectToAction("UserActivateOK");
        }
        public ActionResult UserActivateError()
        {
            List<string> hataMesaj = null;
            if (TempData["error"]!=null)
            {
                hataMesaj = TempData["error"] as List<string>;
            }
            return View(hataMesaj);
        }
        public ActionResult UserActivateOK()
        {
            return View();
        }



        public ActionResult YetkisizErisim()
        {
            return View();
        }

        public ActionResult HataSayfasi()
        {
            return View();
        }

    }
}