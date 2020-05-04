using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contain_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ ProductId = 1, Name = "Product1"},
                new Product{ ProductId = 2, Name = "Product2"},
                new Product{ ProductId = 3, Name = "Product3"},
                new Product{ ProductId = 4, Name = "Product4"},
                new Product{ ProductId = 5, Name = "Product5"}
            });

            AdminController controller = new AdminController(mock.Object);

            List<Product> result = ((IEnumerable<Product>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result[0].Name, "Product1");
            Assert.AreEqual(result[1].Name, "Product2");
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ ProductId = 1, Name = "Product1"},
                new Product{ ProductId = 2, Name = "Product2"},
                new Product{ ProductId = 3, Name = "Product3"},
                new Product{ ProductId = 4, Name = "Product4"},
                new Product{ ProductId = 5, Name = "Product5"}
            });

            AdminController controller = new AdminController(mock.Object);

            Product product1 = controller.Edit(1).ViewData.Model as Product;
            Product product2 = controller.Edit(2).ViewData.Model as Product;
            Product product3 = controller.Edit(3).ViewData.Model as Product;


            Assert.AreEqual(1, product1.ProductId);
            Assert.AreEqual(2, product2.ProductId); 
            Assert.AreEqual(3, product3.ProductId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{ ProductId = 1, Name = "Product1"},
                new Product{ ProductId = 2, Name = "Product2"},
                new Product{ ProductId = 3, Name = "Product3"},
                new Product{ ProductId = 4, Name = "Product4"},
                new Product{ ProductId = 5, Name = "Product5"}
            });

            AdminController controller = new AdminController(mock.Object);

            Product result = controller.Edit(7).ViewData.Model as Product;


            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);

            Product product = new Product { Name = "Test" };

            ActionResult result = controller.Edit(product.ProductId);

            mock.Verify(m => m.SaveProduct(product));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Save_Invalid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);

            Product product = new Product { Name = "Test" };

            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.Edit(product.ProductId);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}