using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makale.Entities
{
    [Table("Makaleler")]
   public class Metin:EntityBase
    {
        [Required,StringLength(60)]
        public string Baslik { get; set; }
        [Required, StringLength(1000)]
        public string Icerik { get; set; }
        public bool TaslakMi { get; set; }
        public int BegeniSayisi { get; set; }
        public int KategoriId { get; set; }//bu isimde olsun fk dıye verdık normalde olusturuyor kendısı


        public virtual Kategori Kategori { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }
        public virtual List<Begeni> Begeniler { get; set; }

        public Metin()
        {
            Yorumlar = new List<Yorum>();
            Begeniler = new List<Begeni>();
        }
    }
}
