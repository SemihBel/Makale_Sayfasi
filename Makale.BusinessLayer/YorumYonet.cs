using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale.DataAccessLayer;
using Makale.Entities;

namespace Makale.BusinessLayer
{
    public class YorumYonet
    {
        private Repository<Yorum> repYorum = new Repository<Yorum>();
        public Yorum YorumBul(int id)
        {
           return repYorum.Find(x => x.Id == id);
        }

        public int YorumUpdate(Yorum yorum)
        {
            return repYorum.Update(yorum);
        }

        public int YorumSil(Yorum yorum)
        {
            return repYorum.Delete(yorum);
        }

        public int YorumEkle(Yorum yorum)
        {
            return repYorum.Insert(yorum);
        }
    }
}
