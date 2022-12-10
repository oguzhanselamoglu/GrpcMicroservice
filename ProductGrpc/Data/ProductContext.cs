using System;
using ProductGrpc.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductGrpc.Data
{
	public class ProductContext: DbContext
	{
		public ProductContext(DbContextOptions<ProductContext> options):base(options)
		{

		}

		public DbSet<Product> Products { get; set; }
	}
}

