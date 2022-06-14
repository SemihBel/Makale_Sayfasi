using Makale.BusinessLayer;
using Makale.Entities;
using Makale_Sayfasi.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Makale_Sayfasi.Controllers
{
    [Exc]
    public class YorumController : Controller
    {
        // GET: Yorum
        public ActionResult YorumGoster(int? id)
        {
            if (id==null)
            {
                return  new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MakaleYonet makaleYonet = new MakaleYonet();
            Metin metin = makaleYonet.MakaleBul(id.Value);

            if (metin==null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialYorum",metin.Yorumlar);
        }
        YorumYonet yorumYonet = new YorumYonet();
        [Auth]
        [HttpPost]
        public ActionResult YorumUpdate(int? id,string text)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = yorumYonet.YorumBul(id.Value);

            if (yorum==null)
            {
                return new HttpNotFoundResult();
            }
            yorum.YorumMetni = text;

            if (yorumYonet.YorumUpdate(yorum)>0)
            {
                return Json(new { sonuc = true });
            };
            return Json(new { sonuc = false });

        }
        [Auth]
        public ActionResult YorumSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = yorumYonet.YorumBul(id.Value);

            if (yorum == null)
            {
                return new HttpNotFoundResult();
            }
            
            //get oldugu ıcın jsonrequestbehavıor.allowget yazdık
            if (yorumYonet.YorumSil(yorum) > 0)
            {
                return Json(new { sonuc = true },JsonRequestBehavior.AllowGet);
            };
            return Json(new { sonuc = false }, JsonRequestBehavior.AllowGet);

        }
        MakaleYonet makaleYonet = new MakaleYonet();

        [Auth]
        [HttpPost]
        public ActionResult YorumEkle(Yorum yorum,int? makaleid)
        {
            if (makaleid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metin metin = makaleYonet.MakaleBul(makaleid.Value);
            if (metin==null)
            {
                return new HttpNotFoundResult();
            }
            yorum.Makale = metin;
            yorum.Kullanici = Session["login"] as Kullanici;

            if (yorumYonet.YorumEkle(yorum) >0)
            {
                return Json(new { sonuc = true }/*, JsonRequestBehavior.AllowGet*/);
            }
            return Json(new { sonuc = false }/*, JsonRequestBehavior.AllowGet*/);
        }
    }
}