using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale.Entities;


using Makale.BusinessLayer;
using Makale_Sayfasi.Filter;
using Makale_Sayfasi.Models;

namespace Makale_Sayfasi.Controllers
{
    [Exc]
    public class MakaleController : Controller
    {

        MakaleYonet makaleYonet = new MakaleYonet();
        KategoriYonet kategoriYonet = new KategoriYonet();
        [Auth]
        // GET: Makale
        public ActionResult Index()
        {
            //var metins = makaleYonet.MakaleGetir(); //profılımızdekı kendı makalelerımız gorebılmek ıcın kapadık queryable ıle yazdık
            Kullanici user =(Kullanici)Session["login"];
            var metins = makaleYonet.MakaleGetirQueryable().Include("Kullanici").Where(x => x.Kullanici.Id == user.Id).OrderByDescending(x => x.DegistirmeTarihi);
            return View(metins.ToList());
        }
        public ActionResult Begendiklerim()

        {
            LikeYonet likeYonet = new LikeYonet();
            Kullanici kullanici = Session["login"] as Kullanici;

            var makale = likeYonet.BegeniGetirQueryable().Include("Kullanici").Include("Makale").Where(x => x.Kullanici.Id == kullanici.Id).Select(x => x.Makale).Include("Kategori").Include("Kullanici").OrderByDescending(x => x.DegistirmeTarihi);

            return View("Index", makale.ToList());
        }

        [Auth]
        // GET: Makale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metin metin = makaleYonet.MakaleBul(id.Value);
            if (metin == null)
            {
                return HttpNotFound();
            }
            return View(metin);
        }
        [Auth]
        // GET: Makale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik");
            return View();
        }
        [Auth]
        // POST: Makale/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Metin metin)
        {
            ModelState.Remove("KayitTarihi");
            ModelState.Remove("DegistirmeTarihi");
            ModelState.Remove("DegistirenKullanici");

            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", metin.KategoriId);

            if (ModelState.IsValid)
            {
                metin.Kullanici =(Kullanici)Session["login"];
                BusinessLayerResult<Metin>sonuc= makaleYonet.MakaleKaydet(metin);

                if (sonuc.hata.Count > 0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(metin);
                }
                return RedirectToAction("Index");
            }

            
            return View(metin);
        }
        [Auth]
        // GET: Makale/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metin metin = makaleYonet.MakaleBul(id.Value);
            if (metin == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", metin.KategoriId);
            return View(metin);
        }
        [Auth]
        // POST: Makale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Metin metin)
        {
            ModelState.Remove("KayitTarihi");
            ModelState.Remove("DegistirmeTarihi");
            ModelState.Remove("DegistirenKullanici");

            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", metin.KategoriId);
            if (ModelState.IsValid)
            {
                metin.Kullanici = (Kullanici)Session["login"];
                BusinessLayerResult<Metin> sonuc = makaleYonet.MakaleUpdate(metin);

                if (sonuc.hata.Count > 0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(metin);
                }
                //makaleYonet.MakaleUpdate(metin);
                return RedirectToAction("Index");
            }
           
            return View(metin);
        }
        [Auth]
        // GET: Makale/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metin metin = makaleYonet.MakaleBul(id.Value);
            if (metin == null)
            {
                return HttpNotFound();
            }
            return View(metin);
        }
        [Auth]
        // POST: Makale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           BusinessLayerResult<Metin>sonuc= makaleYonet.MakaleSil(id);
            //if (sonuc.hata.Count > 0)
            //{
            //    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
            //    return View();
            //} validation olmadıgından gerek yok bu kısma

            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        LikeYonet likeYonet = new LikeYonet();
        public ActionResult LikeMakale(int[]dizi)
        {
            
            List<int> likeDizi = new List<int>();

            Kullanici kullanici=Session["login"] as Kullanici;
            if (kullanici!=null)
            {
                if (dizi != null)
                {
                    likeDizi = likeYonet.List(x => x.Kullanici.Id == kullanici.Id && dizi.Contains(x.Makale.Id)).Select(x => x.Makale.Id).ToList();
                    //select makaleıd from begeni b where b.kullaniciıd=user.ıd and b.makaleid in(3,5,9)
                }
            }

           

           

            return Json(new { sonuc =likeDizi});
        }

        [HttpPost]
        public ActionResult LikeDurumuUpdate(int makaleid,bool like)
        {
            int sonuc = 0;
            Kullanici kullanici = Session["login"] as Kullanici;

            Begeni begeni = likeYonet.BegeniGetir(makaleid,kullanici.Id);
            Metin makale = makaleYonet.MakaleBul(makaleid);

            if (begeni!=null && like==false)
            {
                //delete 
               sonuc=  likeYonet.BegeniSil(begeni);
            }
            else if (begeni==null && like==true)
            {
                //insert
                sonuc = likeYonet.BegeniEkle(new Begeni() 
                { 
                    Kullanici=kullanici,
                    Makale=makale

                });
            }

            if (sonuc>0)
            {
                if (like)
                {
                    makale.BegeniSayisi++;
                }
                else
                {
                    makale.BegeniSayisi--;
                }

                BusinessLayerResult<Metin>result= makaleYonet.MakaleUpdate(makale);

                if (result.hata.Count==0)
                {
                    return Json(new { hata = false, sonuc = makale.BegeniSayisi });
                }
                
            }
            return Json(new { hata = true, sonuc = makale.BegeniSayisi });

        }
    }
}
