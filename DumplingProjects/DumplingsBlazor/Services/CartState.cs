using System.Collections.Generic;
using System.Linq;
using DumplingsBlazor.Models;

namespace DumplingsBlazor.Services;

public class CartState
{
    public List<CartItem> Cart { get; private set; } = new();

    public decimal TotalPrice => Cart.Sum(e => e.Quantity * e.MenuItem!.Price);


    public void AddToCart(MenuItem item)
    {
        var cartItem = Cart.FirstOrDefault(e => e.MenuItem?.Id == item.Id);
        if (cartItem == null)
        {
            Cart.Add(new CartItem { MenuItem = item, Quantity = 1 });
        }
        else
        {
            cartItem.Quantity++;
        }
    }

    public void RemoveFromCart(MenuItem item)
    {
        var cartItem = Cart.FirstOrDefault(e => e.MenuItem?.Id == item.Id);
        if (cartItem != null)
        {
            cartItem.Quantity--;
            if (cartItem.Quantity <= 0)
            {
                Cart.Remove(cartItem);
            }
        }
    }
    public void ClearCart()
    {
        Cart.Clear();
    }
    public void ClearEntry(MenuItem item)
    {
        var cartItem = Cart.FirstOrDefault(e => e.MenuItem?.Id == item.Id);
        if (cartItem != null)
        {
            Cart.Remove(cartItem);
        }
    }

}

public class CartItem
{
    public MenuItem? MenuItem { get; set; }
    public int Quantity { get; set; }
}
