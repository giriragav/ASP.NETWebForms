using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WingtipToys.Models
{
    public class Product
    {
        [ScaffoldColumn(false)]
        public int ProductID { get; set; }

        [Required, StringLength(100)]
        public string ProductName { get; set; }

        [Required, StringLength(10000), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public double?  UnitPrice { get; set; }

        public int? CategoryID { get; set; }

        public virtual Category Category { get; set; }


    }
}