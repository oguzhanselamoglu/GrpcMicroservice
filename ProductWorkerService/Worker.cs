using Grpc.Net.Client;
using ProductGrpc.Protos;

namespace ProductWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly ProductFactory _productFactory;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, ProductFactory productFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _productFactory = productFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Thread.Sleep(2000);
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        
            using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("WorkerService:ServerUrl"));
            var client = new ProductProtoService.ProductProtoServiceClient(channel);

            _logger.LogInformation("AddProductAsync Started..");
            var addProductResponse = await client.AddProductAsync(await _productFactory.Generate());
            _logger.LogInformation("AddProduct response:{product} ", addProductResponse.ToString());
            
            


            await Task.Delay(_configuration.GetValue<int>("WorkerService:TaskInterval"), stoppingToken);
        }
    }
}

