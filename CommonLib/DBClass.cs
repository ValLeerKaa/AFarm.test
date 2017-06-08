using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;



namespace CommonLib
{
    public class DBClass:DbContext
    {
        public DbSet<Product> Products { get; set; }

        static DBClass()
        {
            Database.SetInitializer(new DBInitializer());
        }

        public DBClass() : base("DBConnection")
        {

        }

        class DBInitializer:DropCreateDatabaseAlways<DBClass>
        {
            protected override void Seed(DBClass context)
            {
                context.Products.Add(new Product { ProductID = 1, ProductName = "Мороженое", ProductPrice = 10, Quantity = 5 });
                context.Products.Add(new Product { ProductID = 2, ProductName = "Пирожное", ProductPrice = 12, Quantity = 4 });
                context.SaveChanges();
            }
             
        }
    }
}
