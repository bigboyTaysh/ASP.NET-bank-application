using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class ProductCategory
    {
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
