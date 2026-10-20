using StoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

public class OrderController : Controller
{
    private readonly AppDbContext db = new AppDbContext();

    public ActionResult Index()
    {
        try
        {
            var cart = CartHelper.GetCart(Session);

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var orderVM = new OrderViewModel()
            {
                CartItems = cart,
                ShippingCost = 30000
            };

            ViewBag.Title = "Order page";

            return View(orderVM);
        }
        catch
        {
            return View("Error");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Order(OrderViewModel orderVM)
    {
        try
        {
            var cart = CartHelper.GetCart(Session);

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (ModelState.IsValid)
            {
                var order = new Order()
                {
                    
                    UserId = Guid.NewGuid().ToString(),

                    OrderDate = DateTime.Now,

                    FullName = orderVM.FullName,
                    Email = orderVM.Email,
                    Phone = orderVM.Phone,
                    Address = orderVM.Address,

                    Status = OrderStatus.Pending,

                    ShippingCost = 30000,

                    OrderDetails = new List<OrderDetail>()
                };

                decimal subTotal = 0;

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail()
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };

                    subTotal += item.Total;

                    order.OrderDetails.Add(orderDetail);
                }

                order.Total = subTotal;

                db.Orders.Add(order);

                await db.SaveChangesAsync();

                CartHelper.ClearCart(Session);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Data");

            orderVM.CartItems = cart;

            return View("Index", orderVM);
        }
        catch
        {
            return View("Error");
        }
    }
}