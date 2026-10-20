using System;
using System.Web.Mvc;
using StoreMVC.Models; // Gọi đến thư mục chứa CartHelper và CartViewModel

namespace StoreMVC.Controllers
{
    public class CartController : Controller
    {
        // 1. Hiển thị trang Giỏ hàng chính
        public ActionResult Index() 
        {
            var cart = CartHelper.GetCart(Session); 
            var cartVM = new CartViewModel() 
            {
                CartItems = cart, 
                CartTotal = CartHelper.GetTotal(Session), 
                ShippingCost = 3000 // Phí ship cố định (có thể đổi thành 30000 VNĐ cho thực tế)[cite: 39]
            };
            ViewBag.Title = "Cart page"; 

            return View(cartVM); 
        }

        // 2. Widget giỏ hàng nhỏ trên Navbar (Hiển thị số lượng)
        public ActionResult _Cart() 
        {
            ViewBag.CartCount = CartHelper.GetCartCount(Session); 
            return PartialView(); 
        }

        // 3. Xử lý Thêm sản phẩm vào giỏ
        [HttpPost] 
        public ActionResult Add(int id, string name, string image, decimal price, int quantity) 
        {
            try
            {
                var item = new CartItem() 
                {
                    ProductId = id, 
                    ProductName = name, 
                    ProductImage = image, 
                    Price = price, 
                    Quantity = quantity 
                };

                CartHelper.AddToCart(Session, item); 
                return RedirectToAction("Index", "Cart"); 
            }
            catch
            {
                return View("Error"); 
            }
        }

        // 4. Xử lý Cập nhật số lượng
        [HttpPost] 
        public ActionResult UpdateQuantity(int productId, int quantity) 
        {
            try
            {
                CartHelper.UpdateQuantity(Session, productId, quantity); 
                return RedirectToAction("Index"); 
            }
            catch (Exception ex) 
            {
                return View("Error"); 
            }
        }

        // 5. Xử lý Xóa 1 sản phẩm khỏi giỏ
        public ActionResult Remove(int id) 
        {
            try
            {
                CartHelper.RemoveFromCart(Session, id); 
                return RedirectToAction("Index"); 
            }
            catch (Exception ex) 
            {
                return View("Error"); 
            }
        }

        // 6. Xử lý Xóa sạch giỏ hàng
        public ActionResult Clear() 
        {
            CartHelper.ClearCart(Session); 
            return RedirectToAction("Index"); 
        }
    }
}