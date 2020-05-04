using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }


        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null && uploadImage.ContentType.Contains("image"))
            {
                byte[] imageData = null;
                //считываем следующий переданный файл в массив данных
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    product.Image = imageData;
                }
            }
            repository.SaveProduct(product);
            TempData["message"] = string.Format("\"{0}\" Добавлен в продукты", product.Name);
            return RedirectToAction("Index");

        }


        public ViewResult Edit(int productId)
        {           
             Product product = repository.Products.FirstOrDefault(b => b.ProductId == productId);

            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null && uploadImage.ContentType.Contains("image"))
            {
                byte[] imageData = null;
                //считываем следующий переданный файл в массив данных
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    product.Image = imageData;
                }
            }
            if (ModelState.IsValid)
            {                
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Изменение информации о \"{0}\" сохранены", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, string name)
        {
            repository.DeleteProduct(id);
            TempData["message"] = string.Format("\"{0}\" - Удалён", name);
            return RedirectToAction("Index");
        }
    }
}