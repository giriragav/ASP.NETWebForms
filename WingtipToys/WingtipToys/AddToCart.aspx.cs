using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Logic;

namespace WingtipToys
{
    public partial class AddToCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawID = Request.QueryString["productID"];
            if(!String.IsNullOrEmpty(rawID))
            {
                using(ShoppingCartActions sc = new ShoppingCartActions())
                {
                    sc.AddToCart(int.Parse(rawID));
                }
            }
            Response.Redirect("ShoppingCart.aspx");
        }
    }
}