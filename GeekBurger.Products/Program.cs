using GeekBurger.Products.Repository;
using GeekBurger.Products.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ProductsDbContext));

builder.Services.AddDbContext<ProductsDbContext>
  (o => o.UseInMemoryDatabase("geekburger-products"));

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IProductChangedService, ProductChangedService>();

builder.Services.AddScoped<ILogService, LogService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
SeedDatabase();
app.Run();


void SeedDatabase()
{
  //using var scope = app.Services.CreateScope();
  //var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
  //ProductsContextExtensions.Seed(productsDbContext);

  using var scope = app.Services.CreateScope();
  var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
  productsDbContext.Seed();
}
