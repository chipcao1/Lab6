using System.Collections.Generic;

namespace StoreMVC.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total => CartTotal + ShippingCost;
    }
}