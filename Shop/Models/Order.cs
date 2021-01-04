using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual List<BasketItem> Items { get; set; }
    }
}
