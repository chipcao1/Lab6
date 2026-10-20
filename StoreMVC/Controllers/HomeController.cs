using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreMVC.Models;

namespace StoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            List < Category > Categories = db.Categories.ToList();
            List<Product> AllProducts = db.Products.Where(p => p.IsFeatured == false).Take(8).ToList();
            List<Product> FeaturedProducts = db.Products.Where(p => p.IsFeatured == true).Take(4).ToList();

            model.Categories = Categories;
            model.AllProducts = AllProducts;
            model.FeaturedProducts = FeaturedProducts;
            
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}