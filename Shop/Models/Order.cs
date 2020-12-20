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
        public virtual EnumStatus Status { get; set; }
        public virtual List<Item> Items { get; set; }
    }

    public class EnumStatus
    {
        public const string
            Confirmed = "Zamówienie potwierdzone",
            Sent = "Zamówienie wysłane",
            Canceled = "Zamówienie anulowane";
    }
}
