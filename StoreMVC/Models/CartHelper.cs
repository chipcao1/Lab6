using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreMVC.Models;

namespace StoreMVC.Models // Nếu bạn tạo trong thư mục Helpers, hãy đổi thành StoreMVC.Helpers
{
    public static class CartHelper
    {
        // Khai báo tên (key) để lưu giỏ hàng trong Session
        private const string CartSessionKey = "Cart"; //[cite: 34]

        // 1. Lấy giỏ hàng hiện tại từ Session
        public static List<CartItem> GetCart(HttpSessionStateBase session) //[cite: 34]
        {
            var cart = session[CartSessionKey] as List<CartItem>; //[cite: 34]
            if (cart == null) //[cite: 34]
            {
                cart = new List<CartItem>(); //[cite: 34]
                session[CartSessionKey] = cart; //[cite: 34]
            }
            return cart; //[cite: 34]
        }

        // 2. Lưu lại giỏ hàng vào Session
        public static void SaveCart(HttpSessionStateBase session, List<CartItem> cart) //[cite: 35]
        {
            session[CartSessionKey] = cart; //[cite: 35]
        }

        // 3. Thêm sản phẩm vào giỏ hàng
        public static void AddToCart(HttpSessionStateBase session, CartItem item) //[cite: 35]
        {
            var cart = GetCart(session); //[cite: 35]
            var existing = cart.FirstOrDefault(c => c.ProductId == item.ProductId); //[cite: 35]

            if (existing != null) //[cite: 35]
            {
                existing.Quantity += item.Quantity; // Nếu đã có trong giỏ thì cộng dồn số lượng[cite: 36]
            }
            else
            {
                cart.Add(item); // Nếu chưa có thì thêm mới vào giỏ[cite: 36]
            }
            SaveCart(session, cart);
        }

        // 4. Cập nhật số lượng của một sản phẩm
        public static void UpdateQuantity(HttpSessionStateBase session, int productId, int quantity) //[cite: 36]
        {
            var cart = GetCart(session); //[cite: 36]
            var item = cart.FirstOrDefault(c => c.ProductId == productId); //[cite: 36]

            if (item != null) //[cite: 36]
            {
                if (quantity > 0) //[cite: 36]
                {
                    item.Quantity = quantity; //[cite: 37]
                }
                else
                {
                    cart.Remove(item); // Xóa luôn nếu số lượng <= 0[cite: 37]
                }
                SaveCart(session, cart); //[cite: 37]
            }
        }

        // 5. Xóa hoàn toàn một sản phẩm khỏi giỏ
        public static void RemoveFromCart(HttpSessionStateBase session, int productId) //[cite: 37]
        {
            var cart = GetCart(session); //[cite: 37]
            var existing = cart.FirstOrDefault(c => c.ProductId == productId); //[cite: 37]

            if (existing != null) //[cite: 37]
            {
                cart.Remove(existing); //[cite: 37]
                SaveCart(session, cart);
            }
        }

        // 6. Xóa sạch giỏ hàng
        public static void ClearCart(HttpSessionStateBase session) //[cite: 37]
        {
            session[CartSessionKey] = new List<CartItem>(); //[cite: 37]
        }

        // 7. Tính tổng tiền giỏ hàng
        public static decimal GetTotal(HttpSessionStateBase session) //[cite: 38]
        {
            var cart = GetCart(session); //[cite: 38]
            return cart.Sum(c => c.Total); //[cite: 38]
        }

        // 8. Đếm số loại sản phẩm có trong giỏ
        public static int GetCartCount(HttpSessionStateBase session) //[cite: 38]
        {
            var cart = GetCart(session); //[cite: 38]
            return cart.Count(); //[cite: 38]
        }
    }
}