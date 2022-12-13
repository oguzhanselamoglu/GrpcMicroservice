using ShoppingCartGrpc.Services;

using Microsoft.EntityFrameworkCore;
using ShoppingCartGrpc.Data;
using DiscountGrpc.Protos;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddDbContext<ShoppingCartContext>(options =>
                  options.UseInMemoryDatabase("ShoppingCart"));

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
              (o => o.Address = new Uri(builder.Configuration["GrpcConfigs:DiscountUrl"]));

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<DiscountService>();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ShoppingCartService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
SeedDatabase(app);

void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var shoppingCartContext = services.GetRequiredService<ShoppingCartContext>();
    ShoppingCartContextSeed.SeedAsync(shoppingCartContext);
}

app.Run();

