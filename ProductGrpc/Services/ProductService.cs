using System;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductGrpc.Data;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
	public class ProductService: Protos.ProductProtoService.ProductProtoServiceBase
    {
		private readonly ProductContext _context;
		private readonly ILogger<ProductService> _logger;

        public ProductService(ProductContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public override Task<Empty> Test(Empty request, ServerCallContext context)
        {
            return base.Test(request, context);
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                // throw exception
            }
            var productModel = new ProductModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedTime = Timestamp.FromDateTime(product.CreatedTime),
                Status = ProductStatus.Instock
            };
            return productModel;
           
        }
    }
}

