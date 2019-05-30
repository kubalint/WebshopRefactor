using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using App.Mappers;
using App.Models.ViewModels;
using Contracts;
using Persistence;
using Persistence.Model;

namespace App.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IUserSessionService _userSessionService;
        private StoreContext db = new StoreContext();

        public BasketController(IBasketService basketService, IUserSessionService userSessionService)
        {
            _basketService = basketService;
            _userSessionService = userSessionService;
        }

        // GET: Basket
        public ActionResult Index(string id)
        {
            
            var products = _basketService.GetProductsInBasket(id);
            
            BasketViewModel basketViewModel = new BasketViewModel()
            {
                ProductsInBasket = products.ToDictionary(x => CustomerProductMappers.ProductToViewModel(x.Key), x => x.Value),
                Sum = (int)products.Sum(x => x.Key.Price * x.Value)
            };

            basketViewModel.UserId = id;
            return View(basketViewModel);
        }

        

        //return RedirectToAction("Index", "Basket", new { id = Session.SessionID });
        // GET: Basket
        public ActionResult AddOne(string id)
        {
            var userID = _userSessionService.GetUserID();
            _basketService.IncrementQuantityOfProductForUser(id, userID);
            return RedirectToAction("Index", new { id = userID });
        }

        // GET: Basket
        public ActionResult RemoveOne(string id)
        {
            string userID = HelperMethods.GetUserID(User, Session);

            _basketService.DecrementQuantityOfProductForUser(id, userID);

            return RedirectToAction("Index", new { id = userID });
        }

        // GET: Basket
        public ActionResult Delete(string id)
        {
            string userID = HelperMethods.GetUserID(User, Session);

            _basketService.RemoveProductForUser(id, userID);

            return RedirectToAction("Index", new { id = userID });
        }

        public ActionResult EmptyBasket()
        {
            string userID = HelperMethods.GetUserID(User, Session);

            _basketService.EmptyBasketForUser(userID);
            
            return RedirectToAction("Index", new { id = userID });
        }

        public ActionResult OrderProducts(string id)
        {

            //Dictionary<Product, int> productList = HelperMethods.GetBasketEntriesToId(id);

            //if (productList.Count == 0)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //DateTime date = DateTime.Now;
            //Guid orderId = Guid.NewGuid();
            //AnonymShippingInfos dto = new AnonymShippingInfos();
            //dto.DateOfOrder = date;
            //dto.OrderID = orderId.ToString();
            //ViewBag.ProductList = productList;

            var products = _basketService.GetProductsInBasket(id);

            BasketViewModel basketViewModel = new BasketViewModel()
            {
                ProductsInBasket = products.ToDictionary(x => CustomerProductMappers.ProductToViewModel(x.Key), x => x.Value),
                Sum = (int)products.Sum(x => x.Key.Price * x.Value)
            };

            basketViewModel.UserId = id;
            return View(new OrderProductsViewModel
            {
                Basket = basketViewModel,
                ShippingInfo = new AnonymShippingInfos()
            });

        }


    }
}