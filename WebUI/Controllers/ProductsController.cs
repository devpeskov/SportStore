using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private IProductRepository repository;
        public ProductsController(IProductRepository repo)
        {
            repository = repo;
        }
        public int pageSize = 5;

        public ViewResult List(string type,int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(b => type == null || b.Type == type)
                .OrderBy(Product => Product.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = type == null ? 
                        repository.Products.Count() :                     
                        repository.Products.Where(product => product.Type == type).Count()
                },
                CurrentType = type
            };
            return View(model);
        }
    }
}