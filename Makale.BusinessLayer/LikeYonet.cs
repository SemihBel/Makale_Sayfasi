using Makale.DataAccessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class LikeYonet
    {
        Repository<Begeni> repBegeni = new Repository<Begeni>();
        public Begeni BegeniGetir(int makaleid, int kullaniciid)
        {
            return repBegeni.Find(x => x.Makale.Id == makaleid && x.Kullanici.Id == kullaniciid);
        }

        public IQueryable<Begeni> BegeniGetirQueryable()
        {
            return repBegeni.ListQueryable();
        }
        public List<Begeni> List(Expression<Func<Begeni, bool>> kosul)
        {
            return repBegeni.List(kosul);
        }

        public int BegeniEkle(Begeni begeni)
        {
            return repBegeni.Insert(begeni);
        }
        public int BegeniSil(Begeni begeni)
        {
            return repBegeni.Delete(begeni);
        }
    }
}
