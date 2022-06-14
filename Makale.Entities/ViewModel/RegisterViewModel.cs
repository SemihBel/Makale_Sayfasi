using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Makale.Entities
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez."), DisplayName("Kullanıcı Adı"),StringLength(25,ErrorMessage ="{0} max.{1} karakter olmalıdır.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez."), DisplayName("E-mail"), StringLength(50, ErrorMessage = "{0} max.{1} karakter olmalıdır."),EmailAddress(ErrorMessage ="{0} alanı için geçerli bir e-mail adresi giriniz.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez."), DisplayName("Şifre"), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} max.{1} karakter olmalıdır.")]
        public string Sifre { get; set; }


        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez."), DisplayName("Şifre Tekrar"), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} max.{1} karakter olmalıdır."),Compare("Sifre",ErrorMessage ="{0} ile {1} uyuşmuyor.")]
        public string Sifre2 { get; set; }
    }
}