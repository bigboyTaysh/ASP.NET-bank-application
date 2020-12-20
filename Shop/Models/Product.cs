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
        public string Details { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public virtual Category Category { get; set; }
    }
}
