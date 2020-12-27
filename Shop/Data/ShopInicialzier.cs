using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models;

namespace Shop.Data
{
    public class ShopInicialzier
    {
        public static void SeedAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var pictures = new List<Picture>
                {
                    new Picture { Path = "1.jpg" },
                    new Picture { Path = "2.jpg" },
                    new Picture { Path = "3.jpg" },
                    new Picture { Path = "4.jpg" },
                    new Picture { Path = "5.jpg" },
                    new Picture { Path = "6.jpg" },
                    new Picture { Path = "7.jpg" },
                };

                pictures.ForEach(p => context.Pictures.Add(p));
                context.SaveChanges();

                var categories = new List<Category>
                {
                    new Category { Name = "Polecane"},
                    new Category { Name = "Tradycyjne"},
                    new Category { Name = "Z mięsem"},
                    new Category { Name = "Wegetariańska"},
                    new Category { Name = "Na ostro"},
                };

                categories.ForEach(p => context.Categories.Add(p));
                context.SaveChanges();

                var products = new List<Product>
                {
                    new Product {
                        Name = "Margherita",
                        Description = "ciasto, sos pomidorowy, ser, oregano",
                        Price = 19.99m,
                        SalePrice = 19.99m,
                        Available = true,
                        Pictures = new List<Picture>(){ pictures[6] }
                    },
                    new Product {
                        Name = "Margheritana",
                        Description = "ciasto, sos pomidorowy, oregano, czosnek",
                        Price = 20.99m,
                        SalePrice =20.99m,
                        Available = false,
                        Pictures = new List<Picture>(){ pictures[0] }
                    },
                    new Product {
                        Name = "Americana",
                        Description = "ciasto, ser, sos barbecue, kurczak, cebula, jalapeno",
                        Price = 26.99m,
                        SalePrice =23.99m,
                        Available = true,
                        Pictures = new List<Picture>(){ pictures[1] }
                    },
                    new Product {
                        Name = "Siciliana",
                        Description = "ciasto, ser, sos pomidorowy, oliwki, pieczarki, jalapeno, salami",
                        Price = 28.99m,
                        SalePrice =28.99m,
                        Available = true,
                        Pictures = new List<Picture>(){ pictures[2] }
                    },
                    new Product {
                        Name = "Italiano",
                        Description = "ciasto, ser, sos śmietanowy, kapary, oregano, oliwki, kurczak",
                        Price = 27.99m,
                        SalePrice =27.99m,
                        Available = true,
                        Pictures = new List<Picture>(){ pictures[3] }
                    },
                    new Product {
                        Name = "Simple",
                        Description = "ciasto, ser, szynka, pomidor, papryka",
                        Price = 27.99m,
                        SalePrice =27.99m,
                        Available = true,
                        Pictures = new List<Picture>(){ pictures[4] }
                    },
                    new Product {
                        Name = "Inferno",
                        Description = "ciasto, ser, sos pomidorowy, papryczka, oregano",
                        Price = 22.99m,
                        SalePrice =22.99m,
                        Available = true,
                        Pictures = new List<Picture>(){ pictures[5] }
                    }
                };

                products.ForEach(p => context.Products.Add(p));
                context.SaveChanges();

                var productCategories = new List<ProductCategory>
                {
                    new ProductCategory { Product = products[0], Category = categories[1] },
                    new ProductCategory { Product = products[1], Category = categories[1] },
                    new ProductCategory { Product = products[1], Category = categories[3] },
                    new ProductCategory { Product = products[2], Category = categories[0] },
                    new ProductCategory { Product = products[2], Category = categories[2] },
                    new ProductCategory { Product = products[2], Category = categories[3] },
                    new ProductCategory { Product = products[3], Category = categories[0] },
                    new ProductCategory { Product = products[3], Category = categories[2] },
                    new ProductCategory { Product = products[3], Category = categories[3] },
                    new ProductCategory { Product = products[4], Category = categories[0] },
                    new ProductCategory { Product = products[4], Category = categories[2] },
                    new ProductCategory { Product = products[5], Category = categories[2] },
                    new ProductCategory { Product = products[6], Category = categories[2] },
                };

                productCategories.ForEach(p => context.ProductCategories.Add(p));
                context.SaveChanges();
            }
        }
    }
}
