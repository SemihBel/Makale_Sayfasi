using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale.Common;
using Makale.DataAccessLayer;
using Makale.Entities;

namespace Makale.BusinessLayer
{
    public class KullaniciYonet
    {
        private Repository<Kullanici> repoKul = new Repository<Kullanici>();
        BusinessLayerResult<Kullanici> kulSonuc = new BusinessLayerResult<Kullanici>();

        public BusinessLayerResult<Kullanici> KullaniciKaydet(RegisterViewModel model)
        {
            //Kullanıcıadı ve eposte kontrolu
            //kayıt ıslemı
            //aktıvasyon maılı
            Kullanici kullanici = repoKul.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email == model.Email);
            //string hata;
            if (kullanici!=null)
            {
                if (kullanici.KullaniciAdi == model.KullaniciAdi)
                {
                    kulSonuc.hata.Add("Kullanıcı adı kayıtlı");
                    //hata = "Kullanıcı adı kayıtlı";
                }
                if (kullanici.Email==model.Email)
                {
                    kulSonuc.hata.Add("E-mail adresi kayıtlı");
                    //hata = "E-mail adresi kayıtlı";
                }
            }
            else
            {
                int sonuc = repoKul.Insert(new Kullanici()
                {
                    KullaniciAdi=model.KullaniciAdi,
                    Email = model.Email,
                    Sifre = model.Sifre,
                    AktifGuid = Guid.NewGuid(),
                    AktifMi=false,
                    AdminMi=false
                });
                if (sonuc>0)
                {
                    kulSonuc.Sonuc = repoKul.Find(x => x.Email == model.Email && x.KullaniciAdi == model.KullaniciAdi);

                    //aktivasyon maili gonderiliecek
                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");

                    string activeUrl = $"{siteUrl}/Home/UserActivate/{kulSonuc.Sonuc.AktifGuid}";

                    string body = $"Merhaba{kulSonuc.Sonuc.Adi}{kulSonuc.Sonuc.Soyadi}<br> Hesabınızı aktifleştirmek için, <a href='{activeUrl}' target='_blank'>tıklayınız</a>"; //_blank yenı sekmede ac demek

                    MailHelper.SendMail(body, kulSonuc.Sonuc.Email, "Hesap Aktifleştirme"); 
                }
            }
            return kulSonuc;
        }
        public BusinessLayerResult<Kullanici> LoginKullanici(LoginViewModel model)
        {
            kulSonuc.Sonuc = repoKul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre == model.Sifre);
            if (kulSonuc.Sonuc!=null)
            {
                if (!kulSonuc.Sonuc.AktifMi)
                {
                    kulSonuc.hata.Add("Kullanıcı aktifleştirilmemiştir. Lütfen E-mailinizi kontrol ediniz.");
                }
                
            }
            else
            {
                kulSonuc.hata.Add("Kullanıcı Adı ya da Şifre uyuşmuyor.");
            }
            return kulSonuc;
        }

        public BusinessLayerResult<Kullanici> ActivateUser(Guid aktifGuid)
        {
            kulSonuc.Sonuc = repoKul.Find(x => x.AktifGuid ==aktifGuid);
            if (kulSonuc!=null)
            {
                if (kulSonuc.Sonuc.AktifMi)
                {
                    kulSonuc.hata.Add("Kullanıcı zaten aktif edilmiştir.");
                    return kulSonuc;
                }
                kulSonuc.Sonuc.AktifMi = true;
                repoKul.Update(kulSonuc.Sonuc);
            }
            return kulSonuc;

        }

        public BusinessLayerResult<Kullanici> KullaniciBul(int id)
        {
            kulSonuc.Sonuc = repoKul.Find(x => x.Id == id);
            if (kulSonuc.Sonuc==null)
            {
                kulSonuc.hata.Add("Kullanıcı Bulunamadı.");
            }
            return kulSonuc;
        }

        public BusinessLayerResult<Kullanici> KullaniciUpdate(Kullanici model)
        {
            Kullanici user = repoKul.Find(x=>x.Id!=model.Id &&(x.KullaniciAdi==model.KullaniciAdi || x.Email==model.Email));
            if (user!=null && user.Id!=model.Id)
            {
                if (user.KullaniciAdi==model.KullaniciAdi)
                {
                    kulSonuc.hata.Add("Bu kullanıcı adı kayıtlıdır.");
                }
                if (user.Email==model.Email)
                {
                    kulSonuc.hata.Add("Bu email adresi kayıtlıdır.");
                }
                return kulSonuc;
            }
            kulSonuc.Sonuc = repoKul.Find(x => x.Id == model.Id);

            kulSonuc.Sonuc.Email = model.Email;
            kulSonuc.Sonuc.Adi = model.Adi;
            kulSonuc.Sonuc.Soyadi = model.Soyadi;
            kulSonuc.Sonuc.KullaniciAdi = model.KullaniciAdi;
            kulSonuc.Sonuc.Sifre = model.Sifre;

            if (string.IsNullOrEmpty(model.ProfilResmi)==false)
            {
                kulSonuc.Sonuc.ProfilResmi = model.ProfilResmi;
            }

            if (repoKul.Update(kulSonuc.Sonuc) == 0)
                kulSonuc.hata.Add("Kullanıcı Güncellenemedi");

            return kulSonuc;
        }

        public BusinessLayerResult<Kullanici> KullaniciSil(int id)
        {
            Kullanici user = repoKul.Find(x => x.Id == id);

            if (user==null)
            {
                kulSonuc.hata.Add("Kullanıcı Bulunamadı.");
            }
            else
            {
                if (repoKul.Delete(user) == 0)
                {
                    kulSonuc.hata.Add("Kullanıcı Silinemedi.");
                }
            }
            return kulSonuc;
        }
    }
}
