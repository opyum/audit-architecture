namespace SagaPattern.Shared;

// InventoryUpdatedEvent.cs
public record InventoryUpdatedEvent
{
    public Guid OrderId { get; init; }
    public bool Success { get; init; }
    public string? FailureReason { get; init; }
}
