using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WingtipToys.Models;

namespace WingtipToys.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCartId { get; set; }

        private ProductContext db = new ProductContext();

        public const string CartSessionKey = "CartId";


        public void AddToCart(int id)
        {
            ShoppingCartId = GetCartID();

            var cartItem = db.ShoppingCartItems.SingleOrDefault(s => s.CartId == ShoppingCartId && s.productID==id);

            if(cartItem == null)
            {
                cartItem.CartId = ShoppingCartId;
                cartItem.ItemId = Guid.NewGuid().ToString();
                cartItem.Quantity = 1;
                cartItem.productID = id;
                cartItem.Product = db.Products.SingleOrDefault(p => p.ProductID == id);
                cartItem.DateCreated = DateTime.Now;

                db.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += 1;
            }

            db.SaveChanges();
        }

        private string GetCartID()
        {
            if(HttpContext.Current.Session[CartSessionKey] == null)
            {
                if(!String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    HttpContext.Current.Session[CartSessionKey] = new Guid().ToString();
                }
            }

            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public void Dispose()
        {
            if(db != null)
            {
                db.Dispose();
                db = null;
            }
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            ShoppingCartId = GetCartID();
            return db.ShoppingCartItems.Where(s => s.CartId == ShoppingCartId).ToList();
        }


    }
}