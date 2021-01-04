using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
