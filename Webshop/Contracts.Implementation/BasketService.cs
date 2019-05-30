using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Persistence;
using Persistence.Model;

namespace Contracts.Implementation
{
    public class BasketService : IBasketService
    {
        private readonly IStoreContextFactory _contextFactory;

        public BasketService(IStoreContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IDictionary<Product, int> GetProductsInBasket(string id)
        {
            using (var db = _contextFactory.CreateContext())
            {
                List<BasketEntry> basketEntries = db.BasketEntries.Where(x => x.UserID == id).ToList();
                var products = new Dictionary<Product, int>();

                foreach (BasketEntry entry in basketEntries)
                {
                    Product product = db.Products.Include(x => x.Category).Include(x => x.Photo).FirstOrDefault(x => x.ID.ToString() == entry.ProductID);
                    products.Add(product, entry.Quantity);
                }

                return products;
            }
        }

        public void IncrementQuantityOfProductForUser(string productId, string userID)
        {
            using (var db = _contextFactory.CreateContext())
            {
                BasketEntry entry = db.BasketEntries.Where(x => x.UserID == userID && x.ProductID == productId).SingleOrDefault();
                BasketEntry newEntry = new BasketEntry() { ProductID = entry.ProductID, UserID = userID, Quantity = entry.Quantity + 1 };
                db.BasketEntries.Remove(entry);
                db.BasketEntries.Add(newEntry);

                db.SaveChanges();
            }
        }

        public void DecrementQuantityOfProductForUser(string productId, string userID)
        {
            using (var db = _contextFactory.CreateContext())
            {
                BasketEntry entry = db.BasketEntries.Where(x => x.UserID == userID && x.ProductID == productId).SingleOrDefault();

                if (entry.Quantity > 1)
                {
                    entry.Quantity--;
                    db.SaveChanges();
                }
                else
                {
                    RemoveProductForUser(productId, userID);
                }
            }
        }

        public void RemoveProductForUser(string productId, string userId)
        {
            using (var db = _contextFactory.CreateContext())
            {
                BasketEntry entry = db.BasketEntries.Where(x => x.UserID == userId && x.ProductID == productId).SingleOrDefault();

                db.BasketEntries.Remove(entry);

                db.SaveChanges();
            }
        }

        public void EmptyBasketForUser(string userId)
        {
            using (var db = _contextFactory.CreateContext())
            {
                
                List<BasketEntry> entries = db.BasketEntries.Where(x => x.UserID == userId).ToList();

                foreach (BasketEntry entry in entries)
                {
                    db.BasketEntries.Remove(entry);
                }

                db.SaveChanges();
            }
        }
    }
}