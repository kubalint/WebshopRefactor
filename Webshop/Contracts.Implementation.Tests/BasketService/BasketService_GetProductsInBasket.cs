using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Persistence;
using Persistence.Model;
using Contracts.Implementation;
using Contracts.Implementation.Tests.Extensions;
using FluentAssertions;

namespace Contracts.Implementation.Tests.BasketService
{
    [TestClass]
    public class BasketService_GetProductsInBasket
    {
        [TestMethod]
        public void ReturnsEmpty_If_IdNotFound()
        {
            // arrange
            var ctx = Mock.Of<IStoreContext>(x => x.BasketEntries == Enumerable.Empty<BasketEntry>().AsDbSet());
            var ctxFactory = Mock.Of<IStoreContextFactory>(x => x.CreateContext() == ctx);
            var srv = new Implementation.BasketService(ctxFactory);

            // act 
            var result = srv.GetProductsInBasket("nemleszilyen");

            // assert (fluent assertions)
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void ReturnOnlyProductsInBasket_If_Found()
        {
            // arrange
            var userId = "me";
            var productIds = new[] {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()};
            var basketEntries = new List<BasketEntry>
            {
                new BasketEntry {ProductID = productIds[0].ToString(), Quantity = 3, UserID = userId},
                new BasketEntry {ProductID = productIds[1].ToString(), Quantity = 3, UserID = userId},
                new BasketEntry {ProductID = productIds[2].ToString(), Quantity = 3, UserID = "someone_else"},
            };
            var products = new List<Product>
            {
                new Product{ID = productIds[0]},
                new Product{ID = productIds[1]},
                new Product{ID = productIds[2]}
            };
            
            var ctx = Mock.Of<IStoreContext>(x => x.BasketEntries == basketEntries.AsDbSet() && x.Products == products.AsDbSet());
            var ctxFactory = Mock.Of<IStoreContextFactory>(x => x.CreateContext() == ctx);
            var srv = new Implementation.BasketService(ctxFactory);

            // act
            var result = srv.GetProductsInBasket(userId);

            // assert
            result.Should().HaveCount(2).And.Subject.Select(x => x.Key.ID).Should().NotContain(productIds[2]);
        }
    }
}