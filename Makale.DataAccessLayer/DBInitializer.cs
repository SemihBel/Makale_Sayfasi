using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Makale.Entities;

namespace Makale.DataAccessLayer
{
    public class DBInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Kullanici admin = new Kullanici()
            {
                Adi = "Semih",
                Soyadi = "Bel",
                Email = "semihbel@gmail.com",
                AktifMi = true,
                AdminMi = true,
                KullaniciAdi = "semih",
                Sifre = "12131213",
                ProfilResmi = "Lionel_Messi_20180626.jpg",
                AktifGuid = Guid.NewGuid(),
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now.AddMinutes(5),
                DegistirenKullanici = "semih"

            };
            context.Kullanicilar.Add(admin);
            context.SaveChanges();


            for (int i = 1; i < 10; i++)
            {
                Kullanici user = new Kullanici()
                {
                    Adi = FakeData.NameData.GetFirstName(),
                    Soyadi = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    AktifGuid=Guid.NewGuid(),
                    AktifMi=true,
                    AdminMi=false,
                    KullaniciAdi=$"user{i}",
                    Sifre="123",
                    ProfilResmi = "Lionel_Messi_20180626.jpg",
                    KayitTarihi =FakeData.DateTimeData.GetDatetime(),
                    DegistirmeTarihi=DateTime.Now,
                    DegistirenKullanici= $"user{i}"
                };
                context.Kullanicilar.Add(user);
            }
            context.SaveChanges();

            List<Kullanici> klist = context.Kullanicilar.ToList();//metin degıstıren kullanıcılara userları verebılmek ıcın yaptık

            //kategori verilerini ekledik
            for (int i = 1; i < 10; i++)
            {
                Kategori kategori = new Kategori()
                {
                    Baslik=FakeData.PlaceData.GetStreetName(),
                    Aciklama=FakeData.PlaceData.GetAddress(),
                    KayitTarihi= FakeData.DateTimeData.GetDatetime(),
                    DegistirmeTarihi= DateTime.Now,
                    DegistirenKullanici="semih"
                };
                context.Kategoriler.Add(kategori);
                //kategoriye makale eklemek ıcın for ıcıne for dongusu koyduk
                for (int j = 0; j < 6; j++)
                {
                    Metin metin = new Metin()
                    {
                        Baslik= FakeData.NameData.GetCompanyName(),
                        Icerik=FakeData.TextData.GetSentences(3),
                        TaslakMi=false,
                        BegeniSayisi=FakeData.NumberData.GetNumber(1,9),
                        Kategori=kategori,
                        KayitTarihi= FakeData.DateTimeData.GetDatetime(),
                        DegistirmeTarihi= DateTime.Now,
                        Kullanici=klist[j],
                        DegistirenKullanici=klist[j].KullaniciAdi

                    };
                    kategori.Makaleler.Add(metin);

                    for (int k = 0; k < 3; k++)
                    {
                        //makaleye yorum ekledık
                        Yorum yorum = new Yorum()
                        {
                            YorumMetni=FakeData.TextData.GetSentence(),
                            KayitTarihi= FakeData.DateTimeData.GetDatetime(),
                            DegistirmeTarihi = DateTime.Now,
                            Kullanici=klist[FakeData.NumberData.GetNumber(1,9)],
                            DegistirenKullanici= klist[FakeData.NumberData.GetNumber(1, 9)].KullaniciAdi
                        };
                        metin.Yorumlar.Add(yorum);
                    }
                    //makaleye begenı ekleyecez
                    for (int x = 0; x < metin.BegeniSayisi; x++)
                    {
                        Begeni begeni = new Begeni()
                        {
                            Kullanici=klist[FakeData.NumberData.GetNumber(1, 9)],
                            Makale=metin
                        };
                        metin.Begeniler.Add(begeni);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
