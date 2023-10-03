using GeekBurger.Products.Contract.Extension;
using GeekBurger.Products.Contract.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ProductsDbContext>
  (o => o.UseInMemoryDatabase("geekburger-products"));

builder.Services
  .AddScoped<IProductsRepository, ProductsRepository>();
var app = builder.Build();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
SeedDatabase();
app.MapControllers();

app.Run();

void SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    productsDbContext.Seed();
}

