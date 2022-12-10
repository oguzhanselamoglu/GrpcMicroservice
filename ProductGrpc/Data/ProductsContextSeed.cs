using System;
using ProductGrpc.Models;

namespace ProductGrpc.Data
{
	public class ProductsContextSeed
	{
		public static void SeedAsync(ProductContext context)
		{
			if (!context.Products.Any())
			{
				var products = new List<Product>
				{
					new Product
					{
						ProductId=1,
						Name="ABC",
						Description="New ABC Product",
						Price =100,
						Status = ProductStatus.INSTOCK,
						CreatedTime = DateTime.Now
					},
					new Product
					{
						ProductId=2,
						Name="BCD",
						Description="New BCD Product",
						Price =100,
						Status = ProductStatus.INSTOCK,
						CreatedTime = DateTime.Now
					},
					new Product
					{
						ProductId=3,
						Name="CDF",
						Description="New CDF Product",
						Price =100,
						Status = ProductStatus.INSTOCK,
						CreatedTime = DateTime.Now
					}

				};

				context.Products.AddRange(products);
				context.SaveChanges();
			}
		}
	}
}

