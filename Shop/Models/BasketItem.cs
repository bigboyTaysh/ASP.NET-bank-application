using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class BasketItem
    {
        public int ID { get; set; }
        public Product Product { get; set; }
        public Basket Basket { get; set; }
    }
}
