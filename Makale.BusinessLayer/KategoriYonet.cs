using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale.Entities;
using Makale.DataAccessLayer;

namespace Makale.BusinessLayer
{
    
    public class KategoriYonet
    {
        private Repository<Kategori> repoKat = new Repository<Kategori>();
        public List<Kategori> KategoriGetir()
        {
            return repoKat.List();
        }

        public Kategori KategoriBul(int id)
        {
            return repoKat.Find(x => x.Id == id);
        }
        BusinessLayerResult<Kategori> katSonuc = new BusinessLayerResult<Kategori>();
        public BusinessLayerResult<Kategori> KategoriKaydet(Kategori model)
        {
          Kategori kategori = repoKat.Find(x => x.Baslik == model.Baslik);
            
            if (kategori != null)
            {
                if (kategori.Baslik == model.Baslik)
                {
                    katSonuc.hata.Add("Kategori adı kayıtlı");
                    
                }
                
            }
            else
            {
                int sonuc = repoKat.Insert(new Kategori()
                {
                   Baslik=model.Baslik,
                   Aciklama=model.Aciklama
                });
              
            }
            return katSonuc;
        }

        public BusinessLayerResult<Kategori> KategoriUpdate(Kategori model)
        {
            Kategori kategori = repoKat.Find(x=>x.Baslik==model.Baslik && x.Id!=model.Id);

            if (kategori!=null)
            {
                katSonuc.hata.Add("Kategori adı kayıtlı");
            }
            else
            {
                katSonuc.Sonuc = repoKat.Find(x => x.Id == model.Id);
                katSonuc.Sonuc.Baslik = model.Baslik;
                katSonuc.Sonuc.Aciklama = model.Aciklama;

                int sonuc=repoKat.Update(katSonuc.Sonuc);

                if (sonuc>0)
                {
                    katSonuc.Sonuc = repoKat.Find(x => x.Id == model.Id);
                }
              

            }
            return katSonuc;
        }

        public BusinessLayerResult<Kategori> KategoriSil(int id)
        {
            Kategori kategori = repoKat.Find(x => x.Id == id);

            //kategorinin notlarını bul sil
            //metin yorumlarını bul sil
            //metin begenılerını bul sil
            //metini sil
            //kategori sil
            if (kategori==null)
            {
                katSonuc.hata.Add("Kategori Bulunamadı.");
            }


            int sonuc= repoKat.Delete(kategori);
            return katSonuc;

        }
    }
}
