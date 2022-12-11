// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;

Console.WriteLine("Hello, World!");
Thread.Sleep(2000);
using var channel = GrpcChannel.ForAddress("http://localhost:5286");
var client = new ProductProtoService.ProductProtoServiceClient(channel);
await GetProductAsync(client);
await GetAllProductAsync(client);






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

Console.ReadKey();


