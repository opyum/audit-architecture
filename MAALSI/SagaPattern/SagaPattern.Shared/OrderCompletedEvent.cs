namespace SagaPattern.Shared;

// OrderCompletedEvent.cs
public record OrderCompletedEvent
{
    public Guid OrderId { get; init; }
    public OrderStatus Status { get; init; }
}
