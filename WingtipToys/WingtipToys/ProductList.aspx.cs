using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Models;

namespace WingtipToys
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Product> GetProducts([QueryString("id")] int? categoryID)
        {
            var db = new ProductContext();

            return (categoryID.HasValue && categoryID>0)?db.Products.Where(p => p.CategoryID == categoryID) :db.Products;
        }
    }
}