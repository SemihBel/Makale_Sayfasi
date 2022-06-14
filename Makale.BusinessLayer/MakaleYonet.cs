using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale.Entities;
using Makale.DataAccessLayer;

namespace Makale.BusinessLayer
{
    public class MakaleYonet
    {
        private Repository<Metin> repoMetin = new Repository<Metin>();

        BusinessLayerResult<Metin> metinResult = new BusinessLayerResult<Metin>();
        public List<Metin> MakaleGetir()
        {
            return repoMetin.List();
        }
        public IQueryable<Metin> MakaleGetirQueryable()
        {
            return repoMetin.ListQueryable();
        }

        public Metin MakaleBul(int id)
        {
            return repoMetin.Find(x => x.Id == id);
        }

        public BusinessLayerResult<Metin> MakaleKaydet(Metin metin)
        {
            metinResult.Sonuc = repoMetin.Find(x => x.Baslik == metin.Baslik && x.KategoriId == metin.KategoriId);

            if (metinResult.Sonuc!=null)
            {
                metinResult.hata.Add("Bu makale kayıtlı");
            }
            else
            {
                Metin m = new Metin();
                m.Kullanici = metin.Kullanici;
                m.KategoriId = metin.KategoriId;
                m.Baslik = metin.Baslik;
                m.Icerik = metin.Icerik;
                m.TaslakMi = metin.TaslakMi;
                m.DegistirenKullanici = metin.Kullanici.KullaniciAdi;

                int sonuc = repoMetin.Insert(m);

                if (sonuc==0)
                {
                    metinResult.hata.Add("Makale kaydedilemedi.");
                }
                else
                {
                    metinResult.Sonuc = m;
                }
              
                  
            }
            return metinResult;
        }

        public BusinessLayerResult<Metin> MakaleUpdate(Metin metin)
        {
            metinResult.Sonuc = repoMetin.Find(x => x.Baslik == metin.Baslik && x.KategoriId == metin.KategoriId &&x.Id!=metin.Id);

            if (metinResult.Sonuc!=null)
            {
                metinResult.hata.Add("Bu makale kayıtlı");
            }
            else
            {
                metinResult.Sonuc = repoMetin.Find(x => x.Id == metin.Id);
                metinResult.Sonuc.KategoriId = metin.KategoriId;
                metinResult.Sonuc.Baslik = metin.Baslik;
                metinResult.Sonuc.Icerik = metin.Icerik;
                metinResult.Sonuc.TaslakMi = metin.TaslakMi;
                metinResult.Sonuc.DegistirenKullanici = metin.Kullanici.KullaniciAdi;

                int sonuc= repoMetin.Update(metinResult.Sonuc);

                if (sonuc == 0)
                {
                    metinResult.hata.Add("Makale değiştirilemedi.");
                }
                else
                {
                    metinResult.Sonuc = repoMetin.Find(x => x.Id == metin.Id); 
                }

            }
            return metinResult;
        }

        public BusinessLayerResult<Metin> MakaleSil(int id)
        {
            Metin metin = repoMetin.Find(x => x.Id == id);

            if (metin!=null)
            {
                int sonuc= repoMetin.Delete(metin);

                if (sonuc==0)
                {
                    metinResult.hata.Add("Makale Silinemedi");
                }
            }
            else
            {
                metinResult.hata.Add("Makale Bulunamadı");
            }

            return metinResult;
        }
    }
}
