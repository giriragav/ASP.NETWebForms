using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Logic;
using WingtipToys.Models;

namespace WingtipToys
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawID = Request.QueryString["productID"];
            if (!String.IsNullOrEmpty(rawID))
            {
                using (ShoppingCartActions sc = new ShoppingCartActions())
                {
                    sc.AddToCart(int.Parse(rawID));
                }
               // Response.Redirect("ShoppingCart.aspx");
            }
            
            decimal cartTotal = 0;

            ShoppingCartActions sca = new ShoppingCartActions();

            lblTotal.Text = String.Format("{0:c}",sca.GetTotal());
            
            
        }

        public List<CartItem> GetShoppingCartItems()
        {
            ShoppingCartActions sca = new ShoppingCartActions();
            return sca.GetCartItems().ToList();
        }
    }
}