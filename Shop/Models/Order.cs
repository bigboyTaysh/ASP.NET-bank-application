using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Order
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public string CardNumber { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual List<BasketItem> Items { get; set; }
    }
}
