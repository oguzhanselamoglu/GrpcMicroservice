using Grpc.Net.Client;
using ProductGrpc.Protos;

namespace ProductWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Thread.Sleep(2000);
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        
            using var channel = GrpcChannel.ForAddress("http://localhost:5286");
            var client = new ProductProtoService.ProductProtoServiceClient(channel);
            Console.WriteLine("GetproductAsync started..");
            var response = await client.GetProductAsync(new GetProductRequest { ProductId = 1 });
            Console.WriteLine($"GetProductAsync Response :{response.ToString()}");


            await Task.Delay(_configuration.GetValue<int>("WorkerService:TaskInterval"), stoppingToken);
        }
    }
}

