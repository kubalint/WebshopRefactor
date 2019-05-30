using System.Collections.Generic;
using Persistence.Model;

namespace Contracts
{
    public interface IBasketService
    {
        IDictionary<Product, int> GetProductsInBasket(string id);
        void IncrementQuantityOfProductForUser(string productId, string userID);
        void DecrementQuantityOfProductForUser(string productId, string userID);
        void RemoveProductForUser(string productId, string userId);
        void EmptyBasketForUser(string userId);
    }
}