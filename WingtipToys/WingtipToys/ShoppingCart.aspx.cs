using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            using(ShoppingCartActions sca = new ShoppingCartActions())
            {
                var cartID = sca.GetCartID();

                ShoppingCartActions.ShoppingCartUpdates[] cartUpdates = new ShoppingCartActions.ShoppingCartUpdates[CartList.Rows.Count];

                for(int i = 0; i < CartList.Rows.Count; i++)
                {
                    IOrderedDictionary rowValues = new OrderedDictionary();
                    rowValues = GetValues(CartList.Rows[i]);
                    cartUpdates[i].ProductId = Convert.ToInt16(rowValues["ProductID"]);

                    CheckBox cbRemove = new CheckBox();
                    cbRemove = (CheckBox)CartList.Rows[i].FindControl("Remove");
                    cartUpdates[i].RemoveItem = cbRemove.Checked;

                    TextBox txtBox = new TextBox();
                    txtBox = (TextBox)CartList.Rows[i].FindControl("PurchaseQuantity");
                    cartUpdates[i].PurchaseQuantity = Convert.ToInt16(txtBox.Text);
                }

                sca.UpdateShoppingCartDatabase(cartID, cartUpdates);

                CartList.DataBind();

                lblTotal.Text = String.Format("{0:c}", sca.GetTotal());

            }

        }

        public static IOrderedDictionary GetValues(GridViewRow gridViewRow)
        {

            IOrderedDictionary values = new OrderedDictionary();
            foreach(DataControlFieldCell cell in gridViewRow.Cells)
            {
                if (cell.Visible)
                {
                    cell.ContainingField.ExtractValuesFromCell(values, cell, gridViewRow.RowState, true);
                }
            }
            return values;
        }

        protected void CheckoutBtn_Click(object sender, EventArgs e)
        {

        }

        protected void EmptyCart_Click(object sender, EventArgs e)
        {
            using(var sca = new ShoppingCartActions())
            {
                CartList.DataSource = null;
                CartList.DataBind();
                sca.EmptyCart();
            }
        }
    }
}