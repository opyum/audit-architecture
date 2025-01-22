// PaymentService/Consumers/OrderCreatedEventConsumer.cs
using MassTransit;
using SagaPattern.Shared;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCreatedEventConsumer(
        ILogger<OrderCreatedEventConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderId = context.Message.OrderId;
        _logger.LogInformation("Traitement du paiement pour la commande {OrderId}", orderId);

        try
        {
            // Simulation du traitement du paiement
            await Task.Delay(1000); // Simule un appel à un service de paiement
            
            // Dans un cas réel, nous aurions ici la logique de paiement
            var paymentSuccess = new Random().Next(0, 10) > 2; // 80% de chances de succès

            await _publishEndpoint.Publish(new PaymentProcessedEvent
            {
                OrderId = orderId,
                Success = paymentSuccess,
                FailureReason = paymentSuccess ? null : "Paiement refusé"
            });

            _logger.LogInformation("Paiement {Status} pour la commande {OrderId}", 
                paymentSuccess ? "réussi" : "échoué", orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du traitement du paiement pour la commande {OrderId}", orderId);
            await _publishEndpoint.Publish(new PaymentProcessedEvent
            {
                OrderId = orderId,
                Success = false,
                FailureReason = "Erreur technique lors du paiement"
            });
        }
    }
}