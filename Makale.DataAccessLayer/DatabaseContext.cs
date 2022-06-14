using System;
using System.Data.Entity;
using System.Linq;
using Makale.Entities;

namespace Makale.DataAccessLayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.SetInitializer(new DBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Metin>().HasMany(n => n.Yorumlar).WithRequired(c => c.Makale).WillCascadeOnDelete(true);

            modelBuilder.Entity<Metin>().HasMany(n => n.Begeniler).WithRequired(c => c.Makale).WillCascadeOnDelete(true);
        }





        public virtual DbSet<Kullanici> Kullanicilar { get; set; }
         public virtual DbSet<Metin> Makaleler { get; set; }
         public virtual DbSet<Yorum> Yorumlar { get; set; }
         public virtual DbSet<Kategori> Kategoriler { get; set; }
         public virtual DbSet<Begeni> Begeniler { get; set; }



    }

  
}