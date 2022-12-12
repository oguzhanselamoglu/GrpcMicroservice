using System;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : Protos.ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductContext context, ILogger<ProductService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
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

            var productModel = _mapper.Map<ProductModel>(product);
            return productModel;

        }
        public override async Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            var products = await _context.Products.ToListAsync();
            foreach (var product in products)
            {
                var productModel = _mapper.Map<ProductModel>(product);

                await responseStream.WriteAsync(productModel);
            }



        }

        public override async Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            var product = _mapper.Map<Product>(request.Product);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var productModel = _mapper.Map<ProductModel>(product);
            return productModel;
        }
    }
}

