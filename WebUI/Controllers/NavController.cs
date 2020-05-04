using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            repository = repo;
        }


        public PartialViewResult Menu(string type = null)
        {
            ViewBag.SelectedType = type;

            IEnumerable<string> types = repository.Products
                .Select(product => product.Type)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("FlexMenu", types);
        }
    }
}