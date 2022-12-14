using ShoppingCartGrpc.Services;

using Microsoft.EntityFrameworkCore;
using ShoppingCartGrpc.Data;
using DiscountGrpc.Protos;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.ConfigureKestrel(options =>
// {
//     // Setup a HTTP/2 endpoint without TLS.
//     options.ListenLocalhost(7002, o => o.Protocols =
//         HttpProtocols.Http2);
// });

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
//builder.Services.AddAuthentication("Bearer")
//                .AddJwtBearer("Bearer", opt =>
//                {
//                    opt.Authority = "http://localhost:5005";
//                    //opt.RequireHttpsMetadata = false;
//                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//                    {
//                        ValidateAudience = false,
//                       // RequireHttpsMetadata
//                    };
//                });
//builder.Services.AddAuthorization();


var app = builder.Build();
//app.UseAuthentication();
//app.UseAuthorization();
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

