using System;
using System.Data.Entity;
using Persistence.Model;

namespace Persistence
{
    public interface IStoreContext: IDisposable
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<BasketEntry> BasketEntries { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Photo> Photos { get; set; }

        int SaveChanges();
    }
}