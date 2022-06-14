using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Makale.Entities
{
    [Table("Kategoriler")]
    public class Kategori:EntityBase
    {
       [Required,StringLength(50),DisplayName("Kategori")]
        public string Baslik { get; set; }
        [StringLength(50)]
        public string Aciklama { get; set; }
       


        public virtual List<Metin> Makaleler { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }

        public Kategori()
        {
            Makaleler = new List<Metin>();
        }//null referans hatası almamak için olusturduk 
    }
}
