using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makale.Entities
{
    [Table("Kullanicilar")]
    public class Kullanici:EntityBase
    {
        [StringLength(25)]
        public string Adi { get; set; }
        [StringLength(25)]
        public string Soyadi { get; set; }
        [StringLength(25),Required]
        public string KullaniciAdi { get; set; }
        [Required,StringLength(50)]
        public string Email { get; set; }
        [StringLength(25),Required]
        public string Sifre { get; set; }

        [StringLength(30)]
        public string ProfilResmi { get; set; }
        public bool AdminMi { get; set; }
        public bool AktifMi { get; set; }
        [Required]
        public Guid AktifGuid { get; set; }




        public virtual List<Metin> Makaleler { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }
        public virtual List<Begeni> Begeniler { get; set; }
    }
}
