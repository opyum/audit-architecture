namespace SagaPattern.Shared;

// OrderCreatedEvent.cs
public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public decimal TotalAmount { get; init; }
    public string ProductId { get; init; }
    public int Quantity { get; init; }
}
