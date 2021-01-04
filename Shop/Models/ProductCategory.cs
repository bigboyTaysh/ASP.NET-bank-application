using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class ProductCategory
    {
        [JsonIgnore]
        public int ProductID { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
        [JsonIgnore]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
