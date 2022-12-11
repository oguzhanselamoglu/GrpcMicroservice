// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using ProductGrpc.Protos;

Console.WriteLine("Hello, World!");
Thread.Sleep(2000);
using var channel = GrpcChannel.ForAddress("http://localhost:5286");
var client = new ProductProtoService.ProductProtoServiceClient(channel);
Console.WriteLine("GetproductAsync started..");
var response = await client.GetProductAsync(new GetProductRequest { ProductId = 1 });
Console.WriteLine($"GetProductAsync Response :{response.ToString()}");



//GetAllProducts
Console.WriteLine("GetAllProducts started..");

using var clientData = client.GetAllProducts(new GetAllProductsRequest());
while (await clientData.ResponseStream.MoveNext(new CancellationToken()))
{

    var currentProduct = clientData.ResponseStream.Current;
    Console.WriteLine(currentProduct);
}
Console.ReadKey();


