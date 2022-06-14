using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale.DataAccessLayer;
using Makale.Entities;

namespace Makale.BusinessLayer
{
   public class Test
    {
        Repository<Kullanici> repoKul = new Repository<Kullanici>();
        

        Repository<Kategori> repoKat = new Repository<Kategori>();
       
        public Test()
        {
            //DatabaseContext db = new DatabaseContext(); REPOSİTORY EDE KULLANDIGIMZI ICIN KAPADIK
            //db.Kullanicilar.ToList(); repository ı yazdıktan sonra bunu kapadık
            //db.Database.CreateIfNotExists();

            List<Kategori> katList = repoKat.List();
            List<Kullanici> kulList = repoKul.List();

        }

        public void InsertTest()
        {
            repoKul.Insert(new Kullanici()
            {
                Adi="xx",
                Soyadi="yy",
                Email="xx@yy",
                KullaniciAdi="zz",
                Sifre="123",
                AktifMi=true,
                AdminMi=true,
                AktifGuid=Guid.NewGuid(),
                KayitTarihi=DateTime.Now,
                DegistirmeTarihi=DateTime.Now.AddMinutes(5),
                DegistirenKullanici="samet"
            });
        }
        public void UpdateTest()
        {
            Kullanici kul = repoKul.Find(x => x.Adi == "xx");

            if (kul!=null)
            {
                kul.Adi = "Ömer";
                repoKul.Save();
            }
        }
        public void DeleteTest()
        {
            Kullanici kul = repoKul.Find(x => x.Adi == "Ömer");

            if (kul != null)
            {
                repoKul.Delete(kul);
            }
        }
        Repository<Yorum> repYorum = new Repository<Yorum>();
        Repository<Metin> repMetin = new Repository<Metin>();
        public void YorumTest()
        {
            
            Kullanici kul = repoKul.Find(x => x.Id == 1);
            Metin makale = repMetin.Find(x => x.Id == 3);

            Yorum yorum = new Yorum()
            {
                YorumMetni="Bu bir test yorumdur.",
                KayitTarihi=DateTime.Now,
                DegistirmeTarihi=DateTime.Now,
                DegistirenKullanici="semih",
                Kullanici=kul,

            };
            repYorum.Insert(yorum);
        }
    }
}
