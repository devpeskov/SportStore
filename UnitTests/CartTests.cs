using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //organization
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };

            Cart cart = new Cart();

            //Action
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            List<CartLine> results = cart.Lines.ToList();
            
            //Statement
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Product, product1);
            Assert.AreEqual(results[1].Product, product2);

        }
        [TestMethod]
        public void Can_Add_Quantity_For_Exiscting_Lines()
        {
            //organization
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };

            Cart cart = new Cart();

            //Action
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            List<CartLine> results = cart.Lines.OrderBy(m => m.Product.ProductId).ToList();

            //Statement
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 1);

        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            //organization
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };
            Product product3 = new Product { ProductId = 3, Name = "Product3" };

            Cart cart = new Cart();

            //Action
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            cart.AddItem(product3, 2);
            cart.RemoveLine(product2);

            //Statement
            Assert.AreEqual(cart.Lines.Where(m => m.Product == product2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);

        }
        [TestMethod]
        public void Calculate_Cart_Total ()
        {
            //organization
            Product product1 = new Product { ProductId = 1, Name = "Product1", Price = 100 };
            Product product2 = new Product { ProductId = 2, Name = "Product2", Price = 55 };

            Cart cart = new Cart();

            //Action
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            decimal result = cart.ComputeTotalValue();

            //Statement
            Assert.AreEqual(result, 655);
        }
        [TestMethod]
        public void Calculate_Clear_Contents()
        {
            //organization
            Product product1 = new Product { ProductId = 1, Name = "Product1", Price = 100 };
            Product product2 = new Product { ProductId = 2, Name = "Product2", Price = 55 };

            Cart cart = new Cart();

            //Action
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            cart.Clear();

            //Statement
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        //Add element to the trolley
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ ProductId = 1, Name = "Product1", Type="Type1"},
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);
            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Product.ProductId, 1);
        }

        // Affter added product into cart - redirect to cart's page
        [TestMethod]
        public void Adding_Book_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ ProductId = 1, Name = "Product1", Type="Type1"},
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);

            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Content()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null, null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();

            CartController cartController = new CartController(null, mock.Object);

            ViewResult result = cartController.Checkout(cart, shippingDetails);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);


            CartController cartController = new CartController(null, mock.Object);
            cartController.ModelState.AddModelError("error", "error");


            ViewResult result = cartController.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);

        }
        [TestMethod]
        public void Cannot_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);


            CartController cartController = new CartController(null, mock.Object);


            ViewResult result = cartController.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);

        }

    }
}
