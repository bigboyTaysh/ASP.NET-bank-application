using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public bool Available { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<Picture> Pictures { get; set; }
    }
}
