// InventoryService/Consumers/PaymentProcessedEventConsumer.cs
using MassTransit;
using SagaPattern.Shared;

public class PaymentProcessedEventConsumer : IConsumer<PaymentProcessedEvent>
{
    private readonly ILogger<PaymentProcessedEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentProcessedEventConsumer(
        ILogger<PaymentProcessedEventConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        var message = context.Message;
        
        // Si le paiement a échoué, nous n'avons pas besoin de traiter l'inventaire
        if (!message.Success)
        {
            _logger.LogInformation("Paiement échoué pour la commande {OrderId}, abandon de la mise à jour du stock", 
                message.OrderId);
            return;
        }

        _logger.LogInformation("Mise à jour du stock pour la commande {OrderId}", message.OrderId);

        try
        {
            // Simulation de la mise à jour du stock
            await Task.Delay(500);
            var stockUpdateSuccess = new Random().Next(0, 10) > 1; // 90% de chances de succès

            await _publishEndpoint.Publish(new InventoryUpdatedEvent
            {
                OrderId = message.OrderId,
                Success = stockUpdateSuccess,
                FailureReason = stockUpdateSuccess ? null : "Stock insuffisant"
            });

            _logger.LogInformation("Mise à jour du stock {Status} pour la commande {OrderId}",
                stockUpdateSuccess ? "réussie" : "échouée", message.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour du stock pour la commande {OrderId}", 
                message.OrderId);
            await _publishEndpoint.Publish(new InventoryUpdatedEvent
            {
                OrderId = message.OrderId,
                Success = false,
                FailureReason = "Erreur technique lors de la mise à jour du stock"
            });
        }
    }
}