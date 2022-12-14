using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Services;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.ConfigureKestrel(options =>
// {
//     // Setup a HTTP/2 endpoint without TLS.
//     options.ListenLocalhost(7001, o => o.Protocols =
//         HttpProtocols.Http2);
// });
// builder.WebHost.UseUrls();
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddDbContext<ProductContext>(options => options.UseInMemoryDatabase("ProductApp"));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddGrpc(opt =>
{
    opt.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
SeedDatabase(app);

void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var productContext = services.GetRequiredService<ProductContext>();
    ProductsContextSeed.SeedAsync(productContext);
}

app.Run();

