using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Makale.Entities

{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="{0} Alanı Boş Geçilemez."),DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez."), DisplayName("Şifre"),DataType(DataType.Password)]
        public string Sifre { get; set; }
    }
}