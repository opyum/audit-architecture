// OrderService/Controllers/OrdersController.cs
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaPattern.Shared;

public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(
        ILogger<OrdersController> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        // Génération d'un ID unique pour la commande
        var orderId = Guid.NewGuid();
        
        _logger.LogInformation("Création d'une nouvelle commande avec l'ID {OrderId}", orderId);

        // Publication de l'événement qui démarre la saga
        await _publishEndpoint.Publish(new OrderCreatedEvent
        {
            OrderId = orderId,
            TotalAmount = request.Quantity * 10.0m, // Prix unitaire fictif de 10€
            ProductId = request.ProductId,
            Quantity = request.Quantity
        });

        // Retourne l'ID de la commande pour suivi
        return Ok(new { OrderId = orderId });
    }
}

// OrderService/Models/CreateOrderRequest.cs
public class CreateOrderRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}