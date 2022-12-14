// See https://aka.ms/new-console-template for more information
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;

Console.WriteLine("Hello, World!");
Thread.Sleep(2000);
using var channel = GrpcChannel.ForAddress("https://localhost:7001");
var client = new ProductProtoService.ProductProtoServiceClient(channel);
await GetProductAsync(client);
await GetAllProductAsync(client);
await AddProductAsync(client);
await UpdateProductAsync(client);
await DeleteProductAsync(client);
await InsertBulkProductAsync(client);
await GetAllProductAsync(client);

async Task AddProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("AddProductAsync started..");
    var addProductResponse = await client.AddProductAsync(new AddProductRequest
    {
        Product = new ProductModel
        {
            Name = "ASD",
            Description = "New ASD Product",
            Price = 200,
            Status = ProductStatus.Instock,
            CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
        }
    });
    Console.WriteLine($"AddProductAsync Response :{addProductResponse.ToString()}");

}

async Task GetProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("GetproductAsync started..");
    var response = await client.GetProductAsync(new GetProductRequest { ProductId = 1 });
    Console.WriteLine($"GetProductAsync Response :{response.ToString()}");

}

async Task GetAllProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    //GetAllProducts
    Console.WriteLine("GetAllProducts started..");

    //using var clientData = client.GetAllProducts(new GetAllProductsRequest());
    //while (await clientData.ResponseStream.MoveNext(new CancellationToken()))
    //{

    //    var currentProduct = clientData.ResponseStream.Current;
    //    Console.WriteLine(currentProduct);
    //}

  
    using var clientData = client.GetAllProducts(new GetAllProductsRequest());
    await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine(responseData);
    }

}
async Task UpdateProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("UpdateProductAsync started..");
    var updateProductResponse = await client.UpdateProductAsync(new UpdateProductRequest
    {
        Product = new ProductModel
        {
            ProductId=1,
            Name = "ASD",
            Description = "New ASD Product",
            Price = 200,
            Status = ProductStatus.Instock,
            CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
        }
    });

    Console.WriteLine($"UpdateProductAsync Response :{updateProductResponse.ToString()}");

}
async Task DeleteProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("DeleteProductAsync started..");
    var deleteResponse = await client.DeleteProductAsync(new DeleteProductRequest
    {
        ProductId=2
    });

    Console.WriteLine($"DeleteProductAsync Response :{deleteResponse.ToString()}");

}
async Task InsertBulkProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("InsertBulkProductAsync started..");
    using var clientBulk = client.InsertBulkProduct();
    for (int i = 0; i < 3; i++)
    {
        var productModel = new ProductModel
        {
            Name = $"Product{i}",
            Price = 400,
            Status = ProductStatus.Instock,
            Description = "Bulk insertyed",
            CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)

        };
        await clientBulk.RequestStream.WriteAsync(productModel);
    }
    await clientBulk.RequestStream.CompleteAsync();
    var responseBulk = await clientBulk;

    Console.WriteLine($"Status: {responseBulk.Succes}. Insert Count: { responseBulk.InsertCount}");

}
Console.ReadKey();


