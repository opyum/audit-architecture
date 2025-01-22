// OrderService/Consumers/PaymentProcessedEventConsumer.cs
using MassTransit;
using SagaPattern.Shared;

public class PaymentProcessedEventConsumer : IConsumer<PaymentProcessedEvent>
{
    private readonly ILogger<PaymentProcessedEventConsumer> _logger;

    public PaymentProcessedEventConsumer(ILogger<PaymentProcessedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        var message = context.Message;
        
        if (!message.Success)
        {
            _logger.LogInformation("La commande {OrderId} a échoué au paiement : {Reason}", 
                message.OrderId, message.FailureReason);
            // Ici, vous mettriez à jour l'état de la commande en base de données
        }
    }
}
