// OrderService/Consumers/PaymentProcessedEventConsumer.cs
using MassTransit;
using SagaPattern.Shared;
// OrderService/Consumers/InventoryUpdatedEventConsumer.cs
public class InventoryUpdatedEventConsumer : IConsumer<InventoryUpdatedEvent>
{
    private readonly ILogger<InventoryUpdatedEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public InventoryUpdatedEventConsumer(
        ILogger<InventoryUpdatedEventConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<InventoryUpdatedEvent> context)
    {
        var message = context.Message;

        await _publishEndpoint.Publish(new OrderCompletedEvent
        {
            OrderId = message.OrderId,
            Status = message.Success ? OrderStatus.Completed : OrderStatus.Failed
        });

        _logger.LogInformation("La commande {OrderId} est {Status}", 
            message.OrderId, 
            message.Success ? "terminée avec succès" : "échouée");
    }
}