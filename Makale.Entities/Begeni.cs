using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makale.Entities
{
    [Table("Begeniler")]
    public class Begeni
    { //kim hangi makaleyi beğendi
        [Key]
        public int Id { get; set; }
       



        public virtual Metin Makale { get; set; }
        public virtual Kullanici Kullanici { get; set; }

       
    }
}
