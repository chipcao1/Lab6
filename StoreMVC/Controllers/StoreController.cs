using System;
using System.Linq;
using System.Web.Mvc;
using StoreMVC.Models;

namespace StoreMVC.Controllers
{
    public class StoreController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private readonly int pageSize = 5;

        public ActionResult Index(int? page, int? categoryId)
        {
            int pageNumber = (page ?? 1);
            var products = db.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
                ViewBag.CurrentCategoryId = categoryId.Value;
            }

            int totalProduct = products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProduct / pageSize);
            ViewBag.CurrentPage = pageNumber;

            var pagedProducts = products.OrderBy(p => p.Id)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

            return View(pagedProducts);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
    }
}