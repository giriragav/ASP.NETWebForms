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

        public struct ShoppingCartUpdates
        {
            public int ProductId;
            public int PurchaseQuantity;
            public bool RemoveItem;
        }
        public void AddToCart(int id)
        {
            ShoppingCartId = GetCartID();

            var cartItem = db.ShoppingCartItems.SingleOrDefault(s => s.CartId == ShoppingCartId && s.productID==id);

            if(cartItem == null)
            {
                cartItem = new CartItem();
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

        public string GetCartID()
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

        public void UpdateShoppingCartDatabase(string cartID, ShoppingCartUpdates[] cartUpdates)
        {
            using(var db = new ProductContext())
            {
                try
                {
                    IEnumerable<CartItem> lstCart = GetCartItems();

                    foreach(CartItem cartItem in lstCart)
                    {
                        for(int i = 0; i < cartUpdates.Count(); i++)
                        {
                            if(cartItem.productID == cartUpdates[i].ProductId)
                            {
                                if(cartUpdates[i].RemoveItem || cartUpdates[i].PurchaseQuantity < 1)
                                {
                                    RemoveCartItem(cartID, cartUpdates[i].ProductId);
                                }
                                else
                                {
                                    UpdateCartItem(cartID, cartUpdates[i].ProductId, cartUpdates[i].PurchaseQuantity);
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception("Error: Unable to update cart database - " + ex.Message.ToString() + " - " + ex.InnerException.Message.ToString() + " - " + ex.InnerException.StackTrace.ToString());
                }
            }
            
        }

        private void UpdateCartItem(string cartID, int productId, int purchaseQuantity)
        {
            try
            {

                using (var db = new ProductContext())
                {
                    var cartItem = db.ShoppingCartItems.SingleOrDefault(s => s.CartId == cartID && s.productID == productId);
                    cartItem.Quantity = purchaseQuantity;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error: Unable to update cart item - " + ex.Message.ToString() + " - " + ex.StackTrace.ToString());
            }

        }

        private void RemoveCartItem(string cartID, int productId)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var cartItem = db.ShoppingCartItems.SingleOrDefault(s => s.CartId == cartID && s.productID == productId);
                    db.ShoppingCartItems.Remove(cartItem);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error: Unable to remove cart item- " + ex.Message.ToString() + " - " + ex.StackTrace.ToString());
            }
          
        }

        public decimal GetTotal()
        {
            var cartID = GetCartID();
            return (decimal?)db.ShoppingCartItems.Where(s => s.CartId == cartID).Sum(s => (s.Product.UnitPrice) * (s.Quantity)) ?? decimal.Zero;
        }

        public int GetCount()
        {
            var cartID = GetCartID();
            return (int?)db.ShoppingCartItems.Where(s => s.CartId == cartID).Sum(s => s.Quantity)??0;
        }

        public void EmptyCart()
        {
            var cartID = GetCartID();
            var cartItem = db.ShoppingCartItems.Where(s => s.CartId == cartID);
            db.ShoppingCartItems.RemoveRange(cartItem);
            db.SaveChanges();
        }
    }
}