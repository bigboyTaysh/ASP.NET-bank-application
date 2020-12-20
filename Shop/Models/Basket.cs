using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Basket
    {
        public int ID { get; set; }
        public virtual List<Item> Items { get; set; }
    }
}
