using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//var rabbitmq = builder.AddRabbitMQ("messaging");

var orderService = builder.AddProject<Projects.SagaPattern_OrderService>("orderservice");

var paymentService = builder.AddProject<Projects.SagaPattern_PaymentService>("paymentservice");

var inventoryService = builder.AddProject<Projects.SagaPattern_InventoryService>("inventoryservice");
    
builder.Build().Run();
