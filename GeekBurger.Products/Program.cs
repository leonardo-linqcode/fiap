using GeekBurger.Products.Repository;

private const string TopicName = "ProductChangedTopic";
private static IConfiguration _configuration;
private const string SubscriptionName = "paulista_store";

var builder = WebApplication.CreateBuilder(args);

_configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json")
  .Build();

if (!serviceBusNamespace.Topics.List()
  .Any(t => t.Name
  .Equals(TopicName, StringComparison.InvariantCultureIgnoreCase)))
{
  serviceBusNamespace.Topics
      .Define(TopicName)
      .WithSizeInMB(1024)
      .Create();
}

var serviceBusNamespace =
   _configuration.GetServiceBusNamespace();

var topic = serviceBusNamespace.Topics.GetByName(TopicName);

if (topic.Subscriptions.List()
  .Any(subscription => subscription.Name
  .Equals(SubscriptionName,
         StringComparison.InvariantCultureIgnoreCase)))
{
  topic.Subscriptions.DeleteByName(SubscriptionName);
}

topic.Subscriptions
    .Define(SubscriptionName)
    .Create();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ProductsDbContext));

builder.Services.AddDbContext<ProductsDbContext>
  (o => o.UseInMemoryDatabase("geekburger-products"));

builder.Services
  .AddScoped<IProductsRepository, ProductsRepository>();
builder.Services
  .AddScoped<IStoreRepository, StoreRepository>();

//IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//builder.Services.AddSingleton(mapper);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(typeof(ProductsDbContext));
var app = builder.Build();


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
  ProductsContextExtensions.Seed(productsDbContext);
}

static IServiceBusNamespace GetServiceBusNamespace(this IConfiguration configuration)
{
  var config = configuration.GetSection("serviceBus")
               .Get<ServiceBusConfiguration>();

  var credentials = SdkContext.AzureCredentialsFactory
      .FromServicePrincipal(config.ClientId,
                     config.ClientSecret,
                     config.TenantId,
                     AzureEnvironment.AzureGlobalCloud);

  var serviceBusManager = ServiceBusManager
      .Authenticate(credentials, config.SubscriptionId);
  return serviceBusManager.Namespaces
         .GetByResourceGroup(config.ResourceGroup,
         config.NamespaceName);
}

static async void ReceiveMessages()
{
  var subscriptionClient = new SubscriptionClient(serviceBusConfiguration.ConnectionString, TopicName, SubscriptionName);

  //by default a 1=1 rule is added when subscription is created, so we need to remove it
  await subscriptionClient.RemoveRuleAsync("$Default");

  await subscriptionClient.AddRuleAsync(new RuleDescription
  {
    Filter = new CorrelationFilter { Label = _storeId },
    Name = "filter-store"
  });

  var mo = new MessageHandlerOptions(ExceptionHandler) { AutoComplete = true };

  subscriptionClient.RegisterMessageHandler(MessageHandler, mo);

  Console.ReadLine();
}

static Task MessageHandler(Message message,
   CancellationToken arg2)
{
  Console.WriteLine($"message Label: {message.Label}");
  Console.WriteLine($"CorrelationId: {message.CorrelationId}");
  var prodChangesString = Encoding.UTF8.GetString(message.Body);

  Console.WriteLine("Message Received");
  Console.WriteLine(prodChangesString);

  //Thread.Sleep(40000);

  return Task.CompletedTask;
}

static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
{
  Console.WriteLine($"Handler exception {arg.Exception}.");
  var context = arg.ExceptionReceivedContext;
  Console.WriteLine($"Endpoint: {context.Endpoint}, Path: {context.EntityPath}, Action: {context.Action}");
  return Task.CompletedTask;
}
